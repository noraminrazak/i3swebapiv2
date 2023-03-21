using i3sAuth;
using I3SwebAPIv2.Models;
using I3SwebAPIv2.Resources;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace I3SwebAPIv2.Class
{
    public class AuthRepository
    {
        public LoginDataModel ValidateUser(string UserName, string Password)
        {

            LoginDataModel data = new LoginDataModel();
            MySqlTransaction trans = null;
            data.Success = false;
            string sqlQuery = string.Empty;
            string status_code = string.Empty;
            string last_login = string.Empty;
            string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(constr))
            {
                try
                {
                    conn.Open();
                    trans = conn.BeginTransaction();

                    using (MySqlCommand cmd = new MySqlCommand("sp_get_user_login", conn))
                    {
                        cmd.Transaction = trans;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@p_username", UserName);
                        cmd.Parameters.AddWithValue("@p_password", Password);
                        cmd.Parameters.Add("@p_status_code", MySqlDbType.String);
                        cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@p_last_login", MySqlDbType.String);
                        cmd.Parameters["@p_last_login"].Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                        trans.Commit();

                        status_code = Convert.ToString(cmd.Parameters["@p_status_code"].Value);
                        last_login = Convert.ToString(cmd.Parameters["@p_last_login"].Value);

                        if (status_code == "active")
                        {
                            data.Success = true;
                            data.Code = "select_role";
                            data.Message = last_login;
                        }
                        else if (status_code == "inactive")
                        {
                            data.Success = false;
                            data.Code = "account_not_active";
                            data.Message = "Sorry, your account is not active.";
                        }
                        else if (status_code == "no_record")
                        {
                            data.Success = false;
                            data.Code = "wrong_password";
                            data.Message = "Your password is incorrect.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    if(trans != null) trans.Rollback();
                    data.Success = false;
                    data.Code = "error_occured";
                    data.Message = WebApiResources.ErrorOccured;
                    ExceptionUtility.LogException(ex, "auth-login");
                }
                finally
                {
                    conn.Close();
                }
            }
            return data;
        }
    }
}