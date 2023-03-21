using I3SwebAPIv2.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Http;
using i3sAuth;
using I3SwebAPIv2.Class;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Globalization;
using System.Threading;
using I3SwebAPIv2.Resources;
using System.Text;
using Microsoft.Office.Core;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;

namespace I3SwebAPIv2.Controllers
{
    public class UsersV2Controller : ApiController
    {
        DataChecker check = new DataChecker();
        Rijndael auth = new Rijndael();
        MPayWallet mpay = new MPayWallet();
        public string salt = ConfigurationManager.AppSettings["passPhrase"];
        // GET: api/Users
        [HttpPost]
        [Route("api/v2/user/init-login")]
        public IHttpActionResult Post_User_Init_Login(string culture, [FromBody]LoginParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            LoginDataModel data = new LoginDataModel();
            if (value.username != null)
            {
                MySqlTransaction trans = null;
                data.Success = false;
                string sqlQuery = string.Empty;
                string status_code = string.Empty;
                //int merchant_exist = 0;

                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_user_username", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_username", value.username);
                            cmd.Parameters.Add("@p_status_code", MySqlDbType.String);
                            cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd.Parameters.Add("@p_password", MySqlDbType.String);
                            cmd.Parameters["@p_password"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            status_code = Convert.ToString(cmd.Parameters["@p_status_code"].Value);

                            if (status_code == "active")
                            {
                                data.Success = true;
                                data.Code = "auth_login";
                                data.Message = WebApiResources.PlsEnterPswd;
                            }
                            else if (status_code == "create_password")
                            {
                                data.Success = true;
                                data.Code = "create_password";
                                data.Message = WebApiResources.PlsCreateNewPswd;
                            }
                            else if (status_code == "register_account")
                            {
                                data.Success = true;
                                data.Code = "register_account";
                                data.Message = WebApiResources.NewRegistrationText;
                            }
                            else if (status_code == "no_record")
                            {
                                data.Success = false;
                                data.Code = "no_record_found";
                                data.Message = WebApiResources.UsernameIncorrect;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "user/init-login");
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            else {
                return BadRequest("Missing parameters.");
            }
            return Ok(data);
        }

        [HttpPost]
        [Route("api/v2/user/last-login")]
        public IHttpActionResult Post_User_Last_Login(string culture, [FromBody] LoginParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            LoginDataModel data = new LoginDataModel();
            if (value.username != null)
            {
                MySqlTransaction trans = null;
                data.Success = false;
                string sqlQuery = string.Empty;
                string last_login = string.Empty;
                //int merchant_exist = 0;

                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_user_last_login", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_username", value.username);
                            cmd.Parameters.Add("@p_last_login", MySqlDbType.String);
                            cmd.Parameters["@p_last_login"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            last_login = Convert.ToString(cmd.Parameters["@p_last_login"].Value);

                            data.Success = true;
                            data.Code = "OK";
                            data.Message = last_login;

                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "user/last-login");
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            else
            {
                return BadRequest("Missing parameters.");
            }
            return Ok(data);
        }

        //[HttpPost]
        //[Route("api/v2/user/register")]
        //public async Task<IHttpActionResult> Post_User_Register(string culture, [FromBody] RegisterParam value)
        //{
        //    if (!string.IsNullOrEmpty(culture))
        //    {
        //        var language = new CultureInfo(culture);
        //        Thread.CurrentThread.CurrentUICulture = language;
        //    }

        //    DateTime dob = new DateTime();
        //    MOcean ocean = new MOcean();
        //    UsersV2Class cls = new UsersV2Class();
        //    DataChecker check = new DataChecker();
        //    RegisterDataModel data = new RegisterDataModel();
        //    if (value.full_name != null && value.nationality_id != 0 && value.card_type_id != 0 && value.identity_number != null && value.mobile_number != null
        //        && value.email != null && value.password != null && value.mother_maiden_name != null && value.occupation != null && value.employer_name != null
        //        && value.address != null && value.postcode != null && value.state_id != 0 && value.city != null && value.country_id != 0
        //        && value.date_of_birth != null && value.user_role_id != 0)
        //    {
        //        //check idenity number
        //        if (check.IsValidIdentity(value.card_type_id, value.identity_number) == true)
        //        {
        //            if (value.card_type_id == 1)
        //            {
        //                int curYear = Convert.ToInt32(DateTime.Now.ToString("yy"));
        //                int year = Convert.ToInt32(value.identity_number.Substring(0, 2));
        //                if (curYear <= year)
        //                {
        //                    year = 1900 + year;
        //                }
        //                else
        //                {
        //                    year = 2000 + year;
        //                }
        //                int month = Convert.ToInt32(value.identity_number.Substring(2, 2));
        //                int day = Convert.ToInt32(value.identity_number.Substring(4, 2));

        //                dob = new DateTime(year, month, day);
        //            }

        //            if (check.IsMobileNumber(value.mobile_number) == true)
        //            {
        //                string salt = ConfigurationManager.AppSettings["passPhrase"];
        //                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

        //                //generate otp
        //                PasswordGenerator generate = new PasswordGenerator().IncludeNumeric().LengthRequired(6);
        //                string otp = generate.Next();

        //                using (MySqlConnection conn = new MySqlConnection(constr))
        //                {
        //                    conn.Open();

        //                    using (MySqlCommand cmd = new MySqlCommand("sp_insert_user_self_register", conn))
        //                    {
        //                        MySqlTransaction trans;
        //                        MySqlTransaction trans2;
        //                        trans = conn.BeginTransaction();
        //                        cmd.Transaction = trans;

        //                        try
        //                        {
        //                            cmd.CommandType = CommandType.StoredProcedure;
        //                            cmd.Parameters.Clear();
        //                            cmd.Parameters.AddWithValue("@p_full_name", value.full_name);
        //                            cmd.Parameters.AddWithValue("@p_nationality_id", value.nationality_id);
        //                            cmd.Parameters.AddWithValue("@p_mother_maiden_name", value.mother_maiden_name);
        //                            cmd.Parameters.AddWithValue("@p_card_type_id", value.card_type_id);
        //                            cmd.Parameters.AddWithValue("@p_identity_number", value.identity_number);
        //                            cmd.Parameters.AddWithValue("@p_date_of_birth", dob.ToString("yyyy-MM-dd"));
        //                            if (value.card_type_id == 1)
        //                            {
        //                                cmd.Parameters.AddWithValue("@p_date_of_birth", dob.ToString("yyyy-MM-dd"));
        //                            }
        //                            else
        //                            {
        //                                cmd.Parameters.AddWithValue("@p_date_of_birth", value.date_of_birth.Split('-')[2] + "-" + value.date_of_birth.Split('-')[1] + "-" + value.date_of_birth.Split('-')[0]);
        //                            }
        //                            cmd.Parameters.AddWithValue("@p_mobile_number", value.mobile_number);
        //                            cmd.Parameters.AddWithValue("@p_email", value.email);
        //                            cmd.Parameters.AddWithValue("@p_address", value.address);
        //                            cmd.Parameters.AddWithValue("@p_postcode", value.postcode);
        //                            cmd.Parameters.AddWithValue("@p_state_id", value.state_id);
        //                            cmd.Parameters.AddWithValue("@p_city", value.city);
        //                            cmd.Parameters.AddWithValue("@p_country_id", value.country_id);
        //                            cmd.Parameters.AddWithValue("@p_password", auth.EncryptRijndael(value.password, salt));
        //                            cmd.Parameters.AddWithValue("@p_otp", auth.EncryptRijndael(otp, salt));
        //                            cmd.Parameters.AddWithValue("@p_occupation", value.occupation);
        //                            cmd.Parameters.AddWithValue("@p_employer_name", value.employer_name);
        //                            cmd.Parameters.AddWithValue("@p_marketing_flag", value.marketing_flag);
        //                            cmd.Parameters.Add("@p_profile_id", MySqlDbType.Int16);
        //                            cmd.Parameters["@p_profile_id"].Direction = ParameterDirection.Output;
        //                            cmd.Parameters.Add("@p_user_status", MySqlDbType.Int16);
        //                            cmd.Parameters["@p_user_status"].Direction = ParameterDirection.Output;
        //                            cmd.ExecuteNonQuery();
        //                            trans.Commit();

        //                            int profile_id = Convert.ToInt32(cmd.Parameters["@p_profile_id"].Value);
        //                            int user_status_id = Convert.ToInt32(cmd.Parameters["@p_user_status"].Value);

        //                            if (value.user_role_id == 9)
        //                            {
        //                                string wallet_number = cls.GenerateWalletNumber();
        //                                string token = cls.GenerateToken();

        //                                using (MySqlCommand cmd2 = new MySqlCommand("sp_insert_parent_self_register", conn))
        //                                {
        //                                    trans2 = conn.BeginTransaction();
        //                                    cmd2.Transaction = trans2;

        //                                    try
        //                                    {
        //                                        cmd2.CommandType = CommandType.StoredProcedure;
        //                                        cmd2.Parameters.Clear();
        //                                        cmd2.Parameters.AddWithValue("@p_profile_id", profile_id);
        //                                        cmd2.Parameters.AddWithValue("@p_wallet_number", wallet_number);
        //                                        cmd2.Parameters.AddWithValue("@p_token", token);
        //                                        cmd2.Parameters.AddWithValue("@p_create_by", value.full_name);
        //                                        cmd2.Parameters.Add("@p_parent_exists", MySqlDbType.Int16);
        //                                        cmd2.Parameters["@p_parent_exists"].Direction = ParameterDirection.Output;
        //                                        cmd2.Parameters.Add("@p_parent_id", MySqlDbType.Int16);
        //                                        cmd2.Parameters["@p_parent_id"].Direction = ParameterDirection.Output;
        //                                        cmd2.Parameters.Add("@p_wallet_id", MySqlDbType.Int16);
        //                                        cmd2.Parameters["@p_wallet_id"].Direction = ParameterDirection.Output;
        //                                        cmd2.Parameters.Add("@p_wallet_exists", MySqlDbType.Int16);
        //                                        cmd2.Parameters["@p_wallet_exists"].Direction = ParameterDirection.Output;
        //                                        cmd2.Parameters.Add("@p_parent_status", MySqlDbType.Int16);
        //                                        cmd2.Parameters["@p_parent_status"].Direction = ParameterDirection.Output;
        //                                        cmd2.ExecuteNonQuery();
        //                                        trans2.Commit();

        //                                        int parent_exists = Convert.ToInt32(cmd2.Parameters["@p_parent_exists"].Value);
        //                                        int parent_id = Convert.ToInt32(cmd2.Parameters["@p_parent_id"].Value);
        //                                        int wallet_id = Convert.ToInt32(cmd2.Parameters["@p_wallet_id"].Value);
        //                                        int wallet_exists = Convert.ToInt32(cmd2.Parameters["@p_wallet_exists"].Value);
        //                                        int parent_status_id = Convert.ToInt32(cmd2.Parameters["@p_parent_status"].Value);

        //                                        if (parent_status_id == 2)
        //                                        {
        //                                            try
        //                                            {
        //                                                //var result = ocean.Send_OTP(otp, value.mobile_number);
        //                                                var result = ocean.Send_OTP(otp, "+60169050115"); // for testing only
        //                                                string jsonStr = await result;
        //                                                MoceanModel json = JsonConvert.DeserializeObject<MoceanModel>(jsonStr);
        //                                                List<Mocean> list = new List<Mocean>();
        //                                                foreach (Mocean item in json.messages)
        //                                                {
        //                                                    if (item.status != "0")
        //                                                    {
        //                                                        data.Success = false;
        //                                                        data.Code = "error_occured";
        //                                                        data.Message = item.err_msg;
        //                                                    }
        //                                                    else
        //                                                    {
        //                                                        string front = string.Empty;
        //                                                        int m = value.mobile_number.Length - 3;
        //                                                        for (int n = 0; n < m; n++)
        //                                                        {
        //                                                            front += "X";
        //                                                        }

        //                                                        if (wallet_exists == 0)
        //                                                        {
        //                                                            cls.UpdateWalletNumber();
        //                                                        }

        //                                                        string last_four = value.mobile_number.Substring(value.mobile_number.Length - 4, 4);
        //                                                        data.Success = true;
        //                                                        data.Code = "verify_account";
        //                                                        data.Message = WebApiResources.OTPHasBeenSent + value.mobile_number;
        //                                                    }
        //                                                }
        //                                            }
        //                                            catch (Exception ex)
        //                                            {
        //                                                data.Success = false;
        //                                                data.Code = "error_occured";
        //                                                data.Message = WebApiResources.ErrorOccured;
        //                                                ExceptionUtility.LogException(ex, "user/register");
        //                                            }
        //                                        }
        //                                        else
        //                                        {
        //                                            data.Success = false;
        //                                            data.Code = "record_exists";
        //                                            data.Message = WebApiResources.ParentIDAlreadyExist;
        //                                        }

        //                                    }
        //                                    catch (Exception ex)
        //                                    {
        //                                        trans2.Rollback();
        //                                        data.Success = false;
        //                                        data.Code = "error_occured";
        //                                        data.Message = WebApiResources.ErrorOccured;
        //                                        ExceptionUtility.LogException(ex, "user/register");
        //                                    }
        //                                }
        //                            }
        //                            else
        //                            {

        //                                try
        //                                {
        //                                    //var result = ocean.Send_OTP(otp, value.mobile_number);
        //                                    var result = ocean.Send_OTP(otp, "+60169050115"); // for testing only
        //                                    string jsonStr = await result;
        //                                    MoceanModel json = JsonConvert.DeserializeObject<MoceanModel>(jsonStr);
        //                                    List<Mocean> list = new List<Mocean>();
        //                                    foreach (Mocean item in json.messages)
        //                                    {
        //                                        if (item.status != "0")
        //                                        {
        //                                            data.Success = false;
        //                                            data.Code = "error_occured";
        //                                            data.Message = item.err_msg;
        //                                        }
        //                                        else
        //                                        {
        //                                            string front = string.Empty;
        //                                            int m = value.mobile_number.Length - 3;
        //                                            for (int n = 0; n < m; n++)
        //                                            {
        //                                                front += "X";
        //                                            }

        //                                            string last_four = value.mobile_number.Substring(value.mobile_number.Length - 4, 4);
        //                                            data.Success = true;
        //                                            data.Code = "verify_account";
        //                                            data.Message = WebApiResources.OTPHasBeenSent + value.mobile_number;
        //                                        }
        //                                    }
        //                                }
        //                                catch (Exception ex)
        //                                {
        //                                    data.Success = false;
        //                                    data.Code = "error_occured";
        //                                    data.Message = WebApiResources.ErrorOccured;
        //                                    ExceptionUtility.LogException(ex, "user/register");
        //                                }
        //                            }
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            trans.Rollback();
        //                            data.Success = false;
        //                            data.Code = "error_occured";
        //                            data.Message = WebApiResources.ErrorOccured;
        //                            ExceptionUtility.LogException(ex, "user/register");
        //                        }
        //                        finally
        //                        {
        //                            conn.Close();
        //                        }
        //                    }
        //                }

        //            }
        //            else
        //            {
        //                data.Success = false;
        //                data.Code = "invalid_mobile_number";
        //                data.Message = WebApiResources.MobileNoInvalid;
        //            }
        //        }
        //        else
        //        {
        //            data.Success = false;
        //            data.Code = "invalid_identity_number";
        //            data.Message = WebApiResources.IdentityNoInvalid;
        //        }
        //    }
        //    else
        //    {
        //        return BadRequest("Missing parameters.");
        //    }
        //    return Ok(data);
        //}


        //[HttpPost]
        //[Route("api/v2/user/get-password")]
        //public string Post_User_Get_Password(string culture, [FromBody] LoginParam value)
        //{
        //    string pass = string.Empty;
        //    pass = auth.DecryptRijndael(value.username, salt);

        //    return pass;
        //}

        [HttpPost]
        [Route("api/v2/user/create-password")]
        public async Task<IHttpActionResult> Post_User_Create_Password(string culture, [FromBody]CreatePasswordParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            MOcean ocean = new MOcean();
            LoginDataModel data = new LoginDataModel();
            if (value.mobile_number != null &&  value.username != null && value.password != null && value.confirm_password != null)
            {
                if (value.password == value.confirm_password)
                {
                    string salt = ConfigurationManager.AppSettings["passPhrase"];
                    string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;
                    //generate otp
                    PasswordGenerator generate = new PasswordGenerator().IncludeNumeric().LengthRequired(6);
                    string otp = generate.Next();

                    using (MySqlConnection conn = new MySqlConnection(constr))
                    {
                        conn.Open();

                        using (MySqlCommand cmd = new MySqlCommand("sp_update_password_create", conn))
                        {
                            MySqlTransaction trans;
                            trans = conn.BeginTransaction();
                            cmd.Transaction = trans;

                            try
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@p_mobile_number", value.mobile_number);
                                cmd.Parameters.AddWithValue("@p_username", value.username);
                                cmd.Parameters.AddWithValue("@p_password", auth.EncryptRijndael(value.password, salt));
                                cmd.Parameters.AddWithValue("@p_otp", auth.EncryptRijndael(otp, salt));
                                cmd.Parameters.Add("@p_user_id", MySqlDbType.Int16);
                                cmd.Parameters["@p_user_id"].Direction = ParameterDirection.Output;
                                cmd.ExecuteNonQuery();
                                trans.Commit();

                                //var user_id = Convert.ToInt16(cmd.Parameters["@p_user_id"].Value);

                                try
                                {
                                    var result = ocean.Send_OTP(otp, value.mobile_number);
                                    //var result = ocean.Send_OTP(otp, "+60169050115"); // for testing only
                                    //var result = ocean.Send_OTP(otp, "+60179117641"); // for testing only
                                    string jsonStr = await result;
                                    MoceanModel json = JsonConvert.DeserializeObject<MoceanModel>(jsonStr);
                                    List<Mocean> list = new List<Mocean>();
                                    foreach (Mocean item in json.messages)
                                    {
                                        if (item.status != "0")
                                        {
                                            data.Success = false;
                                            data.Code = "error_occured";
                                            data.Message = item.err_msg;
                                        }
                                        else
                                        {
                                            string front = string.Empty;
                                            string mobile = string.Empty;
                                            mobile = value.mobile_number.Replace("+", "");
                                            int m = mobile.Length - 4;
                                            for (int n = 0; n < m; n++)
                                            {
                                                front += "X";
                                            }
                                            string last_four = value.mobile_number.Substring(value.mobile_number.Length - 4, 4);
                                            data.Success = true;
                                            data.Code = "verify_account";
                                            data.Message = WebApiResources.OTPHasBeenSent + front + last_four;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    data.Success = false;
                                    data.Code = "error_occured";
                                    data.Message = WebApiResources.ErrorOccured;
                                    ExceptionUtility.LogException(ex, "user/create-password");
                                }
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                data.Success = false;
                                data.Code = "error_occured";
                                data.Message = WebApiResources.ErrorOccured;
                                ExceptionUtility.LogException(ex, "user/create-password");
                            }
                            finally
                            {
                                conn.Close();
                            }
                        }
                    }
                }
                else {
                    data.Success = false;
                    data.Code = "password_not_match";
                    data.Message = WebApiResources.PswdNotMatch;
                }
            }
            else
            {
                return BadRequest("Missing parameters.");
            }
            return Ok(data);
        }

        [HttpPost]
        [Route("api/v2/user/verify-account")]
        public async Task<IHttpActionResult> Post_User_Verify_Account(string culture, [FromBody]VerifyAccountParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            LoginDataModel data = new LoginDataModel();
            if (value.mobile_number != null && value.username != null && value.otp != null)
            {
                string salt = ConfigurationManager.AppSettings["passPhrase"];
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;
                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand("sp_update_user_verify_account", conn))
                    {
                        MySqlTransaction trans;
                        trans = conn.BeginTransaction();
                        cmd.Transaction = trans;

                        try
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@p_mobile_number", value.mobile_number);
                            cmd.Parameters.AddWithValue("@p_username", value.username);
                            cmd.Parameters.AddWithValue("@p_otp", auth.EncryptRijndael(value.otp, salt));
                            cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                            cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd.Parameters.Add("@p_mpay_uid", MySqlDbType.Int32);
                            cmd.Parameters["@p_mpay_uid"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            int mpay_uid = 0;
                            string status_code = (string)cmd.Parameters["@p_status_code"].Value;
                            if (cmd.Parameters["@p_mpay_uid"].Value != DBNull.Value) 
                            {
                                mpay_uid = Convert.ToInt32(cmd.Parameters["@p_mpay_uid"].Value);
                            }

                            if (status_code == "verify_success")
                            {
                                if (mpay_uid == 0)
                                {
                                    RegisterAccParam prop = new RegisterAccParam();

                                    using (MySqlCommand cmd2 = new MySqlCommand("sp_get_user_profile_register", conn))
                                    {
                                        MySqlTransaction trans2;
                                        trans2 = conn.BeginTransaction();
                                        cmd2.Transaction = trans2;

                                        try
                                        {
                                            cmd2.CommandType = CommandType.StoredProcedure;
                                            cmd2.Parameters.Clear();
                                            cmd2.Parameters.AddWithValue("@p_nric", value.username);
                                            MySqlDataReader dataReader = cmd2.ExecuteReader();

                                            if (dataReader.HasRows == true)
                                            {
                                                while (dataReader.Read())
                                                {
                                                    prop.name = dataReader["full_name"].ToString();
                                                    prop.nationality = dataReader["nationality_id"].ToString();
                                                    prop.idno = dataReader["nric"].ToString();
                                                    prop.email = dataReader["email"].ToString();
                                                    prop.mobileno = dataReader["mobile_number"].ToString().TrimStart('+');
                                                    prop.dob = Convert.ToDateTime(dataReader["date_of_birth"]).ToString("yyyyMMdd");
                                                    prop.loginid = dataReader["email"].ToString();
                                                    prop.address = dataReader["address"].ToString();
                                                    prop.state = dataReader["state_id"].ToString();
                                                    prop.province = dataReader["state_name"].ToString();
                                                    prop.addCountry = dataReader["country_id"].ToString();
                                                    prop.city = dataReader["city"].ToString();
                                                    prop.postalcode = dataReader["postcode"].ToString();
                                                    prop.mothermaidenname = dataReader["mother_maiden_name"].ToString();
                                                    prop.occupation = dataReader["occupation"].ToString();
                                                    prop.employer_name = dataReader["employer_name"].ToString();
                                                    prop.useridimagefilename = dataReader["photo_id_url"].ToString();
                                                    prop.useridimagestring = dataReader["photo_id_url"].ToString();
                                                    prop.userselfieimagefilename = dataReader["photo_url"].ToString();
                                                    prop.userselfieimagestring = dataReader["photo_url"].ToString();
                                                    prop.marketingflag = dataReader["marketing_flag"].ToString();
                                                }
                                            }

                                            if (dataReader != null)
                                                dataReader.Close();

                                            var result = mpay.PostAccountRegistration(prop);
                                            string jsonStr = await result;
                                            RegisterAccModel response = JsonConvert.DeserializeObject<RegisterAccModel>(jsonStr);

                                            if (response.Header.status == "00")
                                            {
                                                UserAccInfo acc = response.Body.useracc_info;

                                                CardInfo card = new CardInfo();
                                                foreach (CardInfo sl in response.Body.cardinfo)
                                                {

                                                    card.card_id = sl.card_id;
                                                    card.mask_cardno = sl.mask_cardno;
                                                    card.cardtoken = sl.cardtoken;
                                                    card.cardtype = sl.cardtype;
                                                    card.cardGroup = sl.cardGroup;
                                                    card.card_temporary_pin = sl.card_temporary_pin;
                                                }

                                                if (dataReader != null)
                                                {
                                                    dataReader.Close();
                                                }

                                                using (MySqlCommand cmd3 = new MySqlCommand("sp_update_wallet_info", conn))
                                                {
                                                    try
                                                    {
                                                        cmd3.Transaction = trans2;
                                                        cmd3.CommandType = CommandType.StoredProcedure;
                                                        cmd3.Parameters.AddWithValue("@p_id_number", acc.idno);
                                                        cmd3.Parameters.AddWithValue("@p_mpay_uid", acc.uid);
                                                        cmd3.Parameters.AddWithValue("@p_mpay_card_id", card.card_id);
                                                        cmd3.Parameters.AddWithValue("@p_mpay_mask_cardno", card.mask_cardno);
                                                        cmd3.Parameters.AddWithValue("@p_mpay_card_token", card.cardtoken);
                                                        cmd3.Parameters.AddWithValue("@p_mpay_card_type", card.cardtype);
                                                        cmd3.Parameters.AddWithValue("@p_mpay_card_group", card.cardGroup);
                                                        cmd3.Parameters.AddWithValue("@p_mpay_card_pin", card.card_temporary_pin);
                                                        cmd3.Parameters.AddWithValue("@p_update_by", acc.name);
                                                        cmd3.ExecuteNonQuery();
                                                        trans2.Commit();

                                                        data.Success = true;
                                                        data.Code = status_code;
                                                        data.Message = WebApiResources.AccountVerifySuccess;

                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        trans2.Rollback();
                                                        data.Success = false;
                                                        data.Code = "error_occured";
                                                        data.Message = WebApiResources.ErrorOccured;
                                                        ExceptionUtility.LogException(ex, "user/verify-account");
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                data.Success = false;
                                                data.Message = response.Header.message;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            trans2.Rollback();
                                            data.Success = false;
                                            data.Code = "error_occured";
                                            data.Message = WebApiResources.ErrorOccured;
                                            ExceptionUtility.LogException(ex, "user/verify-account");
                                        }
                                    }
                                }
                                else
                                {
                                    data.Success = true;
                                    data.Code = status_code;
                                    data.Message = WebApiResources.AccountAlreadyVerify;
                                }
                            }
                            else if (status_code == "already_verify")
                            {
                                data.Success = true;
                                data.Code = status_code;
                                data.Message = WebApiResources.AccountAlreadyVerify;
                            }
                            else if (status_code == "merchant_verify")
                            {
                                data.Success = true;
                                data.Code = status_code;
                                data.Message = WebApiResources.AccountVerifySuccessMerchant;
                            }
                            else if(status_code == "otp_expired")
                            {
                                data.Success = false;
                                data.Code = status_code;
                                data.Message = WebApiResources.OTPHasExpired;
                            }
                            else if (status_code == "otp_tryout_exceed")
                            {
                                data.Success = false;
                                data.Code = status_code;
                                data.Message = WebApiResources.MaxOTPTryout;
                            }
                            else if (status_code == "wrong_otp")
                            {
                                data.Success = false;
                                data.Code = status_code;
                                data.Message = WebApiResources.WrongOTP;
                            }
                            else if (status_code == "no_record_found")
                            {
                                data.Success = false;
                                data.Code = status_code;
                                data.Message = WebApiResources.IncorrectUsername;
                            }

                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            data.Success = false;
                            data.Code = "error_occured";
                            data.Message = WebApiResources.ErrorOccured;
                            ExceptionUtility.LogException(ex, "user/verify-account");
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
            }
            else
            {
                return BadRequest("Missing parameters.");
            }
            return Ok(data);
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/user/change-password")]
        public IHttpActionResult Post_User_Change_Password(string culture, [FromBody]ChangePasswordParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            LoginDataModel data = new LoginDataModel();
            if (value.profile_id != 0 && value.username != null && value.password != null && value.new_password != null && value.update_by != null)
            {
                if (value.password == value.new_password)
                {
                    string salt = ConfigurationManager.AppSettings["passPhrase"];
                    string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;
                    using (MySqlConnection conn = new MySqlConnection(constr))
                    {
                        conn.Open();

                        using (MySqlCommand cmd = new MySqlCommand("sp_update_password_change", conn))
                        {
                            MySqlTransaction trans;
                            trans = conn.BeginTransaction();
                            cmd.Transaction = trans;

                            try
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                                cmd.Parameters.AddWithValue("@p_username", value.username);
                                cmd.Parameters.AddWithValue("@p_password", auth.EncryptRijndael(value.password, salt));
                                cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                                cmd.ExecuteNonQuery();
                                trans.Commit();

                                data.Success = true;
                                data.Code = "update_success";
                                data.Message = WebApiResources.PswdUpdateSuccess;

                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                data.Success = false;
                                data.Code = "error_occured";
                                data.Message = WebApiResources.ErrorOccured;
                                ExceptionUtility.LogException(ex, "user/change-password");
                            }
                            finally
                            {
                                conn.Close();
                            }
                        }
                    }
                }
                else
                {
                    data.Success = false;
                    data.Code = "password_not_match";
                    data.Message = WebApiResources.PswdNotMatch;
                }
            }
            else
            {
                return BadRequest("Missing parameters.");
            }
            return Ok(data);
        }

        [HttpPost]
        [Route("api/v2/user/reset-password")]
        public IHttpActionResult Post_User_Reset_Password([FromBody]ResetPasswordParam value)
        {
            LoginDataModel data = new LoginDataModel();
            if (value.user_id != 0 && value.email != null && value.token != null && value.password != null && value.confirm_password != null)
            {
                if (value.password == value.confirm_password)
                {
                    string salt = ConfigurationManager.AppSettings["passPhrase"];
                    string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;
                    using (MySqlConnection conn = new MySqlConnection(constr))
                    {
                        conn.Open();

                        using (MySqlCommand cmd = new MySqlCommand("sp_update_user_password_reset", conn))
                        {
                            MySqlTransaction trans;
                            trans = conn.BeginTransaction();
                            cmd.Transaction = trans;

                            try
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@p_user_id", value.user_id);
                                cmd.Parameters.AddWithValue("@p_email", value.email);
                                cmd.Parameters.AddWithValue("@p_token", value.token);
                                cmd.Parameters.AddWithValue("@p_password", auth.EncryptRijndael(value.password, salt));
                                cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                                cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                                cmd.ExecuteNonQuery();
                                trans.Commit();

                                var status_code = (string)cmd.Parameters["@p_status_code"].Value;
                                if (status_code == "reset_success")
                                {
                                    data.Success = true;
                                    data.Message = "Password reset successfully.";
                                }
                                else if (status_code == "link_expired")
                                {
                                    data.Success = false;
                                    data.Message = "Password reset link has expired.";
                                }
                                else if (status_code == "invalid_link")
                                {
                                    data.Success = false;
                                    data.Message = "Invalid password reset link.";
                                }
                                data.Code = status_code;
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                data.Success = false;
                                data.Code = "error_occured";
                                data.Message = WebApiResources.ErrorOccured;
                                ExceptionUtility.LogException(ex, "user/reset-password");
                            }
                            finally
                            {
                                conn.Close();
                            }
                        }
                    }
                }
                else
                {
                    data.Success = false;
                    data.Code = "password_not_match";
                    data.Message = "Password not match.";
                }
            }
            else
            {
                return BadRequest("Missing parameters.");
            }
            return Ok(data);
        }

        [HttpPost]
        [Route("api/v2/user/forgot-password")] //i-3s and GVIIS
        public IHttpActionResult Post_User_Forgot_Password(string culture, [FromBody]ForgetPasswordParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            EmailV2Class mail = new EmailV2Class();
            LoginDataModel data = new LoginDataModel();
            string status_code = string.Empty;
            string password = string.Empty;
            string full_name = string.Empty;
            string token = string.Empty;
            int user_id;

            if (value.username != null && value.email != null)
            {
                string salt = ConfigurationManager.AppSettings["passPhrase"];
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;
                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    MySqlTransaction trans;
                    MySqlTransaction trans2;
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand("sp_get_user_email", conn))
                    {

                        trans = conn.BeginTransaction();
                        cmd.Transaction = trans;

                        try
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_username", value.username);
                            cmd.Parameters.AddWithValue("@p_email", value.email);
                            cmd.Parameters.Add("@p_status_code", MySqlDbType.String);
                            cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd.Parameters.Add("@p_password", MySqlDbType.String);
                            cmd.Parameters["@p_password"].Direction = ParameterDirection.Output;
                            cmd.Parameters.Add("@p_full_name", MySqlDbType.String);
                            cmd.Parameters["@p_full_name"].Direction = ParameterDirection.Output;
                            cmd.Parameters.Add("@p_user_id", MySqlDbType.String);
                            cmd.Parameters["@p_user_id"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            status_code = Convert.ToString(cmd.Parameters["@p_status_code"].Value);
                            password = Convert.ToString(cmd.Parameters["@p_password"].Value);
                            full_name = Convert.ToString(cmd.Parameters["@p_full_name"].Value);
                            user_id = Convert.ToInt32(cmd.Parameters["@p_user_id"].Value);

                            if (status_code == "active")
                            {
                                if (!string.IsNullOrEmpty(password))
                                {
                                    if (value.email.Contains("@") == true)
                                    {
                                        token = mail.Get_Token(value.email);

                                        using (MySqlCommand cmd2 = new MySqlCommand("sp_insert_password_reset", conn))
                                        {
                                            trans2 = conn.BeginTransaction();
                                            cmd2.Transaction = trans2;

                                            try
                                            {
                                                cmd2.CommandType = CommandType.StoredProcedure;
                                                cmd2.Parameters.Clear();
                                                cmd2.Parameters.AddWithValue("@p_email", value.email);
                                                cmd2.Parameters.AddWithValue("@p_token", token);
                                                cmd2.Parameters.AddWithValue("@p_create_by", full_name);
                                                cmd2.ExecuteNonQuery();
                                                trans2.Commit();
                                            }
                                            catch (Exception ex)
                                            {
                                                trans2.Rollback();
                                                data.Success = false;
                                                data.Code = "error_occured";
                                                data.Message = WebApiResources.ErrorOccured;
                                                ExceptionUtility.LogException(ex, "user/forgot-password");
                                            }
                                            finally
                                            {
                                                conn.Close();
                                            }
                                        }
                                        //Change to Send_Email_Reset_Password_GV for GVIIS
                                        if (mail.Send_Email_Reset_Password_GV(value.email, full_name, user_id.ToString(), token) == true)
                                        {
                                            data.Success = true;
                                            data.Code = "reset_password";
                                            data.Message = WebApiResources.PleaseCheckYourEmail;
                                        }
                                        else
                                        {
                                            data.Success = false;
                                            data.Code = "error_occured";
                                            data.Message = WebApiResources.FailToSendEmail;
                                        }
                                    }
                                    else {
                                        data.Success = false;
                                        data.Code = "invalid_email";
                                        data.Message = WebApiResources.PleaseEnterValidEmail;
                                    }
                                }
                                else {
                                    data.Success = true;
                                    data.Code = "create_password";
                                    data.Message = WebApiResources.PleaseCreateNewPassword;
                                }

                            }
                            else if (status_code == "inactive")
                            {
                                data.Success = true;
                                data.Code = "account_not_active";
                                data.Message = WebApiResources.SorryAccountInactive;
                            }
                            else if (status_code == "no_record")
                            {
                                data.Success = true;
                                data.Code = "no_record_found";
                                data.Message = WebApiResources.UsernameIncorrect;
                            }

                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            data.Success = false;
                            data.Code = "error_occured";
                            data.Message = WebApiResources.ErrorOccured;
                            ExceptionUtility.LogException(ex, "user/forgot-password");
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
            }
            else
            {
                return BadRequest("Missing parameters.");
            }
            return Ok(data);
        }

        [HttpPost]
        [Route("api/v2/user/delete-account")] //i-3s and GVIIS
        public IHttpActionResult Post_User_Delete_Account(string culture, [FromBody] DeleteAccountParam value)
        {
            string ProfileID;
            string AccountBalance;
            string CardNumber;

            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            EmailV2Class mail = new EmailV2Class();
            LoginDataModel data = new LoginDataModel();

            if (value.username != null)
            {
                string salt = ConfigurationManager.AppSettings["passPhrase"];
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;
                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    conn.Open();
                    RegisterAccParam prop = new RegisterAccParam();

                    using (MySqlCommand cmd = new MySqlCommand("sp_get_user_profile_register", conn))
                    {
                        MySqlTransaction trans;
                        trans = conn.BeginTransaction();
                        cmd.Transaction = trans;

                        try
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_nric", value.username);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    prop.name = dataReader["full_name"].ToString();
                                    prop.idno = dataReader["nric"].ToString();
                                    prop.email = dataReader["email"].ToString();
                                    prop.mobileno = dataReader["mobile_number"].ToString().TrimStart('+');
                                }

                                if (dataReader != null)
                                    dataReader.Close();
                                    trans.Dispose();

                                using (MySqlCommand cmd2 = new MySqlCommand("sp_delete_user_account", conn))
                                {
                                    MySqlTransaction trans2;
                                    trans2 = conn.BeginTransaction();
                                    cmd2.Transaction = trans2;

                                    try
                                    {
                                        cmd2.CommandType = CommandType.StoredProcedure;
                                        cmd2.Parameters.Clear();
                                        cmd2.Parameters.AddWithValue("@p_nric", value.username);
                                        cmd2.Parameters.Add("@p_account_balance", MySqlDbType.Decimal);
                                        cmd2.Parameters["@p_account_balance"].Direction = ParameterDirection.Output;
                                        cmd2.Parameters.Add("@p_card_number", MySqlDbType.String);
                                        cmd2.Parameters["@p_card_number"].Direction = ParameterDirection.Output;
                                        cmd2.Parameters.Add("@p_profile_id", MySqlDbType.Int64);
                                        cmd2.Parameters["@p_profile_id"].Direction = ParameterDirection.Output;
                                        cmd2.ExecuteNonQuery();
                                        trans2.Commit();

                                        AccountBalance = Convert.ToString(cmd2.Parameters["@p_account_balance"].Value);
                                        CardNumber = Convert.ToString(cmd2.Parameters["@p_card_number"].Value);
                                        ProfileID = Convert.ToString(cmd2.Parameters["@p_profile_id"].Value);

                                        //Change to Send_Email_Reset_Password_GV for GVIIS
                                        if (mail.Send_Email_Delete_Account_GV(prop.email, prop.name, prop.idno, prop.mobileno) == true)
                                        {
                                            data.Success = true;
                                            data.Code = "delete_account";
                                            data.Message = WebApiResources.AccountDeleteSuccess;
                                        }
                                        else
                                        {
                                            data.Success = false;
                                            data.Code = "error_occured";
                                            data.Message = WebApiResources.FailToSendEmail;
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        trans2.Rollback();
                                        data.Success = false;
                                        data.Code = "error_occured";
                                        data.Message = WebApiResources.ErrorOccured;
                                        ExceptionUtility.LogException(ex, "user/delete-account");
                                    }
                                }
                            }
                            else 
                            {
                                data.Success = false;
                                data.Code = "error_occured";
                                data.Message = WebApiResources.AccountNotFound;
                            }

                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            data.Success = false;
                            data.Code = "error_occured";
                            data.Message = WebApiResources.ErrorOccured;
                            ExceptionUtility.LogException(ex, "user/delete-account");
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
            }
            else
            {
                return BadRequest("Missing parameters.");
            }
            return Ok(data);
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/user/role-account")]
        public IHttpActionResult Post_User_Role_Account([FromBody]LoginParam value)
        {
            LoginDataModel data = new LoginDataModel();
            if (value.username != null)
            {
                List<Role> list = new List<Role>();
                RoleModel listData = new RoleModel();
                MySqlTransaction trans = null;
                listData.Success = false;
                string sqlQuery = string.Empty;
                int profile_id = 0;
                string full_name = string.Empty;
                string photo_url = string.Empty;

                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_user_role_account", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_nric", value.username);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    Role prop = new Role();
                                    profile_id = Convert.ToInt16(dataReader["profile_id"]);
                                    full_name = dataReader["full_name"].ToString();
                                    photo_url = dataReader["photo_url"].ToString();
                                    prop.user_role_id = Convert.ToInt16(dataReader["user_role_id"]);
                                    prop.user_role = dataReader["user_role"].ToString();
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "select_roles";
                                listData.Message = "Please select your roles.";
                                listData.profile_id = profile_id;
                                listData.full_name = full_name;
                                listData.photo_url = photo_url;
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "No record found. Please contact our customer support.";
                                listData.Data = list;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        //listData.Success = false;
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "user/roles-account");
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

                if (listData.Success == true)
                    return Ok(listData);

                return Ok(data);
            }
            else {
                return BadRequest("Missing parameters.");
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/user/profile")]
        public IHttpActionResult Post_User_Profile([FromBody]ProfileParam value)
        {
            UserDataModel data = new UserDataModel();
            if (value.profile_id != 0)
            {
                List<Profile> list = new List<Profile>();
                ProfileModel listData = new ProfileModel();
                MySqlTransaction trans = null;
                listData.Success = false;
                string sqlQuery = string.Empty;
                string status_Code = string.Empty;
                string password = string.Empty;

                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_user_profile", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    Profile prop = new Profile();
                                    prop.profile_id = Convert.ToInt16(dataReader["profile_id"]);
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.photo_url = dataReader["photo_url"].ToString();
                                    prop.mobile_number = dataReader["mobile_number"].ToString();
                                    prop.email = dataReader["email"].ToString();
                                    prop.card_type_id = dataReader["card_type_id"] as int? ?? default(int);
                                    prop.card_type = dataReader["card_type"].ToString();
                                    prop.nric = dataReader["nric"].ToString();
                                    if (dataReader["date_of_birth"] == DBNull.Value)
                                    {
                                        prop.date_of_birth = string.Empty;
                                    }
                                    else {
                                        prop.date_of_birth = Convert.ToDateTime(dataReader["date_of_birth"]).ToString("dd-MM-yyyy");
                                    }
                                    prop.user_race_id = dataReader["user_race_id"] as int? ?? default(int);
                                    prop.user_race = dataReader["user_race"].ToString();
                                    prop.address = dataReader["address"].ToString();
                                    prop.city = dataReader["city"].ToString();
                                    prop.postcode = dataReader["postcode"].ToString();
                                    prop.state_id = dataReader["state_id"] as int? ?? default(int);
                                    prop.state_name = dataReader["state_name"].ToString();
                                    prop.country_id = dataReader["country_id"] as int? ?? default(int);
                                    prop.country_name = dataReader["country_name"].ToString();
                                    if (dataReader["mother_maiden_name"] != DBNull.Value)
                                    {
                                        prop.mother_maiden_name = dataReader["mother_maiden_name"].ToString();
                                    }
                                    else 
                                    {
                                        prop.mother_maiden_name = "";
                                    }
                                    if (dataReader["occupation"] != DBNull.Value)
                                    {
                                        prop.occupation = dataReader["occupation"].ToString();
                                    }
                                    else
                                    {
                                        prop.occupation = "";
                                    }
                                    if (dataReader["employer_name"] != DBNull.Value)
                                    {
                                        prop.employer_name = dataReader["employer_name"].ToString();
                                    }
                                    else
                                    {
                                        prop.employer_name = "";
                                    }
                                    if (dataReader["status_code"].ToString() == "Aktif")
                                    {
                                        listData.Code = "active_profile";
                                        listData.Message = "Profile is active.";
                                    }
                                    else 
                                    {
                                        listData.Code = "inactive_profile";
                                        listData.Message = "Profile is not active.";
                                    }
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Profile could not be found.";
                                listData.Data = list;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //listData.Success = false;
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "user/profile");
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

                if (listData.Success == true)
                    return Ok(listData);

                return Ok(data);
            }
            else {
                return BadRequest("Missing parameters.");
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/user/update-device-token")]
        public IHttpActionResult Post_User_Update_Device_Token([FromBody]UpdateDeviceToken value)
        {
            UserDataModel data = new UserDataModel();
            if (value.profile_id != 0 && value.device_token != null && value.device_platform_id != 0 && value.update_by != null)
            {

                MySqlTransaction trans = null;
                data.Success = false;
                string sqlQuery = string.Empty;

                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd = new MySqlCommand("sp_update_user_device_token", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                            cmd.Parameters.AddWithValue("@p_device_token", value.device_token);
                            cmd.Parameters.AddWithValue("@p_device_platform_id", value.device_platform_id);
                            cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            data.Success = true;
                            data.Code = "update_success";
                            data.Message = "Device token updated successfully.";
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "user/update-device-token");
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

                return Ok(data);
            }
            else
            {
                return BadRequest("Missing parameters.");
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/user/update-profile")]
        public IHttpActionResult Post_User_Update_Profiles(string culture, [FromBody]UpdateProfileParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            UserDataModel data = new UserDataModel();
            if (value.profile_id != 0 && value.address != null && value.card_type_id != 0 && value.city != null && value.country_id != 0 && value.date_of_birth != null &&
                value.full_name != null && value.nric != null && value.postcode != null && value.state_id != 0 &&
                value.update_by != null && value.user_race_id != 0)
            {
                MySqlTransaction trans = null;
                data.Success = false;
                string sqlQuery = string.Empty;

                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd = new MySqlCommand("sp_update_user_profile", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                            cmd.Parameters.AddWithValue("@p_full_name", value.full_name);
                            cmd.Parameters.AddWithValue("@p_user_race_id", value.user_race_id);
                            cmd.Parameters.AddWithValue("@p_card_type_id", value.card_type_id);
                            cmd.Parameters.AddWithValue("@p_nric", value.nric);
                            cmd.Parameters.AddWithValue("@p_date_of_birth", value.date_of_birth);
                            cmd.Parameters.AddWithValue("@p_mobile_number", value.mobile_number);
                            cmd.Parameters.AddWithValue("@p_email", value.email);
                            cmd.Parameters.AddWithValue("@p_address", value.address);
                            cmd.Parameters.AddWithValue("@p_postcode", value.postcode);
                            cmd.Parameters.AddWithValue("@p_city", value.city);
                            cmd.Parameters.AddWithValue("@p_state_id", value.state_id);
                            cmd.Parameters.AddWithValue("@p_country_id", value.country_id);
                            cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            data.Success = true;
                            data.Code = "update_success";
                            data.Message = WebApiResources.ProfileUpdateSuccess;
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "user/update-profile");
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

                return Ok(data);
            }
            else
            {
                return BadRequest("Missing parameters.");
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/user/update-photo")]
        public IHttpActionResult Post_User_Update_Photo([FromBody]UpdatePhotoParam value)
        {
            FileTransferProtocol ftp = new FileTransferProtocol();
            UserDataModel data = new UserDataModel();
            if (value.file_name != null && value.photo_base64 != null && value.profile_id != 0 && value.update_by != null)
            {
                MySqlTransaction trans = null;
                data.Success = false;
                string sqlQuery = string.Empty;
                bool upload = false;

                string ftp_address = ConfigurationManager.AppSettings["ftpAddress"];
                string ftp_username = ConfigurationManager.AppSettings["ftpUName"];
                string ftp_password = ConfigurationManager.AppSettings["ftpPWord"];
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                string photo_url = "/images/profiles/" + value.profile_id + "/" + value.file_name;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd = new MySqlCommand("sp_update_user_photo", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                            cmd.Parameters.AddWithValue("@p_photo_url", photo_url);
                            cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (value.file_name != null && value.photo_base64 != null)
                            {
                                var inputText = ValidateBase64EncodedString(value.photo_base64);
                                byte[] fileBytes = Convert.FromBase64String(inputText);
                                string directory = "/images/profiles/" + value.profile_id;

                                if (ftp.Check_Directory_Exists(ftp_address, directory, ftp_username, ftp_password) == true)
                                {
                                    if (ftp.Retrieve_Delete_Directory_File(ftp_address,directory,ftp_username, ftp_password) == true)
                                    {
                                        try
                                        {
                                            //Create FTP Request.
                                            FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/profiles/" + value.profile_id + "/" + value.file_name);
                                            ftprequest.Method = WebRequestMethods.Ftp.UploadFile;

                                            //Enter FTP Server credentials.
                                            ftprequest.Credentials = new NetworkCredential(ftp_username, ftp_password);
                                            ftprequest.ContentLength = fileBytes.Length;
                                            ftprequest.UsePassive = true;
                                            ftprequest.UseBinary = true;
                                            ftprequest.ServicePoint.ConnectionLimit = fileBytes.Length;
                                            ftprequest.EnableSsl = false;

                                            using (Stream requestStream = ftprequest.GetRequestStream())
                                            {
                                                requestStream.Write(fileBytes, 0, fileBytes.Length);
                                                requestStream.Close();
                                            }

                                            FtpWebResponse ftpresponse = (FtpWebResponse)ftprequest.GetResponse();
                                            ftpresponse.Close();
                                            upload = true;
                                        }
                                        catch (WebException ex)
                                        {
                                            throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
                                        }
                                    }
                                }
                                else {
                                    if (ftp.Create_Directory(ftp_address, directory, ftp_username, ftp_password) == true)
                                    {
                                        if (ftp.Check_Directory_Exists(ftp_address, directory, ftp_username, ftp_password) == true)
                                        {
                                            if (ftp.Retrieve_Delete_Directory_File(ftp_address, directory, ftp_username, ftp_password) == true)
                                            {
                                                try
                                                {
                                                    //Create FTP Request.
                                                    FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/profiles/" + value.profile_id + "/" + value.file_name);
                                                    ftprequest.Method = WebRequestMethods.Ftp.UploadFile;

                                                    //Enter FTP Server credentials.
                                                    ftprequest.Credentials = new NetworkCredential(ftp_username, ftp_password);
                                                    ftprequest.ContentLength = fileBytes.Length;
                                                    ftprequest.UsePassive = true;
                                                    ftprequest.UseBinary = true;
                                                    ftprequest.ServicePoint.ConnectionLimit = fileBytes.Length;
                                                    ftprequest.EnableSsl = false;

                                                    using (Stream requestStream = ftprequest.GetRequestStream())
                                                    {
                                                        requestStream.Write(fileBytes, 0, fileBytes.Length);
                                                        requestStream.Close();
                                                    }

                                                    FtpWebResponse ftpresponse = (FtpWebResponse)ftprequest.GetResponse();
                                                    ftpresponse.Close();
                                                    upload = true;
                                                }
                                                catch (WebException ex)
                                                {
                                                    throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
                                                }
                                            }
                                        }
                                    }
                                }

                                if (upload == true)
                                {
                                    dataReader.Close();
                                    trans.Commit();

                                    data.Success = true;
                                    data.Code = "update_success";
                                    data.Message = "Photo updated successfully.";
                                }
                            }
                            else
                            {
                                return BadRequest("Missing parameters.");
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "user/update-photo");
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

                return Ok(data);
            }
            else
            {
                return BadRequest("Missing parameters.");
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/user/remove-photo")]
        public IHttpActionResult Post_User_Remove_Photo([FromBody] RemovePhotoParam value)
        {
            FileTransferProtocol ftp = new FileTransferProtocol();
            UserDataModel data = new UserDataModel();
            if (value.profile_id != 0 && value.update_by != null)
            {
                MySqlTransaction trans = null;
                data.Success = false;

                string ftp_address = ConfigurationManager.AppSettings["ftpAddress"];
                string ftp_username = ConfigurationManager.AppSettings["ftpUName"];
                string ftp_password = ConfigurationManager.AppSettings["ftpPWord"];
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd = new MySqlCommand("sp_delete_user_photo", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                            cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            string directory = "/images/profiles/" + value.profile_id;

                            if (ftp.Check_Directory_Exists(ftp_address, directory, ftp_username, ftp_password) == true)
                            {
                                if (ftp.Retrieve_Delete_Directory_File(ftp_address, directory, ftp_username, ftp_password) == true)
                                {
                                    dataReader.Close();
                                    trans.Commit();

                                    data.Success = true;
                                    data.Code = "delete_success";
                                    data.Message = "Photo successfully removed.";
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "user/remove-photo");
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

                return Ok(data);
            }
            else
            {
                return BadRequest("Missing parameters.");
            }
        }

        [HttpPost]
        [Route("api/v2/user/upload-image")]
        public IHttpActionResult Post_User_Upload_Image(string culture, [FromBody] UploadPhotoParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            string salt = ConfigurationManager.AppSettings["passPhrase"];
            FileTransferProtocol ftp = new FileTransferProtocol();
            UserDataModel data = new UserDataModel();
            if (value.identity_number != null && value.file_name != null && value.photo_base64 != null && value.image_type_id != 0 && value.create_by != null)
            {
                MySqlTransaction trans = null;
                data.Success = false;
                string sqlQuery = string.Empty;
                bool upload = false;

                string ftp_address = ConfigurationManager.AppSettings["ftpAddress"];
                string ftp_username = ConfigurationManager.AppSettings["ftpUName"];
                string ftp_password = ConfigurationManager.AppSettings["ftpPWord"];
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                Guid guid = Guid.NewGuid();
                string newFolder = auth.EncryptRijndael(check.RemoveNonAlphaNumeric(value.identity_number), salt);
                newFolder = newFolder.Replace("+", "");
                newFolder = newFolder.Replace("=", "");
                newFolder = newFolder.Replace("/", "");

                string photo_url = "/images/temps/" + newFolder + "/" + value.image_type_id + "/" + value.file_name;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd = new MySqlCommand("sp_insert_image_temp", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_identity_number", check.RemoveNonAlphaNumeric(value.identity_number));
                            cmd.Parameters.AddWithValue("@p_image_type_id", value.image_type_id);
                            cmd.Parameters.AddWithValue("@p_image_url", photo_url);
                            cmd.Parameters.AddWithValue("@p_create_by", value.create_by);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (value.file_name != null && value.photo_base64 != null)
                            {
                                var inputText = ValidateBase64EncodedString(value.photo_base64);
                                byte[] fileBytes = Convert.FromBase64String(inputText);

                                string directory = "/images/temps/" + newFolder + "/" + value.image_type_id;

                                if (ftp.Check_Directory_Exists(ftp_address, directory, ftp_username, ftp_password) == true)
                                {
                                    if (ftp.Retrieve_Delete_Directory_File(ftp_address, directory, ftp_username, ftp_password) == true)
                                    {
                                        try
                                        {
                                            //Create FTP Request.
                                            FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/temps/" + newFolder + "/" + value.image_type_id + "/" + value.file_name);
                                            ftprequest.Method = WebRequestMethods.Ftp.UploadFile;

                                            //Enter FTP Server credentials.
                                            ftprequest.Credentials = new NetworkCredential(ftp_username, ftp_password);
                                            ftprequest.ContentLength = fileBytes.Length;
                                            ftprequest.UsePassive = true;
                                            ftprequest.UseBinary = true;
                                            ftprequest.ServicePoint.ConnectionLimit = fileBytes.Length;
                                            ftprequest.EnableSsl = false;

                                            using (Stream requestStream = ftprequest.GetRequestStream())
                                            {
                                                requestStream.Write(fileBytes, 0, fileBytes.Length);
                                                requestStream.Close();
                                            }

                                            FtpWebResponse ftpresponse = (FtpWebResponse)ftprequest.GetResponse();
                                            ftpresponse.Close();
                                            upload = true;
                                        }
                                        catch (WebException ex)
                                        {
                                            throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
                                        }
                                    }
                                }
                                else
                                {
                                    string folder_idno = "/images/temps/" + newFolder;
                                    string folder_image_type = "/images/temps/" + newFolder + "/" + value.image_type_id;

                                    if (ftp.Check_Directory_Exists(ftp_address, folder_idno, ftp_username, ftp_password) == true) //idno
                                    {
                                        if (ftp.Check_Directory_Exists(ftp_address, folder_image_type, ftp_username, ftp_password) == true) //image type
                                        {
                                            if (ftp.Retrieve_Delete_Directory_File(ftp_address, folder_image_type, ftp_username, ftp_password) == true)
                                            {
                                                try
                                                {
                                                    //Create FTP Request.
                                                    FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/temps/" + newFolder + "/" + value.image_type_id + "/" + value.file_name);
                                                    ftprequest.Method = WebRequestMethods.Ftp.UploadFile;

                                                    //Enter FTP Server credentials.
                                                    ftprequest.Credentials = new NetworkCredential(ftp_username, ftp_password);
                                                    ftprequest.ContentLength = fileBytes.Length;
                                                    ftprequest.UsePassive = true;
                                                    ftprequest.UseBinary = true;
                                                    ftprequest.ServicePoint.ConnectionLimit = fileBytes.Length;
                                                    ftprequest.EnableSsl = false;

                                                    using (Stream requestStream = ftprequest.GetRequestStream())
                                                    {
                                                        requestStream.Write(fileBytes, 0, fileBytes.Length);
                                                        requestStream.Close();
                                                    }

                                                    FtpWebResponse ftpresponse = (FtpWebResponse)ftprequest.GetResponse();
                                                    ftpresponse.Close();
                                                    upload = true;
                                                }
                                                catch (WebException ex)
                                                {
                                                    throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
                                                }
                                            }

                                        }
                                        else
                                        {
                                            if (ftp.Create_Directory(ftp_address, folder_image_type, ftp_username, ftp_password) == true) // image type
                                            {
                                                if (ftp.Check_Directory_Exists(ftp_address, folder_image_type, ftp_username, ftp_password) == true)
                                                {
                                                    try
                                                    {
                                                        //Create FTP Request.
                                                        FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/temps/" + newFolder + "/" + value.image_type_id + "/" + value.file_name);
                                                        ftprequest.Method = WebRequestMethods.Ftp.UploadFile;

                                                        //Enter FTP Server credentials.
                                                        ftprequest.Credentials = new NetworkCredential(ftp_username, ftp_password);
                                                        ftprequest.ContentLength = fileBytes.Length;
                                                        ftprequest.UsePassive = true;
                                                        ftprequest.UseBinary = true;
                                                        ftprequest.ServicePoint.ConnectionLimit = fileBytes.Length;
                                                        ftprequest.EnableSsl = false;

                                                        using (Stream requestStream = ftprequest.GetRequestStream())
                                                        {
                                                            requestStream.Write(fileBytes, 0, fileBytes.Length);
                                                            requestStream.Close();
                                                        }

                                                        FtpWebResponse ftpresponse = (FtpWebResponse)ftprequest.GetResponse();
                                                        ftpresponse.Close();
                                                        upload = true;
                                                    }
                                                    catch (WebException ex)
                                                    {
                                                        throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (ftp.Create_Directory(ftp_address, folder_idno, ftp_username, ftp_password) == true) //idno
                                        {
                                            if (ftp.Check_Directory_Exists(ftp_address, folder_image_type, ftp_username, ftp_password) == true)
                                            {
                                                try
                                                {
                                                    //Create FTP Request.
                                                    FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/temps/" + newFolder + "/" + value.image_type_id + "/" + value.file_name);
                                                    ftprequest.Method = WebRequestMethods.Ftp.UploadFile;

                                                    //Enter FTP Server credentials.
                                                    ftprequest.Credentials = new NetworkCredential(ftp_username, ftp_password);
                                                    ftprequest.ContentLength = fileBytes.Length;
                                                    ftprequest.UsePassive = true;
                                                    ftprequest.UseBinary = true;
                                                    ftprequest.ServicePoint.ConnectionLimit = fileBytes.Length;
                                                    ftprequest.EnableSsl = false;

                                                    using (Stream requestStream = ftprequest.GetRequestStream())
                                                    {
                                                        requestStream.Write(fileBytes, 0, fileBytes.Length);
                                                        requestStream.Close();
                                                    }

                                                    FtpWebResponse ftpresponse = (FtpWebResponse)ftprequest.GetResponse();
                                                    ftpresponse.Close();
                                                    upload = true;
                                                }
                                                catch (WebException ex)
                                                {
                                                    throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
                                                }
                                            }
                                            else
                                            {
                                                if (ftp.Create_Directory(ftp_address, folder_image_type, ftp_username, ftp_password) == true)
                                                {
                                                    if (ftp.Check_Directory_Exists(ftp_address, folder_image_type, ftp_username, ftp_password) == true)
                                                    {
                                                        try
                                                        {
                                                            //Create FTP Request.
                                                            FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/temps/" + newFolder + "/" + value.image_type_id + "/" + value.file_name);
                                                            ftprequest.Method = WebRequestMethods.Ftp.UploadFile;

                                                            //Enter FTP Server credentials.
                                                            ftprequest.Credentials = new NetworkCredential(ftp_username, ftp_password);
                                                            ftprequest.ContentLength = fileBytes.Length;
                                                            ftprequest.UsePassive = true;
                                                            ftprequest.UseBinary = true;
                                                            ftprequest.ServicePoint.ConnectionLimit = fileBytes.Length;
                                                            ftprequest.EnableSsl = false;

                                                            using (Stream requestStream = ftprequest.GetRequestStream())
                                                            {
                                                                requestStream.Write(fileBytes, 0, fileBytes.Length);
                                                                requestStream.Close();
                                                            }

                                                            FtpWebResponse ftpresponse = (FtpWebResponse)ftprequest.GetResponse();
                                                            ftpresponse.Close();
                                                            upload = true;
                                                        }
                                                        catch (WebException ex)
                                                        {
                                                            throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                if (upload == true)
                                {
                                    dataReader.Close();
                                    trans.Commit();

                                    data.Success = true;
                                    data.Code = "upload_success";
                                    data.Message = WebApiResources.ImageUploadSuccess;
                                }
                            }
                            else
                            {
                                return BadRequest("Missing parameters.");
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "user/upload-image");
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

                return Ok(data);
            }
            else
            {
                return BadRequest("Missing parameters.");
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/user/notify")]
        public IHttpActionResult Post_User_Notify([FromBody] SystemNotifyParam value)
        {
            LoginDataModel data = new LoginDataModel();
            if (value.profile_id != 0)
            {
                List<SystemNotify> list = new List<SystemNotify>();
                SystemNotifyModel listData = new SystemNotifyModel();
                MySqlTransaction trans = null;
                listData.Success = false;

                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_system_notify", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    SystemNotify prop = new SystemNotify();
                                    prop.notify_id = Convert.ToInt32(dataReader["notify_id"]);
                                    prop.notify_subject = dataReader["notify_subject"].ToString();
                                    prop.notify_message = dataReader["notify_message"].ToString();
                                    prop.notify_photo_url = dataReader["notify_photo_url"].ToString();
                                    prop.notify_link = dataReader["notify_link"].ToString();
                                    prop.notify_link_text = dataReader["notify_link_text"].ToString();
                                    prop.notify_link_param = dataReader["notify_link_param"].ToString();
                                    prop.read_flag = dataReader["read_flag"].ToString();
                                    prop.create_at = Convert.ToDateTime(dataReader["create_at"]).ToString("yyyy-MM-dd");
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Message = "System notification list.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Message = "No record found.";
                                listData.Data = list;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        //listData.Success = false;
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "user/notify");
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

                if (listData.Success == true)
                    return Ok(listData);

                return Ok(data);
            }
            else
            {
                return BadRequest("Missing parameters.");
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/user/update-notify")]
        public IHttpActionResult Post_User_Update_Notify([FromBody] UpdateSystemNotifyParam value)
        {
            UserDataModel data = new UserDataModel();
            if (value.profile_id != 0 &&  value.notify_id != 0 && value.update_by != null)
            {

                MySqlTransaction trans = null;
                data.Success = false;
                string sqlQuery = string.Empty;

                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd = new MySqlCommand("sp_update_system_notify", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_notify_id", value.notify_id);
                            cmd.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                            cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            data.Success = true;
                            data.Code = "update_success";
                            data.Message = "System notify successfully updated.";
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "user/update-notify");
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

                return Ok(data);
            }
            else
            {
                return BadRequest("Missing parameters.");
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/user/delete-notify")]
        public IHttpActionResult Post_User_Delete_Notify([FromBody] UpdateSystemNotifyParam value)
        {
            UserDataModel data = new UserDataModel();
            if (value.profile_id != 0 && value.notify_id != 0 && value.update_by != null)
            {

                MySqlTransaction trans = null;
                data.Success = false;
                string sqlQuery = string.Empty;

                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd = new MySqlCommand("sp_delete_system_notify", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_notify_id", value.notify_id);
                            cmd.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                            cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            data.Success = true;
                            data.Code = "delete_success";
                            data.Message = "System notify successfully deleted.";
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "user/delete-notify");
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

                return Ok(data);
            }
            else
            {
                return BadRequest("Missing parameters.");
            }
        }

        private static string ValidateBase64EncodedString(string inputText)
        {
            string stringToValidate = inputText;
            stringToValidate = stringToValidate.Replace('-', '+'); // 62nd char of encoding
            stringToValidate = stringToValidate.Replace('_', '/'); // 63rd char of encoding
            switch (stringToValidate.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 2: stringToValidate += "=="; break; // Two pad chars
                case 3: stringToValidate += "="; break; // One pad char
                default:
                    throw new System.Exception(
             "Illegal base64url string!");
            }

            return stringToValidate;
        }
    }
}
