using I3SwebAPIv2.Class;
using I3SwebAPIv2.Models;
using I3SwebAPIv2.Resources;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace I3SwebAPIv2.Controllers
{
    public class AccountV2Controller : ApiController
    {
        AccountV2Class cls = new AccountV2Class();
        MPayWallet mpay = new MPayWallet();
        i3sAuth.Rijndael auth = new i3sAuth.Rijndael();
        DataChecker check = new DataChecker();
        public string salt = ConfigurationManager.AppSettings["passPhrase"];
        public string timestamp = "";
        #region EKYC
        //[HttpPost]
        //[Route("api/v2/account/register")]
        //public async Task<IHttpActionResult> Post_Account_Register(string culture, [FromBody] RegisterAccountParam value)
        //{
        //    if (!string.IsNullOrEmpty(culture))
        //    {
        //        var language = new CultureInfo(culture);
        //        Thread.CurrentThread.CurrentUICulture = language;
        //    }

        //    //DateTime dob = new DateTime();
        //    MOcean ocean = new MOcean();
        //    UsersV2Class cls = new UsersV2Class();

        //    RegisterAccountDataModel data = new RegisterAccountDataModel();
        //    RegisterAccountDataListModel listData = new RegisterAccountDataListModel();
        //    List<RegisterAccountData> list = new List<RegisterAccountData>();

        //    if (value.full_name != null && value.id_number != null && value.mobile_number != null && value.email != null && value.id_type != 0 && value.nationality_id != 0)
        //    {
        //        //check idenity number
        //        if (check.IsValidIdNumber(check.RemoveNonAlphaNumeric(value.id_number)) == true)
        //        {
        //            if (check.IsMobileNumber(value.mobile_number) == true)
        //            {

        //                if (check.IsValidEmail(value.email) == true)
        //                {
        //                    string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

        //                    //generate otp
        //                    PasswordGenerator generate = new PasswordGenerator().IncludeNumeric().LengthRequired(6);
        //                    string otp = generate.Next();

        //                    using (MySqlConnection conn = new MySqlConnection(constr))
        //                    {
        //                        conn.Open();

        //                        using (MySqlCommand cmd = new MySqlCommand("sp_insert_register_account_log", conn))
        //                        {
        //                            MySqlTransaction trans;
        //                            trans = conn.BeginTransaction();

        //                            try
        //                            {
        //                                cmd.Transaction = trans;
        //                                cmd.CommandType = CommandType.StoredProcedure;
        //                                cmd.Parameters.Clear();
        //                                cmd.Parameters.AddWithValue("@p_full_name", value.full_name);
        //                                cmd.Parameters.AddWithValue("@p_id_type", value.id_type);
        //                                cmd.Parameters.AddWithValue("@p_nationality_id", value.nationality_id);
        //                                cmd.Parameters.AddWithValue("@p_id_number", check.RemoveNonAlphaNumeric(value.id_number));
        //                                cmd.Parameters.AddWithValue("@p_mobile_number", value.mobile_number);
        //                                cmd.Parameters.AddWithValue("@p_email", value.email);
        //                                cmd.Parameters.AddWithValue("@p_otp", auth.EncryptRijndael(otp, salt));
        //                                cmd.Parameters.Add("@p_account_id", MySqlDbType.Int16);
        //                                cmd.Parameters["@p_account_id"].Direction = ParameterDirection.Output;
        //                                cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
        //                                cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
        //                                cmd.ExecuteNonQuery();
        //                                trans.Commit();

        //                                int account_id = Convert.ToInt32(cmd.Parameters["@p_account_id"].Value);
        //                                string status_code = cmd.Parameters["@p_status_code"].Value.ToString();

        //                                RegisterAccountData prop = new RegisterAccountData();
        //                                prop.account_id = account_id;
        //                                list.Add(prop);

        //                                if (status_code == "verify_account")
        //                                {
        //                                    try
        //                                    {
        //                                        //var result = ocean.Send_OTP(otp, value.mobile_number);
        //                                        var result = ocean.Send_OTP(otp, "+60169050115"); // for testing only
        //                                        //var result = ocean.Send_OTP(otp, "+601157746255"); // for testing only
        //                                        string jsonStr = await result;
        //                                        MoceanModel json = JsonConvert.DeserializeObject<MoceanModel>(jsonStr);

        //                                        foreach (Mocean item in json.messages)
        //                                        {
        //                                            if (item.status != "0")
        //                                            {
        //                                                data.Success = false;
        //                                                data.Code = "error_occured";
        //                                                data.Message = item.err_msg;
        //                                            }
        //                                            else
        //                                            {
        //                                                string front = string.Empty;
        //                                                int m = value.mobile_number.Length - 3;
        //                                                for (int n = 0; n < m; n++)
        //                                                {
        //                                                    front += "X";
        //                                                }

        //                                                string last_four = value.mobile_number.Substring(value.mobile_number.Length - 4, 4);
        //                                                data.Success = true;
        //                                                listData.Success = true;
        //                                                listData.Code = "verify_account";
        //                                                listData.Message = WebApiResources.OTPHasBeenSent + value.mobile_number;
        //                                                listData.Data = list;
        //                                            }
        //                                        }
        //                                    }
        //                                    catch (Exception ex)
        //                                    {
        //                                        data.Success = false;
        //                                        data.Code = "error_occured";
        //                                        data.Message = WebApiResources.ErrorOccured;
        //                                        ExceptionUtility.LogException(ex, "account/register");
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    data.Success = true;
        //                                    listData.Success = true;
        //                                    listData.Code = "proceed_ekyc";
        //                                    listData.Message = WebApiResources.PleaseUploadYourImage;
        //                                    listData.Data = list;
        //                                }

        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                data.Success = false;
        //                                data.Code = "error_occured";
        //                                data.Message = WebApiResources.ErrorOccured;
        //                                ExceptionUtility.LogException(ex, "account/register");
        //                            }
        //                        }
        //                    }
        //                }
        //                else 
        //                {
        //                    data.Success = false;
        //                    data.Code = "invalid_email_format";
        //                    data.Message = WebApiResources.EmailInvalid;
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

        //        if (data.Success == true)
        //            return Ok(listData);

        //        return Ok(data);
        //    }
        //    else
        //    {
        //        return BadRequest("Missing parameters.");
        //    }

        //}

        [HttpPost]
        [Authorize]
        [Route("api/v2/account/check-pin")]
        public string Post_Account_Check_Pin(int mode, [FromBody] PasswordCheckParam value)
        {
            string pin = string.Empty;
            if (mode == 1)
            {
                pin = auth.EncryptRijndael(value.password, salt);
            }
            else
            {
                pin = auth.DecryptRijndael(value.password, salt);
            }

            return pin;
        }

        [HttpPost]
        [Route("api/v2/account/register")]
        public async Task<IHttpActionResult> Post_Account_Register(string culture, [FromBody] RegisterAccountParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            DateTime dob = new DateTime();
            MOcean ocean = new MOcean();
            UsersV2Class cls = new UsersV2Class();

            RegisterAccountDataModel data = new RegisterAccountDataModel();
            RegisterAccountDataListModel listData = new RegisterAccountDataListModel();
            //List<RegisterAccountData> list = new List<RegisterAccountData>();

            if (value.user_role_id != 0 && !string.IsNullOrEmpty(value.full_name) && !string.IsNullOrEmpty(value.id_number) && !string.IsNullOrEmpty(value.mobile_number) && !string.IsNullOrEmpty(value.email) 
                && value.id_type != 0 && value.nationality_id != 0 && !string.IsNullOrEmpty(value.password))
            {
                //check identity number
                if (check.IsValidIdNumber(check.RemoveNonAlphaNumeric(value.id_number)) == true)
                {
                    if (check.IsMobileNumber(value.mobile_number) == true)
                    {
                        if (check.IsValidEmail(value.email) == true)
                        {
                            if (value.id_type == 1)
                            {
                                int curYear = Convert.ToInt32(DateTime.Now.ToString("yy"));
                                int year = Convert.ToInt32(value.id_number.Substring(0, 2));
                                if (curYear <= year)
                                {
                                    year = 1900 + year;
                                }
                                else
                                {
                                    year = 2000 + year;
                                }
                                int month = Convert.ToInt32(value.id_number.Substring(2, 2));
                                int day = Convert.ToInt32(value.id_number.Substring(4, 2));

                                dob = new DateTime(year, month, day);
                            }

                            string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                            //generate otp
                            PasswordGenerator generate = new PasswordGenerator().IncludeNumeric().LengthRequired(6);
                            string otp = generate.Next();

                            using (MySqlConnection conn = new MySqlConnection(constr))
                            {
                                conn.Open();

                                using (MySqlCommand cmd = new MySqlCommand("sp_insert_user_self_register", conn))
                                {
                                    MySqlTransaction trans;
                                    MySqlTransaction trans2;
                                    trans = conn.BeginTransaction();
                                    cmd.Transaction = trans;

                                    try
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Clear();
                                        cmd.Parameters.AddWithValue("@p_full_name", value.full_name);
                                        cmd.Parameters.AddWithValue("@p_nationality_id", value.nationality_id);
                                        cmd.Parameters.AddWithValue("@p_mother_maiden_name", string.Empty);
                                        cmd.Parameters.AddWithValue("@p_card_type_id", value.id_type);
                                        cmd.Parameters.AddWithValue("@p_identity_number", check.RemoveNonAlphaNumeric(value.id_number));
                                        if (value.nationality_id == 130)
                                        {
                                            cmd.Parameters.AddWithValue("@p_date_of_birth", dob.ToString("yyyy-MM-dd"));
                                        }
                                        else
                                        {
                                            cmd.Parameters.AddWithValue("@p_date_of_birth", value.date_of_birth.Split('-')[2] + "-" + value.date_of_birth.Split('-')[1] + "-" + value.date_of_birth.Split('-')[0]);
                                        }
                                        cmd.Parameters.AddWithValue("@p_mobile_number", value.mobile_number);
                                        cmd.Parameters.AddWithValue("@p_email", value.email);
                                        cmd.Parameters.AddWithValue("@p_address", string.Empty);
                                        cmd.Parameters.AddWithValue("@p_postcode", string.Empty);
                                        cmd.Parameters.AddWithValue("@p_state_id", null);
                                        cmd.Parameters.AddWithValue("@p_city", string.Empty);
                                        cmd.Parameters.AddWithValue("@p_country_id", null);
                                        cmd.Parameters.AddWithValue("@p_password", auth.EncryptRijndael(value.password, salt));
                                        cmd.Parameters.AddWithValue("@p_otp", auth.EncryptRijndael(otp, salt));
                                        cmd.Parameters.AddWithValue("@p_occupation", string.Empty);
                                        cmd.Parameters.AddWithValue("@p_employer_name", string.Empty);
                                        cmd.Parameters.AddWithValue("@p_marketing_flag", value.marketing_flag);
                                        cmd.Parameters.Add("@p_profile_id", MySqlDbType.Int16);
                                        cmd.Parameters["@p_profile_id"].Direction = ParameterDirection.Output;
                                        cmd.Parameters.Add("@p_user_status", MySqlDbType.Int16);
                                        cmd.Parameters["@p_user_status"].Direction = ParameterDirection.Output;
                                        cmd.ExecuteNonQuery();
                                        trans.Commit();

                                        int profile_id = Convert.ToInt32(cmd.Parameters["@p_profile_id"].Value);
                                        int user_status_id = Convert.ToInt32(cmd.Parameters["@p_user_status"].Value);

                                        string wallet_number = cls.GenerateWalletNumber();
                                        string token = cls.GenerateToken();

                                        if (value.user_role_id == 9)
                                        {
                                            using (MySqlCommand cmd2 = new MySqlCommand("sp_insert_parent_self_register", conn))
                                            {
                                                trans2 = conn.BeginTransaction();
                                                cmd2.Transaction = trans2;

                                                try
                                                {
                                                    cmd2.CommandType = CommandType.StoredProcedure;
                                                    cmd2.Parameters.Clear();
                                                    cmd2.Parameters.AddWithValue("@p_profile_id", profile_id);
                                                    cmd2.Parameters.AddWithValue("@p_wallet_number", wallet_number);
                                                    cmd2.Parameters.AddWithValue("@p_token", token);
                                                    cmd2.Parameters.AddWithValue("@p_create_by", value.full_name);
                                                    cmd2.Parameters.Add("@p_parent_exists", MySqlDbType.Int16);
                                                    cmd2.Parameters["@p_parent_exists"].Direction = ParameterDirection.Output;
                                                    cmd2.Parameters.Add("@p_parent_id", MySqlDbType.Int16);
                                                    cmd2.Parameters["@p_parent_id"].Direction = ParameterDirection.Output;
                                                    cmd2.Parameters.Add("@p_wallet_id", MySqlDbType.Int16);
                                                    cmd2.Parameters["@p_wallet_id"].Direction = ParameterDirection.Output;
                                                    cmd2.Parameters.Add("@p_wallet_exists", MySqlDbType.Int16);
                                                    cmd2.Parameters["@p_wallet_exists"].Direction = ParameterDirection.Output;
                                                    cmd2.Parameters.Add("@p_parent_status", MySqlDbType.Int16);
                                                    cmd2.Parameters["@p_parent_status"].Direction = ParameterDirection.Output;
                                                    cmd2.ExecuteNonQuery();
                                                    trans2.Commit();

                                                    int parent_exists = Convert.ToInt32(cmd2.Parameters["@p_parent_exists"].Value);
                                                    int parent_id = Convert.ToInt32(cmd2.Parameters["@p_parent_id"].Value);
                                                    int wallet_id = Convert.ToInt32(cmd2.Parameters["@p_wallet_id"].Value);
                                                    int wallet_exists = Convert.ToInt32(cmd2.Parameters["@p_wallet_exists"].Value);
                                                    int parent_status_id = Convert.ToInt32(cmd2.Parameters["@p_parent_status"].Value);

                                                    if (parent_status_id == 2)
                                                    {
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
                                                                    int m = value.mobile_number.Length - 3;
                                                                    for (int n = 0; n < m; n++)
                                                                    {
                                                                        front += "X";
                                                                    }

                                                                    if (wallet_exists == 0)
                                                                    {
                                                                        cls.UpdateWalletNumber();
                                                                    }

                                                                    string last_four = value.mobile_number.Substring(value.mobile_number.Length - 4, 4);
                                                                    data.Success = true;
                                                                    data.Code = "verify_account";
                                                                    data.Message = WebApiResources.OTPHasBeenSent + value.mobile_number;
                                                                }
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            data.Success = false;
                                                            data.Code = "error_occured";
                                                            data.Message = WebApiResources.ErrorOccured;
                                                            ExceptionUtility.LogException(ex, "account/register");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        data.Success = false;
                                                        data.Code = "record_exists";
                                                        data.Message = WebApiResources.ParentIDAlreadyExist;
                                                    }

                                                }
                                                catch (Exception ex)
                                                {
                                                    trans2.Rollback();
                                                    data.Success = false;
                                                    data.Code = "error_occured";
                                                    data.Message = WebApiResources.ErrorOccured;
                                                    ExceptionUtility.LogException(ex, "account/register");
                                                }
                                            }
                                        }
                                        else
                                        {

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
                                                        int m = value.mobile_number.Length - 3;
                                                        for (int n = 0; n < m; n++)
                                                        {
                                                            front += "X";
                                                        }

                                                        string last_four = value.mobile_number.Substring(value.mobile_number.Length - 4, 4);
                                                        data.Success = true;
                                                        data.Code = "verify_account";
                                                        data.Message = WebApiResources.OTPHasBeenSent + value.mobile_number;
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                data.Success = false;
                                                data.Code = "error_occured";
                                                data.Message = WebApiResources.ErrorOccured;
                                                ExceptionUtility.LogException(ex, "account/register");
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        trans.Rollback();
                                        data.Success = false;
                                        data.Code = "error_occured";
                                        data.Message = WebApiResources.ErrorOccured;
                                        ExceptionUtility.LogException(ex, "account/register");
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
                            data.Code = "invalid_email_format";
                            data.Message = WebApiResources.EmailInvalid;
                        }
                    }
                    else
                    {
                        data.Success = false;
                        data.Code = "invalid_mobile_number";
                        data.Message = WebApiResources.MobileNoInvalid;
                    }
                }
                else
                {
                    data.Success = false;
                    data.Code = "invalid_identity_number";
                    data.Message = WebApiResources.IdentityNoInvalid;
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
        [Route("api/v2/account/add-virtual-balance")]
        public IHttpActionResult Post_Account_Add_Virtual_Balance(string culture, [FromBody] RegisterStudentParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            ParentDataModel data = new ParentDataModel();
            if (value.parent_id != 0 && value.student_id != 0 && value.school_id != 0 && value.class_id != 0 && value.create_by != null)
            {
                List<RegisterStudent> list = new List<RegisterStudent>();
                RegisterStudentModel listData = new RegisterStudentModel();
                MySqlTransaction trans = null;
                data.Success = false;
                //int uid = 0;
                string sqlQuery = string.Empty;
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd = new MySqlCommand("sp_insert_parent_add_virtual_balance", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_parent_id", value.parent_id);
                            cmd.Parameters.AddWithValue("@p_student_id", value.student_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_class_id", value.class_id);
                            cmd.Parameters.AddWithValue("@p_create_by", value.create_by);
                            cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                            cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            var status_code = (string)cmd.Parameters["@p_status_code"].Value;

                            if (status_code == "record_saved")
                            {
                                data.Success = true;
                                data.Code = status_code;
                                data.Message = WebApiResources.ParentStudentRelationshipCreateSuccess;
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = status_code;
                                data.Message = WebApiResources.ParentStudentRelationshipExist;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "account/add-virtual-balance");
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
        [Route("api/v2/account/update-virtual-balance")]
        public async Task<IHttpActionResult> Post_Account_Update_Virtual_Balance(string culture, [FromBody] ParentVirtualBalanceParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            ParentDataModel data = new ParentDataModel();
            if (value.parent_profile_id != 0 && value.student_profile_id != 0 && !string.IsNullOrEmpty(value.create_by))
            {
                AddVirtualBalanceParam prop = new AddVirtualBalanceParam();
                data.Success = false;
                int uid = 0;
                string sqlQuery = string.Empty;
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    MySqlTransaction trans = null;

                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_parent_virtual_balance", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_parent_profile_id", value.parent_profile_id);
                            cmd.Parameters.AddWithValue("@p_student_profile_id", value.student_profile_id);
                            cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                            cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd.Parameters.Add("@p_mpay_uid", MySqlDbType.Int32);
                            cmd.Parameters["@p_mpay_uid"].Direction = ParameterDirection.Output;
                            cmd.Parameters.Add("@p_wallet_id", MySqlDbType.Int32); //student wallet_id
                            cmd.Parameters["@p_wallet_id"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();

                            var status_code = (string)cmd.Parameters["@p_status_code"].Value;

                            if (status_code == "record_exists")
                            {
                                if (DBNull.Value != cmd.Parameters["@p_mpay_uid"].Value)
                                {
                                    uid = Convert.ToInt32(cmd.Parameters["@p_mpay_uid"].Value);
                                    prop.uid = uid.ToString();
                                }

                                var wallet_id = Convert.ToInt32(cmd.Parameters["@p_wallet_id"].Value);

                                var result = cls.PostAddVirtualBalance(prop);
                                string jsonStr = await result;
                                AddVirtualBalanceModel response = JsonConvert.DeserializeObject<AddVirtualBalanceModel>(jsonStr);

                                if (response.Header.status == "00")
                                {
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

                                    ChangePINParam param = new ChangePINParam();
                                    param.cardtoken = card.cardtoken;
                                    param.oldPin = card.card_temporary_pin;
                                    param.newPin = "123456";
                                    param.uid = prop.uid;

                                    var result2 = mpay.PostChangePIN(param);
                                    string jsonStr2 = await result2;

                                    ChangPINModel response2 = JsonConvert.DeserializeObject<ChangPINModel>(jsonStr2);

                                    if (response2.Header.status == "00")
                                    {
                                        BodyPin bPin = response2.Body;

                                        if (bPin.pin_change_status == "00")
                                        {
                                            using (MySqlCommand cmd2 = new MySqlCommand("sp_update_student_wallet_info", conn))
                                            {
                                                try
                                                {
                                                    cmd2.Transaction = trans;
                                                    cmd2.CommandType = CommandType.StoredProcedure;
                                                    cmd2.Parameters.AddWithValue("@p_wallet_id", wallet_id);
                                                    cmd2.Parameters.AddWithValue("@p_mpay_uid", uid);
                                                    cmd2.Parameters.AddWithValue("@p_mpay_card_id", card.card_id);
                                                    cmd2.Parameters.AddWithValue("@p_mpay_mask_cardno", card.mask_cardno);
                                                    cmd2.Parameters.AddWithValue("@p_mpay_card_token", card.cardtoken);
                                                    cmd2.Parameters.AddWithValue("@p_mpay_card_type", card.cardtype);
                                                    cmd2.Parameters.AddWithValue("@p_mpay_card_group", card.cardGroup);
                                                    cmd2.Parameters.AddWithValue("@p_mpay_card_pin", auth.EncryptRijndael("123456", salt));
                                                    cmd2.Parameters.AddWithValue("@p_update_by", value.create_by);
                                                    cmd2.Parameters.Add("@p_code_status", MySqlDbType.VarChar);
                                                    cmd2.Parameters["@p_code_status"].Direction = ParameterDirection.Output;
                                                    cmd2.ExecuteNonQuery();
                                                    trans.Commit();

                                                    var code_status = cmd2.Parameters["@p_code_status"].Value.ToString();

                                                    if (code_status == "record_saved" || code_status == "token_exists")
                                                    {
                                                        data.Success = true;
                                                        data.Code = code_status;
                                                        data.Message = WebApiResources.StudentAccUpdateSuccess;
                                                    }
                                                    else
                                                    {
                                                        data.Success = false;
                                                        data.Code = code_status;
                                                        data.Message = WebApiResources.NoRecordFoundText;
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    trans.Rollback();
                                                    data.Success = false;
                                                    data.Code = "error_occured";
                                                    data.Message = WebApiResources.ErrorOccured;
                                                    ExceptionUtility.LogException(ex, "account/update-virtual-balance");
                                                }
                                            }
                                        }
                                        else 
                                        {
                                            data.Success = false;
                                            data.Code = response.Header.status;
                                            data.Message = response.Header.message;
                                        }
                                    }
                                    else 
                                    {
                                        data.Success = false;
                                        data.Code = response.Header.status;
                                        data.Message = response.Header.message;
                                    }
                                }
                                else if (response.Header.status == "01")
                                {
                                    data.Success = false;
                                    data.Code = response.Header.status;
                                    data.Message = WebApiResources.SomethingWentWrong;
                                }
                                else if (response.Header.status == "02")
                                {
                                    data.Success = false;
                                    data.Code = response.Header.status;
                                    data.Message = WebApiResources.CustomerDocumentRejected;
                                }
                                else if (response.Header.status == "03")
                                {
                                    data.Success = false;
                                    data.Code = response.Header.status;
                                    data.Message = WebApiResources.CustomerRejected;
                                }
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = status_code;
                                data.Message = WebApiResources.NoRecordFoundText;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "account/update-virtual-balance");
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

                ////for maintenance
                //data.Success = false;
                //data.Code = "00";
                //data.Message = WebApiResources.ServiceTemporaryDown;

                return Ok(data);
            }
            else
            {
                return BadRequest("Missing parameters.");
            }
        }

        //[HttpPost]
        //[Route("api/v2/account/verify")]
        //public IHttpActionResult Post_Account_Verify(string culture, [FromBody] AccountVerifyParam value)
        //{
        //    if (!string.IsNullOrEmpty(culture))
        //    {
        //        var language = new CultureInfo(culture);
        //        Thread.CurrentThread.CurrentUICulture = language;
        //    }

        //    AccountDataModel data = new AccountDataModel();

        //    if (value.account_id != 0 && value.otp != null)
        //    {
        //        string salt = ConfigurationManager.AppSettings["passPhrase"];
        //        string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;
        //        using (MySqlConnection conn = new MySqlConnection(constr))
        //        {
        //            conn.Open();

        //            using (MySqlCommand cmd = new MySqlCommand("sp_update_register_account_log", conn))
        //            {
        //                MySqlTransaction trans;
        //                trans = conn.BeginTransaction();
        //                cmd.Transaction = trans;

        //                try
        //                {
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    cmd.Parameters.AddWithValue("@p_account_id", value.account_id);
        //                    cmd.Parameters.AddWithValue("@p_otp", auth.EncryptRijndael(value.otp, salt));
        //                    cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
        //                    cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
        //                    cmd.ExecuteNonQuery();
        //                    trans.Commit();

        //                    string status_code = (string)cmd.Parameters["@p_status_code"].Value;

        //                    if (status_code == "proceed_ekyc")
        //                    {
        //                        data.Success = true;
        //                        data.Code = status_code;
        //                        data.Message = WebApiResources.PleaseUploadYourImage;
        //                    }
        //                    else if (status_code == "otp_expired")
        //                    {
        //                        data.Success = false;
        //                        data.Code = status_code;
        //                        data.Message = WebApiResources.OTPHasExpired;
        //                    }
        //                    else if (status_code == "otp_tryout_exceed")
        //                    {
        //                        data.Success = false;
        //                        data.Code = status_code;
        //                        data.Message = WebApiResources.MaxOTPTryout;
        //                    }
        //                    else if (status_code == "wrong_otp")
        //                    {
        //                        data.Success = false;
        //                        data.Code = status_code;
        //                        data.Message = WebApiResources.WrongOTP;
        //                    }
        //                    else if (status_code == "no_record_found")
        //                    {
        //                        data.Success = false;
        //                        data.Code = status_code;
        //                        data.Message = WebApiResources.AccountNotFound;
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    trans.Rollback();
        //                    data.Success = false;
        //                    data.Code = "error_occured";
        //                    data.Message = WebApiResources.ErrorOccured;
        //                    ExceptionUtility.LogException(ex, "account/verify");
        //                }
        //                finally
        //                {
        //                    conn.Close();
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        return BadRequest("Missing parameters.");
        //    }

        //    return Ok(data);
        //}

        [HttpPost]
        [Route("api/v2/account/verify")]
        public IHttpActionResult Post_Account_Verify(string culture, [FromBody] VerifyAccountParam value)
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
                                data.Success = true;
                                data.Code = status_code;
                                data.Message = WebApiResources.RegisterSuccess;
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
                            else if (status_code == "otp_expired")
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
                            ExceptionUtility.LogException(ex, "account/verify");
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
        [Route("api/v2/account/update-profile")]
        public IHttpActionResult Post_Account_Update_Profile(string culture, [FromBody] UpdateProfileInfoParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            UserDataModel data = new UserDataModel();
            if (value.profile_id != 0 && !string.IsNullOrEmpty(value.address) && !string.IsNullOrEmpty(value.city) && value.country_id != 0 &&
                !string.IsNullOrEmpty(value.full_name) && !string.IsNullOrEmpty(value.nric) && !string.IsNullOrEmpty(value.postcode) && value.state_id != 0 &&
                !string.IsNullOrEmpty(value.update_by) && !string.IsNullOrEmpty(value.mother_maiden_name) && !string.IsNullOrEmpty(value.occupation) &&
                !string.IsNullOrEmpty(value.employer_name))
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_update_user_account", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                            cmd.Parameters.AddWithValue("@p_full_name", value.full_name);
                            cmd.Parameters.AddWithValue("@p_nric", value.nric);
                            cmd.Parameters.AddWithValue("@p_address", value.address);
                            cmd.Parameters.AddWithValue("@p_postcode", value.postcode);
                            cmd.Parameters.AddWithValue("@p_city", value.city);
                            cmd.Parameters.AddWithValue("@p_state_id", value.state_id);
                            cmd.Parameters.AddWithValue("@p_country_id", value.country_id);
                            cmd.Parameters.AddWithValue("@p_mother_maiden_name", value.mother_maiden_name);
                            cmd.Parameters.AddWithValue("@p_occupation", value.occupation);
                            cmd.Parameters.AddWithValue("@p_employer_name", value.employer_name);
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
                        ExceptionUtility.LogException(ex, "account/update-profile");
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
        [Route("api/v2/account/update-user-info")]
        public async Task<IHttpActionResult> Post_Account_Update_User_Info(string culture, [FromBody] UpdateProfileInfoParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            UserDataModel data = new UserDataModel();
            if (value.profile_id != 0 && value.address != null && value.card_type_id != 0 && value.city != null && value.country_id != 0 && value.date_of_birth != null &&
                value.full_name != null && value.nric != null && value.postcode != null && value.state_id != 0 && !string.IsNullOrEmpty(value.state_name) &&
                value.update_by != null && value.user_race_id != 0 && !string.IsNullOrEmpty(value.mother_maiden_name) && !string.IsNullOrEmpty(value.occupation) && 
                !string.IsNullOrEmpty(value.employer_name))
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_update_user_info", conn))
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
                            cmd.Parameters.AddWithValue("@p_mother_maiden_name", value.mother_maiden_name);
                            cmd.Parameters.AddWithValue("@p_occupation", value.occupation);
                            cmd.Parameters.AddWithValue("@p_employer_name", value.employer_name);
                            cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                            cmd.Parameters.Add("@p_mpay_uid", MySqlDbType.Int32);
                            cmd.Parameters["@p_mpay_uid"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            int mpay_uid = 0;
                            if (cmd.Parameters["@p_mpay_uid"].Value != DBNull.Value)
                            {
                                mpay_uid = Convert.ToInt32(cmd.Parameters["@p_mpay_uid"].Value);
                            }

                            if (mpay_uid > 0)
                            {
                                UpdateUserInfoParam param = new UpdateUserInfoParam();
                                param.uid = mpay_uid.ToString();
                                param.occupation = value.occupation;
                                param.employer_name = value.employer_name;
                                param.address = value.address;
                                param.postcode = value.postcode;
                                param.city = value.city;
                                param.province = value.state_name;
                                param.add_country = value.country_id.ToString();

                                var result = cls.PostUpdateUserInfo(param);
                                string jsonStr = await result;
                                RegisterAccModel response = JsonConvert.DeserializeObject<RegisterAccModel>(jsonStr);

                                if (response.Header.status == "00")
                                {
                                    data.Success = true;
                                    data.Code = "update_success";
                                    data.Message = WebApiResources.ProfileUpdateSuccess;
                                }
                                else
                                {
                                    data.Success = false;
                                    data.Code = response.Header.status;
                                    data.Message = response.Header.message;
                                }
                            }
                            else 
                            {
                                data.Success = true;
                                data.Code = "update_success";
                                data.Message = WebApiResources.ProfileUpdateSuccess;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "account/update-user-info");
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
        [Route("api/v2/account/balance")] //for ipay88
        public IHttpActionResult Post_Account_Balance(string culture, [FromBody] AccountStatusParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            WalletDataModel data = new WalletDataModel();
            List<AccBalance> list = new List<AccBalance>();
            AccBalanceDataModel listData = new AccBalanceDataModel();

            listData.Success = false;

            if (!string.IsNullOrEmpty(value.wallet_number) && value.profile_id != 0)
            {
                MySqlTransaction trans = null;
                data.Success = false;
                string sqlQuery = string.Empty;
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    conn.Open();
                    AccBalance prop = new AccBalance();

                    using (MySqlCommand cmd = new MySqlCommand("sp_get_wallet_status", conn))
                    {
                        try
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_wallet_number", value.wallet_number);
                            cmd.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    if (dataReader["account_balance"] != DBNull.Value)
                                    {
                                        prop.balance = dataReader["account_balance"].ToString();
                                    }
                                    else
                                    {
                                        prop.balance = "0.00";
                                    }

                                    list.Add(prop);
                                }

                                listData.Success = true;
                                listData.Code = "200";
                                listData.Message = "OK";
                                listData.Data = list;
                            }
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            data.Success = false;
                            data.Code = "error_occured";
                            data.Message = WebApiResources.ErrorOccured;
                            ExceptionUtility.LogException(ex, "account/balance");
                        }
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
        [Route("api/v2/account/status")]
        public IHttpActionResult Post_Account_Status(string culture, [FromBody] AccountStatusParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            WalletDataModel data = new WalletDataModel();
            List<AccStatus> list = new List<AccStatus>();
            AccStatusDataModel listData = new AccStatusDataModel();

            listData.Success = false;

            if (!string.IsNullOrEmpty(value.wallet_number) && value.profile_id != 0)
            {
                MySqlTransaction trans = null;
                data.Success = false;
                string sqlQuery = string.Empty;
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    conn.Open();
                    AccStatus prop = new AccStatus();

                    using (MySqlCommand cmd = new MySqlCommand("sp_get_wallet_status", conn))
                    {
                        try
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_wallet_number", value.wallet_number);
                            cmd.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    if (dataReader["mpay_uid"] != DBNull.Value)
                                    {
                                        prop.mpay_uid = dataReader["mpay_uid"].ToString();
                                    }
                                    else
                                    {
                                        prop.mpay_uid = "0";
                                    }

                                    if (dataReader["account_status_id"] != DBNull.Value)
                                    {
                                        prop.account_status_id = dataReader["account_status_id"].ToString();
                                        if (culture == "ms-MY")
                                        {
                                            prop.account_status = dataReader["account_status_description_bm"].ToString();
                                        }
                                        else {
                                            prop.account_status = dataReader["account_status_description"].ToString();
                                        }
                                    }
                                    else
                                    {
                                        prop.account_status_id = "0";
                                        prop.account_status = WebApiResources.NoStatus;
                                    }

                                    prop.kyc_status_id = dataReader["kyc_status_id"].ToString();
                                    if (culture == "ms-MY")
                                    {
                                        prop.kyc_status = dataReader["kyc_status_description_bm"].ToString();
                                    }
                                    else
                                    {
                                        prop.kyc_status = dataReader["kyc_status_description"].ToString();
                                    }

                                    list.Add(prop);
                                }

                                listData.Success = true;
                                listData.Code = "OK";
                                listData.Message = prop.account_status;
                                listData.Data = list;
                            }
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            data.Success = false;
                            data.Code = "error_occured";
                            data.Message = WebApiResources.ErrorOccured;
                            ExceptionUtility.LogException(ex, "account/status");
                        }
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
        [Route("api/v2/account/upload-image")]
        public IHttpActionResult Post_Account_Upload_Image(string culture, [FromBody] UploadImageParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            string salt = ConfigurationManager.AppSettings["passPhrase"];
            FileTransferProtocol ftp = new FileTransferProtocol();
            UserDataModel data = new UserDataModel();
            if (value.id_number != null && value.file_name != null && value.photo_base64 != null && value.image_type_id != 0 && value.create_by != null)
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
                string newFolder = auth.EncryptRijndael(check.RemoveNonAlphaNumeric(value.id_number), salt);
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
                            cmd.Parameters.AddWithValue("@p_identity_number", check.RemoveNonAlphaNumeric(value.id_number));
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
                        ExceptionUtility.LogException(ex, "account/upload-image");
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

        [HttpPost]
        [Route("api/v2/account/do-ekyc")]
        public async Task<IHttpActionResult> Post_Account_Do_EKYC(string culture, [FromBody] AccEkycParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            AccountDataModel data = new AccountDataModel();

            if (value.account_id != 0)
            {
                data.Success = false;
                string sqlQuery = string.Empty;
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                MySqlTransaction trans = null;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    conn.Open();

                    DoEkycParam prop = new DoEkycParam();

                    using (MySqlCommand cmd = new MySqlCommand("sp_get_register_account_log", conn))
                    {
                        try
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_account_id", value.account_id);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            DateTime localDate = DateTime.Now;
                            timestamp = localDate.ToString("yyyyMMddHHmmss");

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    prop.idType = dataReader["id_type"].ToString();
                                    prop.idImgString = dataReader["id_img_url"].ToString();
                                    prop.selfieImgString = dataReader["selfie_img_url"].ToString();
                                    prop.timeStamp = timestamp;
                                }
                            }

                            if (dataReader != null)
                                dataReader.Close();

                            var result = cls.PostDoEKYC(prop);
                            string jsonStr = await result;
                            AccDoEKYCModel response = JsonConvert.DeserializeObject<AccDoEKYCModel>(jsonStr);

                            if (response.Header.status == "00")
                            {

                                AccDoEkycInfo info = response.Body.EKYC_info;

                                //foreach (OCRData sl in info.OCR_data)
                                //{
                                OCRData ocr = new OCRData();

                                if (info.OCR_data != null)
                                    ocr = info.OCR_data;
                                //}

                                if (dataReader != null)
                                    dataReader.Close();

                                using (MySqlCommand cmd2 = new MySqlCommand("sp_update_register_account_log_status", conn))
                                {
                                    try
                                    {
                                        cmd.Transaction = trans;
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@p_account_id", value.account_id);
                                        cmd.Parameters.AddWithValue("@p_uid", info.uid);
                                        cmd.Parameters.AddWithValue("@p_EKYC_id", info.EKYC_id);
                                        cmd.Parameters.AddWithValue("@p_status", info.status);
                                        cmd.Parameters.AddWithValue("@p_message", info.message);
                                        cmd.Parameters.AddWithValue("@p_timestamp", timestamp);
                                        cmd.Parameters.AddWithValue("@p_id_number", ocr.id_number);
                                        cmd.Parameters.AddWithValue("@p_extra_passport_data", ocr.extra_passport_data);
                                        cmd.Parameters.AddWithValue("@p_address", ocr.address);
                                        cmd.Parameters.AddWithValue("@p_sex", ocr.sex);
                                        cmd.Parameters.AddWithValue("@p_id_type_name", ocr.id_type_name);
                                        cmd.Parameters.AddWithValue("@p_id_type_id", ocr.id_type_id);
                                        cmd.Parameters.AddWithValue("@p_uuid", ocr.uuid);
                                        cmd.Parameters.AddWithValue("@p_religion", ocr.religion);
                                        cmd.Parameters.AddWithValue("@p_religion", ocr.colour);
                                        cmd.Parameters.AddWithValue("@p_religion", ocr.nationality);
                                        cmd.ExecuteNonQuery();
                                        trans.Commit();

                                        data.Success = true;
                                        data.Code = "";
                                        data.Message = WebApiResources.AccountVerifySuccess;

                                    }
                                    catch (Exception ex)
                                    {
                                        trans.Rollback();
                                        data.Success = false;
                                        data.Code = "error_occured";
                                        data.Message = WebApiResources.ErrorOccured;
                                        ExceptionUtility.LogException(ex, "account/do-ekyc");
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
                            trans.Rollback();
                            data.Success = false;
                            data.Code = "error_occured";
                            data.Message = WebApiResources.ErrorOccured;
                            ExceptionUtility.LogException(ex, "account/do-ekyc");
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
        [Route("api/v2/account/do-captcha")]
        public async Task<IHttpActionResult> Post_Account_Do_Captcha(string culture)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            AccountDataModel data = new AccountDataModel();

            var result = cls.PostDoCaptchaLivliness();
            string jsonStr = await result;
            DoCaptchaModel response = JsonConvert.DeserializeObject<DoCaptchaModel>(jsonStr);

            if (response.Header.status == "00")
            {
                var url = response.Body.recaptcha_URL;

                data.Success = true;
                data.Code = response.Header.status;
                data.Message = url;
            }
            else 
            {
                data.Success = false;
                data.Code = response.Header.status;
                data.Message = response.Header.message;
            }

            return Ok(data);
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/account/resubmit-kyc")]
        public async Task<IHttpActionResult> Post_Account_Resubmit_KYC(string culture, [FromBody] ResubmitParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            LoginDataModel data = new LoginDataModel();
            if (!string.IsNullOrEmpty(value.username))
            {
                string salt = ConfigurationManager.AppSettings["passPhrase"];
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                MySqlTransaction trans = null;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    conn.Open();

                    ResubmitKYCParam prop = new ResubmitKYCParam();

                    using (MySqlCommand cmd = new MySqlCommand("sp_get_user_account_resubmit", conn))
                    {
                        trans = conn.BeginTransaction();
                        cmd.Transaction = trans;

                        try
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_id_number", value.username);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    if (dataReader["mother_maiden_name"] != DBNull.Value)
                                    {
                                        prop.mothermaidenname = dataReader["mother_maiden_name"].ToString();
                                    }
                                    else {
                                        prop.mothermaidenname = "";
                                    }
                                    prop.useridimagefilename = dataReader["photo_id_url"].ToString();
                                    prop.useridimagestring = dataReader["photo_id_url"].ToString();
                                    prop.userselfieimagefilename = dataReader["photo_url"].ToString();
                                    prop.userselfieimagestring = dataReader["photo_url"].ToString();
                                }

                                if (dataReader != null)
                                {
                                    dataReader.Close();
                                }

                                using (MySqlCommand cmd2 = new MySqlCommand("sp_get_wallet_info", conn))
                                {
                                    cmd2.Transaction = trans;
                                    cmd2.CommandType = CommandType.StoredProcedure;
                                    cmd2.Parameters.Clear();
                                    cmd2.Parameters.AddWithValue("@p_username", value.username);
                                    cmd2.Parameters.AddWithValue("@p_wallet_id", 0);
                                    MySqlDataReader dataReader2 = cmd2.ExecuteReader();


                                    if (dataReader2.HasRows == true)
                                    {
                                        while (dataReader2.Read())
                                        {
                                            prop.uid = dataReader2["mpay_uid"].ToString();
                                        }
                                    }

                                    if (dataReader2 != null)
                                    {
                                        dataReader2.Close();
                                    }

                                    var result = cls.PostResubmitKYC(prop);
                                    string jsonStr = await result;
                                    ResubmitKYCModel response = JsonConvert.DeserializeObject<ResubmitKYCModel>(jsonStr);

                                    if (response.Header.status == "00")
                                    {
                                        data.Success = true;
                                        data.Message = WebApiResources.DocumentResubmitSuccess;
                                    }
                                    else
                                    {
                                        data.Success = false;
                                        data.Message = response.Header.message;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            data.Success = false;
                            data.Code = "error_occured";
                            data.Message = WebApiResources.ErrorOccured;
                            ExceptionUtility.LogException(ex, "account/resubmit-kyc");
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
        [Route("api/v2/account/verify-kyc")]
        public async Task<IHttpActionResult> Post_Account_Verify_KYC(string culture, [FromBody] ResubmitParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            LoginDataModel data = new LoginDataModel();
            if (!string.IsNullOrEmpty(value.username))
            {
                string salt = ConfigurationManager.AppSettings["passPhrase"];
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                MySqlTransaction trans;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    conn.Open();

                    RegisterAccParam prop = new RegisterAccParam();

                    using (MySqlCommand cmd = new MySqlCommand("sp_get_user_profile_register", conn))
                    {
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

                            var result = cls.PostAccountVerify(prop);
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
                                    dataReader.Close();

                                ChangePINParam param = new ChangePINParam();
                                param.cardtoken = card.cardtoken;
                                param.oldPin = "";
                                param.newPin = "123456";
                                param.uid = acc.uid;

                                var result2 = mpay.PostChangePIN(param);
                                string jsonStr2 = await result2;

                                ChangPINModel response2 = JsonConvert.DeserializeObject<ChangPINModel>(jsonStr2);

                                if (response2.Header.status == "00")
                                {
                                    BodyPin bPin = response2.Body;

                                    if (bPin.pin_change_status == "00")
                                    {
                                        using (MySqlCommand cmd2 = new MySqlCommand("sp_update_wallet_info", conn))
                                        {
                                            try
                                            {
                                                cmd2.Transaction = trans;
                                                cmd2.CommandType = CommandType.StoredProcedure;
                                                cmd2.Parameters.AddWithValue("@p_id_number", acc.idno);
                                                cmd2.Parameters.AddWithValue("@p_mpay_uid", acc.uid);
                                                cmd2.Parameters.AddWithValue("@p_mpay_card_id", card.card_id);
                                                cmd2.Parameters.AddWithValue("@p_mpay_mask_cardno", card.mask_cardno);
                                                cmd2.Parameters.AddWithValue("@p_mpay_card_token", card.cardtoken);
                                                cmd2.Parameters.AddWithValue("@p_mpay_card_type", card.cardtype);
                                                cmd2.Parameters.AddWithValue("@p_mpay_card_group", card.cardGroup);
                                                cmd2.Parameters.AddWithValue("@p_mpay_card_pin", auth.EncryptRijndael(param.newPin, salt));
                                                cmd2.Parameters.AddWithValue("@p_update_by", acc.name);
                                                cmd2.ExecuteNonQuery();
                                                trans.Commit();

                                                data.Success = true;
                                                data.Code = response.Header.message;
                                                data.Message = WebApiResources.AwaitingVerificationMpay;

                                            }
                                            catch (Exception ex)
                                            {
                                                trans.Rollback();
                                                data.Success = false;
                                                data.Code = "error_occured";
                                                data.Message = WebApiResources.ErrorOccured;
                                                ExceptionUtility.LogException(ex, "account/verify-kyc");
                                            }
                                        }
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
                            trans.Rollback();
                            data.Success = false;
                            data.Code = "error_occured";
                            data.Message = WebApiResources.ErrorOccured;
                            ExceptionUtility.LogException(ex, "account/verify-kyc");
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

        #endregion

        [HttpPost]
        [Authorize]
        [Route("api/v2/account/topup-status")]
        public IHttpActionResult Post_Account_Topup_Status(string culture, [FromBody] WalletTopupStatusParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            string reference_number = string.Empty;
            WalletV2Class cls = new WalletV2Class();
            WalletDataModel data = new WalletDataModel();
            if (value.wallet_id != 0 && !string.IsNullOrEmpty(value.reference_id) && value.topup_amount != 0 && value.status_id != 0 && !string.IsNullOrEmpty(value.update_by))
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_update_wallet_topup", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_wallet_id", value.wallet_id);
                            cmd.Parameters.AddWithValue("@p_reference_id", value.reference_id);
                            cmd.Parameters.AddWithValue("@p_topup_amount", value.topup_amount);
                            cmd.Parameters.AddWithValue("@p_status_id", value.status_id);
                            cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            if (value.status_id == 200)
                            {
                                data.Success = true;
                                data.Code = "success";
                                data.Message = WebApiResources.YourTopupSuccess;
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "failed";
                                data.Message = WebApiResources.YourTopupFail;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "account/topup-status");
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
        //[Authorize]
        [Route("api/v2/account/register-check")]
        public IHttpActionResult Post_Account_Register_Check(string culture, [FromBody] RegisterCheckParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            string reference_number = string.Empty;
            WalletDataModel data = new WalletDataModel();
            if (!string.IsNullOrEmpty(value.username) && value.user_role_id != 0)
            {
                MySqlTransaction trans = null;
                data.Success = false;
                string sqlQuery = string.Empty;
                string status_code = string.Empty;
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_user_register_check", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_username", value.username);
                            cmd.Parameters.AddWithValue("@p_user_role_id", value.user_role_id);
                            cmd.Parameters.Add("@p_status_code", MySqlDbType.String);
                            cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            status_code = Convert.ToString(cmd.Parameters["@p_status_code"].Value);

                            if (status_code == "active")
                            {
                                data.Success = true;
                                data.Code = "auth_login";
                                data.Message = WebApiResources.PlsEnterPswd;
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
                        ExceptionUtility.LogException(ex, "account/register-check");
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

        [HttpPost]
        [Authorize]
        [Route("api/v2/account/transfer")]
        public IHttpActionResult Post_Account_Transfer(string culture, [FromBody] WalletTransferParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            string reference_number = string.Empty;
            WalletV2Class cls = new WalletV2Class();
            WalletDataModel data = new WalletDataModel();
            if (value.wallet_id != 0 && !string.IsNullOrEmpty(value.wallet_number) && value.recipient_id != 0 &&
                !string.IsNullOrEmpty(value.recipient_wallet) && value.transfer_amount != 0 && value.transfer_date != null &&
                !string.IsNullOrEmpty(value.create_by))
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

                        reference_number = "TRF-" + cls.GetAlphaUniqueKey() + "-" + cls.GetNumericUniqueKey() + "-" + cls.GetNumericUniqueKey();

                        using (MySqlCommand cmd = new MySqlCommand("sp_insert_wallet_transfer", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_wallet_id", value.wallet_id);
                            cmd.Parameters.AddWithValue("@p_wallet_number", value.wallet_number);
                            cmd.Parameters.AddWithValue("@p_recipient_id", value.recipient_id);
                            cmd.Parameters.AddWithValue("@p_recipient_wallet", value.recipient_wallet);
                            cmd.Parameters.AddWithValue("@p_transfer_amount", value.transfer_amount);
                            cmd.Parameters.AddWithValue("@p_transfer_date", value.transfer_date.ToString("yyyy-MM-dd HH:mm:ss"));
                            cmd.Parameters.AddWithValue("@p_reference_number", reference_number);
                            cmd.Parameters.AddWithValue("@p_create_by", value.create_by);
                            cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                            cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            var status_code = (string)cmd.Parameters["@p_status_code"].Value;

                            if (status_code == "invalid_wallet_number")
                            {
                                data.Success = false;
                                data.Code = "invalid_wallet_number";
                                data.Message = WebApiResources.InvalidWalletNo;
                            }
                            else if (status_code == "invalid_recipient_wallet")
                            {
                                data.Success = false;
                                data.Code = "invalid_recipient_wallet";
                                data.Message = WebApiResources.InvalidRecipientWalletNo;
                            }
                            else if (status_code == "invalid_amount")
                            {
                                data.Success = false;
                                data.Code = "invalid_amount";
                                data.Message = WebApiResources.InvalidAmount;
                            }
                            else if (status_code == "insufficient_balance")
                            {
                                data.Success = false;
                                data.Code = "insufficient_balance";
                                data.Message = WebApiResources.InsufficientWalletBalance;
                            }
                            else if (status_code == "success")
                            {
                                data.Success = true;
                                data.Code = "success";
                                data.Message = WebApiResources.YourTransferSuccess;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "account/transfer");
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
        [Route("api/v2/account/purchase")]
        public IHttpActionResult Post_Account_Purchase(string culture, [FromBody] CheckOutParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            FirebaseCloudMessagingPush fcm = new FirebaseCloudMessagingPush();
            ReferenceV2Class cls = new ReferenceV2Class();
            PurchaseDataModel data = new PurchaseDataModel();
            if (value.profile_id != 0 && value.wallet_id != 0 && value.order_status_id != 0 && value.user_role_id != 0 &&
                value.total_amount != 0 && value.payment_method_id != 0 && value.create_by != null)
            {
                List<Purchase> list = new List<Purchase>();
                bool proceed = true;
                MySqlTransaction trans = null;
                data.Success = false;
                int profileID;
                int recipientID;
                int merchantID = 0;
                string reference_number = string.Empty;
                string sqlQuery = string.Empty;
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();

                        using (MySqlCommand cmd2 = new MySqlCommand("sp_get_order_cart", conn))
                        {
                            trans = conn.BeginTransaction();
                            cmd2.Transaction = trans;
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.Clear();
                            cmd2.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                            cmd2.Parameters.AddWithValue("@p_wallet_id", value.wallet_id);
                            cmd2.Parameters.AddWithValue("@p_order_status_id", value.order_status_id);
                            MySqlDataReader dataReader = cmd2.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {

                                while (dataReader.Read())
                                {
                                    DateTime dtPickup = new DateTime();
                                    TimeSpan tsPickup = new TimeSpan();
                                    Purchase prop = new Purchase();
                                    prop.cart_id = Convert.ToInt32(dataReader["cart_id"]);
                                    prop.profile_id = Convert.ToInt32(dataReader["profile_id"]);
                                    profileID = Convert.ToInt32(dataReader["profile_id"]);
                                    prop.wallet_id = Convert.ToInt32(dataReader["wallet_id"]);
                                    prop.wallet_number = dataReader["wallet_number"].ToString();
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.merchant_id = Convert.ToInt32(dataReader["merchant_id"]);
                                    merchantID = Convert.ToInt32(dataReader["merchant_id"]);
                                    prop.company_name = dataReader["company_name"].ToString();
                                    prop.school_id = Convert.ToInt16(dataReader["school_id"]);
                                    prop.school_name = dataReader["school_name"].ToString();
                                    prop.recipient_id = Convert.ToInt16(dataReader["recipient_id"]);
                                    recipientID = Convert.ToInt16(dataReader["recipient_id"]);
                                    prop.recipient_name = dataReader["recipient_name"].ToString();
                                    prop.recipient_role_id = Convert.ToInt16(dataReader["recipient_role_id"]);
                                    prop.recipient_role = dataReader["recipient_role"].ToString();
                                    prop.pickup_date = Convert.ToDateTime(dataReader["pickup_date"]);
                                    dtPickup = Convert.ToDateTime(dataReader["pickup_date"]);
                                    if (dataReader["pickup_time"] != DBNull.Value)
                                    {
                                        prop.pickup_time = dataReader.GetTimeSpan(dataReader.GetOrdinal("pickup_time"));
                                        tsPickup = dataReader.GetTimeSpan(dataReader.GetOrdinal("pickup_time"));
                                    }
                                    if (dataReader["service_method_id"] != DBNull.Value)
                                    {
                                        prop.service_method_id = Convert.ToInt32(dataReader["service_method_id"]);
                                    }
                                    prop.delivery_location = dataReader["delivery_location"].ToString();
                                    prop.product_id = Convert.ToInt32(dataReader["product_id"]);
                                    prop.product_name = dataReader["product_name"].ToString();
                                    prop.product_photo_url = dataReader["product_photo_url"].ToString();
                                    prop.product_qty = Convert.ToInt16(dataReader["product_qty"]);
                                    prop.unit_price = Convert.ToDecimal(dataReader["unit_price"]);
                                    prop.sub_total_amount = Convert.ToDecimal(dataReader["sub_total_amount"]);
                                    prop.order_status_id = Convert.ToInt16(dataReader["order_status_id"]);
                                    prop.order_status = dataReader["order_status"].ToString();
                                    prop.order_status_bm = dataReader["order_status_bm"].ToString();
                                    list.Add(prop);

                                    if (profileID != recipientID)
                                    {
                                        TimeSpan ts = new TimeSpan(7, 0, 0);
                                        if (DateTime.Now >= dtPickup.Add(ts))
                                        {
                                            proceed = false;
                                        }
                                    }
                                    else
                                    {
                                        if (DateTime.Now >= dtPickup.Add(tsPickup))
                                        {
                                            proceed = false;
                                        }
                                    }
                                }
                            }

                            if (proceed == true)
                            {
                                dataReader.Close();
                                trans.Dispose();

                                using (MySqlCommand cmd3 = new MySqlCommand("sp_get_wallet_detail", conn))
                                {
                                    trans = conn.BeginTransaction();
                                    cmd3.Transaction = trans;
                                    cmd3.CommandType = CommandType.StoredProcedure;
                                    cmd3.Parameters.Clear();
                                    cmd3.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                                    cmd3.Parameters.AddWithValue("@p_wallet_id", value.wallet_id);
                                    cmd3.Parameters.Add("@p_account_balance", MySqlDbType.Decimal);
                                    cmd3.Parameters["@p_account_balance"].Direction = ParameterDirection.Output;
                                    cmd3.Parameters.Add("@p_school_id", MySqlDbType.Int16);
                                    cmd3.Parameters["@p_school_id"].Direction = ParameterDirection.Output;
                                    cmd3.ExecuteNonQuery();
                                    trans.Commit();

                                    var account_balance = Convert.ToDecimal(cmd3.Parameters["@p_account_balance"].Value);
                                    var school_id = Convert.ToInt16(cmd3.Parameters["@p_school_id"].Value);

                                    bool proceedPay = true;

                                    if (value.total_amount > account_balance)
                                    {
                                        if (value.payment_method_id == 1)
                                        {
                                            proceedPay = false;
                                        }
                                    }

                                    if (proceedPay == true)
                                    {
                                        reference_number = "PAY-I3S-" + cls.GetNumericUniqueKey() + "-" + cls.GetNumericUniqueKey();
                                        trans.Dispose();

                                        using (MySqlCommand cmd4 = new MySqlCommand("sp_insert_order_master", conn))
                                        {
                                            trans = conn.BeginTransaction();
                                            cmd4.Transaction = trans;
                                            cmd4.CommandType = CommandType.StoredProcedure;
                                            cmd4.Parameters.Clear();
                                            cmd4.Parameters.AddWithValue("@p_reference_number", reference_number);
                                            cmd4.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                                            cmd4.Parameters.AddWithValue("@p_wallet_id", value.wallet_id);
                                            cmd4.Parameters.AddWithValue("@p_user_role_id", value.user_role_id);
                                            cmd4.Parameters.AddWithValue("@p_sub_total_amount", value.total_amount);
                                            cmd4.Parameters.AddWithValue("@p_total_amount", value.total_amount);
                                            cmd4.Parameters.AddWithValue("@p_payment_method_id", value.payment_method_id);
                                            cmd4.Parameters.AddWithValue("@p_received_amount", value.total_amount);
                                            cmd4.Parameters.AddWithValue("@p_create_by", value.create_by);
                                            cmd4.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                                            cmd4.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                                            cmd4.Parameters.Add("@p_order_id", MySqlDbType.Int16);
                                            cmd4.Parameters["@p_order_id"].Direction = ParameterDirection.Output;
                                            cmd4.ExecuteNonQuery();
                                            trans.Commit();

                                            var order_status = (string)cmd4.Parameters["@p_status_code"].Value;
                                            var _order_id = Convert.ToInt16(cmd4.Parameters["@p_order_id"].Value);


                                            if (order_status == "record_saved")
                                            {
                                                int detail_list = 0;
                                                int saved_count = 0;
                                                string detail_status = string.Empty;

                                                detail_list = list.Count;
                                                foreach (Purchase item in list)
                                                {
                                                    trans.Dispose();

                                                    using (MySqlCommand cmd5 = new MySqlCommand("sp_insert_order_detail", conn))
                                                    {
                                                        trans = conn.BeginTransaction();
                                                        cmd5.Transaction = trans;
                                                        cmd5.CommandType = CommandType.StoredProcedure;
                                                        cmd5.Parameters.Clear();
                                                        cmd5.Parameters.AddWithValue("@p_cart_id", item.cart_id);
                                                        cmd5.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                                                        cmd5.Parameters.AddWithValue("@p_wallet_id", value.wallet_id);
                                                        cmd5.Parameters.AddWithValue("@p_order_id", _order_id);
                                                        cmd5.Parameters.AddWithValue("@p_recipient_id", item.recipient_id);
                                                        cmd5.Parameters.AddWithValue("@p_recipient_role_id", item.recipient_role_id);
                                                        cmd5.Parameters.AddWithValue("@p_school_id", item.school_id);
                                                        cmd5.Parameters.AddWithValue("@p_merchant_id", item.merchant_id);
                                                        cmd5.Parameters.AddWithValue("@p_pickup_date", item.pickup_date);
                                                        cmd5.Parameters.AddWithValue("@p_pickup_time", item.pickup_time);
                                                        cmd5.Parameters.AddWithValue("@p_service_method_id", item.service_method_id);
                                                        cmd5.Parameters.AddWithValue("@p_delivery_location", item.delivery_location);
                                                        cmd5.Parameters.AddWithValue("@p_product_id", item.product_id);
                                                        cmd5.Parameters.AddWithValue("@p_product_qty", item.product_qty);
                                                        cmd5.Parameters.AddWithValue("@p_unit_price", item.unit_price);
                                                        cmd5.Parameters.AddWithValue("@p_sub_total_amount", item.sub_total_amount);
                                                        cmd5.Parameters.AddWithValue("@p_total_amount", item.sub_total_amount);
                                                        cmd5.Parameters.AddWithValue("@p_create_by", value.create_by);
                                                        cmd5.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                                                        cmd5.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                                                        cmd5.ExecuteNonQuery();
                                                        trans.Commit();

                                                        detail_status = (string)cmd5.Parameters["@p_status_code"].Value;

                                                    }
                                                    saved_count++;
                                                }

                                                if (detail_list == saved_count)
                                                {
                                                    trans.Dispose();

                                                    using (MySqlCommand cmd6 = new MySqlCommand("sp_insert_wallet_transaction_history", conn))
                                                    {
                                                        trans = conn.BeginTransaction();
                                                        cmd6.Transaction = trans;
                                                        cmd6.CommandType = CommandType.StoredProcedure;
                                                        cmd6.Parameters.Clear();
                                                        cmd6.Parameters.AddWithValue("@p_reference_number", reference_number);
                                                        cmd6.Parameters.AddWithValue("@p_school_id", school_id);
                                                        cmd6.Parameters.AddWithValue("@p_transaction_type_id", 5);
                                                        cmd6.Parameters.AddWithValue("@p_wallet_id", value.wallet_id);
                                                        cmd6.Parameters.AddWithValue("@p_total_amount", value.total_amount);
                                                        cmd6.Parameters.AddWithValue("@p_payment_method_id", value.payment_method_id);
                                                        cmd6.Parameters.AddWithValue("@p_status_id", 200);
                                                        cmd6.Parameters.AddWithValue("@p_create_by", value.create_by);
                                                        cmd6.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                                                        cmd6.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                                                        cmd6.ExecuteNonQuery();
                                                        trans.Commit();

                                                        var wallet_status = (string)cmd6.Parameters["@p_status_code"].Value;
                                                        int profile_id;

                                                        if (wallet_status == "record_saved")
                                                        {
                                                            trans.Dispose();

                                                            using (MySqlCommand cmd7 = new MySqlCommand("sp_get_merchant_profile_id", conn))
                                                            {
                                                                trans = conn.BeginTransaction();
                                                                cmd7.Transaction = trans;
                                                                cmd7.CommandType = CommandType.StoredProcedure;
                                                                cmd7.Parameters.Clear();
                                                                cmd7.Parameters.AddWithValue("@p_merchant_id", merchantID);
                                                                cmd7.Parameters.Add("@p_profile_id", MySqlDbType.Int16);
                                                                cmd7.Parameters["@p_profile_id"].Direction = ParameterDirection.Output;
                                                                cmd7.ExecuteNonQuery();
                                                                trans.Commit();

                                                                profile_id = Convert.ToInt16(cmd7.Parameters["@p_profile_id"].Value);
                                                            }

                                                            fcm.PurchaseOrderStatusNotification(profile_id, "New order received", value.create_by + " has placed a new order #" + reference_number + " total of RM" + value.total_amount.ToString("F"));

                                                            data.Success = true;
                                                            data.Code = "success";
                                                            data.Message = WebApiResources.YourOrderHasBeenReceived;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        data.Success = false;
                                        data.Code = "unsufficient_balance";
                                        data.Message = WebApiResources.UnsufficientWalletBalance;
                                    }
                                }
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "time_exceed";
                                data.Message = WebApiResources.OrderMustBePlacedBfor7;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "account/purchase");
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
        [Route("api/v2/account/school-info")]
        public IHttpActionResult Post_Account_School_Info(string culture, [FromBody] AccountCSInfoParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            //SchoolInfoModel data = new SchoolInfoModel();
            if (value.profile_id != 0 && value.user_role_id != 0)
            {
                List<AccountCSInfo> list = new List<AccountCSInfo>();
                AccountCSInfoModel listData = new AccountCSInfoModel();
                MySqlTransaction trans = null;
                listData.Success = false;
                string sqlQuery = string.Empty;
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_user_account_cs", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                            cmd.Parameters.AddWithValue("@p_user_role_id", value.user_role_id);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    AccountCSInfo prop = new AccountCSInfo();
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.email = dataReader["email"].ToString();
                                    prop.school_name = dataReader["school_name"].ToString();
                                    prop.coordinate = dataReader["coordinate"].ToString();
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "OK";
                                listData.Message = "Record found.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = WebApiResources.NoRecordFoundText;
                                listData.Data = list;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        //listData.Success = false;
                        listData.Success = false;
                        listData.Code = "error_occured";
                        listData.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "account/info");
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

                //if (listData.Success == true)
                return Ok(listData);

                //return Ok(data);
            }
            else
            {
                return BadRequest("Missing parameters.");
            }
        }
    }
}
