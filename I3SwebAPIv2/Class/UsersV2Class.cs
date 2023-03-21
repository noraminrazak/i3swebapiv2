using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Linq;


namespace I3SwebAPIv2.Class
{
    public class UsersV2Class
    {
        public string GenerateWalletNumber()
        {
            DateTime dt = new DateTime();

            string value = string.Empty;
            string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(constr))
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("sp_get_wallet_number", conn))
                {
                    MySqlTransaction trans;
                    trans = conn.BeginTransaction();
                    cmd.Transaction = trans;

                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@p_running_number", MySqlDbType.VarChar);
                        cmd.Parameters["@p_running_number"].Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                        trans.Commit();

                        value = DateTime.Now.ToString("yyMM") + cmd.Parameters["@p_running_number"].Value.ToString();

                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            return value;
        }

        public void UpdateWalletNumber()
        {
            string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(constr))
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("sp_update_wallet_number", conn))
                {
                    MySqlTransaction trans;
                    trans = conn.BeginTransaction();
                    cmd.Transaction = trans;

                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }
        public string GenerateToken()
        {
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();

            return Convert.ToBase64String(time.Concat(key).ToArray());
        }
    }
}