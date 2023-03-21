using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace I3SwebAPIv2.Class
{
    public class EmailV2Class
    {
        public bool Send_Email_Reset_Password(string email, string full_name, string user_id, string token)
        {
            bool result = false;

            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.i-3s.com.my");

                mail.From = new MailAddress("noreply@i-3s.com.my");
                mail.To.Add(email);
                mail.Subject = "Reset your password";

                string body = string.Empty;
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Templates/ResetPassword.html")))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{full_name}", full_name);
                body = body.Replace("{uid}", user_id);
                body = body.Replace("{email}", email);
                body = body.Replace("{token}", token);

                mail.Body = body;
                mail.IsBodyHtml = true;

                SmtpServer.Port = 25;
                SmtpServer.Credentials = new System.Net.NetworkCredential("noreply@i-3s.com.my", "n0rep|y");
                //SmtpServer.EnableSsl = true;
                //SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                //SmtpServer.UseDefaultCredentials = false;
                ServicePointManager.Expect100Continue = false;

                SmtpServer.Send(mail);
                result = true;
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "user/forgot-password");
                result = false;
            }
            return result;
        }

        public bool Send_Email_Reset_Password_GV(string email, string full_name, string user_id, string token)
        {
            bool result = false;

            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.myedutech.my");

                mail.From = new MailAddress("noreply@myedutech.my");
                mail.To.Add(email);
                mail.Subject = "Reset your password";

                string body = string.Empty;
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Templates/ResetPasswordGV.html")))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{full_name}", full_name);
                body = body.Replace("{uid}", user_id);
                body = body.Replace("{email}", email);
                body = body.Replace("{token}", token);

                mail.Body = body;
                mail.IsBodyHtml = true;

                SmtpServer.Port = 25;
                SmtpServer.Credentials = new System.Net.NetworkCredential("noreply@myedutech.my", "n0rep|yGV!");
                //SmtpServer.EnableSsl = true;
                //SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                //SmtpServer.UseDefaultCredentials = false;
                ServicePointManager.Expect100Continue = false;

                SmtpServer.Send(mail);
                result = true;
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "user/forgot-password");
                result = false;
            }
            return result;
        }

        public bool Send_Email_Delete_Account(string email, string full_name, string nric, string mobile_number)
        {
            bool result = false;

            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.i-3s.com.my");

                mail.From = new MailAddress("noreply@i-3s.com.my");
                mail.To.Add("cs@emerging.com.my");
                mail.Subject = "i-3s Account Deletion";

                string body = string.Empty;
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Templates/DeleteAccount.html")))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{full_name}", full_name);
                body = body.Replace("{nric}", nric);
                body = body.Replace("{email}", email);
                body = body.Replace("{mobile_number}", mobile_number);

                mail.Body = body;
                mail.IsBodyHtml = true;

                SmtpServer.Port = 25;
                SmtpServer.Credentials = new System.Net.NetworkCredential("noreply@i-3s.com.my", "n0rep|y");
                //SmtpServer.EnableSsl = true;
                //SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                //SmtpServer.UseDefaultCredentials = false;
                ServicePointManager.Expect100Continue = false;

                SmtpServer.Send(mail);
                result = true;
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "user/delete-account");
                result = false;
            }
            return result;
        }

        public bool Send_Email_Delete_Account_GV(string email, string full_name, string nric, string mobile_number)
        {
            bool result = false;

            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.myedutech.my");

                mail.From = new MailAddress("noreply@myedutech.my");
                mail.To.Add("cs@emerging.com.my");
                mail.Subject = "GVIIS Account Deletion";

                string body = string.Empty;
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Templates/DeleteAccountGV.html")))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{full_name}", full_name);
                body = body.Replace("{nric}", nric);
                body = body.Replace("{email}", email);
                body = body.Replace("{mobile_number}", mobile_number);

                mail.Body = body;
                mail.IsBodyHtml = true;

                SmtpServer.Port = 25;
                SmtpServer.Credentials = new System.Net.NetworkCredential("noreply@myedutech.my", "n0rep|yGV!");
                //SmtpServer.EnableSsl = true;
                //SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                //SmtpServer.UseDefaultCredentials = false;
                ServicePointManager.Expect100Continue = false;

                SmtpServer.Send(mail);
                result = true;
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "user/delete-account");
                result = false;
            }
            return result;
        }

        public string Get_Token(string email)
        {
            string salt = ConfigurationManager.AppSettings["passPhrase"];
            byte[] key = Encoding.ASCII.GetBytes(salt);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                      new Claim(ClaimTypes.Email, email)}),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(securityKey,
                SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }
    }
}