using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace I3SwebAPIv2.Class
{
    public class DataChecker
    {
        public bool IsValidIdentity(int card_type_id, string identity_number) 
        {
            bool value = false;
            if (card_type_id == 1) //MyKad
            {
                if (IsNumeric(identity_number) == true && identity_number.Length == 12)
                {
                    value = true;
                }
            }
            else if (card_type_id == 2) //Passport
            {
                {
                    if (IsAlphaNumeric(identity_number) && identity_number.Length <= 10)
                    {
                        value = true;
                    }
                }
            }
            return value;
        }

        public bool IsValidIdNumber(string id_number)
        {
            bool value = false;

            if (IsNumeric(id_number) == true && id_number.Length == 12)
            {
                value = true;
            }

            if (IsAlphaNumeric(id_number) && id_number.Length <= 10)
            {
                value = true;
            }

            return value;
        }

        public bool IsNumeric(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }

        public bool IsAlphaNumeric(string input)
        {
            return Regex.IsMatch(input, "^[a-zA-Z0-9]+$");
        }

        public string RemoveNonAlphaNumeric(string s) 
        {
            if (string.IsNullOrEmpty(s))
                return s;

            StringBuilder sb = new StringBuilder();
            foreach (var c in s)
            {
                if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9'))
                    sb.Append(c);
            }
            return sb.ToString();
        }

        public bool IsMobileNumber(string input)
        {
            return Regex.Match(input, @"\+?[0-9]{11,12}").Success;
        }

        //public bool IsValidEmail(string input)
        //{
        //    try
        //    {
        //        var addr = new System.Net.Mail.MailAddress(input);
        //        return addr.Address == input;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}