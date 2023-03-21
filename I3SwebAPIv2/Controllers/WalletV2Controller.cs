using I3SwebAPIv2.Class;
using I3SwebAPIv2.Models;
using I3SwebAPIv2.Resources;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Web.Http;

namespace I3SwebAPIv2.Controllers
{
    public class WalletV2Controller : ApiController
    {
        MPayWallet mpay = new MPayWallet();

        [HttpPost]
        [Authorize]
        [Route("api/v2/wallet/topup")]
        public IHttpActionResult Post_Walet_Topup(string culture, [FromBody]WalletTopupParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            string reference_number = string.Empty;
            WalletV2Class cls = new WalletV2Class();
            WalletDataModel data = new WalletDataModel();
            if (value.wallet_id != 0 && !string.IsNullOrEmpty(value.wallet_number) && value.topup_amount != 0 && value.topup_date != null &&  !string.IsNullOrEmpty(value.create_by))
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

                        reference_number = "TPP-" + cls.GetAlphaUniqueKey() + "-" + cls.GetNumericUniqueKey() + "-" + cls.GetNumericUniqueKey();

                        using (MySqlCommand cmd = new MySqlCommand("sp_insert_wallet_topup", conn))
                        {

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
                            trans.Commit();

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
                            else if (status_code == "success")
                            {
                                data.Success = true;
                                data.Code = "success";
                                data.Message = WebApiResources.YourTopupSuccess;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "wallet/topup");
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
        [Route("api/v2/wallet/transfer")]
        public IHttpActionResult Post_Wallet_Transfer(string culture, [FromBody]WalletTransferParam value)
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
                        ExceptionUtility.LogException(ex, "wallet/transfer");
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
        [Route("api/v2/wallet/transaction-history")]
        public IHttpActionResult Post_Wallet_Transaction_History([FromBody] WalletTransactionHistoryParam value )
        {
            WalletDataModel data = new WalletDataModel();
            if (value.wallet_number != null)
            {
                List<WalletTransactionHistory> list = new List<WalletTransactionHistory>();
                WalletTransactionHistoryModel listData = new WalletTransactionHistoryModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_wallet_transaction_history", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_wallet_number", value.wallet_number);
                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    WalletTransactionHistory prop = new WalletTransactionHistory();
                                    prop.transaction_id = Convert.ToInt16(dataReader["transaction_id"]);
                                    prop.reference_number = dataReader["reference_number"].ToString();
                                    prop.transaction_type_id = Convert.ToInt16(dataReader["transaction_type_id"]);
                                    prop.transaction_type = dataReader["transaction_type"].ToString();
                                    prop.transaction_type_bm = dataReader["transaction_type_bm"].ToString();
                                    prop.wallet_number = dataReader["wallet_number"].ToString();
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.wallet_number_reference = dataReader["wallet_number_reference"].ToString();
                                    prop.full_name_reference = dataReader["full_name_reference"].ToString();
                                    prop.amount = Convert.ToDecimal(dataReader["amount"]);
                                    prop.status_id = Convert.ToInt16(dataReader["status_id"]);
                                    prop.status_code = dataReader["status_code"].ToString();
                                    prop.status_code_bm = dataReader["status_code_bm"].ToString();
                                    prop.create_at = dataReader["create_at"] as DateTime? ?? default(DateTime);
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "transaction_history";
                                listData.Message = "Transaction history listing.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Transaction history could not be found.";
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
                        ExceptionUtility.LogException(ex, "wallet/transaction-history");
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
                return BadRequest("Missing Parameters.");
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/wallet/transaction-reference")]
        public IHttpActionResult Post_Wallet_Transaction_Reference([FromBody] WalletTransactionReferenceParam value)
        {
            WalletDataModel data = new WalletDataModel();
            if (value.transaction_id != 0 && value.reference_number != null)
            {
                List<WalletTransactionReference> list = new List<WalletTransactionReference>();
                WalletTransactionReferenceModel listData = new WalletTransactionReferenceModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_wallet_transaction_reference", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_transaction_id", value.transaction_id);
                            cmd.Parameters.AddWithValue("@p_reference_number", value.reference_number);
                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    WalletTransactionReference prop = new WalletTransactionReference();
                                    prop.transaction_id = Convert.ToInt16(dataReader["transaction_id"]);
                                    prop.reference_number = dataReader["reference_number"].ToString();
                                    prop.transaction_type_id = Convert.ToInt16(dataReader["transaction_type_id"]);
                                    prop.transaction_type = dataReader["transaction_type"].ToString();
                                    prop.transaction_type_bm = dataReader["transaction_type_bm"].ToString();
                                    prop.wallet_number = dataReader["wallet_number"].ToString();
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.wallet_number_reference = dataReader["wallet_number_reference"].ToString();
                                    prop.full_name_reference = dataReader["full_name_reference"].ToString();
                                    prop.amount = Convert.ToDecimal(dataReader["amount"]);
                                    prop.status_id = Convert.ToInt16(dataReader["status_id"]);
                                    prop.status_code = dataReader["status_code"].ToString();
                                    prop.status_code_bm = dataReader["status_code_bm"].ToString();
                                    prop.create_at = dataReader["create_at"] as DateTime? ?? default(DateTime);
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "transaction_reference";
                                listData.Message = "Transaction reference listing.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Transaction reference could not be found.";
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
                        ExceptionUtility.LogException(ex, "wallet/transaction-reference");
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
                return BadRequest("Missing Parameters.");
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/wallet/transaction-detail")]
        public IHttpActionResult Post_Wallet_Transaction_Detail([FromBody] WalletTransactionDetailParam value)
        {
            WalletDataModel data = new WalletDataModel();
            if (value.wallet_number != null && value.reference_number != null)
            {
                List<WalletTransactionDetail> list = new List<WalletTransactionDetail>();
                WalletTransactionDetailModel listData = new WalletTransactionDetailModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_wallet_transaction_detail", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_wallet_number", value.wallet_number);
                            cmd.Parameters.AddWithValue("@p_reference_number", value.reference_number);
                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    WalletTransactionDetail prop = new WalletTransactionDetail();
                                    prop.rcpt_detail_id = Convert.ToInt16(dataReader["rcpt_detail_id"]);
                                    prop.product_id = Convert.ToInt16(dataReader["product_id"]);
                                    prop.product_name = dataReader["product_name"].ToString();
                                    prop.product_qty = Convert.ToInt16(dataReader["product_qty"]);
                                    prop.unit_price = Convert.ToDecimal(dataReader["unit_price"]);
                                    prop.discount_amount = Convert.ToDecimal(dataReader["discount_amount"]);
                                    prop.sub_total_amount = Convert.ToDecimal(dataReader["sub_total_amount"]);
                                    prop.total_amount = Convert.ToDecimal(dataReader["total_amount"]);
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "transaction_detail";
                                listData.Message = "Transaction detail listing.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Transaction detail could not be found.";
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
                        ExceptionUtility.LogException(ex, "wallet/transaction-detail");
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
                return BadRequest("Missing Parameters.");
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/wallet/transaction-master")]
        public IHttpActionResult Post_Wallet_Transaction_Master([FromBody] WalletTransactionDetailParam value)
        {
            WalletDataModel data = new WalletDataModel();
            if (value.wallet_number != null && value.reference_number != null)
            {
                List<WalletTransactionMaster> list = new List<WalletTransactionMaster>();
                WalletTransactionMasterModel listData = new WalletTransactionMasterModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_wallet_transaction_master", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_wallet_number", value.wallet_number);
                            cmd.Parameters.AddWithValue("@p_reference_number", value.reference_number);
                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    WalletTransactionMaster prop = new WalletTransactionMaster();
                                    int tax_rate_int = 0;
                                    prop.sub_total_amount = Convert.ToDecimal(dataReader["sub_total_amount"]);
                                    prop.total_amount = Convert.ToDecimal(dataReader["total_amount"]);
                                    prop.tax_amount = prop.total_amount - prop.sub_total_amount;
                                    if (prop.tax_amount > 0)
                                    {
                                        tax_rate_int = (int)((prop.total_amount - prop.sub_total_amount) / prop.sub_total_amount * 100);
                                        if (prop.tax_rate == null)
                                        {
                                            prop.tax_rate = tax_rate_int.ToString();
                                        }
                                        else
                                        {
                                            if (!prop.tax_rate.ToString().Contains(tax_rate_int.ToString()))
                                            {
                                                prop.tax_rate = prop.tax_rate + "-" + tax_rate_int;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        prop.tax_rate = "0";
                                    }

                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "transaction_master";
                                listData.Message = "Transaction master listing.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Transaction master could not be found.";
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
                        ExceptionUtility.LogException(ex, "wallet/transaction-master");
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
                return BadRequest("Missing Parameters.");
            }
        }
    }
}
