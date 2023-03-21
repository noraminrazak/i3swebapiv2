using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace I3SwebAPIv2.Class
{
    public class WalletV2Class
    {
        public string GetAlphaUniqueKey()
        {
            int maxSize = 3;
            //int minSize = 5;
            char[] chars = new char[62];
            string a;
            a = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            chars = a.ToCharArray();
            int size = maxSize;
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length - 1)]);
            }
            return result.ToString();
        }

        public string GetNumericUniqueKey()
        {
            int maxSize = 4;
            //int minSize = 5;
            char[] chars = new char[62];
            string a;
            a = "1234567890";
            chars = a.ToCharArray();
            int size = maxSize;
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length - 1)]);
            }
            return result.ToString();
        }

        public string GenerateWalletNumber() {

            string value = string.Empty;
            DateTime dt = DateTime.Now;
            var julian = string.Format("{0:yy}{1:D3}", dt, dt.DayOfYear);

            MySqlTransaction trans = null;

            string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(constr))
            {
                try
                {
                    conn.Open();
                    trans = conn.BeginTransaction();

                    using (MySqlCommand cmd = new MySqlCommand("sp_get_running_number", conn))
                    {
                        cmd.Transaction = trans;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@p_running_code", "wallet_number");
                        cmd.Parameters.Add("@p_running_number", MySqlDbType.Int32);
                        cmd.Parameters["@p_running_number"].Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                        trans.Commit();

                        var running_number = Convert.ToInt32(cmd.Parameters["@p_running_number"].Value);

                        value = julian + string.Format("{0:00000}", running_number);
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtility.LogException(ex, "wallet/running_number");
                }
                finally
                {
                    conn.Close();
                }
            }

            return value;
        }
    }
}