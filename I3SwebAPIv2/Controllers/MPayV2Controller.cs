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
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace I3SwebAPIv2.Controllers
{
    public class MPayV2Controller : ApiController
    {
        i3sAuth.Rijndael auth = new i3sAuth.Rijndael();
        MPayWallet mpay = new MPayWallet();
        public string salt = ConfigurationManager.AppSettings["passPhrase"];
        ////uat
        //public string PID = "100000119002620";
        //public string PartnerKey = "EN35PR90NWDVV2XG3YUAABCDUH8W1ABJIHKFD8MH1H2ZXJ8EZH";
        //production
        public string PID = "100000119012320";
        public string PartnerKey = "EN35PR90NWDVV2XG3YUAAAHNUH8W1ABJIHKFD8MH1H2ZXJ8EZH";
        public string authToken = "";
        public string timeStamp = "";
        public string messageBody = "";
        public String LRC = "";
        public char del = (char)30;

        [HttpPost]
        [Route("api/v2/mpay/get-lrc")]
        public IHttpActionResult Post_MPay_LRC([FromBody] LRCParam value)
        {
            WalletDataModel data = new WalletDataModel();
            if (!string.IsNullOrEmpty(value.lrc)) {

                string strLRC = GetLRC(value.lrc);

                data.Success = true;
                data.Code = "OK";
                data.Message = strLRC;
            }
            else
            {
                return BadRequest("Missing parameters.");
            }

            return Ok(data);
        }

        #region EKYC

            #endregion

        [HttpPost]
        [Route("api/v2/mpay/account-register")]
        public async Task<IHttpActionResult> Post_MPay_Account_Register(string culture, [FromBody] RegisterParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            DateTime dob = new DateTime();
            MOcean ocean = new MOcean();
            UsersV2Class cls = new UsersV2Class();
            DataChecker check = new DataChecker();
            RegisterDataModel data = new RegisterDataModel();
            if (value.full_name != null && value.nationality_id != 0 && value.card_type_id != 0 && value.identity_number != null && value.mobile_number != null
                && value.email != null && value.password != null && value.mother_maiden_name != null && value.occupation != null && value.employer_name != null
                && value.address != null && value.postcode != null && value.state_id != 0 && value.city != null && value.country_id != 0
                && value.date_of_birth != null && value.user_role_id != 0)
            {
                //check idenity number
                if (check.IsValidIdentity(value.card_type_id, check.RemoveNonAlphaNumeric(value.identity_number)) == true)
                {
                    if (value.card_type_id == 1)
                    {
                        int curYear = Convert.ToInt32(DateTime.Now.ToString("yy"));
                        int year = Convert.ToInt32(check.RemoveNonAlphaNumeric(value.identity_number).Substring(0, 2));
                        if (curYear <= year)
                        {
                            year = 1900 + year;
                        }
                        else
                        {
                            year = 2000 + year;
                        }
                        int month = Convert.ToInt32(check.RemoveNonAlphaNumeric(value.identity_number).Substring(2, 2));
                        int day = Convert.ToInt32(check.RemoveNonAlphaNumeric(value.identity_number).Substring(4, 2));

                        dob = new DateTime(year, month, day);
                    }

                    if (check.IsMobileNumber(value.mobile_number) == true)
                    {
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
                                trans = conn.BeginTransaction();

                                try
                                {
                                    cmd.Transaction = trans;
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddWithValue("@p_full_name", value.full_name);
                                    cmd.Parameters.AddWithValue("@p_nationality_id", value.nationality_id);
                                    cmd.Parameters.AddWithValue("@p_mother_maiden_name", value.mother_maiden_name);
                                    cmd.Parameters.AddWithValue("@p_card_type_id", value.card_type_id);
                                    cmd.Parameters.AddWithValue("@p_identity_number", check.RemoveNonAlphaNumeric(value.identity_number));
                                    //cmd.Parameters.AddWithValue("@p_date_of_birth", dob.ToString("yyyy-MM-dd"));
                                    if (value.card_type_id == 1)
                                    {
                                        cmd.Parameters.AddWithValue("@p_date_of_birth", dob.ToString("yyyy-MM-dd"));
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@p_date_of_birth", value.date_of_birth.Split('-')[2] + "-" + value.date_of_birth.Split('-')[1] + "-" + value.date_of_birth.Split('-')[0]);
                                    }
                                    cmd.Parameters.AddWithValue("@p_mobile_number", value.mobile_number);
                                    cmd.Parameters.AddWithValue("@p_email", value.email);
                                    cmd.Parameters.AddWithValue("@p_address", value.address);
                                    cmd.Parameters.AddWithValue("@p_postcode", value.postcode);
                                    cmd.Parameters.AddWithValue("@p_state_id", value.state_id);
                                    cmd.Parameters.AddWithValue("@p_city", value.city);
                                    cmd.Parameters.AddWithValue("@p_country_id", value.country_id);
                                    cmd.Parameters.AddWithValue("@p_password", auth.EncryptRijndael(value.password, salt));
                                    cmd.Parameters.AddWithValue("@p_otp", auth.EncryptRijndael(otp, salt));
                                    cmd.Parameters.AddWithValue("@p_occupation", value.occupation);
                                    cmd.Parameters.AddWithValue("@p_employer_name", value.employer_name);
                                    cmd.Parameters.AddWithValue("@p_marketing_flag", value.marketing_flag);
                                    cmd.Parameters.Add("@p_profile_id", MySqlDbType.Int16);
                                    cmd.Parameters["@p_profile_id"].Direction = ParameterDirection.Output;
                                    cmd.Parameters.Add("@p_user_status", MySqlDbType.Int16);
                                    cmd.Parameters["@p_user_status"].Direction = ParameterDirection.Output;
                                    cmd.ExecuteNonQuery();
                                    trans.Commit();

                                    int profile_id = Convert.ToInt32(cmd.Parameters["@p_profile_id"].Value);
                                    int user_status_id = Convert.ToInt32(cmd.Parameters["@p_user_status"].Value);

                                    if (value.user_role_id == 9)
                                    {
                                        string wallet_number = cls.GenerateWalletNumber();
                                        string token = cls.GenerateToken();

                                        using (MySqlCommand cmd2 = new MySqlCommand("sp_insert_parent_self_register", conn))
                                        {
                                            trans = conn.BeginTransaction();

                                            try
                                            {
                                                cmd2.Transaction = trans;
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
                                                trans.Commit();

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
                                                        //var result = ocean.Send_OTP(otp, "+601157746255"); // for testing only
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
                                                        ExceptionUtility.LogException(ex, "mpay/account-register");
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
                                                trans.Rollback();
                                                data.Success = false;
                                                data.Code = "error_occured";
                                                data.Message = WebApiResources.ErrorOccured;
                                                ExceptionUtility.LogException(ex, "mpay/account-register");
                                            }
                                        }
                                    }
                                    else
                                    {

                                        try
                                        {
                                            var result = ocean.Send_OTP(otp, value.mobile_number);
                                            //var result = ocean.Send_OTP(otp, "+60169050115"); // for testing only
                                            //var result = ocean.Send_OTP(otp, "+601157746255"); // for testing only
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
                                            ExceptionUtility.LogException(ex, "mpay/account-register");
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    trans.Rollback();
                                    data.Success = false;
                                    data.Code = "error_occured";
                                    data.Message = WebApiResources.ErrorOccured;
                                    ExceptionUtility.LogException(ex, "mpay/account-register");
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
            }
            else
            {
                return BadRequest("Missing parameters.");
            }
            return Ok(data);
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/mpay/resubmit-kyc")]
        public async Task<IHttpActionResult> Post_MPay_Account_Resubmit_KYC(string culture, [FromBody] ResubmitParam value)
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
                                    prop.mothermaidenname = dataReader["mother_maiden_name"].ToString();
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

                                    var result = mpay.PostResubmitKYC(prop);
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
                            ExceptionUtility.LogException(ex, "mpay/resubmit-kyc");
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
        //[Authorize]
        [Route("api/v2/mpay/change-pin")]
        public async Task<IHttpActionResult> Post_MPay_Change_Pin(string culture, [FromBody] ChangePinParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            LoginDataModel data = new LoginDataModel();
            if (value.username != null && value.new_pin != null && value.update_by != null)
            {
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    conn.Open();
                    ChangePINParam prop = new ChangePINParam();

                    prop.newPin = value.new_pin;

                    using (MySqlCommand cmd = new MySqlCommand("sp_get_wallet_info", conn))
                    {
                        MySqlTransaction trans;
                        trans = conn.BeginTransaction();

                        try
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_username", value.username);
                            cmd.Parameters.AddWithValue("@p_wallet_id", 0);
                            MySqlDataReader dataReader = cmd.ExecuteReader();


                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    prop.wallet_id = dataReader["wallet_id"].ToString();
                                    prop.uid = dataReader["mpay_uid"].ToString();
                                    prop.card_id = dataReader["mpay_card_id"].ToString();
                                    prop.cardtoken = dataReader["mpay_card_token"].ToString();
                                    //prop.oldPin = "123456";
                                    if (!string.IsNullOrEmpty(dataReader["mpay_card_pin"].ToString()))
                                    {
                                        prop.oldPin = auth.DecryptRijndael(dataReader["mpay_card_pin"].ToString(), salt);
                                    }
                                    else
                                    {
                                        prop.oldPin = "";
                                    }
                                }
                            }

                            var result = mpay.PostChangePIN(prop);
                            string jsonStr = await result;
                            ChangPINModel response = JsonConvert.DeserializeObject<ChangPINModel>(jsonStr);

                            if (response.Header.status == "00")
                            {
                                BodyPin acc = response.Body;

                                if (acc.pin_change_status == "00")
                                {
                                    if (dataReader != null)
                                    {
                                        dataReader.Close();
                                    }

                                    using (MySqlCommand cmd2 = new MySqlCommand("sp_update_mpay_card_pin", conn))
                                    {

                                        try
                                        {
                                            cmd2.Transaction = trans;
                                            cmd2.CommandType = CommandType.StoredProcedure;
                                            cmd2.Parameters.AddWithValue("@p_wallet_id", prop.wallet_id);
                                            cmd2.Parameters.AddWithValue("@p_mpay_uid", prop.uid);
                                            cmd2.Parameters.AddWithValue("@p_mpay_card_id", prop.card_id);
                                            cmd2.Parameters.AddWithValue("@p_mpay_card_pin", auth.EncryptRijndael(prop.newPin, salt));
                                            cmd2.Parameters.AddWithValue("@p_update_by", value.update_by);
                                            cmd2.ExecuteNonQuery();
                                            trans.Commit();
                                        }
                                        catch (Exception ex)
                                        {
                                            trans.Rollback();
                                            data.Success = false;
                                            data.Code = "error_occured";
                                            data.Message = WebApiResources.ErrorOccured;
                                            ExceptionUtility.LogException(ex, "mpay/change-pin");
                                        }

                                        data.Success = true;
                                        data.Message = WebApiResources.CardPinChangeSuccessText;
                                    }
                                }
                                else
                                {
                                    data.Success = false;
                                    data.Message = response.Header.message;
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
                            ExceptionUtility.LogException(ex, "mpay/change-pin");
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
        [Route("api/v2/mpay/topup")]
        public async Task<IHttpActionResult> Post_MPay_Account_Topup(string culture, [FromBody] WalletTopupParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            string reference_number = string.Empty;
            WalletV2Class cls = new WalletV2Class();
            WalletDataModel data = new WalletDataModel();
            if (value.wallet_id != 0 && !string.IsNullOrEmpty(value.wallet_number) && value.topup_amount != 0 && value.topup_date != null && !string.IsNullOrEmpty(value.create_by))
            {

                data.Success = false;
                string sqlQuery = string.Empty;
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                MySqlTransaction trans = null;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();

                        reference_number = "TPP-" + cls.GetAlphaUniqueKey() + "-" + cls.GetNumericUniqueKey() + "-" + cls.GetNumericUniqueKey();

                        using (MySqlCommand cmd = new MySqlCommand("sp_insert_wallet_topup", conn))
                        {
                            trans = conn.BeginTransaction();
                            cmd.Transaction = trans;

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_wallet_id", value.wallet_id);
                            cmd.Parameters.AddWithValue("@p_wallet_number", value.wallet_number);
                            cmd.Parameters.AddWithValue("@p_topup_amount", value.topup_amount);
                            cmd.Parameters.AddWithValue("@p_topup_date", value.topup_date.ToString("yyyy-MM-dd HH:mm:ss"));
                            cmd.Parameters.AddWithValue("@p_reference_number", reference_number);
                            cmd.Parameters.AddWithValue("@p_create_by", value.create_by);
                            cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                            cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            //trans.Commit();

                            var status_code = (string)cmd.Parameters["@p_status_code"].Value;

                            if (status_code == "invalid_wallet_number")
                            {
                                data.Success = false;
                                data.Code = "invalid_wallet_number";
                                data.Message = WebApiResources.InvalidWalletNo;
                            }
                            else if (status_code == "invalid_amount")
                            {
                                data.Success = false;
                                data.Code = "invalid_amount";
                                data.Message = WebApiResources.InvalidAmount;
                            }
                            else if (status_code == "processing")
                            {
                                AccountTopupParam prop = new AccountTopupParam();

                                //prop.amount = (Math.Truncate(100 * value.topup_amount) / 100).ToString();
                                prop.amount = String.Format("{0:0}", value.topup_amount * 100);
                                prop.channel = "1";

                                using (MySqlCommand cmd2 = new MySqlCommand("sp_get_wallet_info", conn))
                                {
                                    cmd2.Transaction = trans;

                                    try
                                    {
                                        cmd2.CommandType = CommandType.StoredProcedure;
                                        cmd2.Parameters.Clear();
                                        cmd2.Parameters.AddWithValue("@p_username", "");
                                        cmd2.Parameters.AddWithValue("@p_wallet_id", value.wallet_id);
                                        MySqlDataReader dataReader = cmd2.ExecuteReader();


                                        if (dataReader.HasRows == true)
                                        {
                                            while (dataReader.Read())
                                            {
                                                prop.uid = dataReader["mpay_uid"].ToString();
                                                prop.cardtoken = dataReader["mpay_card_token"].ToString();
                                            }
                                        }

                                        if (dataReader != null)
                                        {
                                            dataReader.Close();
                                        }

                                        var result = mpay.PostAccountTopup(prop);
                                        string jsonStr = await result;
                                        AccountTopupModel response = JsonConvert.DeserializeObject<AccountTopupModel>(jsonStr);

                                        if (response.Header.status == "00")
                                        {
                                            TopupInfo acc = response.Body.topupinfo;

                                            using (MySqlCommand cmd3 = new MySqlCommand("sp_update_wallet_reference", conn))
                                            {
                                                //trans = conn.BeginTransaction();
                                                try
                                                {
                                                    cmd3.Transaction = trans;
                                                    cmd3.CommandType = CommandType.StoredProcedure;
                                                    cmd3.Parameters.AddWithValue("@p_reference_id", acc.reference_id);
                                                    cmd3.Parameters.AddWithValue("@p_reference_number", reference_number);
                                                    cmd3.Parameters.AddWithValue("@p_wallet_id", value.wallet_id);
                                                    cmd3.Parameters.AddWithValue("@p_mpay_uid", prop.uid);
                                                    cmd3.Parameters.AddWithValue("@p_update_by", value.create_by);
                                                    cmd3.ExecuteNonQuery();
                                                    trans.Commit();

                                                    data.Success = true;
                                                    data.Message = acc.topupurl;
                                                    data.Code = acc.reference_id;
                                                }
                                                catch (Exception ex)
                                                {
                                                    trans.Rollback();
                                                    data.Success = false;
                                                    data.Code = "error_occured";
                                                    data.Message = WebApiResources.ErrorOccured;
                                                    ExceptionUtility.LogException(ex, "mpay/topup");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            data.Success = false;
                                            //if (culture == "ms-MY")
                                            //{
                                            //    data.Message = TranslateWithGoogle(response.Header.message, "en|ms");
                                            //}
                                            //else
                                            //{
                                            data.Message = response.Header.message;
                                            //}
                                            data.Code = response.Header.status;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        trans.Rollback();
                                        data.Success = false;
                                        data.Code = "error_occured";
                                        data.Message = WebApiResources.ErrorOccured;
                                        ExceptionUtility.LogException(ex, "mpay/topup");
                                    }
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
                        ExceptionUtility.LogException(ex, "mpay/topup");
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
        [Route("api/v2/mpay/topup-status")]
        public async Task<IHttpActionResult> Post_MPay_Account_Topup_Status(string culture, [FromBody] MPayTopupStatusParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            int wallet_id = 0;
            WalletDataModel data = new WalletDataModel();

            if (!string.IsNullOrEmpty(value.username) && !string.IsNullOrEmpty(value.update_by))
            {
                data.Success = false;
                string sqlQuery = string.Empty;
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                MySqlTransaction trans = null;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        MPayBalanceParam prop = new MPayBalanceParam();

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_wallet_info", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_username", value.username);
                            cmd.Parameters.AddWithValue("@p_wallet_id", 0);
                            MySqlDataReader dataReader = cmd.ExecuteReader();


                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    prop.uid = dataReader["mpay_uid"].ToString();
                                    prop.cardtoken = dataReader["mpay_card_token"].ToString();
                                    wallet_id = Convert.ToInt32(dataReader["wallet_id"].ToString());
                                }
                            }

                            if (dataReader != null)
                            {
                                dataReader.Close();
                            }

                            var result = mpay.PostCardBalance(prop);
                            string jsonStr = await result;
                            CardBalanceModel response = JsonConvert.DeserializeObject<CardBalanceModel>(jsonStr);

                            if (response.Header.status == "00")
                            {
                                CardList acc = response.Body.cardlist;

                                CardList card = new CardList();
                                card.accountType = acc.accountType;
                                card.accountGroup = acc.accountGroup;
                                card.accountTypeName = acc.accountTypeName;
                                card.cardno = acc.cardno;
                                card.balance = acc.balance;
                                card.accStatus = acc.accStatus;
                                card.name = acc.name;

                                using (MySqlCommand cmd2 = new MySqlCommand("sp_update_wallet_topup_status", conn))
                                {

                                    cmd2.Transaction = trans;
                                    cmd2.CommandType = CommandType.StoredProcedure;
                                    cmd2.Parameters.Clear();
                                    cmd2.Parameters.AddWithValue("@p_wallet_id", wallet_id);
                                    cmd2.Parameters.AddWithValue("@p_reference_id", value.reference_id);
                                    cmd2.Parameters.AddWithValue("@p_mpay_uid", prop.uid);
                                    cmd2.Parameters.AddWithValue("@p_mpay_mask_cardno", card.cardno);
                                    cmd2.Parameters.AddWithValue("@p_balance", card.balance);
                                    cmd2.Parameters.AddWithValue("@p_update_by", value.update_by);
                                    cmd2.ExecuteNonQuery();
                                    trans.Commit();

                                    data.Success = true;
                                    data.Code = response.Header.status;
                                    //if (culture == "ms-MY")
                                    //{
                                    //    data.Message = TranslateWithGoogle(response.Header.message, "en|ms");
                                    //}
                                    //else
                                    //{
                                    data.Message = response.Header.message;
                                    //}
                                }
                            }
                            else
                            {
                                data.Success = false;
                                if (culture == "ms-MY")
                                    //if (culture == "ms-MY")
                                    //{
                                    //    data.Message = TranslateWithGoogle(response.Header.message, "en|ms");
                                    //}
                                    //else
                                    //{
                                    data.Message = response.Header.message;
                                //}
                                data.Code = response.Header.status;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "mpay/topup-status");
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
        [Route("api/v2/mpay/add-virtual-balance")]
        public async Task<IHttpActionResult> Post_MPay_Add_Virtual_Balance(string culture, [FromBody] RegisterStudentParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            ParentDataModel data = new ParentDataModel();
            if (value.parent_id != 0 && value.student_id != 0 && value.school_id != 0 && value.class_id != 0 && value.create_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_insert_parent_register_student", conn))
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
                            cmd.Parameters.Add("@p_mpay_uid", MySqlDbType.Int32);
                            cmd.Parameters["@p_mpay_uid"].Direction = ParameterDirection.Output;
                            cmd.Parameters.Add("@p_wallet_id", MySqlDbType.Int32); //student wallet_id
                            cmd.Parameters["@p_wallet_id"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();

                            var status_code = (string)cmd.Parameters["@p_status_code"].Value;
                            if (DBNull.Value != cmd.Parameters["@p_mpay_uid"].Value)
                            {
                                uid = Convert.ToInt32(cmd.Parameters["@p_mpay_uid"].Value);
                                prop.uid = uid.ToString();
                            }

                            var wallet_id = Convert.ToInt32(cmd.Parameters["@p_wallet_id"].Value);

                            if (status_code == "record_saved")
                            {

                                var result = mpay.PostAddVirtualBalance(prop);
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
                                                data.Message = WebApiResources.ParentStudentRelationshipCreateSuccess;
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
                                            ExceptionUtility.LogException(ex, "mpay/add-virtual-balance");
                                        }
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
                        ExceptionUtility.LogException(ex, "mpay/add-virtual-balance");
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

        [HttpPost]
        [Authorize]
        [Route("api/v2/mpay/transfer")]
        public async Task<IHttpActionResult> Post_MPay_Fund_Transfer(string culture, [FromBody] WalletTransferParam value)
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
                !string.IsNullOrEmpty(value.ip_address) && !string.IsNullOrEmpty(value.create_by))
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
                            cmd.Parameters.AddWithValue("@p_wallet_id", value.wallet_id); // source_wallet_id
                            cmd.Parameters.AddWithValue("@p_wallet_number", value.wallet_number);
                            cmd.Parameters.AddWithValue("@p_recipient_id", value.recipient_id); // destination_wallet_id
                            cmd.Parameters.AddWithValue("@p_recipient_wallet", value.recipient_wallet);
                            cmd.Parameters.AddWithValue("@p_transfer_amount", value.transfer_amount);
                            cmd.Parameters.AddWithValue("@p_transfer_date", value.transfer_date.ToString("yyyy-MM-dd HH:mm:ss"));
                            cmd.Parameters.AddWithValue("@p_reference_number", reference_number);
                            cmd.Parameters.AddWithValue("@p_create_by", value.create_by);
                            cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                            cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            //trans.Commit();

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
                            else if (status_code == "processing")
                            {
                                FundTransferParam prop = new FundTransferParam();

                                using (MySqlCommand cmd2 = new MySqlCommand("sp_get_wallet_info", conn))
                                {
                                    cmd2.Transaction = trans;

                                    try
                                    {
                                        cmd2.CommandType = CommandType.StoredProcedure;
                                        cmd2.Parameters.Clear();
                                        cmd2.Parameters.AddWithValue("@p_username", "");
                                        cmd2.Parameters.AddWithValue("@p_wallet_id", value.wallet_id);
                                        MySqlDataReader dataReader = cmd2.ExecuteReader();


                                        if (dataReader.HasRows == true)
                                        {
                                            while (dataReader.Read())
                                            {
                                                prop.uid = dataReader["mpay_uid"].ToString();
                                                prop.source_cardtoken = dataReader["mpay_card_token"].ToString();
                                                prop.cardpin = auth.DecryptRijndael(dataReader["mpay_card_pin"].ToString(),salt);
                                            }
                                        }

                                        if (dataReader != null)
                                        {
                                            dataReader.Close();
                                        }

                                        cmd2.CommandType = CommandType.StoredProcedure;
                                        cmd2.Parameters.Clear();
                                        cmd2.Parameters.AddWithValue("@p_username", "");
                                        cmd2.Parameters.AddWithValue("@p_wallet_id", value.recipient_id);
                                        MySqlDataReader dataReader2 = cmd2.ExecuteReader();


                                        if (dataReader2.HasRows == true)
                                        {
                                            while (dataReader2.Read())
                                            {
                                                prop.destination_cardtoken = dataReader2["mpay_card_token"].ToString();
                                            }
                                        }

                                        if (dataReader2 != null)
                                        {
                                            dataReader2.Close();
                                        }

                                        string hostName = Dns.GetHostName(); // Retrive the Name of HOST
                                        if (value.ip_address == "0.0.0.0")
                                        {
                                            prop.ip_address = Dns.GetHostByName(hostName).AddressList[0].ToString();
                                        }
                                        else {
                                            prop.ip_address = value.ip_address;
                                        }

                                        prop.remark = reference_number;
                                        prop.amount = value.transfer_amount.ToString("F");

                                        var result = mpay.PostFundTransfer(prop);
                                        string jsonStr = await result;
                                        FundTransferModel response = JsonConvert.DeserializeObject<FundTransferModel>(jsonStr);


                                        BodyTransfer body = new BodyTransfer();

                                        if (response.Header.status == "00")
                                        {
                                            body = response.Body;
                                            data.Success = true;
                                            data.Code = "success";
                                            data.Message = WebApiResources.YourTransferSuccess;
                                        }
                                        else 
                                        {
                                            data.Success = false;
                                            body = response.Body;
                                            data.Code = response.Header.status;
                                            data.Message = response.Header.message;
                                        }

                                        using (MySqlCommand cmd3 = new MySqlCommand("sp_update_wallet_transfer", conn))
                                        {
                                            try
                                            {
                                                cmd3.Transaction = trans;
                                                cmd3.CommandType = CommandType.StoredProcedure;
                                                cmd3.Parameters.Clear();
                                                cmd3.Parameters.AddWithValue("@p_status", response.Header.status);
                                                cmd3.Parameters.AddWithValue("@p_status_description", response.Header.message);
                                                cmd3.Parameters.AddWithValue("@p_reference_number", reference_number);
                                                cmd3.Parameters.AddWithValue("@p_wallet_id", value.wallet_id);
                                                cmd3.Parameters.AddWithValue("@p_transfer_amount", value.transfer_amount);
                                                cmd3.Parameters.AddWithValue("@p_recipient_id", value.recipient_id);
                                                cmd3.Parameters.AddWithValue("@p_sourceAccount", body.sourceAccount);
                                                cmd3.Parameters.AddWithValue("@p_destinationAccount", body.destinationAccount);
                                                cmd3.Parameters.AddWithValue("@p_transfer_in_reference_id", body.transfer_in_reference_id);
                                                cmd3.Parameters.AddWithValue("@p_transfer_out_reference_id", body.transfer_out_reference_id);
                                                if (!string.IsNullOrEmpty(body.trx_date))
                                                {
                                                    cmd3.Parameters.AddWithValue("@p_trx_date", body.trx_date);
                                                }
                                                else {
                                                    cmd3.Parameters.AddWithValue("@p_trx_date", DateTime.Now);
                                                }
                                                if (!string.IsNullOrEmpty(body.amount))
                                                {
                                                    cmd3.Parameters.AddWithValue("@p_amount", body.amount);
                                                }
                                                else
                                                {
                                                    cmd3.Parameters.AddWithValue("@p_amount", 0);
                                                }

                                                cmd3.Parameters.AddWithValue("@p_update_by", value.create_by);
                                                cmd3.ExecuteNonQuery();
                                                trans.Commit();

                                            }
                                            catch (Exception ex)
                                            {
                                                data.Success = false;
                                                data.Code = "error_occured";
                                                data.Message = WebApiResources.ErrorOccured;
                                                ExceptionUtility.LogException(ex, "mpay/transfer");
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        data.Success = false;
                                        data.Code = "error_occured";
                                        data.Message = WebApiResources.ErrorOccured;
                                        ExceptionUtility.LogException(ex, "mpay/transfer");
                                    }
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
                        ExceptionUtility.LogException(ex, "mpay/transfer");
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
        [Route("api/v2/mpay/card-info")]
        public async Task<IHttpActionResult> Post_MPay_Card_Info(string culture, [FromBody] CardBalanceParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            WalletDataModel data = new WalletDataModel();
            List<CardList> list = new List<CardList>();
            CardListModel listData = new CardListModel();

            listData.Success = false;

            if (!string.IsNullOrEmpty(value.username))
            {
                data.Success = false;
                string sqlQuery = string.Empty;
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                MySqlTransaction trans = null;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        MPayBalanceParam prop = new MPayBalanceParam();

                        using (MySqlCommand cmd2 = new MySqlCommand("sp_get_wallet_info", conn))
                        {
                            try
                            {
                                cmd2.Transaction = trans;
                                cmd2.CommandType = CommandType.StoredProcedure;
                                cmd2.Parameters.Clear();
                                cmd2.Parameters.AddWithValue("@p_username", value.username);
                                cmd2.Parameters.AddWithValue("@p_wallet_id", 0);
                                MySqlDataReader dataReader = cmd2.ExecuteReader();


                                if (dataReader.HasRows == true)
                                {
                                    while (dataReader.Read())
                                    {
                                        prop.uid = dataReader["mpay_uid"].ToString();
                                        prop.cardtoken = dataReader["mpay_card_token"].ToString();
                                    }
                                }

                                if (dataReader != null)
                                {
                                    dataReader.Close();
                                }

                                var result = mpay.PostCardBalance(prop);
                                string jsonStr = await result;
                                CardBalanceModel response = JsonConvert.DeserializeObject<CardBalanceModel>(jsonStr);

                                if (response.Header.status == "00")
                                {
                                    CardList acc = response.Body.cardlist;

                                    CardList card = new CardList();
                                    card.accountType = acc.accountType;
                                    card.accountGroup = acc.accountGroup;
                                    card.accountTypeName = acc.accountTypeName;
                                    card.cardno = acc.cardno;
                                    card.balance = acc.balance;
                                    card.accStatus = acc.accStatus;
                                    card.name = acc.name;
                                    list.Add(card);


                                    listData.Success = true;
                                    listData.Code = response.Header.status;
                                    //if (culture == "ms-MY")
                                    //{
                                    //    listData.Message = TranslateWithGoogle(response.Header.message, "en|ms");
                                    //}
                                    //else
                                    //{
                                    listData.Message = response.Header.message;
                                    //}
                                    listData.Data = list;
                                }
                                else
                                {
                                    data.Success = false;
                                    //if (culture == "ms-MY")
                                    //{
                                    //    data.Message = TranslateWithGoogle(response.Header.message, "en|ms");
                                    //}
                                    //else
                                    //{
                                    data.Message = response.Header.message;
                                    //}
                                    data.Code = response.Header.status;
                                }
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                data.Success = false;
                                data.Code = "error_occured";
                                data.Message = WebApiResources.ErrorOccured;
                                ExceptionUtility.LogException(ex, "mpay/card-info");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "mpay/card-info");
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
        [Route("api/v2/mpay/account-info")]
        public async Task<IHttpActionResult> Post_MPay_Account_Info(string culture, [FromBody] AccountInfoParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            WalletDataModel data = new WalletDataModel();
            List<AccInfo> list = new List<AccInfo>();
            AccInfoDataModel listData = new AccInfoDataModel();

            listData.Success = false;

            if (!string.IsNullOrEmpty(value.username))
            {
                MySqlTransaction trans = null;
                data.Success = false;
                string sqlQuery = string.Empty;
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    conn.Open();
                    GetAccountInfoParam prop = new GetAccountInfoParam();

                    using (MySqlCommand cmd2 = new MySqlCommand("sp_get_wallet_info", conn))
                    {
                        try
                        {
                            cmd2.Transaction = trans;
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.Clear();
                            cmd2.Parameters.AddWithValue("@p_username", value.username);
                            cmd2.Parameters.AddWithValue("@p_wallet_id", 0);
                            MySqlDataReader dataReader = cmd2.ExecuteReader();


                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    prop.uid = dataReader["mpay_uid"].ToString();
                                }
                            }

                            if (dataReader != null)
                            {
                                dataReader.Close();
                            }

                            var result = mpay.PostGetAccountInfo(prop);
                            string jsonStr = await result;
                            AccountInfoModel response = JsonConvert.DeserializeObject<AccountInfoModel>(jsonStr);

                            if (response.Header.status == "00")
                            {
                                AccInfo acc = response.Body.accountinfo;

                                AccInfo info = new AccInfo();
                                info.countrylookup_id = acc.countrylookup_id;
                                info.mobile_no = acc.mobile_no;
                                info.id_no = acc.id_no;
                                info.dob = acc.dob;
                                info.name = acc.name;
                                info.userstatuslookup_id = acc.userstatuslookup_id;
                                info.kycstatuslookup_id = acc.kycstatuslookup_id;
                                info.docstatuslookup_id = acc.docstatuslookup_id;
                                info.occupation = acc.occupation;
                                info.occupation_flag = acc.occupation_flag;
                                info.address_flag = acc.address_flag;
                                list.Add(info);


                                listData.Success = true;
                                listData.Code = response.Header.status;
                                //if (culture == "ms-MY")
                                //{
                                //    listData.Message = TranslateWithGoogle(response.Header.message, "en|ms");
                                //}
                                //else
                                //{
                                listData.Message = response.Header.message;
                                //}
                                listData.Data = list;
                            }
                            else
                            {
                                data.Success = false;
                                //if (culture == "ms-MY")
                                //{
                                //    data.Message = TranslateWithGoogle(response.Header.message, "en|ms");
                                //}
                                //else
                                //{
                                data.Message = response.Header.message;
                                //}
                                data.Code = response.Header.status;
                            }
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            data.Success = false;
                            data.Code = "error_occured";
                            data.Message = WebApiResources.ErrorOccured;
                            ExceptionUtility.LogException(ex, "mpay/account-info");
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
        [Route("api/v2/mpay/trx-history")]
        public async Task<IHttpActionResult> Post_MPay_Trx_History(string culture, [FromBody] AccountInfoParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            WalletDataModel data = new WalletDataModel();
            List<TrxHist> list = new List<TrxHist>();
            TrxHistoryDataModel listData = new TrxHistoryDataModel();

            listData.Success = false;

            if (!string.IsNullOrEmpty(value.username))
            {
                MySqlTransaction trans = null;
                data.Success = false;
                string sqlQuery = string.Empty;
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    conn.Open();
                    GetTrxHistoryParam prop = new GetTrxHistoryParam();

                    using (MySqlCommand cmd2 = new MySqlCommand("sp_get_wallet_info", conn))
                    {
                        try
                        {
                            cmd2.Transaction = trans;
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.Clear();
                            cmd2.Parameters.AddWithValue("@p_username", value.username);
                            cmd2.Parameters.AddWithValue("@p_wallet_id", 0);
                            MySqlDataReader dataReader = cmd2.ExecuteReader();


                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    prop.uid = dataReader["mpay_uid"].ToString();
                                    prop.cardtoken = dataReader["mpay_card_token"].ToString();
                                    prop.cardpin = auth.DecryptRijndael(dataReader["mpay_card_pin"].ToString(), salt);
                                }
                            }

                            if (dataReader != null)
                            {
                                dataReader.Close();
                            }

                            var result = mpay.PostGetTrxHistory(prop);
                            string jsonStr = await result;
                            TrxHistoryModel response = JsonConvert.DeserializeObject<TrxHistoryModel>(jsonStr);

                            if (response.Header.status == "00")
                            {
                                foreach (TrxHist sl in response.Body.trxhist_info) 
                                {
                                    TrxHist info = new TrxHist();
                                    info.datetimecreated = sl.datetimecreated;
                                    info.trx_detail = sl.trx_detail;
                                    info.credit_amount = sl.credit_amount;
                                    info.debit_amount = sl.debit_amount;
                                    info.Currency = sl.Currency;
                                    info.masked_acc_no = sl.masked_acc_no;
                                    list.Add(info);
                                }

                                listData.Success = true;
                                listData.Code = response.Header.status;
                                //if (culture == "ms-MY")
                                //{
                                //    listData.Message = TranslateWithGoogle(response.Header.message, "en|ms");
                                //}
                                //else
                                //{
                                listData.Message = response.Header.message;
                                //}
                                listData.Data = list;
                            }
                            else
                            {
                                data.Success = false;
                                //if (culture == "ms-MY")
                                //{
                                //    data.Message = TranslateWithGoogle(response.Header.message, "en|ms");
                                //}
                                //else
                                //{
                                data.Message = response.Header.message;
                                //}
                                data.Code = response.Header.status;
                            }
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            data.Success = false;
                            data.Code = "error_occured";
                            data.Message = WebApiResources.ErrorOccured;
                            ExceptionUtility.LogException(ex, "mpay/trx-history");
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
        [Route("api/v2/mpay/purchase")]
        public async Task<IHttpActionResult> Post_MPay_Purchase(string culture, [FromBody] CheckOutParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            //DoPaymentParam param = new DoPaymentParam();
            DoPaymentWithMIDParam param = new DoPaymentWithMIDParam();
            FirebaseCloudMessagingPush fcm = new FirebaseCloudMessagingPush();
            ReferenceV2Class cls = new ReferenceV2Class();
            PurchaseDataModel data = new PurchaseDataModel();
            if (value.profile_id != 0 && value.wallet_id != 0 && value.order_status_id != 0 && value.user_role_id != 0 &&
                value.sub_total_amount != 0 && value.tax_amount != 0 && value.total_amount != 0 && value.payment_method_id != 0 && value.create_by != null)
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
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd2 = new MySqlCommand("sp_get_order_cart", conn))
                        {
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
                                    prop.tax_amount = Convert.ToDecimal(dataReader["tax_amount"]);
                                    prop.total_amount = Convert.ToDecimal(dataReader["total_amount"]);
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

                            if (dataReader != null)
                                dataReader.Close();

                            if (proceed == true)
                            {
                                using (MySqlCommand cmd3 = new MySqlCommand("sp_get_wallet_detail", conn))
                                {
                                    //trans = conn.BeginTransaction();
                                    cmd3.Transaction = trans;
                                    cmd3.CommandType = CommandType.StoredProcedure;
                                    cmd3.Parameters.Clear();
                                    cmd3.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                                    cmd3.Parameters.AddWithValue("@p_wallet_id", value.wallet_id);
                                    cmd3.Parameters.Add("@p_account_balance", MySqlDbType.Decimal);
                                    cmd3.Parameters["@p_account_balance"].Direction = ParameterDirection.Output;
                                    cmd3.Parameters.Add("@p_mpay_uid", MySqlDbType.VarChar);
                                    cmd3.Parameters["@p_mpay_uid"].Direction = ParameterDirection.Output;
                                    cmd3.Parameters.Add("@p_mpay_card_token", MySqlDbType.VarChar);
                                    cmd3.Parameters["@p_mpay_card_token"].Direction = ParameterDirection.Output;
                                    cmd3.Parameters.Add("@p_mpay_card_pin", MySqlDbType.VarChar);
                                    cmd3.Parameters["@p_mpay_card_pin"].Direction = ParameterDirection.Output;
                                    cmd3.Parameters.Add("@p_school_id", MySqlDbType.Int16);
                                    cmd3.Parameters["@p_school_id"].Direction = ParameterDirection.Output;
                                    cmd3.ExecuteNonQuery();

                                    var account_balance = Convert.ToDecimal(cmd3.Parameters["@p_account_balance"].Value);

                                    if (DBNull.Value != cmd3.Parameters["@p_mpay_uid"].Value) 
                                    {
                                        param.uid = cmd3.Parameters["@p_mpay_uid"].Value.ToString();
                                    }

                                    if (DBNull.Value != cmd3.Parameters["@p_mpay_card_token"].Value)
                                    {
                                        param.cardtoken = cmd3.Parameters["@p_mpay_card_token"].Value.ToString();
                                    }

                                    if (DBNull.Value != cmd3.Parameters["@p_mpay_card_pin"].Value)
                                    {
                                        if (!string.IsNullOrEmpty(cmd3.Parameters["@p_mpay_card_pin"].Value.ToString()))
                                        {
                                            param.cardpin = auth.DecryptRijndael(cmd3.Parameters["@p_mpay_card_pin"].Value.ToString(), salt);
                                        }
                                        else {
                                            param.cardpin = "";
                                        }
                                    }

                                    var school_id = Convert.ToInt16(cmd3.Parameters["@p_school_id"].Value);

                                    bool proceedPay = true;

                                    if (value.total_amount > account_balance)
                                    {
                                        if (value.payment_method_id == 1)
                                        {
                                            if (!string.IsNullOrEmpty(param.uid) && !string.IsNullOrEmpty(param.cardpin) && !string.IsNullOrEmpty(param.cardtoken))
                                            {
                                                if (value.payment_method_id == 1) 
                                                {
                                                    proceedPay = false;
                                                }
                                            }
                                            else {
                                                proceedPay = false;
                                            }
                                        }
                                    }

                                    if (proceedPay == true)
                                    {
                                        reference_number = "PAY-I3S-" + cls.GetNumericUniqueKey() + "-" + cls.GetNumericUniqueKey();
                                        //trans.Dispose();
                                        param.referenceno = reference_number;
                                        //param.amount = String.Format("{0:0}", value.total_amount * 100);

                                        using (MySqlCommand cmd4 = new MySqlCommand("sp_insert_order_master", conn))
                                        {
                                            //trans = conn.BeginTransaction();
                                            cmd4.Transaction = trans;
                                            cmd4.CommandType = CommandType.StoredProcedure;
                                            cmd4.Parameters.Clear();
                                            cmd4.Parameters.AddWithValue("@p_reference_number", reference_number);
                                            cmd4.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                                            cmd4.Parameters.AddWithValue("@p_wallet_id", value.wallet_id);
                                            cmd4.Parameters.AddWithValue("@p_user_role_id", value.user_role_id);
                                            cmd4.Parameters.AddWithValue("@p_sub_total_amount", value.sub_total_amount);
                                            cmd4.Parameters.AddWithValue("@p_tax_amount", value.tax_amount);
                                            cmd4.Parameters.AddWithValue("@p_total_amount", value.total_amount);
                                            cmd4.Parameters.AddWithValue("@p_payment_method_id", value.payment_method_id);
                                            cmd4.Parameters.AddWithValue("@p_received_amount", value.total_amount);
                                            cmd4.Parameters.AddWithValue("@p_create_by", value.create_by);
                                            cmd4.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                                            cmd4.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                                            cmd4.Parameters.Add("@p_order_id", MySqlDbType.Int16);
                                            cmd4.Parameters["@p_order_id"].Direction = ParameterDirection.Output;
                                            cmd4.ExecuteNonQuery();

                                            var order_status = (string)cmd4.Parameters["@p_status_code"].Value;
                                            var _order_id = Convert.ToInt16(cmd4.Parameters["@p_order_id"].Value);


                                            if (order_status == "record_saved")
                                            {
                                                int detail_list = 0;
                                                int saved_count = 0;
                                                string detail_status = string.Empty;
                                                string product_desc = string.Empty;
                                                detail_list = list.Count;
                                                foreach (Purchase item in list)
                                                {
                                                    //trans.Dispose();
                                                    string product_name = string.Empty;

                                                    using (MySqlCommand cmd5 = new MySqlCommand("sp_insert_order_detail", conn))
                                                    {
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
                                                        cmd5.Parameters.Add("@p_product_name", MySqlDbType.VarChar);
                                                        cmd5.Parameters["@p_product_name"].Direction = ParameterDirection.Output;
                                                        cmd5.ExecuteNonQuery();

                                                        detail_status = (string)cmd5.Parameters["@p_status_code"].Value;
                                                        product_name = (string)cmd5.Parameters["@p_product_name"].Value;
                                                    }

                                                    //product_desc += product_name + ", ";

                                                    saved_count++;
                                                }

                                                //param.productdescription = product_desc.Remove(product_desc.Length - 2);

                                                if (detail_list == saved_count)
                                                {
                                                    //trans.Dispose();

                                                    using (MySqlCommand cmd6 = new MySqlCommand("sp_insert_wallet_transaction_history", conn))
                                                    {
                                                        //trans = conn.BeginTransaction();
                                                        cmd6.Transaction = trans;
                                                        cmd6.CommandType = CommandType.StoredProcedure;
                                                        cmd6.Parameters.Clear();
                                                        cmd6.Parameters.AddWithValue("@p_reference_number", reference_number);
                                                        cmd6.Parameters.AddWithValue("@p_school_id", school_id);
                                                        cmd6.Parameters.AddWithValue("@p_transaction_type_id", 5);
                                                        cmd6.Parameters.AddWithValue("@p_wallet_id", value.wallet_id);
                                                        cmd6.Parameters.AddWithValue("@p_total_amount", value.total_amount);
                                                        cmd6.Parameters.AddWithValue("@p_payment_method_id", value.payment_method_id);
                                                        cmd6.Parameters.AddWithValue("@p_status_id", 101);
                                                        cmd6.Parameters.AddWithValue("@p_create_by", value.create_by);
                                                        cmd6.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                                                        cmd6.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                                                        cmd6.Parameters.Add("@p_transaction_id", MySqlDbType.Int32);
                                                        cmd6.Parameters["@p_transaction_id"].Direction = ParameterDirection.Output;
                                                        cmd6.ExecuteNonQuery();

                                                        var wallet_status = (string)cmd6.Parameters["@p_status_code"].Value;
                                                        var txn_id = Convert.ToInt32(cmd6.Parameters["@p_transaction_id"].Value);
                                                        int profile_id;
                                                        string mpay_MID;

                                                        if (wallet_status == "record_saved")
                                                        {
                                                            List<DoPayment> listDoPay = new List<DoPayment>();

                                                            using (MySqlCommand cmd7 = new MySqlCommand("sp_get_order_detail_merchant", conn))
                                                            {
                                                                //trans = conn.BeginTransaction();
                                                                cmd7.Transaction = trans;
                                                                cmd7.CommandType = CommandType.StoredProcedure;
                                                                cmd7.Parameters.Clear();
                                                                cmd7.Parameters.AddWithValue("@p_order_id", _order_id);
                                                                cmd7.Parameters.AddWithValue("@p_wallet_id", value.wallet_id);
                                                                MySqlDataReader dataReader2 = cmd7.ExecuteReader();

                                                                if (dataReader2.HasRows == true)
                                                                {
                           
                                                                    while (dataReader2.Read())
                                                                    {
                                                                        DoPayment dopay = new DoPayment();
                                                                        dopay.merchant_id = Convert.ToInt32(dataReader2["merchant_id"]);
                                                                        dopay.profile_id = Convert.ToInt32(dataReader2["merchant_profile_id"]);
                                                                        dopay.school_id = Convert.ToInt32(dataReader2["school_id"]);
                                                                        dopay.mpay_mid = dataReader2["company_wallet_number"].ToString();
                                                                        dopay.total_amount = Convert.ToDecimal(dataReader2["total_amount"]);
                                                                        dopay.product_desc = dataReader2["product_desc"].ToString();
                                                                        listDoPay.Add(dopay);
                                                                    }
                                                                }

                                                                if (dataReader2 != null)
                                                                        dataReader2.Close();

                                                                foreach (DoPayment sl in listDoPay) 
                                                                {
                                                                    profile_id = sl.profile_id;
                                                                    mpay_MID = sl.mpay_mid;

                                                                    param.mpay_mid = mpay_MID;
                                                                    param.amount = String.Format("{0:0}", sl.total_amount * 100);
                                                                    param.productdescription = sl.product_desc;

                                                                    //var result = mpay.PostDoPayment(param);
                                                                    var result = mpay.PostDoPaymentWithMID(param);
                                                                    string jsonStr = await result;
                                                                    DoPaymentModel response = JsonConvert.DeserializeObject<DoPaymentModel>(jsonStr);

                                                                    BodyPayAuth acc = new BodyPayAuth();

                                                                    if (response.Header.status == "00")
                                                                    {
                                                                        data.Success = true;
                                                                        acc = response.Body;
                                                                    }
                                                                    else
                                                                    {
                                                                        data.Success = false;
                                                                        acc = response.Body;
                                                                    }

                                                                    using (MySqlCommand cmd8 = new MySqlCommand("sp_insert_mpay_payment", conn))
                                                                    {
                                                                        try
                                                                        {
                                                                            cmd8.Transaction = trans;
                                                                            cmd8.CommandType = CommandType.StoredProcedure;
                                                                            cmd8.Parameters.AddWithValue("@p_wallet_id", value.wallet_id);
                                                                            cmd8.Parameters.AddWithValue("@p_payment_method_id", value.payment_method_id);
                                                                            cmd8.Parameters.AddWithValue("@p_transaction_type_id", 5);
                                                                            cmd8.Parameters.AddWithValue("@p_transaction_id", txn_id);
                                                                            cmd8.Parameters.AddWithValue("@p_status", acc.transactionstatus);
                                                                            cmd8.Parameters.AddWithValue("@p_status_description", acc.transactionstatusdesc);
                                                                            cmd8.Parameters.AddWithValue("@p_auth_token", acc.responseauthtoken);
                                                                            cmd8.Parameters.AddWithValue("@p_masked_cardno", acc.maskedcardno);
                                                                            cmd8.Parameters.AddWithValue("@p_total_amount", acc.amount.Split(' ')[1]);
                                                                            cmd8.Parameters.AddWithValue("@p_auth_code", acc.authcode);
                                                                            cmd8.Parameters.AddWithValue("@p_mpay_refno", acc.mpayrefno);
                                                                            cmd8.Parameters.AddWithValue("@p_create_by", value.create_by);
                                                                            cmd8.Parameters.AddWithValue("@p_school_id", school_id);
                                                                            cmd8.Parameters.AddWithValue("@p_reference_number", reference_number);
                                                                            cmd8.ExecuteNonQuery();

                                                                            if (response.Header.status == "00")
                                                                            {
                                                                                fcm.PurchaseOrderStatusNotification(profile_id, "New order received", value.create_by + " has placed a new order #" + reference_number + " total of RM" + value.total_amount.ToString("F"));
                                                                            }

                                                                            //data.Success = true;
                                                                            data.Code = response.Header.status;
                                                                            //if (culture == "ms-MY")
                                                                            //{
                                                                            //    data.Message = TranslateWithGoogle(response.Header.message, "en|ms");
                                                                            //}
                                                                            //else
                                                                            //{
                                                                            data.Message = WebApiResources.ReceiveOrder;
                                                                            //}

                                                                            //data.Message = response.Header.message;
                                                                        }
                                                                        catch (Exception ex)
                                                                        {
                                                                            trans.Rollback();
                                                                            data.Success = false;
                                                                            data.Code = "error_occured";
                                                                            data.Message = WebApiResources.ErrorOccured;
                                                                            ExceptionUtility.LogException(ex, "mpay/purchase");
                                                                        }
                                                                        finally 
                                                                        {
                                                                            cmd8.Dispose();
                                                                        }
                                                                    }
                                                                }
                                                            }
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

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "mpay/purchase");
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

        #region CallBack

        [HttpPost]
        [Route("api/v2/mpay/transaction")]
        public string Post_MPay_Callback_Transaction([FromBody] AccountTransactionParam value)
        {
            WalletDataModel data = new WalletDataModel();

            //if (!string.IsNullOrEmpty(value.authtoken) && !string.IsNullOrEmpty(value.timestamp) && !string.IsNullOrEmpty(value.PID) &&
            //    !string.IsNullOrEmpty(value.uid) && !string.IsNullOrEmpty(value.masked_card_no) && !string.IsNullOrEmpty(value.amount) &&
            //    !string.IsNullOrEmpty(value.ref_mcauthtrx_id) && !string.IsNullOrEmpty(value.merchantName) && !string.IsNullOrEmpty(value.trx_date) &&
            //    !string.IsNullOrEmpty(value.notifyType) && !string.IsNullOrEmpty(value.lrc))
            //{
            //    authToken = GetSha256Hash(string.Concat(PID, PartnerKey, value.timestamp, value.amount, value.masked_card_no, value.merchantName,
            //        value.notifyType, value.ref_mcauthtrx_id, value.trx_date, value.uid));

            //    LRC = GetLRC(authToken.ToUpper() + del + value.timestamp + del + PID + del + value.amount + del + value.masked_card_no +
            //    del + value.merchantName + del + value.notifyType + del + value.ref_mcauthtrx_id + del + value.trx_date +
            //    del + value.uid + del);

            //    if (value.authtoken == authToken)
            //    {
            //        if (value.lrc == LRC)
            //        {
                        MySqlTransaction trans = null;
                        string sqlQuery = string.Empty;

                        string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                        using (MySqlConnection conn = new MySqlConnection(constr))
                        {
                            try
                            {
                                conn.Open();
                                trans = conn.BeginTransaction();

                                using (MySqlCommand cmd = new MySqlCommand("sp_insert_mpay_transaction", conn))
                                {

                                    cmd.Transaction = trans;
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddWithValue("@p_auth_token", value.authtoken);
                                    cmd.Parameters.AddWithValue("@p_time_stamp", value.timestamp);
                                    cmd.Parameters.AddWithValue("@p_mpay_uid", Convert.ToInt32(value.uid));
                                    cmd.Parameters.AddWithValue("@p_amount", value.amount);
                                    cmd.Parameters.AddWithValue("@p_masked_cardno", value.masked_card_no);
                                    cmd.Parameters.AddWithValue("@p_merchant_name", value.merchant_name);
                                    cmd.Parameters.AddWithValue("@p_notify_type", value.notification_id);
                                    cmd.Parameters.AddWithValue("@p_ref_mcauthtrx_id", value.master_trx_id);
                                    cmd.Parameters.AddWithValue("@p_payment_ref_id", value.payment_ref_id);
                                    cmd.Parameters.AddWithValue("@p_trx_date", value.trx_date);
                                    cmd.Parameters.AddWithValue("@p_lrc", value.lrc);
                                    cmd.ExecuteNonQuery();
                                    trans.Commit();

                                    data.Success = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                data.Success = false;
                                data.Code = "error_occured";
                                data.Message = WebApiResources.ErrorOccured;
                                ExceptionUtility.LogException(ex, "mpay/transaction");
                            }
                            finally
                            {
                                conn.Close();
                            }
                        }

                        if (data.Success == true)
                            return "0";

                        return "1";
            //        }
            //        else
            //        {
            //            return "1";
            //        }
            //    }
            //    else
            //    {
            //        return "1";
            //    }
            //}
            //else
            //{
            //    return "1";
            //}
        }

        [HttpPost]
        [Route("api/v2/mpay/update-user-info")]
        public string Post_MPay_Callback_Update_User_Info([FromBody] AccountUpdateUserParam value)
        {
            WalletDataModel data = new WalletDataModel();

            //if (!string.IsNullOrEmpty(value.authtoken) && !string.IsNullOrEmpty(value.timestamp) && !string.IsNullOrEmpty(value.UID) &&
            //    !string.IsNullOrEmpty(value.dob) && !string.IsNullOrEmpty(value.email) && !string.IsNullOrEmpty(value.idno) &&
            //    !string.IsNullOrEmpty(value.loginid) && !string.IsNullOrEmpty(value.mobileno) && !string.IsNullOrEmpty(value.name) &&
            //    !string.IsNullOrEmpty(value.occupation) && !string.IsNullOrEmpty(value.lrc))
            //{
            //    authToken = GetSha256Hash(string.Concat(PID, PartnerKey, value.timestamp, value.dob, value.email, value.idno,
            //        value.loginid, value.mobileno, value.name, value.occupation, value.UID));

            //    LRC = GetLRC(authToken.ToUpper() + del + PID + del + value.timestamp + del + value.dob + del + value.email +
            //    del + value.idno + del + value.loginid + del + value.mobileno + del + value.name + del + value.occupation +
            //    del + del + value.UID);

            //    if (value.authtoken == authToken)
            //    {
            //        if (value.lrc == LRC)
            //        {
                        MySqlTransaction trans = null;
                        string sqlQuery = string.Empty;

                        string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                        using (MySqlConnection conn = new MySqlConnection(constr))
                        {
                            try
                            {
                                conn.Open();
                                trans = conn.BeginTransaction();

                                using (MySqlCommand cmd = new MySqlCommand("sp_update_mpay_user_info", conn))
                                {

                                    cmd.Transaction = trans;
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddWithValue("@p_auth_token", value.authtoken);
                                    cmd.Parameters.AddWithValue("@p_time_stamp", value.timestamp);
                                    cmd.Parameters.AddWithValue("@p_mpay_uid", Convert.ToInt32(value.UID));
                                    cmd.Parameters.AddWithValue("@p_date_of_birth", value.dob);
                                    cmd.Parameters.AddWithValue("@p_email", value.email);
                                    cmd.Parameters.AddWithValue("@p_id_no", value.idno);
                                    cmd.Parameters.AddWithValue("@p_login_id", value.loginid);
                                    cmd.Parameters.AddWithValue("@p_mobile_number", "+" + value.mobileno);
                                    cmd.Parameters.AddWithValue("@p_full_name", value.name);
                                    cmd.Parameters.AddWithValue("@p_occupation", value.occupation);
                                    cmd.Parameters.AddWithValue("@p_lrc", value.lrc);
                                    cmd.ExecuteNonQuery();
                                    trans.Commit();

                                    data.Success = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                data.Success = false;
                                data.Code = "error_occured";
                                data.Message = WebApiResources.ErrorOccured;
                                ExceptionUtility.LogException(ex, "mpay/update-user-info");
                            }
                            finally
                            {
                                conn.Close();
                            }
                        }

                        if (data.Success == true)
                            return "0";

                        return "1";
            //        }
            //        else
            //        {
            //            return "1";
            //        }
            //    }
            //    else
            //    {
            //        return "1";
            //    }
            //}
            //else
            //{
            //    return "1";
            //}
        }

        [HttpPost]
        [Route("api/v2/mpay/update-kyc-status")]
        public string Post_MPay_Callback_Update_KYC_Status([FromBody] UpdateKYCStatusParam value)
        {
            WalletDataModel data = new WalletDataModel();

            //if (!string.IsNullOrEmpty(value.authtoken) && !string.IsNullOrEmpty(value.timestamp) && !string.IsNullOrEmpty(value.uid) &&
            //    !string.IsNullOrEmpty(value.notification_id) && !string.IsNullOrEmpty(value.lrc))
            //{
            //    authToken = GetSha256Hash(string.Concat(PID, PartnerKey, value.timestamp, value.notification_id, value.uid));

            //    LRC = GetLRC(authToken.ToUpper() + del + PID + del + value.timestamp + del + value.notification_id + del + value.uid);

            //    if (value.authtoken == authToken)
            //    {
            //        if (value.lrc == LRC)
            //        {
                        MySqlTransaction trans = null;
                        string sqlQuery = string.Empty;

                        string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                        using (MySqlConnection conn = new MySqlConnection(constr))
                        {
                            try
                            {
                                conn.Open();
                                trans = conn.BeginTransaction();

                                using (MySqlCommand cmd = new MySqlCommand("sp_update_mpay_kyc_status", conn))
                                {

                                    cmd.Transaction = trans;
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddWithValue("@p_auth_token", value.authtoken);
                                    cmd.Parameters.AddWithValue("@p_time_stamp", value.timestamp);
                                    cmd.Parameters.AddWithValue("@p_mpay_uid", Convert.ToInt32(value.uid));
                                    cmd.Parameters.AddWithValue("@p_notification_id", Convert.ToInt32(value.notification_id));
                                    cmd.Parameters.AddWithValue("@p_lrc", value.lrc);
                                    cmd.ExecuteNonQuery();
                                    trans.Commit();

                                    data.Success = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                data.Success = false;
                                data.Code = "error_occured";
                                data.Message = WebApiResources.ErrorOccured;
                                ExceptionUtility.LogException(ex, "mpay/update-kyc-status");
                            }
                            finally
                            {
                                conn.Close();
                            }
                        }

                        if (data.Success == true)
                            return "0";

                        return "1";
            //        }
            //        else
            //        {
            //            return "1";
            //        }
            //    }
            //    else
            //    {
            //        return "1";
            //    }
            //}
            //else
            //{
            //    return "1";
            //}
        }

        [HttpPost]
        [Route("api/v2/mpay/update-account-status")]
        public string Post_MPay_Callback_Update_Account_Status([FromBody] UpdateAccountStatusParam value)
        {
            WalletDataModel data = new WalletDataModel();

            //if (!string.IsNullOrEmpty(value.authtoken) && !string.IsNullOrEmpty(value.timestamp) && !string.IsNullOrEmpty(value.PID) &&
            //    !string.IsNullOrEmpty(value.cardtoken) && !string.IsNullOrEmpty(value.account_status) && !string.IsNullOrEmpty(value.UID) &&
            //    !string.IsNullOrEmpty(value.lrc))
            //{
            //    authToken = GetSha256Hash(string.Concat(PID, PartnerKey, value.timestamp, value.account_status, value.cardtoken, value.UID));

            //    LRC = GetLRC(authToken.ToUpper() + del + PID + del + value.timestamp + del + value.account_status + del + value.cardtoken +
            //    del + value.UID);

            //    if (value.authtoken == authToken)
            //    {
            //        if (value.lrc == LRC)
            //        {
                        MySqlTransaction trans = null;
                        string sqlQuery = string.Empty;

                        string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                        using (MySqlConnection conn = new MySqlConnection(constr))
                        {
                            try
                            {
                                conn.Open();
                                trans = conn.BeginTransaction();

                                using (MySqlCommand cmd = new MySqlCommand("sp_update_mpay_account_status", conn))
                                {
                                    cmd.Transaction = trans;
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddWithValue("@p_auth_token", value.authtoken);
                                    cmd.Parameters.AddWithValue("@p_time_stamp", value.timestamp);
                                    cmd.Parameters.AddWithValue("@p_partner_id", value.PID);
                                    cmd.Parameters.AddWithValue("@p_card_token", value.cardtoken);
                                    cmd.Parameters.AddWithValue("@p_account_status", Convert.ToInt32(value.account_status));
                                    cmd.Parameters.AddWithValue("@p_mpay_uid", Convert.ToInt32(value.UID));
                                    cmd.Parameters.AddWithValue("@p_lrc", value.lrc);
                                    cmd.ExecuteNonQuery();
                                    trans.Commit();

                                    data.Success = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                data.Success = false;
                                data.Code = "error_occured";
                                data.Message = WebApiResources.ErrorOccured;
                                ExceptionUtility.LogException(ex, "mpay/update-doc-status");
                            }
                            finally
                            {
                                conn.Close();
                            }
                        }

                        if (data.Success == true)
                            return "0";

                        return "1";
            //        }
            //        else
            //        {
            //            return "1";
            //        }
            //    }
            //    else
            //    {
            //        return "1";
            //    }
            //}
            //else
            //{
            //    return "1";
            //}
        }

        #endregion

        public static string GetLRC(String data)
        {
            long lrc;
            string HexCode = string.Empty;

            char[] charStore = data.ToCharArray();
            lrc = charStore[0];

            for (int i = 1; i < charStore.Length; i++)
            {
                lrc = lrc ^ charStore[i];
            }

            HexCode = Convert.ToString(lrc, 16).ToUpper().ToString();

            if (HexCode.Length == 1)
                HexCode = "0" + HexCode;

            return HexCode;
        }

        public static string GetSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public string TranslateWithGoogle(string input, string languagePair)
        {
            try
            {
                string url = String.Format("http://www.google.com/translate_t?hl=en&ie=UTF8&text={0}&langpair={1}", input, languagePair);
                WebClient webClient = new WebClient();
                webClient.Encoding = System.Text.Encoding.Default;
                string result = webClient.DownloadString(url);
                result = result.Substring(result.IndexOf("<span title=\"") + "<span title=\"".Length);
                result = result.Substring(result.IndexOf(">") + 1);
                result = result.Substring(0, result.IndexOf("</span>"));
                return result.Trim();
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "google-translate");
                return string.Empty;
            }

        }
    }
}
