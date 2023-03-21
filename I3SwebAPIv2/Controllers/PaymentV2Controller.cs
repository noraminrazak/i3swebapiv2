using I3SwebAPIv2.Class;
using I3SwebAPIv2.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Http.Cors;
using System.Web.Http;
using System.Threading.Tasks;
using System.Globalization;
using System.Threading;
using I3SwebAPIv2.Resources;
using Newtonsoft.Json;

namespace I3SwebAPIv2.Controllers
{
    public class PaymentV2Controller : ApiController
    {
        i3sAuth.Rijndael auth = new i3sAuth.Rijndael();
        ipay88 ipay = new ipay88();
        public string salt = ConfigurationManager.AppSettings["passPhrase"];

        [HttpPost]
        [Route("api/v2/payment/response")]
        public string Post_Payment_Response([FromBody] PaymentResponseParam value)
        {
            int wallet_id = 0;
            string full_name = string.Empty;
            WalletDataModel data = new WalletDataModel();

            if (!string.IsNullOrEmpty(value.MerchantCode) && !string.IsNullOrEmpty(value.RefNo) && value.PaymentId != 0 &&
                value.Amount != 0 && !string.IsNullOrEmpty(value.Currency) && !string.IsNullOrEmpty(value.Status) && !string.IsNullOrEmpty(value.Signature))
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
                            cmd.Parameters.AddWithValue("@p_username", auth.DecryptRijndael(value.Remark, salt));
                            cmd.Parameters.AddWithValue("@p_wallet_id", 0);
                            MySqlDataReader dataReader = cmd.ExecuteReader();


                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    wallet_id = Convert.ToInt32(dataReader["wallet_id"].ToString());
                                    full_name = dataReader["full_name"].ToString();
                                }
                            }

                            if (dataReader != null)
                            {
                                dataReader.Close();
                            }

                            using (MySqlCommand cmd2 = new MySqlCommand("sp_update_payment_response", conn))
                            {

                                cmd2.Transaction = trans;
                                cmd2.CommandType = CommandType.StoredProcedure;
                                cmd2.Parameters.Clear();
                                cmd2.Parameters.AddWithValue("@p_merchant_code", value.MerchantCode);
                                cmd2.Parameters.AddWithValue("@p_payment_id", value.PaymentId);
                                cmd2.Parameters.AddWithValue("@p_reference_number", value.RefNo);
                                cmd2.Parameters.AddWithValue("@p_amount", value.Amount);
                                cmd2.Parameters.AddWithValue("@p_currency", value.Currency);
                                cmd2.Parameters.AddWithValue("@p_remark", auth.DecryptRijndael(value.Remark, salt));
                                cmd2.Parameters.AddWithValue("@p_txn_id", value.TransId);
                                cmd2.Parameters.AddWithValue("@p_auth_code", value.AuthCode);
                                cmd2.Parameters.AddWithValue("@p_status", value.Status);
                                cmd2.Parameters.AddWithValue("@p_err_desc", value.ErrDesc);
                                cmd2.Parameters.AddWithValue("@p_signature", value.Signature);
                                cmd2.Parameters.AddWithValue("@p_cc_name", value.CCName);
                                cmd2.Parameters.AddWithValue("@p_cc_number", value.CCNo);
                                cmd2.Parameters.AddWithValue("@p_s_bankname", value.S_bankname);
                                cmd2.Parameters.AddWithValue("@p_s_country", value.S_country);
                                cmd2.Parameters.AddWithValue("@p_txn_date", value.TranDate);
                                cmd2.Parameters.AddWithValue("@p_xfield1", value.Xfield1);
                                cmd2.Parameters.AddWithValue("@p_wallet_id", wallet_id);
                                cmd2.Parameters.AddWithValue("@p_update_by", full_name);
                                cmd2.ExecuteNonQuery();
                                trans.Commit();

                                data.Success = true;
                                data.Code = "RECEIVEDOK";
                                data.Message = value.RefNo;

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "payment/response");
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

