using I3SwebAPIv2.Class;
using I3SwebAPIv2.Models;
using I3SwebAPIv2.Resources;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;

namespace I3SwebAPIv2.Controllers
{
    public class MerchantsV2Controller : ApiController
    {
        [HttpPost]
        [Authorize]
        [Route("api/v2/merchant/profile")]
        public IHttpActionResult Post_Merchant([FromBody]ProfileParam value)
        {
            MerchantDataModel data = new MerchantDataModel();
            if (value.profile_id != 0)
            {
                List<Merchant> list = new List<Merchant>();
                MerchantModel listData = new MerchantModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_merchant_profile", conn))
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
                                    Merchant prop = new Merchant();
                                    prop.merchant_id = Convert.ToInt16(dataReader["merchant_id"]);
                                    prop.company_name = dataReader["company_name"].ToString();
                                    prop.registration_number = dataReader["registration_number"].ToString();
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.photo_url = dataReader["photo_url"].ToString();
                                    prop.merchant_type_id = Convert.ToInt16(dataReader["merchant_type_id"]);
                                    prop.merchant_type = dataReader["merchant_type"].ToString();
                                    prop.contact_number = dataReader["contact_number"].ToString();
                                    prop.person_in_charge = dataReader["person_in_charge"].ToString();
                                    if (dataReader["status_code"].ToString() == "Aktif")
                                    {
                                        listData.Code = "active_merchant";
                                        listData.Message = "Merchant status is active.";
                                    }
                                    else
                                    {
                                        listData.Code = "inactive_merchant";
                                        listData.Message = "Merchant status is not active.";
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
                                listData.Message = "Merchant could not be found.";
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
                        ExceptionUtility.LogException(ex, "merchant/profile");
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
        [Route("api/v2/merchant/school-relationship")]
        public IHttpActionResult Post_Merchant_School_Relationship([FromBody]MerchantParam value)
        {
            MerchantDataModel data = new MerchantDataModel();
            if (value.merchant_id != 0)
            {
                List<MerchantSchool> list = new List<MerchantSchool>();
                MerchantSchoolModel listData = new MerchantSchoolModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_merchant_relationship", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_merchant_id", value.merchant_id);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    MerchantSchool prop = new MerchantSchool();
                                    prop.school_id = dataReader["school_id"] as int? ?? default(int);
                                    prop.school_name = dataReader["school_name"].ToString();
                                    prop.school_type_id = dataReader["school_type_id"] as int? ?? default(int);
                                    prop.school_type = dataReader["school_type"].ToString();
                                    prop.city = dataReader["city"].ToString();
                                    prop.state_name = dataReader["state_name"].ToString();
                                    prop.country_name = dataReader["country_name"].ToString();
                                    prop.status_code = dataReader["status_code"].ToString();
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "merchant_school_list";
                                listData.Message = "Merchant school relationship list.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Merchant school relationship could not be found.";
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
                        ExceptionUtility.LogException(ex, "merchant/school-relationship");
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
        [Route("api/v2/merchant/club-relationship")]
        public IHttpActionResult Post_Merchant_Club_Relationship([FromBody]MerchantClubRelationshipParam value)
        {
            MerchantDataModel data = new MerchantDataModel();
            if (value.merchant_id != 0)
            {
                List<ClubRelationship> list = new List<ClubRelationship>();
                ClubRelationshipModel listData = new ClubRelationshipModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_school_club_relationship", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_school_id", 0);
                            cmd.Parameters.AddWithValue("@p_merchant_id", value.merchant_id);
                            cmd.Parameters.AddWithValue("@p_staff_id", 0);
                            cmd.Parameters.AddWithValue("@p_parent_id", 0);
                            cmd.Parameters.AddWithValue("@p_student_id", 0);
                            cmd.Parameters.AddWithValue("@p_user_role_id", 7);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    ClubRelationship prop = new ClubRelationship();
                                    prop.relationship_id = dataReader["relationship_id"] as int? ?? default(int);
                                    prop.club_id = dataReader["club_id"] as int? ?? default(int);
                                    prop.club_name = dataReader["club_name"].ToString();
                                    prop.school_id = Convert.ToInt16(dataReader["school_id"]);
                                    prop.school_name = dataReader["school_name"].ToString();
                                    prop.school_type = dataReader["school_type"].ToString();
                                    prop.total_member = Convert.ToInt16(dataReader["total_member"]);
                                    prop.create_by_staff_id = Convert.ToInt16(dataReader["create_by_staff_id"]);
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "club_relationship_list";
                                listData.Message = "Club relationship list.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Club relationship could not be found.";
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
                        ExceptionUtility.LogException(ex, "merchant/club-relationship");
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
        [Route("api/v2/merchant/join-club")]
        public IHttpActionResult Post_Merchant_Join_Club(string culture, [FromBody] MerchantJoinClubRelationshipParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            StaffDataModel data = new StaffDataModel();
            if (value.merchant_id != 0 && value.club_id != 0 && value.create_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_insert_school_club_relationship", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_club_id", value.club_id);
                            cmd.Parameters.AddWithValue("@p_user_role_id", 7);
                            cmd.Parameters.AddWithValue("@p_staff_id", 0);
                            cmd.Parameters.AddWithValue("@p_student_id", 0);
                            cmd.Parameters.AddWithValue("@p_parent_id", 0);
                            cmd.Parameters.AddWithValue("@p_merchant_id", value.merchant_id);
                            cmd.Parameters.AddWithValue("@p_create_by", value.create_by);
                            cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                            cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            var status_code = cmd.Parameters["@p_status_code"].Value.ToString();

                            if (status_code == "record_saved")
                            {
                                data.Success = true;
                                data.Code = "record_saved";
                                data.Message = WebApiResources.ClubRelationshipCreateSuccess;
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "record_exist";
                                data.Message = WebApiResources.ClubRelationshipExist;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "merchant/join-club");
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
        [Route("api/v2/merchant/terminal")]
        public IHttpActionResult Post_Merchant_Terminal([FromBody]MerchantTerminalParam value)
        {
            MerchantDataModel data = new MerchantDataModel();
            if (value.merchant_id != 0 && value.school_id != 0 && value.receipt_date != null)
            {
                List<MerchantTerminal> list = new List<MerchantTerminal>();
                MerchantTerminalModel listData = new MerchantTerminalModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_merchant_terminal_receipt", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_merchant_id", value.merchant_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_receipt_date", value.receipt_date);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    MerchantTerminal prop = new MerchantTerminal();
                                    prop.terminal_id = dataReader["terminal_id"] as int? ?? default(int);
                                    prop.terminal_model = dataReader["terminal_model"].ToString();
                                    prop.tag_number = dataReader["tag_number"].ToString();
                                    prop.serial_number = dataReader["serial_number"].ToString();
                                    prop.school_id = dataReader["school_id"] as int? ?? default(int);
                                    prop.school_name = dataReader["school_name"].ToString();
                                    prop.school_type = dataReader["school_type"].ToString();
                                    prop.hardware_status = dataReader["hardware_status"].ToString();
                                    prop.total_amount = dataReader["total_amount"] as decimal? ?? default(decimal);
                                    prop.total_transaction = Convert.ToInt16(dataReader["total_transaction"]);
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "merchant_terminal_list";
                                listData.Message = "Merchant terminal listing.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Merchant terminal could not be found.";
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
                        ExceptionUtility.LogException(ex, "merchant/terminal");
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
        [Route("api/v2/merchant/terminal-receipt")]
        public IHttpActionResult Post_Merchant_Terminal_Receipt([FromUri] SqlFilterParam uri, [FromBody]MerchantTerminalReceiptParam value)
        {
            MerchantDataModel data = new MerchantDataModel();
            int? limit = 0;
            int? offset = 0;
            if (uri != null)
            {
                if (uri.limit == null)
                {
                    limit = 0;
                }
                else
                {
                    limit = uri.limit;
                }
                if (uri.offset == null)
                {
                    offset = 0;
                }
                else
                {
                    offset = uri.offset;
                }
            }
            else
            {
                limit = 0;
                offset = 0;
            }
            if (value.merchant_id != 0 && value.school_id != 0 && value.terminal_id != 0 && value.receipt_date != null)
            {
                List<MerchantTerminalReceipt> list = new List<MerchantTerminalReceipt>();
                MerchantTerminalReceiptModel listData = new MerchantTerminalReceiptModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_merchant_terminal_receipt_master", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_limit", limit);
                            cmd.Parameters.AddWithValue("@p_offset", offset);
                            cmd.Parameters.AddWithValue("@p_merchant_id", value.merchant_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_terminal_id", value.terminal_id);
                            cmd.Parameters.AddWithValue("@p_receipt_date", value.receipt_date);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    MerchantTerminalReceipt prop = new MerchantTerminalReceipt();
                                    prop.rcpt_id = dataReader["rcpt_id"] as int? ?? default(int);
                                    prop.wallet_id = dataReader["wallet_id"] as int? ?? default(int);
                                    prop.wallet_number = dataReader["wallet_number"].ToString();
                                    prop.reference_number = dataReader["reference_number"].ToString();
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.receipt_date = dataReader["receipt_date"] as DateTime? ?? default(DateTime);
                                    prop.total_amount = dataReader["total_amount"] as decimal? ?? default(decimal);
                                    prop.payment_method = dataReader["payment_method"].ToString();
                                    prop.payment_method_bm = dataReader["payment_method_bm"].ToString();
                                    prop.company_name = dataReader["company_name"].ToString();
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "merchant_terminal_receipt_list";
                                listData.Message = "Merchant terminal receipt listing.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Merchant terminal receipt could not be found.";
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
                        ExceptionUtility.LogException(ex, "merchant/terminal-receipt");
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
        [Route("api/v2/merchant/terminal-receipt-detail")]
        public IHttpActionResult Post_Merchant_Terminal_Receipt_Detail([FromBody] MerchantTerminalReceiptDetailParam value)
        {
            WalletDataModel data = new WalletDataModel();
            if (value.rcpt_id != 0 && value.wallet_number != null && value.reference_number != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_merchant_terminal_receipt_detail", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_rcpt_id", value.rcpt_id);
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
                                listData.Code = "receipt_detail";
                                listData.Message = "Receipt detail listing.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Receipt detail could not be found.";
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
                        ExceptionUtility.LogException(ex, "merchant/terminal-receipt-detail");
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
        [Route("api/v2/merchant/create-product-category")]
        public IHttpActionResult Post_Merchant_Create_Product_Category(string culture, [FromBody]CreateProductCategoryParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            MerchantDataModel data = new MerchantDataModel();
            if (value.school_id != 0 && value.merchant_id != 0 && value.category_name != null && value.create_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_insert_product_category", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_merchant_id", value.merchant_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_category_name", value.category_name);
                            cmd.Parameters.AddWithValue("@p_category_description", value.category_description);
                            cmd.Parameters.AddWithValue("@p_create_by", value.create_by);
                            cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                            cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            var status_code = (string)cmd.Parameters["@p_status_code"].Value;

                            if (status_code == "record_saved")
                            {
                                data.Success = true;
                                data.Code = "record_saved";
                                data.Message = WebApiResources.ProductCategoryCreateSuccess;
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "record_exist";
                                data.Message = WebApiResources.ProductCategoryExist;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "merchant/create-product-category");
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

                return Ok(data);
            }
            else {
                return BadRequest("Missing parameters.");
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/merchant/update-product-category")]
        public IHttpActionResult Post_Merchant_Update_Product_Category(string culture, [FromBody]UpdateProductCategoryParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            MerchantDataModel data = new MerchantDataModel();
            if (value.school_id != 0 && value.category_id != 0 && value.merchant_id != 0 && value.category_name != null && value.update_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_update_product_category", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_category_id", value.category_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_merchant_id", value.merchant_id);
                            cmd.Parameters.AddWithValue("@p_category_name", value.category_name);
                            cmd.Parameters.AddWithValue("@p_category_description", value.category_description);
                            cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                            cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                            cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            var status_code = (string)cmd.Parameters["@p_status_code"].Value;

                            if (status_code == "update_success")
                            {
                                data.Success = true;
                                data.Code = "update_success";
                                data.Message = WebApiResources.ProductCategoryUpdateSuccess;
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "record_exist";
                                data.Message = WebApiResources.ProductCategoryExist;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "merchant/update-product-category");
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
        [Route("api/v2/merchant/delete-product-category")]
        public IHttpActionResult Post_Merchant_Delete_Product_Category(string culture, [FromBody]DeleteProductCategoryParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            MerchantDataModel data = new MerchantDataModel();
            if (value.category_id != 0 && value.school_id != 0 && value.merchant_id != 0 && value.update_by != null)
            {
                List<ProductDetail> list = new List<ProductDetail>();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_delete_product_category", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_category_id", value.category_id);
                            cmd.Parameters.AddWithValue("@p_merchant_id", value.merchant_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_update_by", value.update_by);

                            using (MySqlDataReader dataReader = cmd.ExecuteReader())
                            {
                                //if (status_code == "product_exist")
                                //{
                                if (dataReader.HasRows == true)
                                {
                                    while (dataReader.Read())
                                    {
                                        ProductDetail prop = new ProductDetail();
                                        prop.product_id = dataReader["product_id"] as int? ?? default(int);
                                        prop.category_id = dataReader["category_id"] as int? ?? default(int);
                                        prop.merchant_id = dataReader["merchant_id"] as int? ?? default(int);
                                        list.Add(prop);
                                    }

                                    dataReader.Close();

                                    foreach (ProductDetail prod in list)
                                    {
                                        using (MySqlCommand cmd2 = new MySqlCommand("sp_delete_product_detail", conn))
                                        {

                                            cmd2.Transaction = trans;
                                            cmd2.CommandType = CommandType.StoredProcedure;
                                            cmd2.Parameters.Clear();
                                            cmd2.Parameters.AddWithValue("@p_product_id", prod.product_id);
                                            cmd2.Parameters.AddWithValue("@p_category_id", value.category_id);
                                            cmd2.Parameters.AddWithValue("@p_merchant_id", value.merchant_id);
                                            cmd2.Parameters.AddWithValue("@p_update_by", value.update_by);
                                            cmd2.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                                            cmd2.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                                            cmd2.ExecuteNonQuery();


                                            var status_code2 = (string)cmd2.Parameters["@p_status_code"].Value;

                                            data.Success = true;
                                            data.Code = "delete_success";
                                            data.Message = WebApiResources.ProductCategoryDeleteSuccess;
                                        }
                                    }
                                }
                                else
                                {
                                    data.Success = true;
                                    data.Code = "delete_success";
                                    data.Message = WebApiResources.ProductCategoryDeleteSuccess;
                                }
                                //}
                                //else
                                //{
                                //    data.Success = true;
                                //    data.Code = "delete_success";
                                //    data.Message = "Product category deleted successfully.";
                                //}
                            }
                            //cmd.ExecuteNonQuery();

                            //var status_code = (string)cmd.Parameters["@p_status_code"].Value;
                            trans.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "merchant/delete-product-category");
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
        [Route("api/v2/merchant/create-product-detail")]
        public IHttpActionResult Post_Merchant_Create_Product_Detail(string culture, [FromBody]CreateProductDetailParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            bool upload = false;
            FileTransferProtocol ftp = new FileTransferProtocol();
            MerchantDataModel data = new MerchantDataModel();
            if (value.merchant_id != 0 && value.category_id != 0 && value.product_name != null && value.create_by != null 
                && value.unit_price != 0 && value.text_color != null && value.background_color != null && !string.IsNullOrEmpty(value.available_day))
            {
                MySqlTransaction trans = null;
                MySqlTransaction trans2 = null;
                data.Success = false;
                string sqlQuery = string.Empty;

                int product_id;
                string password = string.Empty;
                string ftp_address = ConfigurationManager.AppSettings["ftpAddress"];
                string ftp_username = ConfigurationManager.AppSettings["ftpUName"];
                string ftp_password = ConfigurationManager.AppSettings["ftpPWord"];
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;
                DateTime dt = DateTime.Now;
                //string time_stamp = dt.ToString("yyyy_MM_dd");
                string photo_url = "/images/merchants/" + value.merchant_id + "/products/" + value.category_id + "/" + value.file_name;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd = new MySqlCommand("sp_insert_product_detail", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_merchant_id", value.merchant_id);
                            cmd.Parameters.AddWithValue("@p_category_id", value.category_id);
                            cmd.Parameters.AddWithValue("@p_product_name", value.product_name);
                            cmd.Parameters.AddWithValue("@p_product_sku", value.product_sku);
                            cmd.Parameters.AddWithValue("@p_unit_price", value.unit_price);
                            cmd.Parameters.AddWithValue("@p_cost_price", value.cost_price);
                            cmd.Parameters.AddWithValue("@p_discount_amount", value.discount_amount);
                            cmd.Parameters.AddWithValue("@p_product_description", value.product_description);
                            cmd.Parameters.AddWithValue("@p_product_ingredient", value.product_ingredient);
                            cmd.Parameters.AddWithValue("@p_product_weight", value.product_weight);
                            cmd.Parameters.AddWithValue("@p_special_flag", value.special_flag);
                            cmd.Parameters.AddWithValue("@p_available_day", value.available_day);
                            cmd.Parameters.AddWithValue("@p_text_color", value.text_color);
                            cmd.Parameters.AddWithValue("@p_background_color", value.background_color);
                            cmd.Parameters.AddWithValue("@p_create_by", value.create_by);
                            cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                            cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd.Parameters.Add("@p_product_id", MySqlDbType.Int16);
                            cmd.Parameters["@p_product_id"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            var status_code = (string)cmd.Parameters["@p_status_code"].Value;


                            if (status_code == "record_saved")
                            {
                                product_id = Convert.ToInt16(cmd.Parameters["@p_product_id"].Value);

                                if (value.file_name != null && value.photo_base64 != null)
                                {
                                    var inputText = ValidateBase64EncodedString(value.photo_base64);
                                    byte[] fileBytes = Convert.FromBase64String(inputText);
                                    string directory = "/images/merchants/" + value.merchant_id + "/products/" + value.category_id;
                                    if (ftp.Check_Directory_Exists(ftp_address, directory, ftp_username, ftp_password) == true)
                                    {
                                        try
                                        {
                                            //Create FTP Request.
                                            FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + directory + "/" + value.file_name);
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
                                        string folder_merchant_id = "/images/merchants/" + value.merchant_id;
                                        string folder_products = "/images/merchants/" + value.merchant_id + "/products";
                                        string folder_category_id = "/images/merchants/" + value.merchant_id + "/products/" + value.category_id;
                                        //string folder_timestamp = "/images/merchants/" + value.merchant_id + "/products/" + value.club_id + "/" + time_stamp;

                                        if (ftp.Check_Directory_Exists(ftp_address, folder_merchant_id, ftp_username, ftp_password) == true) //merchant id
                                        {
                                            if (ftp.Check_Directory_Exists(ftp_address, folder_products, ftp_username, ftp_password) == true) //products 
                                            {
                                                if (ftp.Check_Directory_Exists(ftp_address, folder_category_id, ftp_username, ftp_password) == true) // category id
                                                {
  
                                                    try
                                                    {
                                                        //Create FTP Request.
                                                        FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/merchants/" + value.merchant_id + "/products/" + value.category_id + "/" + value.file_name);
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
                                                    if (ftp.Create_Directory(ftp_address, folder_category_id, ftp_username, ftp_password) == true)
                                                    {
                                                        try
                                                        {
                                                            //Create FTP Request.
                                                            FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/merchants/" + value.merchant_id + "/products/" + value.category_id + "/" + value.file_name);
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
                                            else
                                            {
                                                if (ftp.Create_Directory(ftp_address, folder_products, ftp_username, ftp_password) == true)
                                                {
                                                    if (ftp.Check_Directory_Exists(ftp_address, folder_category_id, ftp_username, ftp_password) == true)
                                                    {
   
                                                        try
                                                        {
                                                            //Create FTP Request.
                                                            FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/merchants/" + value.merchant_id + "/products/" + value.category_id + "/" + value.file_name);
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
                                                        if (ftp.Create_Directory(ftp_address, folder_category_id, ftp_username, ftp_password) == true)
                                                        {
                                                            try
                                                            {
                                                                //Create FTP Request.
                                                                FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/merchants/" + value.merchant_id + "/products/" + value.category_id + "/" + value.file_name);
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
                                        else
                                        {
                                            if (ftp.Create_Directory(ftp_address, folder_merchant_id, ftp_username, ftp_password) == true)
                                            {
                                                if (ftp.Check_Directory_Exists(ftp_address, folder_products, ftp_username, ftp_password) == true)
                                                {
                                                    if (ftp.Check_Directory_Exists(ftp_address, folder_category_id, ftp_username, ftp_password) == true)
                                                    {
                                                        try
                                                        {
                                                            //Create FTP Request.
                                                            FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/merchants/" + value.merchant_id + "/products/" + value.category_id + "/" + value.file_name);
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
                                                        if (ftp.Create_Directory(ftp_address, folder_category_id, ftp_username, ftp_password) == true)
                                                        {
                                                            try
                                                            {
                                                                //Create FTP Request.
                                                                FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/merchants/" + value.merchant_id + "/products/" + value.category_id + "/" + value.file_name);
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
                                                else
                                                {
                                                    if (ftp.Create_Directory(ftp_address, folder_products, ftp_username, ftp_password) == true)
                                                    {
                                                        if (ftp.Check_Directory_Exists(ftp_address, folder_category_id, ftp_username, ftp_password) == true)
                                                        {
                                                            try
                                                            {
                                                                //Create FTP Request.
                                                                FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/merchants/" + value.merchant_id + "/products/" + value.category_id + "/" + value.file_name);
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
                                                            if (ftp.Create_Directory(ftp_address, folder_category_id, ftp_username, ftp_password) == true)
                                                            {
                                                                try
                                                                {
                                                                    //Create FTP Request.
                                                                    FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/merchants/" + value.merchant_id + "/products/" + value.category_id + "/" + value.file_name);
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
                                    }

                                    if (upload == true)
                                    {
                                        try
                                        {
                                            using (MySqlCommand cmd2 = new MySqlCommand("sp_update_product_detail_photo", conn))
                                            {
                                                trans2 = conn.BeginTransaction();
                                                cmd2.Transaction = trans2;
                                                cmd2.CommandType = CommandType.StoredProcedure;
                                                cmd2.Parameters.Clear();
                                                cmd2.Parameters.AddWithValue("@p_product_id", product_id);
                                                cmd2.Parameters.AddWithValue("@p_category_id", value.category_id);
                                                cmd2.Parameters.AddWithValue("@p_merchant_id", value.merchant_id);
                                                cmd2.Parameters.AddWithValue("@p_file_name", value.file_name);
                                                cmd2.Parameters.AddWithValue("@p_photo_url", photo_url);
                                                MySqlDataReader dataReader2 = cmd2.ExecuteReader();
                                                dataReader2.Close();
                                                trans2.Commit();
                                            }

                                            data.Success = true;
                                            data.Code = "record_saved";
                                            data.Message = WebApiResources.ProductDetailCreateSuccess;
                                        }
                                        catch (Exception ex)
                                        {
                                            data.Success = false;
                                            data.Code = "error_occured";
                                            data.Message = WebApiResources.ErrorOccured;
                                        }

                                    }
                                }
                                else
                                {
                                    data.Success = true;
                                    data.Code = "record_saved";
                                    data.Message = WebApiResources.ProductDetailCreateSuccess;
                                }
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "record_exist";
                                data.Message = WebApiResources.ProductDetailExist;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "merchant/create-product-detail");
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
        [Route("api/v2/merchant/update-product-detail")]
        public IHttpActionResult Post_Merchant_Update_Product_Detail(string culture, [FromBody]UpdateProductDetailParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            bool upload = false;
            FileTransferProtocol ftp = new FileTransferProtocol();
            MerchantDataModel data = new MerchantDataModel();
            if (value.product_id != 0 &&  value.merchant_id != 0 && value.category_id != 0 && value.product_name != null 
                && value.unit_price != 0 && value.update_by != null && value.text_color != null && value.background_color != null
                && !string.IsNullOrEmpty(value.available_day))
            {
                MySqlTransaction trans = null;
                MySqlTransaction trans2 = null;
                data.Success = false;
                string sqlQuery = string.Empty;

                string password = string.Empty;
                string ftp_address = ConfigurationManager.AppSettings["ftpAddress"];
                string ftp_username = ConfigurationManager.AppSettings["ftpUName"];
                string ftp_password = ConfigurationManager.AppSettings["ftpPWord"];
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;
                DateTime dt = DateTime.Now;
                //string time_stamp = dt.ToString("yyyy_MM_dd");
                string photo_url = "/images/merchants/" + value.merchant_id + "/products/" + value.category_id + "/" + value.file_name;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd = new MySqlCommand("sp_update_product_detail", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_product_id", value.product_id);
                            cmd.Parameters.AddWithValue("@p_merchant_id", value.merchant_id);
                            cmd.Parameters.AddWithValue("@p_category_id", value.category_id);
                            cmd.Parameters.AddWithValue("@p_product_name", value.product_name);
                            cmd.Parameters.AddWithValue("@p_product_sku", value.product_sku);
                            cmd.Parameters.AddWithValue("@p_unit_price", value.unit_price);
                            cmd.Parameters.AddWithValue("@p_cost_price", value.cost_price);
                            cmd.Parameters.AddWithValue("@p_discount_amount", value.discount_amount);
                            cmd.Parameters.AddWithValue("@p_product_description", value.product_description);
                            cmd.Parameters.AddWithValue("@p_product_ingredient", value.product_ingredient);
                            cmd.Parameters.AddWithValue("@p_product_weight", value.product_weight);
                            cmd.Parameters.AddWithValue("@p_special_flag", value.special_flag);
                            cmd.Parameters.AddWithValue("@p_available_day", value.available_day);
                            cmd.Parameters.AddWithValue("@p_text_color", value.text_color);
                            cmd.Parameters.AddWithValue("@p_background_color", value.background_color);
                            cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                            cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                            cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            var status_code = (string)cmd.Parameters["@p_status_code"].Value;

                            if (status_code == "update_success")
                            {
                                if (value.file_name != null && value.photo_base64 != null)
                                {
                                    var inputText = ValidateBase64EncodedString(value.photo_base64);
                                    byte[] fileBytes = Convert.FromBase64String(inputText);
                                    string directory = "/images/merchants/" + value.merchant_id + "/products/" + value.category_id;
                                    if (ftp.Check_Directory_Exists(ftp_address, directory, ftp_username, ftp_password) == true)
                                    {
                                        try
                                        {
                                            //Create FTP Request.
                                            FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + directory + "/" + value.file_name);
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
                                        string folder_merchant_id = "/images/merchants/" + value.merchant_id;
                                        string folder_products = "/images/merchants/" + value.merchant_id + "/products";
                                        string folder_category_id = "/images/merchants/" + value.merchant_id + "/products/" + value.category_id;
                                        //string folder_timestamp = "/images/merchants/" + value.merchant_id + "/products/" + value.club_id + "/" + time_stamp;

                                        if (ftp.Check_Directory_Exists(ftp_address, folder_merchant_id, ftp_username, ftp_password) == true) //merchant id
                                        {
                                            if (ftp.Check_Directory_Exists(ftp_address, folder_products, ftp_username, ftp_password) == true) //products 
                                            {
                                                if (ftp.Check_Directory_Exists(ftp_address, folder_category_id, ftp_username, ftp_password) == true) // category id
                                                {

                                                    try
                                                    {
                                                        //Create FTP Request.
                                                        FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/merchants/" + value.merchant_id + "/products/" + value.category_id + "/" + value.file_name);
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
                                                    if (ftp.Create_Directory(ftp_address, folder_category_id, ftp_username, ftp_password) == true)
                                                    {
                                                        try
                                                        {
                                                            //Create FTP Request.
                                                            FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/merchants/" + value.merchant_id + "/products/" + value.category_id + "/" + value.file_name);
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
                                            else
                                            {
                                                if (ftp.Create_Directory(ftp_address, folder_products, ftp_username, ftp_password) == true)
                                                {
                                                    if (ftp.Check_Directory_Exists(ftp_address, folder_category_id, ftp_username, ftp_password) == true)
                                                    {

                                                        try
                                                        {
                                                            //Create FTP Request.
                                                            FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/merchants/" + value.merchant_id + "/products/" + value.category_id + "/" + value.file_name);
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
                                                        if (ftp.Create_Directory(ftp_address, folder_category_id, ftp_username, ftp_password) == true)
                                                        {
                                                            try
                                                            {
                                                                //Create FTP Request.
                                                                FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/merchants/" + value.merchant_id + "/products/" + value.category_id + "/" + value.file_name);
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
                                        else
                                        {
                                            if (ftp.Create_Directory(ftp_address, folder_merchant_id, ftp_username, ftp_password) == true)
                                            {
                                                if (ftp.Check_Directory_Exists(ftp_address, folder_products, ftp_username, ftp_password) == true)
                                                {
                                                    if (ftp.Check_Directory_Exists(ftp_address, folder_category_id, ftp_username, ftp_password) == true)
                                                    {
                                                        try
                                                        {
                                                            //Create FTP Request.
                                                            FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/merchants/" + value.merchant_id + "/products/" + value.category_id + "/" + value.file_name);
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
                                                        if (ftp.Create_Directory(ftp_address, folder_category_id, ftp_username, ftp_password) == true)
                                                        {
                                                            try
                                                            {
                                                                //Create FTP Request.
                                                                FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/merchants/" + value.merchant_id + "/products/" + value.category_id + "/" + value.file_name);
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
                                                else
                                                {
                                                    if (ftp.Create_Directory(ftp_address, folder_products, ftp_username, ftp_password) == true)
                                                    {
                                                        if (ftp.Check_Directory_Exists(ftp_address, folder_category_id, ftp_username, ftp_password) == true)
                                                        {
                                                            try
                                                            {
                                                                //Create FTP Request.
                                                                FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/merchants/" + value.merchant_id + "/products/" + value.category_id + "/" + value.file_name);
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
                                                            if (ftp.Create_Directory(ftp_address, folder_category_id, ftp_username, ftp_password) == true)
                                                            {
                                                                try
                                                                {
                                                                    //Create FTP Request.
                                                                    FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/merchants/" + value.merchant_id + "/products/" + value.category_id + "/" + value.file_name);
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
                                    }

                                    if (upload == true)
                                    {
                                        try
                                        {
                                            using (MySqlCommand cmd2 = new MySqlCommand("sp_update_product_detail_photo", conn))
                                            {
                                                trans2 = conn.BeginTransaction();
                                                cmd2.Transaction = trans2;
                                                cmd2.CommandType = CommandType.StoredProcedure;
                                                cmd2.Parameters.Clear();
                                                cmd2.Parameters.AddWithValue("@p_product_id", value.product_id);
                                                cmd2.Parameters.AddWithValue("@p_category_id", value.category_id);
                                                cmd2.Parameters.AddWithValue("@p_merchant_id", value.merchant_id);
                                                cmd2.Parameters.AddWithValue("@p_file_name", value.file_name);
                                                cmd2.Parameters.AddWithValue("@p_photo_url", photo_url);
                                                MySqlDataReader dataReader2 = cmd2.ExecuteReader();
                                                dataReader2.Close();
                                                trans2.Commit();
                                            }

                                            data.Success = true;
                                            data.Code = "record_saved";
                                            data.Message = WebApiResources.ProductDetailUpdateSuccess;
                                        }
                                        catch (Exception ex)
                                        {
                                            data.Success = false;
                                            data.Code = "error_occured";
                                            data.Message = WebApiResources.ErrorOccured;
                                        }

                                    }
                                }
                                else
                                {
                                    data.Success = true;
                                    data.Code = "record_saved";
                                    data.Message = WebApiResources.ProductDetailUpdateSuccess;
                                }
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "record_exist";
                                data.Message = WebApiResources.ProductDetailExist;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "merchant/update-product-detail");
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
        [Route("api/v2/merchant/remove-product-photo")]
        public IHttpActionResult Post_Merchant_Remove_Product_Photo(string culture, [FromBody] RemoveProductPhotoParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            MerchantDataModel data = new MerchantDataModel();
            if (value.product_id != 0 && value.merchant_id != 0 && value.category_id != 0 && value.update_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_delete_product_photo", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_product_id", value.product_id);
                            cmd.Parameters.AddWithValue("@p_merchant_id", value.merchant_id);
                            cmd.Parameters.AddWithValue("@p_category_id", value.category_id);
                            cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            data.Success = true;
                            data.Code = "success";
                            data.Message = WebApiResources.PhotoSuccessRemove;
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "merchant/remove-product-photo");
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
        [Route("api/v2/merchant/delete-product-detail")]
        public IHttpActionResult Post_Merchant_Delete_Product_Detail(string culture, [FromBody]DeleteProductDetailParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            MerchantDataModel data = new MerchantDataModel();
            if (value.product_id != 0 && value.merchant_id != 0 && value.category_id != 0 && value.update_by != null)
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


                        using (MySqlCommand cmd2 = new MySqlCommand("sp_delete_product_detail", conn))
                        {

                            cmd2.Transaction = trans;
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.Clear();
                            cmd2.Parameters.AddWithValue("@p_product_id", value.product_id);
                            cmd2.Parameters.AddWithValue("@p_category_id", value.category_id);
                            cmd2.Parameters.AddWithValue("@p_merchant_id", value.merchant_id);
                            cmd2.Parameters.AddWithValue("@p_update_by", value.update_by);
                            cmd2.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                            cmd2.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd2.ExecuteNonQuery();
                            trans.Commit();

                            var status_code2 = (string)cmd2.Parameters["@p_status_code"].Value;

                            data.Success = true;
                            data.Code = "delete_success";
                            data.Message = WebApiResources.ProductDetailDeleteSuccess;
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "merchant/delete-product-detail");
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
        [Route("api/v2/merchant/create-product-nutrition")]
        public IHttpActionResult Post_Merchant_Create_Product_Nutrition([FromBody]CreateProductNutritionParam value)
        {
            MerchantDataModel data = new MerchantDataModel();
            if (value.product_id != 0 && value.nutrition_name != null && value.per_serving != null && value.create_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_insert_product_nutrition_info", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_product_id", value.product_id);
                            cmd.Parameters.AddWithValue("@p_nutrition_name", value.nutrition_name);
                            cmd.Parameters.AddWithValue("@p_per_gram", value.per_gram);
                            cmd.Parameters.AddWithValue("@p_per_serving", value.per_serving);
                            cmd.Parameters.AddWithValue("@p_create_by", value.create_by);
                            cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                            cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            var status_code = (string)cmd.Parameters["@p_status_code"].Value;

                            if (status_code == "record_saved")
                            {
                                data.Success = true;
                                data.Code = "record_saved";
                                data.Message = "Product nutrition saved successfully.";
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "record_exist";
                                data.Message = "Product nutrition already exists.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "merchant/create-product-nutrition");
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
        [Route("api/v2/merchant/update-product-nutrition")]
        public IHttpActionResult Post_Merchant_Update_Product_Nutrition([FromBody]UpdateProductNutritionParam value)
        {
            MerchantDataModel data = new MerchantDataModel();
            if (value.info_id != 0 && value.product_id != 0 && value.nutrition_name != null && value.update_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_update_product_nutrition_info", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_info_id", value.info_id);
                            cmd.Parameters.AddWithValue("@p_product_id", value.product_id);
                            cmd.Parameters.AddWithValue("@p_nutrition_name", value.nutrition_name);
                            cmd.Parameters.AddWithValue("@p_per_gram", value.per_gram);
                            cmd.Parameters.AddWithValue("@p_per_serving", value.per_serving);
                            cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                            cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                            cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            var status_code = (string)cmd.Parameters["@p_status_code"].Value;

                            if (status_code == "update_success")
                            {
                                data.Success = true;
                                data.Code = "update_success";
                                data.Message = "Product nutrition updated successfully.";
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "record_exist";
                                data.Message = "Product nutrition already exists.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "merchant/update-product-nutrition");
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
        [Route("api/v2/merchant/delete-product-nutrition")]
        public IHttpActionResult Post_Merchant_Delete_Product_Nutrition([FromBody]DeleteProductNutritionParam value)
        {
            MerchantDataModel data = new MerchantDataModel();
            if (value.info_id != 0 && value.product_id != 0  && value.update_by != null)
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


                        using (MySqlCommand cmd2 = new MySqlCommand("sp_delete_product_nutrition_info", conn))
                        {

                            cmd2.Transaction = trans;
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.Clear();
                            cmd2.Parameters.AddWithValue("@p_product_id", value.product_id);
                            cmd2.Parameters.AddWithValue("@p_info_id", value.info_id);
                            cmd2.Parameters.AddWithValue("@p_update_by", value.update_by);
                            cmd2.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                            cmd2.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd2.ExecuteNonQuery();
                            trans.Commit();

                            var status_code2 = (string)cmd2.Parameters["@p_status_code"].Value;

                            data.Success = true;
                            data.Code = "delete_success";
                            data.Message = "Product nutrition deleted successfully.";
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "merchant/delete-product-nutrition");
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
        [Route("api/v2/merchant/product-category")]
        public IHttpActionResult Post_Merchant_Product_Category([FromUri] SqlFilterParam uri, [FromBody]ProductCategoryParam body)
        {
            MerchantDataModel data = new MerchantDataModel();
            int? limit = 0;
            int? offset = 0;
            if (uri != null)
            {
                if (uri.limit == null)
                {
                    limit = 0;
                }
                else
                {
                    limit = uri.limit;
                }
                if (uri.offset == null)
                {
                    offset = 0;
                }
                else
                {
                    offset = uri.offset;
                }
            }
            else
            {
                limit = 0;
                offset = 0;
            }
            if (body.school_id != 0 && body.merchant_id != 0)
            {
                List<ProductCategory> list = new List<ProductCategory>();
                ProductCategoryModel listData = new ProductCategoryModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_product_category", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_limit", limit);
                            cmd.Parameters.AddWithValue("@p_offset", offset);
                            cmd.Parameters.AddWithValue("@p_school_id", body.school_id);
                            cmd.Parameters.AddWithValue("@p_merchant_id", body.merchant_id);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    ProductCategory prop = new ProductCategory();
                                    prop.category_id = dataReader["category_id"] as int? ?? default(int);
                                    prop.merchant_id = dataReader["merchant_id"] as int? ?? default(int);
                                    prop.school_id = dataReader["school_id"] as int? ?? default(int);
                                    prop.category_name = dataReader["category_name"].ToString();
                                    prop.category_description = dataReader["category_description"].ToString();
                                    prop.total_product = Convert.ToInt16(dataReader["total_product"]);
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "product_category";
                                listData.Message = "Product category listing.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Product category could not be found.";
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
                        ExceptionUtility.LogException(ex, "merchant/product-category");
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

        //[HttpPost]
        //[Route("api/v2/merchant/product-category-unauthorized")]
        //public IHttpActionResult Post_Merchant_Product_Category_Unauthorized([FromUri] SqlFilterParam uri, [FromBody]ProductCategoryParam body)
        //{
        //    MerchantDataModel data = new MerchantDataModel();
        //    int? limit = 0;
        //    int? offset = 0;
        //    if (uri != null)
        //    {
        //        if (uri.limit == null)
        //        {
        //            limit = 0;
        //        }
        //        else
        //        {
        //            limit = uri.limit;
        //        }
        //        if (uri.offset == null)
        //        {
        //            offset = 0;
        //        }
        //        else
        //        {
        //            offset = uri.offset;
        //        }
        //    }
        //    else
        //    {
        //        limit = 0;
        //        offset = 0;
        //    }
        //    if (body.school_id != 0 && body.merchant_id != 0)
        //    {
        //        List<ProductCategory> list = new List<ProductCategory>();
        //        ProductCategoryModel listData = new ProductCategoryModel();
        //        MySqlTransaction trans = null;
        //        listData.Success = false;
        //        string sqlQuery = string.Empty;
        //        string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

        //        using (MySqlConnection conn = new MySqlConnection(constr))
        //        {
        //            try
        //            {
        //                conn.Open();
        //                trans = conn.BeginTransaction();

        //                using (MySqlCommand cmd = new MySqlCommand("sp_get_product_category", conn))
        //                {

        //                    cmd.Transaction = trans;
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    cmd.Parameters.Clear();
        //                    cmd.Parameters.AddWithValue("@p_limit", limit);
        //                    cmd.Parameters.AddWithValue("@p_offset", offset);
        //                    cmd.Parameters.AddWithValue("@p_school_id", body.school_id);
        //                    cmd.Parameters.AddWithValue("@p_merchant_id", body.merchant_id);
        //                    MySqlDataReader dataReader = cmd.ExecuteReader();

        //                    if (dataReader.HasRows == true)
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            ProductCategory prop = new ProductCategory();
        //                            if (Convert.ToInt32(dataReader["total_product"]) > 0) 
        //                            {
        //                                prop.category_id = dataReader["category_id"] as int? ?? default(int);
        //                                prop.merchant_id = dataReader["merchant_id"] as int? ?? default(int);
        //                                prop.school_id = dataReader["school_id"] as int? ?? default(int);
        //                                prop.category_name = dataReader["category_name"].ToString();
        //                                prop.category_description = dataReader["category_description"].ToString();
        //                                prop.total_product = Convert.ToInt32(dataReader["total_product"]);
        //                                list.Add(prop);
        //                            }
        //                        }
        //                        listData.Success = true;
        //                        listData.Code = "product_category";
        //                        listData.Message = "Product category listing.";
        //                        listData.Data = list;
        //                    }
        //                    else
        //                    {
        //                        //listData.Success = false;
        //                        listData.Success = true;
        //                        listData.Code = "no_record_found";
        //                        listData.Message = "Product category could not be found.";
        //                        listData.Data = list;
        //                    }

        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                //listData.Success = false;
        //                data.Success = false;
        //                data.Code = "error_occured";
        //                data.Message = WebApiResources.ErrorOccured;
        //                ExceptionUtility.LogException(ex, "merchant/product-category");
        //            }
        //            finally
        //            {
        //                conn.Close();
        //            }
        //        }

        //        if (listData.Success == true)
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
        [Route("api/v2/merchant/product-detail")]
        public IHttpActionResult Post_Merchant_Product_Detail([FromUri] SqlFilterParam uri, [FromBody]ProductDetailParam body)
        {
            MerchantDataModel data = new MerchantDataModel();
            int? limit = 0;
            int? offset = 0;
            if (uri != null)
            {
                if (uri.limit == null)
                {
                    limit = 0;
                }
                else
                {
                    limit = uri.limit;
                }
                if (uri.offset == null)
                {
                    offset = 0;
                }
                else
                {
                    offset = uri.offset;
                }
            }
            else
            {
                limit = 0;
                offset = 0;
            }
            if (body.school_id != 0 && body.merchant_id != 0 && body.category_id != 0)
            {
                List<ProductDetail> list = new List<ProductDetail>();
                ProductDetailModel listData = new ProductDetailModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_product_detail", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_limit", limit);
                            cmd.Parameters.AddWithValue("@p_offset", offset);
                            cmd.Parameters.AddWithValue("@p_school_id", body.school_id);
                            cmd.Parameters.AddWithValue("@p_merchant_id", body.merchant_id);
                            cmd.Parameters.AddWithValue("@p_category_id", body.category_id);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    ProductDetail prop = new ProductDetail();
                                    prop.product_id = dataReader["product_id"] as int? ?? default(int);
                                    prop.category_id = dataReader["category_id"] as int? ?? default(int);
                                    prop.category_name = dataReader["category_name"].ToString();
                                    prop.merchant_id = dataReader["merchant_id"] as int? ?? default(int);
                                    prop.school_id = dataReader["school_id"] as int? ?? default(int);
                                    prop.product_name = dataReader["product_name"].ToString();
                                    prop.product_sku = dataReader["product_sku"].ToString();
                                    prop.photo_url = dataReader["photo_url"].ToString();
                                    prop.unit_price = dataReader["unit_price"] as decimal? ?? default(decimal);
                                    prop.cost_price = dataReader["cost_price"] as decimal? ?? default(decimal);
                                    prop.discount_amount = dataReader["discount_amount"] as decimal? ?? default(decimal);
                                    prop.product_description = dataReader["product_description"].ToString();
                                    prop.product_weight = dataReader["product_weight"].ToString();
                                    prop.product_ingredient = dataReader["product_ingredient"].ToString();
                                    prop.special_flag = dataReader["special_flag"].ToString();
                                    prop.available_day = dataReader["available_day"].ToString();
                                    prop.text_color = dataReader["text_color"].ToString();
                                    prop.background_color = dataReader["background_color"].ToString();
                                    prop.product_weight = dataReader["product_weight"].ToString();
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "product_detail";
                                listData.Message = "Product detail listing.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Product detail could not be found.";
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
                        ExceptionUtility.LogException(ex, "merchant/product-detail");
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

        //[HttpPost]
        //[Route("api/v2/merchant/product-detail-unauthorized")]
        //public IHttpActionResult Post_Merchant_Product_Detail_Unauthorized([FromUri] SqlFilterParam uri, [FromBody]ProductDetailParam body)
        //{
        //    MerchantDataModel data = new MerchantDataModel();
        //    int? limit = 0;
        //    int? offset = 0;
        //    if (uri != null)
        //    {
        //        if (uri.limit == null)
        //        {
        //            limit = 0;
        //        }
        //        else
        //        {
        //            limit = uri.limit;
        //        }
        //        if (uri.offset == null)
        //        {
        //            offset = 0;
        //        }
        //        else
        //        {
        //            offset = uri.offset;
        //        }
        //    }
        //    else
        //    {
        //        limit = 0;
        //        offset = 0;
        //    }
        //    if (body.school_id != 0 && body.merchant_id != 0 && body.category_id != 0)
        //    {
        //        List<ProductDetail> list = new List<ProductDetail>();
        //        ProductDetailModel listData = new ProductDetailModel();
        //        MySqlTransaction trans = null;
        //        listData.Success = false;
        //        string sqlQuery = string.Empty;
        //        string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

        //        using (MySqlConnection conn = new MySqlConnection(constr))
        //        {
        //            try
        //            {
        //                conn.Open();
        //                trans = conn.BeginTransaction();

        //                using (MySqlCommand cmd = new MySqlCommand("sp_get_product_detail", conn))
        //                {

        //                    cmd.Transaction = trans;
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    cmd.Parameters.Clear();
        //                    cmd.Parameters.AddWithValue("@p_limit", limit);
        //                    cmd.Parameters.AddWithValue("@p_offset", offset);
        //                    cmd.Parameters.AddWithValue("@p_school_id", body.school_id);
        //                    cmd.Parameters.AddWithValue("@p_merchant_id", body.merchant_id);
        //                    cmd.Parameters.AddWithValue("@p_category_id", body.category_id);
        //                    MySqlDataReader dataReader = cmd.ExecuteReader();

        //                    if (dataReader.HasRows == true)
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            ProductDetail prop = new ProductDetail();
        //                            prop.product_id = dataReader["product_id"] as int? ?? default(int);
        //                            prop.category_id = dataReader["category_id"] as int? ?? default(int);
        //                            prop.category_name = dataReader["category_name"].ToString();
        //                            prop.merchant_id = dataReader["merchant_id"] as int? ?? default(int);
        //                            prop.school_id = dataReader["school_id"] as int? ?? default(int);
        //                            prop.product_name = dataReader["product_name"].ToString();
        //                            prop.product_sku = dataReader["product_sku"].ToString();
        //                            prop.photo_url = dataReader["photo_url"].ToString();
        //                            prop.unit_price = dataReader["unit_price"] as decimal? ?? default(decimal);
        //                            prop.cost_price = dataReader["cost_price"] as decimal? ?? default(decimal);
        //                            prop.discount_amount = dataReader["discount_amount"] as decimal? ?? default(decimal);
        //                            prop.product_description = dataReader["product_description"].ToString();
        //                            prop.product_weight = dataReader["product_weight"].ToString();
        //                            prop.text_color = dataReader["text_color"].ToString();
        //                            prop.background_color = dataReader["background_color"].ToString();
        //                            list.Add(prop);
        //                        }
        //                        listData.Success = true;
        //                        listData.Code = "product_detail";
        //                        listData.Message = "Product detail listing.";
        //                        listData.Data = list;
        //                    }
        //                    else
        //                    {
        //                        //listData.Success = false;
        //                        listData.Success = true;
        //                        listData.Code = "no_record_found";
        //                        listData.Message = "Product detail could not be found.";
        //                        listData.Data = list;
        //                    }

        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                //listData.Success = false;
        //                data.Success = false;
        //                data.Code = "error_occured";
        //                data.Message = WebApiResources.ErrorOccured;
        //                ExceptionUtility.LogException(ex, "merchant/product-detail");
        //            }
        //            finally
        //            {
        //                conn.Close();
        //            }
        //        }

        //        if (listData.Success == true)
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
        [Route("api/v2/merchant/product-nutrition-info")]
        public IHttpActionResult Post_Merchant_Product_Nutrition_Info([FromBody]ProductNutritionParam body)
        {
            MerchantDataModel data = new MerchantDataModel();
            if (body.school_id != 0 && body.merchant_id != 0 && body.product_id != 0)
            {
                List<ProductNutrition> list = new List<ProductNutrition>();
                ProductNutritionModel listData = new ProductNutritionModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_product_nutrition_info", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_school_id", body.school_id);
                            cmd.Parameters.AddWithValue("@p_merchant_id", body.merchant_id);
                            cmd.Parameters.AddWithValue("@p_product_id", body.product_id);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    ProductNutrition prop = new ProductNutrition();
                                    prop.info_id = dataReader["info_id"] as int? ?? default(int);
                                    prop.product_id = dataReader["product_id"] as int? ?? default(int);
                                    prop.merchant_id = dataReader["merchant_id"] as int? ?? default(int);
                                    prop.school_id = dataReader["school_id"] as int? ?? default(int);
                                    prop.nutrition_name = dataReader["nutrition_name"].ToString();
                                    prop.per_gram = dataReader["per_gram"].ToString();
                                    prop.per_serving = dataReader["per_serving"].ToString();
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "product_nutrition_info";
                                listData.Message = "Product nutrition info listing.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Product nutrition info could not be found.";
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
                        ExceptionUtility.LogException(ex, "merchant/product-nutrition-info");
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
        [Route("api/v2/merchant/order-history-group")]
        public IHttpActionResult Post_Merchant_Order_History_Group([FromBody] OrderHistoryGroupParam value)
        {
            MerchantDataModel data = new MerchantDataModel();
            if (value.school_id != 0 && value.merchant_id != 0)
            {
                List<PurchaseDate> list = new List<PurchaseDate>();
                PurchaseDateModel listData = new PurchaseDateModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_order_history_group", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_merchant_id", value.merchant_id);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    PurchaseDate prop = new PurchaseDate();
                                    prop.pickup_date = Convert.ToDateTime(dataReader["pickup_date"]);
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "order_history";
                                listData.Message = "Order history group listing.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Order history group is empty.";
                                listData.Data = list;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "merchant/order-history-group");
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
        [Route("api/v2/merchant/order-history")]
        public IHttpActionResult Post_Merchant_Order_History([FromBody] OrderHistoryParam value)
        {
            DateTime dt = DateTime.MinValue;
            MerchantDataModel data = new MerchantDataModel();
            if (value.school_id != 0 && value.merchant_id != 0 && value.pickup_date != dt)
            {
                List<OrderHistory> list = new List<OrderHistory>();
                OrderHistoryModel listData = new OrderHistoryModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_order_history_date", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_merchant_id", value.merchant_id);
                            cmd.Parameters.AddWithValue("@p_pickup_date", value.pickup_date);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    OrderHistory prop = new OrderHistory();
                                    prop.pickup_date = Convert.ToDateTime(dataReader["pickup_date"]);
                                    prop.total_order = Convert.ToInt32(dataReader["total_order"]);
                                    prop.total_amount = Convert.ToDecimal(dataReader["total_amount"]);
                                    prop.merchant_id = Convert.ToInt32(dataReader["merchant_id"]);
                                    prop.school_id = Convert.ToInt32(dataReader["school_id"]);
                                    prop.school_name = dataReader["school_name"].ToString();
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.photo_url = dataReader["photo_url"].ToString();
                                    if (dataReader["class_id"] != DBNull.Value)
                                    {
                                        prop.class_id = Convert.ToInt16(dataReader["class_id"]);
                                        prop.class_name = dataReader["class_name"].ToString();
                                    }
                                    if (dataReader["pickup_time"] != DBNull.Value)
                                    {
                                        prop.pickup_time = dataReader.GetTimeSpan(dataReader.GetOrdinal("pickup_time"));
                                    }
                                    if (dataReader["service_method_id"] != DBNull.Value)
                                    {
                                        prop.service_method_id = Convert.ToInt32(dataReader["service_method_id"]);
                                    }
                                    prop.delivery_location = dataReader["delivery_location"].ToString();
                                    prop.order_id = dataReader["order_id"].ToString();
                                    prop.order_status_id = Convert.ToInt32(dataReader["order_status_id"]);
                                    prop.order_status = dataReader["order_status"].ToString();
                                    prop.order_status_bm = dataReader["order_status_bm"].ToString();
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "order_history";
                                listData.Message = "Order history listing.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Order history is empty.";
                                listData.Data = list;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "merchant/order-history");
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
        [Route("api/v2/merchant/student-order-history")]
        public IHttpActionResult Post_Merchant_Order_History_Student([FromBody] StudentOrderHistoryParam value)
        {
            DateTime dt = DateTime.MinValue;
            MerchantDataModel data = new MerchantDataModel();
            if (value.merchant_id != 0 && value.school_id != 0 && value.class_id != 0 && value.pickup_date != dt)
            {
                List<StudentOrderHistory> list = new List<StudentOrderHistory>();
                StudentOrderHistoryModel listData = new StudentOrderHistoryModel();
                MySqlTransaction trans = null;
                data.Success = false;
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_order_history_student", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_merchant_id", value.merchant_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_class_id", value.class_id);
                            cmd.Parameters.AddWithValue("@p_pickup_date", value.pickup_date);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    StudentOrderHistory prop = new StudentOrderHistory();
                                    prop.pickup_date = Convert.ToDateTime(dataReader["pickup_date"]);
                                    prop.total_amount = Convert.ToDecimal(dataReader["total_amount"]);
                                    prop.recipient_id = Convert.ToInt32(dataReader["recipient_id"]);
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.photo_url = dataReader["photo_url"].ToString();
                                    prop.school_id = Convert.ToInt32(dataReader["school_id"]);
                                    prop.school_name = dataReader["school_name"].ToString();
                                    prop.class_id = Convert.ToInt32(dataReader["class_id"]);
                                    prop.class_name = dataReader["class_name"].ToString();
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "student_order";
                                listData.Message = "Student order history listing.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Student order history is empty.";
                                listData.Data = list;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "merchant/student-order-history");
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
        [Route("api/v2/merchant/student-order-history-detail")]
        public IHttpActionResult Post_Merchant_Order_History_Student_Detail([FromBody] DetailOrderHistoryParam value)
        {
            DateTime dt = DateTime.MinValue;
            MerchantDataModel data = new MerchantDataModel();
            if (value.school_id != 0 && value.class_id != 0 && value.pickup_date != dt && value.profile_id != 0)
            {
                List<StudentDetailOrderHistory> list = new List<StudentDetailOrderHistory>();
                StudentDetailOrderHistoryModel listData = new StudentDetailOrderHistoryModel();
                MySqlTransaction trans = null;
                data.Success = false;
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_order_history_detail", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_merchant_id", 0);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_class_id", value.class_id);
                            cmd.Parameters.AddWithValue("@p_pickup_date", value.pickup_date);
                            cmd.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                            cmd.Parameters.AddWithValue("@p_product_id", 0);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    StudentDetailOrderHistory prop = new StudentDetailOrderHistory();
                                    prop.pickup_date = Convert.ToDateTime(dataReader["pickup_date"]);
                                    prop.recipient_id = Convert.ToInt32(dataReader["recipient_id"]);
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.photo_url = dataReader["photo_url"].ToString();
                                    prop.school_id = Convert.ToInt32(dataReader["school_id"]);
                                    prop.school_name = dataReader["school_name"].ToString();
                                    prop.class_id = Convert.ToInt32(dataReader["class_id"]);
                                    prop.class_name = dataReader["class_name"].ToString();
                                    prop.product_id = Convert.ToInt32(dataReader["product_id"]);
                                    prop.product_name = dataReader["product_name"].ToString();
                                    prop.product_photo_url = dataReader["product_photo_url"].ToString();
                                    prop.unit_price = Convert.ToDecimal(dataReader["unit_price"]);
                                    prop.product_qty = Convert.ToInt32(dataReader["product_qty"]);
                                    prop.sub_total_amount = Convert.ToDecimal(dataReader["sub_total_amount"]);
                                    prop.discount_amount = Convert.ToDecimal(dataReader["discount_amount"]);
                                    prop.total_amount = Convert.ToDecimal(dataReader["total_amount"]);

                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "student_order";
                                listData.Message = "Student order history detail listing.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Student order history detail is empty.";
                                listData.Data = list;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "merchant/student-order-history-detail");
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
        [Route("api/v2/merchant/product-order-history")]
        public IHttpActionResult Post_Merchant_Order_History_Product([FromBody] StudentOrderHistoryParam value)
        {
            DateTime dt = DateTime.MinValue;
            MerchantDataModel data = new MerchantDataModel();
            if (value.merchant_id != 0 &&  value.school_id != 0 && value.class_id != 0 && value.pickup_date != dt)
            {
                List<ProductOrderHistory> list = new List<ProductOrderHistory>();
                ProductOrderHistoryModel listData = new ProductOrderHistoryModel();
                MySqlTransaction trans = null;
                data.Success = false;
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_order_history_product", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_merchant_id", value.merchant_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_class_id", value.class_id);
                            cmd.Parameters.AddWithValue("@p_pickup_date", value.pickup_date);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    ProductOrderHistory prop = new ProductOrderHistory();
                                    prop.pickup_date = Convert.ToDateTime(dataReader["pickup_date"]);
                                    prop.product_id = Convert.ToInt32(dataReader["product_id"]);
                                    prop.product_name = dataReader["product_name"].ToString();
                                    prop.product_photo_url = dataReader["product_photo_url"].ToString();
                                    prop.unit_price = Convert.ToDecimal(dataReader["unit_price"]);
                                    prop.product_qty = Convert.ToInt32(dataReader["product_qty"]);
                                    prop.total_amount = Convert.ToDecimal(dataReader["total_amount"]);
                                    prop.school_id = Convert.ToInt32(dataReader["school_id"]);
                                    prop.school_name = dataReader["school_name"].ToString();
                                    prop.class_id = Convert.ToInt32(dataReader["class_id"]);
                                    prop.class_name = dataReader["class_name"].ToString();
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "product_order";
                                listData.Message = "Product order history listing.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Product order history is empty.";
                                listData.Data = list;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "merchant/product-order-history");
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
        [Route("api/v2/merchant/product-order-history-staff")]
        public IHttpActionResult Post_Merchant_Order_History_Product_Staff([FromBody] StaffOrderHistoryParam value)
        {
            DateTime dt = DateTime.MinValue;
            MerchantDataModel data = new MerchantDataModel();
            if (value.order_id != 0 && value.school_id != 0 && value.pickup_date != dt)
            {
                List<ProductOrderHistory> list = new List<ProductOrderHistory>();
                ProductOrderHistoryModel listData = new ProductOrderHistoryModel();
                MySqlTransaction trans = null;
                data.Success = false;
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_order_detail_staff", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_order_id", value.order_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_pickup_date", value.pickup_date);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    ProductOrderHistory prop = new ProductOrderHistory();
                                    prop.pickup_date = Convert.ToDateTime(dataReader["pickup_date"]);
                                    prop.product_id = Convert.ToInt32(dataReader["product_id"]);
                                    prop.product_name = dataReader["product_name"].ToString();
                                    prop.product_photo_url = dataReader["product_photo_url"].ToString();
                                    prop.unit_price = Convert.ToDecimal(dataReader["unit_price"]);
                                    prop.product_qty = Convert.ToInt32(dataReader["product_qty"]);
                                    prop.total_amount = Convert.ToDecimal(dataReader["total_amount"]);
                                    prop.school_id = Convert.ToInt32(dataReader["school_id"]);
                                    prop.school_name = dataReader["school_name"].ToString();
                                    if (dataReader["class_id"] != DBNull.Value)
                                    {
                                        prop.class_id = Convert.ToInt16(dataReader["class_id"]);
                                        prop.class_name = dataReader["class_name"].ToString();
                                    }
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "product_order";
                                listData.Message = "Product order history listing.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Product order history is empty.";
                                listData.Data = list;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "merchant/product-order-history-staff");
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
        [Route("api/v2/merchant/product-order-history-detail")]
        public IHttpActionResult Post_Merchant_Order_History_Product_Detail([FromBody] DetailOrderHistoryParam value)
        {
            DateTime dt = DateTime.MinValue;
            MerchantDataModel data = new MerchantDataModel();
            if (value.merchant_id != 0 && value.school_id != 0 && value.class_id != 0 && value.pickup_date != dt && value.product_id != 0)
            {
                List<ProductDetailOrderHistory> list = new List<ProductDetailOrderHistory>();
                ProductDetailOrderHistoryModel listData = new ProductDetailOrderHistoryModel();
                MySqlTransaction trans = null;
                data.Success = false;
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_order_history_detail", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_merchant_id", value.merchant_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_class_id", value.class_id);
                            cmd.Parameters.AddWithValue("@p_pickup_date", value.pickup_date);
                            cmd.Parameters.AddWithValue("@p_profile_id", 0);
                            cmd.Parameters.AddWithValue("@p_product_id", value.product_id);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    ProductDetailOrderHistory prop = new ProductDetailOrderHistory();
                                    prop.pickup_date = Convert.ToDateTime(dataReader["pickup_date"]);
                                    prop.product_id = Convert.ToInt32(dataReader["product_id"]);
                                    prop.product_name = dataReader["product_name"].ToString();
                                    prop.product_photo_url = dataReader["product_photo_url"].ToString();
                                    prop.unit_price = Convert.ToDecimal(dataReader["unit_price"]);
                                    prop.product_qty = Convert.ToInt32(dataReader["product_qty"]);
                                    prop.total_amount = Convert.ToDecimal(dataReader["total_amount"]);
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.photo_url = dataReader["photo_url"].ToString();
                                    prop.school_id = Convert.ToInt32(dataReader["school_id"]);
                                    prop.school_name = dataReader["school_name"].ToString();
                                    prop.class_id = Convert.ToInt32(dataReader["class_id"]);
                                    prop.class_name = dataReader["class_name"].ToString();
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "product_order";
                                listData.Message = "Product order history detail listing.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Product order history detail is empty.";
                                listData.Data = list;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "merchant/product-order-history-detail");
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
        [Route("api/v2/merchant/sales")]
        public IHttpActionResult Post_Merchant_Sales([FromBody] MerchantSalesParam value)
        {
            MerchantDataModel data = new MerchantDataModel();
            if (value.merchant_id != 0)
            {
                List<MerchantSales> list = new List<MerchantSales>();
                MerchantSalesModel listData = new MerchantSalesModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_merchant_sales", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_merchant_id", value.merchant_id);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    MerchantSales prop = new MerchantSales();
                                    prop.merchant_id = dataReader["merchant_id"] as int? ?? default(int);
                                    prop.receipt_date = Convert.ToDateTime(dataReader["receipt_date"]);
                                    prop.total_amount = dataReader["total_amount"] as decimal? ?? default(decimal);
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "merchant_sales_list";
                                listData.Message = "Merchant sales listing.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Merchant sales could not be found.";
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
                        ExceptionUtility.LogException(ex, "merchant/sales");
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
        [Route("api/v2/merchant/sales-method")]
        public IHttpActionResult Post_Merchant_Sales_Method([FromBody] MerchantSalesMethodParam value)
        {
            MerchantDataModel data = new MerchantDataModel();
            if (value.merchant_id != 0 && value.receipt_date != null)
            {
                List<MerchantSalesMethod> list = new List<MerchantSalesMethod>();
                MerchantSalesMethodModel listData = new MerchantSalesMethodModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_merchant_sales_method", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_merchant_id", value.merchant_id);
                            cmd.Parameters.AddWithValue("@p_receipt_date", value.receipt_date);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    MerchantSalesMethod prop = new MerchantSalesMethod();
                                    prop.merchant_id = dataReader["merchant_id"] as int? ?? default(int);
                                    prop.school_id = dataReader["school_id"] as int? ?? default(int);
                                    prop.school_name = dataReader["school_name"].ToString();
                                    prop.receipt_date = Convert.ToDateTime(dataReader["receipt_date"]);
                                    prop.total_amount = dataReader["total_amount"] as decimal? ?? default(decimal);
                                    prop.sales_method = dataReader["sales_method"].ToString();
                                    prop.sales_method_bm = dataReader["sales_method_bm"].ToString();
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "merchant_sales_method_list";
                                listData.Message = "Merchant sales method listing.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Merchant sales method could not be found.";
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
                        ExceptionUtility.LogException(ex, "merchant/sales-method");
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
        [Route("api/v2/merchant/settlement-group")]
        public IHttpActionResult Post_Merchant_Settlement_Group([FromBody] OrderHistoryGroupParam value)
        {
            MerchantDataModel data = new MerchantDataModel();
            if (value.school_id != 0 && value.merchant_id != 0)
            {
                List<SettlementDate> list = new List<SettlementDate>();
                SettlementDateModel listData = new SettlementDateModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_settlement_report_group", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_merchant_id", value.merchant_id);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    SettlementDate prop = new SettlementDate();
                                    prop.receipt_date = Convert.ToDateTime(dataReader["receipt_date"]);
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "settlement_history";
                                listData.Message = "Settlement history group listing.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Settlement history group is empty.";
                                listData.Data = list;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "merchant/settlement-group");
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

        //[HttpPost]
        //[Authorize]
        //[Route("api/v2/merchant/settlement")]
        //public IHttpActionResult Post_Merchant_Settlement([FromBody] MerchantSettlementParam value)
        //{
        //    MerchantDataModel data = new MerchantDataModel();
        //    if (value.merchant_id != 0 && value.school_id != 0 && value.receipt_date != DateTime.MinValue)
        //    {
        //        List<MerchantSettlement> list = new List<MerchantSettlement>();
        //        MerchantSettlementModel listData = new MerchantSettlementModel();
        //        MySqlTransaction trans = null;
        //        listData.Success = false;
        //        string sqlQuery = string.Empty;
        //        string status_Code = string.Empty;
        //        string password = string.Empty;

        //        string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

        //        using (MySqlConnection conn = new MySqlConnection(constr))
        //        {
        //            try
        //            {
        //                conn.Open();
        //                trans = conn.BeginTransaction();

        //                using (MySqlCommand cmd = new MySqlCommand("sp_get_settlement_report", conn))
        //                {
        //                    cmd.Transaction = trans;
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    cmd.Parameters.Clear();
        //                    cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
        //                    cmd.Parameters.AddWithValue("@p_merchant_id", value.merchant_id);
        //                    cmd.Parameters.AddWithValue("@p_receipt_date", value.receipt_date);
        //                    MySqlDataReader dataReader = cmd.ExecuteReader();

        //                    if (dataReader.HasRows == true)
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            MerchantSettlement prop = new MerchantSettlement();
        //                            prop.merchant_id = dataReader["merchant_id"] as int? ?? default(int);
        //                            prop.school_id = Convert.ToInt32(dataReader["school_id"]);
        //                            prop.school_name = dataReader["school_name"].ToString();
        //                            prop.receipt_date = Convert.ToDateTime(dataReader["receipt_date"]);
        //                            prop.sales_method_id = Convert.ToInt32(dataReader["sales_method_id"]);
        //                            prop.sales_method = dataReader["sales_method"].ToString();
        //                            prop.sales_method_bm = dataReader["sales_method_bm"].ToString();
        //                            prop.total_amount = dataReader["total_amount"] as decimal? ?? default(decimal);
        //                            prop.settlement_amount = dataReader["settlement_amount"] as decimal? ?? default(decimal);
        //                            prop.fee_amount = dataReader["fee_amount"] as decimal? ?? default(decimal);
        //                            prop.net_amount = dataReader["net_amount"] as decimal? ?? default(decimal);
        //                            prop.status_id = dataReader["status_id"] as int? ?? default(int);
        //                            prop.status = dataReader["status"].ToString();
        //                            prop.status_bm = dataReader["status_bm"].ToString();
        //                            if (dataReader["payment_date"] != DBNull.Value)
        //                            {
        //                                prop.payment_date = Convert.ToDateTime(dataReader["payment_date"]);
        //                            }
        //                            prop.reference_number = dataReader["reference_number"].ToString();
        //                            list.Add(prop);
        //                        }
        //                        listData.Success = true;
        //                        listData.Code = "merchant_settlement_list";
        //                        listData.Message = "Merchant settlement listing.";
        //                        listData.Data = list;
        //                    }
        //                    else
        //                    {
        //                        //listData.Success = false;
        //                        listData.Success = true;
        //                        listData.Code = "no_record_found";
        //                        listData.Message = "Merchant settlement could not be found.";
        //                        listData.Data = list;
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                //listData.Success = false;
        //                data.Success = false;
        //                data.Code = "error_occured";
        //                data.Message = WebApiResources.ErrorOccured;
        //                ExceptionUtility.LogException(ex, "merchant/sales");
        //            }
        //            finally
        //            {
        //                conn.Close();
        //            }
        //        }

        //        if (listData.Success == true)
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
        [Route("api/v2/merchant/settlement")]
        public IHttpActionResult Post_Merchant_Settlement([FromBody] MerchantSettlementParam value)
        {
            MerchantDataModel data = new MerchantDataModel();
            if (value.merchant_id != 0 && value.school_id != 0 && value.receipt_date != DateTime.MinValue)
            {
                List<SettlementReport> list = new List<SettlementReport>();
                SettlementReportModel listData = new SettlementReportModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_settlement_report", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_merchant_id", value.merchant_id);
                            cmd.Parameters.AddWithValue("@p_receipt_date", value.receipt_date);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    SettlementReport prop = new SettlementReport();
                                    prop.merchant_id = dataReader["merchant_id"] as int? ?? default(int);
                                    prop.school_id = Convert.ToInt32(dataReader["school_id"]);
                                    prop.school_name = dataReader["school_name"].ToString();
                                    prop.receipt_date = Convert.ToDateTime(dataReader["receipt_date"]);
                                    prop.sales_method_id = Convert.ToInt32(dataReader["sales_method_id"]);
                                    prop.sales_method = dataReader["sales_method"].ToString();
                                    prop.sales_method_bm = dataReader["sales_method_bm"].ToString();
                                    prop.total_amount = dataReader["total_amount"] as decimal? ?? default(decimal);
                                    prop.fee_amount = dataReader["fee_amount"] as decimal? ?? default(decimal);
                                    prop.net_amount = dataReader["net_amount"] as decimal? ?? default(decimal);
                                    prop.settlement_amount = dataReader["settlement_amount"] as decimal? ?? default(decimal);
                                    prop.total_receipt = dataReader["total_receipt"] as int? ?? default(int);
                                    prop.status = dataReader["status"].ToString();
                                    prop.status_description = dataReader["status_description"].ToString();
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "merchant_settlement_list";
                                listData.Message = "Merchant settlement listing.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Merchant settlement could not be found.";
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
                        ExceptionUtility.LogException(ex, "merchant/sales");
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
        [Route("api/v2/merchant/update-order-status")]
        public IHttpActionResult Post_Merchant_Update_Order_Status(string culture, [FromBody] UpdateOrderStatusParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            FirebaseCloudMessagingPush fcm = new FirebaseCloudMessagingPush();
            MerchantDataModel data = new MerchantDataModel();
            if (value.merchant_id != 0 && value.school_id != 0 && value.order_id != null && value.order_status_id != 0 && value.update_by != null && value.order_date != DateTime.MinValue)
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

                        int[] orderArray;
                        if (value.order_id.Contains(","))
                        {
                            orderArray = value.order_id.Split(',').Select(int.Parse).ToArray();
                        }
                        else 
                        {
                            string[] strings = new string[] { value.order_id };
                            orderArray = strings.Select(int.Parse).ToArray();
                        }

                        if (orderArray.Length > 0) 
                        {
                            for (int i = 0; i < orderArray.Length; i++)
                            {
                                using (MySqlCommand cmd = new MySqlCommand("sp_update_order_status", conn))
                                {
                                    cmd.Transaction = trans;
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddWithValue("@p_merchant_id", value.merchant_id);
                                    cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                                    cmd.Parameters.AddWithValue("@p_order_id", orderArray[i]);
                                    cmd.Parameters.AddWithValue("@p_pickup_date", value.order_date);
                                    cmd.Parameters.AddWithValue("@p_order_status_id", value.order_status_id);
                                    cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                                    cmd.Parameters.Add("@p_reference_number", MySqlDbType.VarChar);
                                    cmd.Parameters["@p_reference_number"].Direction = ParameterDirection.Output;
                                    cmd.Parameters.Add("@p_profile_id", MySqlDbType.Int16);
                                    cmd.Parameters["@p_profile_id"].Direction = ParameterDirection.Output;
                                    cmd.Parameters.Add("@p_company_name", MySqlDbType.VarChar);
                                    cmd.Parameters["@p_company_name"].Direction = ParameterDirection.Output;
                                    cmd.Parameters.Add("@p_merchant_type_id", MySqlDbType.Int16);
                                    cmd.Parameters["@p_merchant_type_id"].Direction = ParameterDirection.Output;
                                    cmd.Parameters.Add("@p_recipient_name", MySqlDbType.VarChar);
                                    cmd.Parameters["@p_recipient_name"].Direction = ParameterDirection.Output;
                                    cmd.ExecuteNonQuery();

                                    var reference_number = (string)cmd.Parameters["@p_reference_number"].Value;
                                    var profile_id = Convert.ToInt16(cmd.Parameters["@p_profile_id"].Value);
                                    var company_name = (string)cmd.Parameters["@p_company_name"].Value;
                                    var merchant_type_id = Convert.ToInt16(cmd.Parameters["@p_merchant_type_id"].Value);
                                    var recipient_name = (string)cmd.Parameters["@p_recipient_name"].Value;

                                    string order_type = string.Empty;
                                    if (merchant_type_id == 1)
                                    {
                                        order_type = "food";
                                    }
                                    else {
                                        order_type = "item(s)";
                                    }

                                    if (value.order_status_id == 4)
                                    {
                                        fcm.PurchaseOrderStatusNotification(profile_id, "Order processing status", company_name + " is preparing your " + order_type + " in order #" + reference_number);
                                    } 
                                    else if (value.order_status_id == 5)
                                    {
                                        fcm.PurchaseOrderStatusNotification(profile_id, "Order has been delivered", "Recipient(s): " + recipient_name + " for order #" + reference_number);
                                    }
                                    

                                    data.Success = true;
                                    data.Code = "update_success";
                                    data.Message = WebApiResources.OrderStatusUpdateSuccess;
                                }
                            }
                            trans.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "merchant/update-order-status");
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