                return "RECEIVEDOK";
            }
            else
            {
                return "BADREQUEST";
            }

        }

        [HttpPost]
        [Route("api/v2/payment/request")]
        public async Task<IHttpActionResult> Post_Payment_Request(string culture, [FromBody] RequestParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            string reference_number = string.Empty;
            WalletV2Class cls = new WalletV2Class();
            WalletDataModel data = new WalletDataModel();
            if (!string.IsNullOrEmpty(value.nric) && value.wallet_id != 0 && !string.IsNullOrEmpty(value.wallet_number) && value.topup_amount != 0 && value.topup_date != null && !string.IsNullOrEmpty(value.create_by))
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
                                PaymentRequestParam prop = new PaymentRequestParam();

                                //prop.amount = (Math.Truncate(100 * value.topup_amount) / 100).ToString();
                                prop.Amount = value.topup_amount;
                                prop.RefNo = reference_number;
                                prop.ProdDesc = "Wallet Top-up";
                                if (value.platform_name == "Android")
                                {
                                    prop.appdeeplink = "http://giis.myedutech.my/.well-known/assetlinks.json";
                                }
                                else if (value.platform_name == "iOS")
                                {
                                    prop.appdeeplink = "http://giis.myedutech.my/.well-known/apple-app-site-association";
                                }

                                using (MySqlCommand cmd2 = new MySqlCommand("sp_get_user_account", conn))
                                {
                                    cmd2.Transaction = trans;

                                    try
                                    {
                                        cmd2.CommandType = CommandType.StoredProcedure;
                                        cmd2.Parameters.Clear();
                                        cmd2.Parameters.AddWithValue("@p_id_number", value.nric);
                                        MySqlDataReader dataReader = cmd2.ExecuteReader();


                                        if (dataReader.HasRows == true)
                                        {
                                            while (dataReader.Read())
                                            {
                                                prop.UserName = dataReader["full_name"].ToString();
                                                prop.UserEmail = dataReader["email"].ToString();
                                                prop.UserContact = dataReader["mobile_number"].ToString();
                                                prop.Remark = auth.EncryptRijndael(dataReader["nric"].ToString(), salt);
                                            }
                                        }

                                        if (dataReader != null)
                                        {
                                            dataReader.Close();
                                        }

                                        var result = ipay.PostPaymentRequest(prop);
                                        string htmlStr = await result;

                                        data.Success = true;
                                        data.Message = htmlStr;
                                        data.Code = reference_number;
  
                                    }
                                    catch (Exception ex)
                                    {
                                        trans.Rollback();
                                        data.Success = false;
                                        data.Code = "error_occured";
                                        data.Message = WebApiResources.ErrorOccured;
                                        ExceptionUtility.LogException(ex, "payment/request");
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
                        ExceptionUtility.LogException(ex, "payment/request");
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
        [Route("api/v2/payment/requery")]
        public async Task<IHttpActionResult> Post_Payment_Requery(string culture, [FromBody] PaymentStatusParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            int wallet_id = 0;
            string full_name = string.Empty;
            PaymentRequeryParam prop = new PaymentRequeryParam();
            WalletV2Class cls = new WalletV2Class();
            WalletDataModel data = new WalletDataModel();
            if (!string.IsNullOrEmpty(value.reference_number) && value.wallet_id != 0 &&  value.amount != 0)
            {
                prop.ReferenceNo = value.reference_number;
                prop.Amount = value.amount.ToString();

                data.Success = false;
                string sqlQuery = string.Empty;
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                MySqlTransaction trans = null;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        var result = ipay.PostRequeryPaymentStatus(prop);
                        string jsonStr = await result;
                        RequeryParam resp = JsonConvert.DeserializeObject<RequeryParam>(jsonStr);

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_wallet_info", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_username", auth.DecryptRijndael(resp.Remark, salt));
                            cmd.Parameters.AddWithValue("@p_wallet_id", 0);
                            MySqlDataReader dataReader = cmd.ExecuteReader();


                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    wallet_id = Convert.ToInt32(dataReader["wallet_id"].ToString());
                                    full_name = dataReader["full_name"].ToString();
                                }
                            }

                            if (dataReader != null)
                            {
                                dataReader.Close();
                            }

                            using (MySqlCommand cmd2 = new MySqlCommand("sp_update_payment_response", conn))
                            {

                                cmd2.Transaction = trans;
                                cmd2.CommandType = CommandType.StoredProcedure;
                                cmd2.Parameters.Clear();
                                cmd2.Parameters.AddWithValue("@p_merchant_code", resp.MerchantCode);
                                cmd2.Parameters.AddWithValue("@p_payment_id", resp.PaymentId);
                                cmd2.Parameters.AddWithValue("@p_reference_number", resp.RefNo);
                                cmd2.Parameters.AddWithValue("@p_amount", resp.Amount);
                                cmd2.Parameters.AddWithValue("@p_currency", resp.Currency);
                                cmd2.Parameters.AddWithValue("@p_remark", resp.Remark);
                                cmd2.Parameters.AddWithValue("@p_txn_id", resp.TransId);
                                cmd2.Parameters.AddWithValue("@p_auth_code", resp.AuthCode);
                                cmd2.Parameters.AddWithValue("@p_status", resp.Status);
                                cmd2.Parameters.AddWithValue("@p_err_desc", resp.ErrDesc);
                                cmd2.Parameters.AddWithValue("@p_cc_name", resp.CCName);
                                cmd2.Parameters.AddWithValue("@p_cc_number", resp.Creditcardno);
                                cmd2.Parameters.AddWithValue("@p_s_bankname", resp.S_bankname);
                                cmd2.Parameters.AddWithValue("@p_s_country", resp.S_country);
                                cmd2.Parameters.AddWithValue("@p_txn_date", resp.TranDate);
                                cmd2.Parameters.AddWithValue("@p_xfield1", "");
                                cmd2.Parameters.AddWithValue("@p_wallet_id", wallet_id);
                                cmd2.Parameters.AddWithValue("@p_update_by", full_name);
                                cmd2.ExecuteNonQuery();
                                trans.Commit();

                                data.Success = true;
                                data.Code = "RECEIVEDOK";
                                data.Message = value.reference_number;

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "payment/requery");
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
        [Route("api/v2/payment/online-order")]
        public async Task<IHttpActionResult> Post_Payment_Online_Order(string culture, [FromBody] CheckOutParam value)
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

                                    //if (DBNull.Value != cmd3.Parameters["@p_mpay_uid"].Value)
                                    //{
                                    //    param.uid = cmd3.Parameters["@p_mpay_uid"].Value.ToString();
                                    //}

                                    //if (DBNull.Value != cmd3.Parameters["@p_mpay_card_token"].Value)
                                    //{
                                    //    param.cardtoken = cmd3.Parameters["@p_mpay_card_token"].Value.ToString();
                                    //}

                                    //if (DBNull.Value != cmd3.Parameters["@p_mpay_card_pin"].Value)
                                    //{
                                    //    if (!string.IsNullOrEmpty(cmd3.Parameters["@p_mpay_card_pin"].Value.ToString()))
                                    //    {
                                    //        param.cardpin = auth.DecryptRijndael(cmd3.Parameters["@p_mpay_card_pin"].Value.ToString(), salt);
                                    //    }
                                    //    else
                                    //    {
                                    //        param.cardpin = "";
                                    //    }
                                    //}

                                    var school_id = Convert.ToInt16(cmd3.Parameters["@p_school_id"].Value);

                                    bool proceedPay = true;

                                    if (value.total_amount > account_balance)
                                    {
                                        if (value.payment_method_id == 1)
                                        {
                                            //if (!string.IsNullOrEmpty(param.uid) && !string.IsNullOrEmpty(param.cardpin) && !string.IsNullOrEmpty(param.cardtoken))
                                            //{
                                                if (value.payment_method_id == 1)
                                                {
                                                    proceedPay = false;
                                                }
                                            //}
                                            //else
                                            //{
                                            //    proceedPay = false;
                                            //}
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
                                                        //string mpay_MID;

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
                                                                    //mpay_MID = sl.mpay_mid;

                                                                    //param.mpay_mid = mpay_MID;
                                                                    //param.amount = String.Format("{0:0}", sl.total_amount * 100);
                                                                    //param.productdescription = sl.product_desc;

                                                                    ////var result = mpay.PostDoPayment(param);
                                                                    //var result = mpay.PostDoPaymentWithMID(param);
                                                                    //string jsonStr = await result;
                                                                    //DoPaymentModel response = JsonConvert.DeserializeObject<DoPaymentModel>(jsonStr);

                                                                    BodyPayAuth acc = new BodyPayAuth();

                                                                    //if (response.Header.status == "00")
                                                                    //{
                                                                    //    data.Success = true;
                                                                    //    acc = response.Body;
                                                                    //}
                                                                    //else
                                                                    //{
                                                                    //    data.Success = false;
                                                                    //    acc = response.Body;
                                                                    //}

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
                                                                            cmd8.Parameters.AddWithValue("@p_status", "00"); //acc.transactionstatus
                                                                            cmd8.Parameters.AddWithValue("@p_status_description", "APPROVED"); //acc.transactionstatusdesc
                                                                            cmd8.Parameters.AddWithValue("@p_auth_token", ""); //acc.responseauthtoken
                                                                            cmd8.Parameters.AddWithValue("@p_masked_cardno", "123456"); //acc.maskedcardno
                                                                            cmd8.Parameters.AddWithValue("@p_total_amount", value.total_amount);
                                                                            cmd8.Parameters.AddWithValue("@p_auth_code", "000000"); //acc.authcode
                                                                            cmd8.Parameters.AddWithValue("@p_mpay_refno", reference_number); //acc.mpayrefno
                                                                            cmd8.Parameters.AddWithValue("@p_create_by", value.create_by);
                                                                            cmd8.Parameters.AddWithValue("@p_school_id", school_id);
                                                                            cmd8.Parameters.AddWithValue("@p_reference_number", reference_number);
                                                                            cmd8.ExecuteNonQuery();

                                                                            //if (response.Header.status == "00")
                                                                            //{
                                                                               fcm.PurchaseOrderStatusNotification(profile_id, "New order received", value.create_by + " has placed a new order #" + reference_number + " total of RM" + value.total_amount.ToString("F"));
                                                                            //}


                                                                            //data.Code = response.Header.status;
                                                                            data.Success = true;
                                                                            data.Code = "00";
                                                                            data.Message = WebApiResources.ReceiveOrder;

                                                                        }
                                                                        catch (Exception ex)
                                                                        {
                                                                            trans.Rollback();
                                                                            data.Success = false;
                                                                            data.Code = "error_occured";
                                                                            data.Message = WebApiResources.ErrorOccured;
                                                                            ExceptionUtility.LogException(ex, "payment/online-order");
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
    }
}
