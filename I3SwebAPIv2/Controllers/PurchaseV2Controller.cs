using I3SwebAPIv2.Class;
using I3SwebAPIv2.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Http;
using System.Transactions;
using System.Globalization;
using System.Threading;
using I3SwebAPIv2.Resources;

namespace I3SwebAPIv2.Controllers
{
    public class PurchaseV2Controller : ApiController
    {
        [HttpPost]
        [Authorize]
        [Route("api/v2/purchase/product-category")]
        public IHttpActionResult Post_Purchase_Product_Category([FromUri] SqlFilterParam uri, [FromBody] ProductCategoryParam body)
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
            if (body.school_id != 0 && body.merchant_id != 0 && body.special_flag != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_order_cart_product_category", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_limit", limit);
                            cmd.Parameters.AddWithValue("@p_offset", offset);
                            cmd.Parameters.AddWithValue("@p_school_id", body.school_id);
                            cmd.Parameters.AddWithValue("@p_merchant_id", body.merchant_id);
                            cmd.Parameters.AddWithValue("@p_special_flag", body.special_flag);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    ProductCategory prop = new ProductCategory();
                                    if (Convert.ToInt32(dataReader["total_product"]) > 0)
                                    {
                                        prop.category_id = dataReader["category_id"] as int? ?? default(int);
                                        prop.merchant_id = dataReader["merchant_id"] as int? ?? default(int);
                                        prop.school_id = dataReader["school_id"] as int? ?? default(int);
                                        prop.category_name = dataReader["category_name"].ToString();
                                        prop.category_description = dataReader["category_description"].ToString();
                                        prop.total_product = Convert.ToInt32(dataReader["total_product"]);
                                        list.Add(prop);
                                    }
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
                        ExceptionUtility.LogException(ex, "purchase/product-category");
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
        [Route("api/v2/purchase/product-detail")]
        public IHttpActionResult Post_Purchase_Product_Detail([FromUri] SqlFilterParam uri, [FromBody] ProductDetailParam body)
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
            if (body.school_id != 0 && body.merchant_id != 0 && body.category_id != 0 && body.special_flag != null && !string.IsNullOrEmpty(body.available_day))
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_order_cart_product_detail", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_limit", limit);
                            cmd.Parameters.AddWithValue("@p_offset", offset);
                            cmd.Parameters.AddWithValue("@p_school_id", body.school_id);
                            cmd.Parameters.AddWithValue("@p_merchant_id", body.merchant_id);
                            cmd.Parameters.AddWithValue("@p_category_id", body.category_id);
                            cmd.Parameters.AddWithValue("@p_special_flag", body.special_flag);
                            cmd.Parameters.AddWithValue("@p_available_day", body.available_day);
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
                        ExceptionUtility.LogException(ex, "purchase/product-detail");
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
        [Route("api/v2/purchase/insert-cart")]
        public IHttpActionResult 
        Post_Purchase_Insert_Order_Cart([FromBody] CreateOrderCartParam value)
        {
            DateTime minDt = new DateTime();
            TimeSpan minTs = new TimeSpan();
            PurchaseDataModel data = new PurchaseDataModel();
            if (value.profile_id != 0 && value.wallet_id != 0 && value.merchant_id != 0 && value.school_id != 0 && value.user_role_id != 0 && value.recipient_id != 0
                && value.recipient_role_id != 0 && value.pickup_date != minDt  && value.service_method_id != 0 && value.product_id != 0  && value.product_qty != 0 &&  value.create_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_insert_order_cart", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                            cmd.Parameters.AddWithValue("@p_wallet_id", value.wallet_id);
                            cmd.Parameters.AddWithValue("@p_merchant_id", value.merchant_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_user_role_id", value.user_role_id);
                            cmd.Parameters.AddWithValue("@p_recipient_id", value.recipient_id);
                            cmd.Parameters.AddWithValue("@p_recipient_role_id", value.recipient_role_id);
                            cmd.Parameters.AddWithValue("@p_pickup_date", value.pickup_date);
                            if (value.pickup_time != minTs)
                            {
                                cmd.Parameters.AddWithValue("@p_pickup_time", value.pickup_time);
                            }
                            else 
                            {
                                cmd.Parameters.AddWithValue("@p_pickup_time", minTs);
                            }
                            cmd.Parameters.AddWithValue("@p_service_method_id", value.service_method_id);
                            cmd.Parameters.AddWithValue("@p_delivery_location", value.delivery_location);
                            cmd.Parameters.AddWithValue("@p_product_id", value.product_id);
                            cmd.Parameters.AddWithValue("@p_product_qty", value.product_qty);
                            cmd.Parameters.AddWithValue("@p_create_by", value.create_by);
                            cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                            cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd.Parameters.Add("@p_product_name", MySqlDbType.VarChar);
                            cmd.Parameters["@p_product_name"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            var status_code = (string)cmd.Parameters["@p_status_code"].Value;
                            var product_name = (string)cmd.Parameters["@p_product_name"].Value;

                            if (status_code == "record_saved")
                            {
                                data.Success = true;
                                data.Code = "record_saved";
                                data.Message = "You added " + product_name + " to your cart.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "purchase/insert-cart");
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
        [Route("api/v2/purchase/update-cart")]
        public IHttpActionResult Post_Purchase_Update_Order_Cart([FromBody] UpdateOrderCartParam value)
        {
            DateTime minDt = new DateTime();
            PurchaseDataModel data = new PurchaseDataModel();
            if (value.cart_id != 0 && value.profile_id != 0 && value.wallet_id != 0 && value.merchant_id != 0 && value.school_id != 0 && value.recipient_id != 0
                && value.pickup_date != minDt && value.product_id != 0 && value.product_qty != 0 && value.update_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_update_order_cart", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_cart_id", value.cart_id);
                            cmd.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                            cmd.Parameters.AddWithValue("@p_wallet_id", value.wallet_id);
                            cmd.Parameters.AddWithValue("@p_merchant_id", value.merchant_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_recipient_id", value.recipient_id);
                            cmd.Parameters.AddWithValue("@p_pickup_date", value.pickup_date);
                            cmd.Parameters.AddWithValue("@p_product_id", value.product_id);
                            cmd.Parameters.AddWithValue("@p_product_qty", value.product_qty);
                            cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                            cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                            cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd.Parameters.Add("@p_product_name", MySqlDbType.VarChar);
                            cmd.Parameters["@p_product_name"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            var status_code = (string)cmd.Parameters["@p_status_code"].Value;
                            var product_name = (string)cmd.Parameters["@p_product_name"].Value;

                            if (status_code == "record_updated")
                            {
                                data.Success = true;
                                data.Code = "record_updated";
                                data.Message = "You updated " + product_name + " in your cart.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "purchase/update-cart");
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
        [Route("api/v2/purchase/delete-cart")]
        public IHttpActionResult Post_Purchase_Delete_Order_Cart([FromBody] DeleteOrderCartParam value)
        {
            PurchaseDataModel data = new PurchaseDataModel();
            if (value.cart_id != 0 && value.profile_id != 0 && value.recipient_id != 0 && value.product_id != 0 && value.update_by != null)
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


                        using (MySqlCommand cmd2 = new MySqlCommand("sp_delete_order_cart", conn))
                        {

                            cmd2.Transaction = trans;
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.Clear();
                            cmd2.Parameters.AddWithValue("@p_cart_id", value.cart_id);
                            cmd2.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                            cmd2.Parameters.AddWithValue("@p_recipient_id", value.recipient_id);
                            cmd2.Parameters.AddWithValue("@p_product_id", value.product_id);
                            cmd2.Parameters.AddWithValue("@p_update_by", value.update_by);
                            cmd2.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                            cmd2.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd2.Parameters.Add("@p_product_name", MySqlDbType.VarChar);
                            cmd2.Parameters["@p_product_name"].Direction = ParameterDirection.Output;
                            cmd2.ExecuteNonQuery();
                            trans.Commit();

                            var status_code = (string)cmd2.Parameters["@p_status_code"].Value;
                            var product_name = (string)cmd2.Parameters["@p_product_name"].Value;

                            if (status_code == "record_deleted") 
                            {
                                data.Success = true;
                                data.Code = "record_deleted";
                                data.Message = "You removed " + product_name + " from your cart.";
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "purchase/delete-cart");
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
        [Route("api/v2/purchase/cart-count")]
        public IHttpActionResult Post_Purchase_Order_Cart_Count([FromBody] CartCountParam value)
        {
            CartCountDataModel data = new CartCountDataModel();
            if (value.profile_id != 0 && value.wallet_id != 0)
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


                        using (MySqlCommand cmd2 = new MySqlCommand("sp_get_order_cart_count", conn))
                        {

                            cmd2.Transaction = trans;
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.Clear();
                            cmd2.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                            cmd2.Parameters.AddWithValue("@p_wallet_id", value.wallet_id);
                            cmd2.Parameters.Add("@p_total_count", MySqlDbType.Int16);
                            cmd2.Parameters["@p_total_count"].Direction = ParameterDirection.Output;
                            cmd2.ExecuteNonQuery();
                            trans.Commit();

                            var total_count = Convert.ToInt16(cmd2.Parameters["@p_total_count"].Value);

                            data.Success = true;
                            data.Code = "cart_item_count";
                            data.Message = "Cart items count";
                            data.Total = total_count;
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "purchase/cart-count");
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
        [Route("api/v2/purchase/cart-total")]
        public IHttpActionResult Post_Purchase_Order_Cart_Total([FromBody] CartCountParam value)
        {
            PurchaseDataModel data = new PurchaseDataModel();
            List<CartTotal> list = new List<CartTotal>();
            CartTotalDataModel listData = new CartTotalDataModel();

            if (value.profile_id != 0 && value.wallet_id != 0)
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


                        using (MySqlCommand cmd2 = new MySqlCommand("sp_get_order_cart_total", conn))
                        {

                            cmd2.Transaction = trans;
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.Clear();
                            cmd2.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                            cmd2.Parameters.AddWithValue("@p_wallet_id", value.wallet_id);
                            MySqlDataReader dataReader = cmd2.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    CartTotal prop = new CartTotal();
                                    prop.sub_total_amount = Convert.ToDecimal(dataReader["sub_total_amount"]);
                                    prop.tax_rate = Convert.ToInt32(dataReader["tax_rate"]);
                                    prop.tax_amount = Convert.ToDecimal(dataReader["tax_amount"]);
                                    prop.total_amount = Convert.ToDecimal(dataReader["total_amount"]);
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "shopping_cart_total";
                                listData.Message = "Shopping cart total.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Shopping cart is empty.";
                                listData.Data = list;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "purchase/cart-total");
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
        [Route("api/v2/purchase/order-history-total")]
        public IHttpActionResult Post_Purchase_Order_History_Total([FromBody] OrderHistoryDetailParam value)
        {
            PurchaseDataModel data = new PurchaseDataModel();
            List<CartTotalHistory> list = new List<CartTotalHistory>();
            CartTotalHistoryDataModel listData = new CartTotalHistoryDataModel();
            if (value.wallet_number != null && value.reference_number != null)
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


                        using (MySqlCommand cmd2 = new MySqlCommand("sp_get_order_master_total", conn))
                        {

                            cmd2.Transaction = trans;
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.Clear();
                            cmd2.Parameters.AddWithValue("@p_wallet_number", value.wallet_number);
                            cmd2.Parameters.AddWithValue("@p_reference_number", value.reference_number);
                            //cmd2.Parameters.Add("@p_total_amount", MySqlDbType.Decimal);
                            //cmd2.Parameters["@p_total_amount"].Direction = ParameterDirection.Output;
                            //cmd2.ExecuteNonQuery();
                            //trans.Commit();
                            MySqlDataReader dataReader = cmd2.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    CartTotalHistory prop = new CartTotalHistory();
                                    int tax_rate_int = 0;
                                    prop.sub_total_amount = Convert.ToDecimal(dataReader["sub_total_amount"]);
                                    prop.tax_amount = Convert.ToDecimal(dataReader["tax_amount"]);
                                    prop.total_amount = Convert.ToDecimal(dataReader["total_amount"]);
                                    if (prop.tax_amount > 0)
                                    {
                                        tax_rate_int = (int)((prop.total_amount - prop.sub_total_amount) / prop.sub_total_amount * 100);
                                        if (prop.tax_rate == null)
                                        {
                                            prop.tax_rate = tax_rate_int.ToString();
                                        }
                                        else {
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
                                listData.Code = "shopping_cart_total";
                                listData.Message = "Shopping cart total.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Shopping cart is empty.";
                                listData.Data = list;
                            }

                            //var total_amount = Convert.ToDecimal(cmd2.Parameters["@p_total_amount"].Value);

                            //data.Success = true;
                            //data.Code = "order_total_amount";
                            //data.Message = "Order total amount";
                            //data.Total = total_amount;
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "purchase/order-history-total");
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
        //[Route("api/v2/purchase/place-order")]
        //public IHttpActionResult Post_Purchase_Place_Order(string culture, [FromBody] CheckOutParam value)
        //{
        //    if (!string.IsNullOrEmpty(culture))
        //    {
        //        var language = new CultureInfo(culture);
        //        Thread.CurrentThread.CurrentUICulture = language;
        //    }

        //    FirebaseCloudMessagingPush fcm = new FirebaseCloudMessagingPush();
        //    ReferenceV2Class cls = new ReferenceV2Class();
        //    PurchaseDataModel data = new PurchaseDataModel();
        //    if (value.profile_id != 0 && value.wallet_id != 0 && value.order_status_id != 0 && value.user_role_id != 0 && 
        //        value.total_amount != 0 && value.payment_method_id != 0  && value.create_by != null)
        //    {
        //        List<Purchase> list = new List<Purchase>();
        //        bool proceed = true;
        //        MySqlTransaction trans = null;
        //        data.Success = false;
        //        int profileID;
        //        int recipientID;
        //        int merchantID = 0;
        //        string reference_number = string.Empty;
        //        string sqlQuery = string.Empty;
        //        string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

        //        using (MySqlConnection conn = new MySqlConnection(constr))
        //        {
        //            try
        //            {
        //                conn.Open();

        //                using (MySqlCommand cmd2 = new MySqlCommand("sp_get_order_cart", conn))
        //                {
        //                    trans = conn.BeginTransaction();
        //                    cmd2.Transaction = trans;
        //                    cmd2.CommandType = CommandType.StoredProcedure;
        //                    cmd2.Parameters.Clear();
        //                    cmd2.Parameters.AddWithValue("@p_profile_id", value.profile_id);
        //                    cmd2.Parameters.AddWithValue("@p_wallet_id", value.wallet_id);
        //                    cmd2.Parameters.AddWithValue("@p_order_status_id", value.order_status_id);
        //                    MySqlDataReader dataReader = cmd2.ExecuteReader();

        //                    if (dataReader.HasRows == true)
        //                    {

        //                        while (dataReader.Read())
        //                        {
        //                            DateTime dtPickup = new DateTime();
        //                            TimeSpan tsPickup = new TimeSpan();
        //                            Purchase prop = new Purchase();
        //                            prop.cart_id = Convert.ToInt32(dataReader["cart_id"]);
        //                            prop.profile_id = Convert.ToInt32(dataReader["profile_id"]);
        //                            profileID = Convert.ToInt32(dataReader["profile_id"]);
        //                            prop.wallet_id = Convert.ToInt32(dataReader["wallet_id"]);
        //                            prop.wallet_number = dataReader["wallet_number"].ToString();
        //                            prop.full_name = dataReader["full_name"].ToString();
        //                            prop.merchant_id = Convert.ToInt32(dataReader["merchant_id"]);
        //                            merchantID = Convert.ToInt32(dataReader["merchant_id"]);
        //                            prop.company_name = dataReader["company_name"].ToString();
        //                            prop.school_id = Convert.ToInt16(dataReader["school_id"]);
        //                            prop.school_name = dataReader["school_name"].ToString();
        //                            prop.recipient_id = Convert.ToInt16(dataReader["recipient_id"]);
        //                            recipientID = Convert.ToInt16(dataReader["recipient_id"]);
        //                            prop.recipient_name = dataReader["recipient_name"].ToString();
        //                            prop.recipient_role_id = Convert.ToInt16(dataReader["recipient_role_id"]);
        //                            prop.recipient_role = dataReader["recipient_role"].ToString();
        //                            prop.pickup_date = Convert.ToDateTime(dataReader["pickup_date"]);
        //                            dtPickup = Convert.ToDateTime(dataReader["pickup_date"]);
        //                            if (dataReader["pickup_time"] != DBNull.Value)
        //                            {
        //                                prop.pickup_time = dataReader.GetTimeSpan(dataReader.GetOrdinal("pickup_time"));
        //                                tsPickup = dataReader.GetTimeSpan(dataReader.GetOrdinal("pickup_time"));
        //                            }
        //                            if (dataReader["service_method_id"] != DBNull.Value)
        //                            {
        //                                prop.service_method_id = Convert.ToInt32(dataReader["service_method_id"]);
        //                            }
        //                            prop.delivery_location = dataReader["delivery_location"].ToString();
        //                            prop.product_id = Convert.ToInt32(dataReader["product_id"]);
        //                            prop.product_name = dataReader["product_name"].ToString();
        //                            prop.product_photo_url = dataReader["product_photo_url"].ToString();
        //                            prop.product_qty = Convert.ToInt16(dataReader["product_qty"]);
        //                            prop.unit_price = Convert.ToDecimal(dataReader["unit_price"]);
        //                            prop.sub_total_amount = Convert.ToDecimal(dataReader["sub_total_amount"]);
        //                            prop.order_status_id = Convert.ToInt16(dataReader["order_status_id"]);
        //                            prop.order_status = dataReader["order_status"].ToString();
        //                            prop.order_status_bm = dataReader["order_status_bm"].ToString();
        //                            list.Add(prop);

        //                            if (profileID != recipientID)
        //                            {
        //                                TimeSpan ts = new TimeSpan(7, 0, 0);
        //                                if (DateTime.Now >= dtPickup.Add(ts))
        //                                {
        //                                    proceed = false;
        //                                }
        //                            }
        //                            else 
        //                            {
        //                                if (DateTime.Now >= dtPickup.Add(tsPickup))
        //                                {
        //                                    proceed = false;
        //                                }
        //                            }
        //                        }
        //                    }

        //                    if (proceed == true)
        //                    {
        //                        dataReader.Close();
        //                        trans.Dispose();

        //                        using (MySqlCommand cmd3 = new MySqlCommand("sp_get_wallet_detail", conn))
        //                        {
        //                            trans = conn.BeginTransaction();
        //                            cmd3.Transaction = trans;
        //                            cmd3.CommandType = CommandType.StoredProcedure;
        //                            cmd3.Parameters.Clear();
        //                            cmd3.Parameters.AddWithValue("@p_profile_id", value.profile_id);
        //                            cmd3.Parameters.AddWithValue("@p_wallet_id", value.wallet_id);
        //                            cmd3.Parameters.Add("@p_account_balance", MySqlDbType.Decimal);
        //                            cmd3.Parameters["@p_account_balance"].Direction = ParameterDirection.Output;
        //                            cmd3.Parameters.Add("@p_school_id", MySqlDbType.Int16);
        //                            cmd3.Parameters["@p_school_id"].Direction = ParameterDirection.Output;
        //                            cmd3.ExecuteNonQuery();
        //                            trans.Commit();

        //                            var account_balance = Convert.ToDecimal(cmd3.Parameters["@p_account_balance"].Value);
        //                            var school_id = Convert.ToInt16(cmd3.Parameters["@p_school_id"].Value);

        //                            bool proceedPay = true;

        //                            if (value.total_amount > account_balance)
        //                            {
        //                                if (value.payment_method_id == 1) 
        //                                {
        //                                    proceedPay = false;
        //                                }
        //                            }

        //                            if (proceedPay == true)
        //                            {
        //                                reference_number = "PAY-I3S-" + cls.GetNumericUniqueKey() + "-" + cls.GetNumericUniqueKey();
        //                                trans.Dispose();

        //                                using (MySqlCommand cmd4 = new MySqlCommand("sp_insert_order_master", conn))
        //                                {
        //                                    trans = conn.BeginTransaction();
        //                                    cmd4.Transaction = trans;
        //                                    cmd4.CommandType = CommandType.StoredProcedure;
        //                                    cmd4.Parameters.Clear();
        //                                    cmd4.Parameters.AddWithValue("@p_reference_number", reference_number);
        //                                    cmd4.Parameters.AddWithValue("@p_profile_id", value.profile_id);
        //                                    cmd4.Parameters.AddWithValue("@p_wallet_id", value.wallet_id);
        //                                    cmd4.Parameters.AddWithValue("@p_user_role_id", value.user_role_id);
        //                                    cmd4.Parameters.AddWithValue("@p_sub_total_amount", value.total_amount);
        //                                    cmd4.Parameters.AddWithValue("@p_total_amount", value.total_amount);
        //                                    cmd4.Parameters.AddWithValue("@p_payment_method_id", value.payment_method_id);
        //                                    cmd4.Parameters.AddWithValue("@p_received_amount", value.total_amount);
        //                                    cmd4.Parameters.AddWithValue("@p_create_by", value.create_by);
        //                                    cmd4.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
        //                                    cmd4.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
        //                                    cmd4.Parameters.Add("@p_order_id", MySqlDbType.Int16);
        //                                    cmd4.Parameters["@p_order_id"].Direction = ParameterDirection.Output;
        //                                    cmd4.ExecuteNonQuery();
        //                                    trans.Commit();

        //                                    var order_status = (string)cmd4.Parameters["@p_status_code"].Value;
        //                                    var _order_id = Convert.ToInt16(cmd4.Parameters["@p_order_id"].Value);


        //                                    if (order_status == "record_saved")
        //                                    {
        //                                        int detail_list = 0;
        //                                        int saved_count = 0;
        //                                        string detail_status = string.Empty;

        //                                        detail_list = list.Count;
        //                                        foreach (Purchase item in list)
        //                                        {
        //                                            trans.Dispose();

        //                                            using (MySqlCommand cmd5 = new MySqlCommand("sp_insert_order_detail", conn))
        //                                            {
        //                                                trans = conn.BeginTransaction();
        //                                                cmd5.Transaction = trans;
        //                                                cmd5.CommandType = CommandType.StoredProcedure;
        //                                                cmd5.Parameters.Clear();
        //                                                cmd5.Parameters.AddWithValue("@p_cart_id", item.cart_id);
        //                                                cmd5.Parameters.AddWithValue("@p_profile_id", value.profile_id);
        //                                                cmd5.Parameters.AddWithValue("@p_wallet_id", value.wallet_id);
        //                                                cmd5.Parameters.AddWithValue("@p_order_id", _order_id);
        //                                                cmd5.Parameters.AddWithValue("@p_recipient_id", item.recipient_id);
        //                                                cmd5.Parameters.AddWithValue("@p_recipient_role_id", item.recipient_role_id);
        //                                                cmd5.Parameters.AddWithValue("@p_school_id", item.school_id);
        //                                                cmd5.Parameters.AddWithValue("@p_merchant_id", item.merchant_id);
        //                                                cmd5.Parameters.AddWithValue("@p_pickup_date", item.pickup_date);
        //                                                cmd5.Parameters.AddWithValue("@p_pickup_time", item.pickup_time);
        //                                                cmd5.Parameters.AddWithValue("@p_service_method_id", item.service_method_id);
        //                                                cmd5.Parameters.AddWithValue("@p_delivery_location", item.delivery_location);
        //                                                cmd5.Parameters.AddWithValue("@p_product_id", item.product_id);
        //                                                cmd5.Parameters.AddWithValue("@p_product_qty", item.product_qty);
        //                                                cmd5.Parameters.AddWithValue("@p_unit_price", item.unit_price);
        //                                                cmd5.Parameters.AddWithValue("@p_sub_total_amount", item.sub_total_amount);
        //                                                cmd5.Parameters.AddWithValue("@p_total_amount", item.sub_total_amount);
        //                                                cmd5.Parameters.AddWithValue("@p_create_by", value.create_by);
        //                                                cmd5.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
        //                                                cmd5.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
        //                                                cmd5.ExecuteNonQuery();
        //                                                trans.Commit();

        //                                                detail_status = (string)cmd5.Parameters["@p_status_code"].Value;

        //                                            }
        //                                            saved_count++;
        //                                        }

        //                                        if (detail_list == saved_count)
        //                                        {
        //                                            trans.Dispose();

        //                                            using (MySqlCommand cmd6 = new MySqlCommand("sp_insert_wallet_transaction_history", conn))
        //                                            {
        //                                                trans = conn.BeginTransaction();
        //                                                cmd6.Transaction = trans;
        //                                                cmd6.CommandType = CommandType.StoredProcedure;
        //                                                cmd6.Parameters.Clear();
        //                                                cmd6.Parameters.AddWithValue("@p_reference_number", reference_number);
        //                                                cmd6.Parameters.AddWithValue("@p_school_id", school_id);
        //                                                cmd6.Parameters.AddWithValue("@p_transaction_type_id", 5);
        //                                                cmd6.Parameters.AddWithValue("@p_wallet_id", value.wallet_id);
        //                                                cmd6.Parameters.AddWithValue("@p_total_amount", value.total_amount);
        //                                                cmd6.Parameters.AddWithValue("@p_payment_method_id", value.payment_method_id);
        //                                                cmd6.Parameters.AddWithValue("@p_status_id", 200);
        //                                                cmd6.Parameters.AddWithValue("@p_create_by", value.create_by);
        //                                                cmd6.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
        //                                                cmd6.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
        //                                                cmd6.ExecuteNonQuery();
        //                                                trans.Commit();

        //                                                var wallet_status = (string)cmd6.Parameters["@p_status_code"].Value;
        //                                                int profile_id;

        //                                                if (wallet_status == "record_saved")
        //                                                {
        //                                                    trans.Dispose();

        //                                                    using (MySqlCommand cmd7 = new MySqlCommand("sp_get_merchant_profile_id", conn))
        //                                                    {
        //                                                        trans = conn.BeginTransaction();
        //                                                        cmd7.Transaction = trans;
        //                                                        cmd7.CommandType = CommandType.StoredProcedure;
        //                                                        cmd7.Parameters.Clear();
        //                                                        cmd7.Parameters.AddWithValue("@p_merchant_id", merchantID);
        //                                                        cmd7.Parameters.Add("@p_profile_id", MySqlDbType.Int16);
        //                                                        cmd7.Parameters["@p_profile_id"].Direction = ParameterDirection.Output;
        //                                                        cmd7.ExecuteNonQuery();
        //                                                        trans.Commit();

        //                                                        profile_id = Convert.ToInt16(cmd7.Parameters["@p_profile_id"].Value);
        //                                                    }

        //                                                    fcm.PurchaseOrderStatusNotification(profile_id, "New order received", value.create_by + " has placed a new order #" + reference_number + " total of RM" + value.total_amount.ToString("F"));

        //                                                    data.Success = true;
        //                                                    data.Code = "success";
        //                                                    data.Message = WebApiResources.YourOrderHasBeenReceived;
        //                                                }
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                            else 
        //                            {
        //                                data.Success = false;
        //                                data.Code = "unsufficient_balance";
        //                                data.Message = WebApiResources.UnsufficientWalletBalance;
        //                            }
        //                        }
        //                    }
        //                    else {
        //                        data.Success = false;
        //                        data.Code = "time_exceed";
        //                        data.Message = WebApiResources.OrderMustBePlacedBfor7;
        //                    }

        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                data.Success = false;
        //                data.Code = "error_occured";
        //                data.Message = WebApiResources.ErrorOccured;
        //                ExceptionUtility.LogException(ex, "purchase/place-order");
        //            }
        //            finally
        //            {
        //                conn.Close();
        //            }
        //        }

        //        return Ok(data);
        //    }
        //    else
        //    {
        //        return BadRequest("Missing parameters.");
        //    }
        //}

        [HttpPost]
        [Authorize]
        [Route("api/v2/purchase/cart")]
        public IHttpActionResult Post_Purchase_Order_Cart([FromBody] OrderCartParam value)
        {
            PurchaseDataModel data = new PurchaseDataModel();
            if (value.profile_id != 0 && value.wallet_id != 0 && value.order_status_id != 0)
            {
                List<Purchase> list = new List<Purchase>();
                PurchaseModel listData = new PurchaseModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_order_cart", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                            cmd.Parameters.AddWithValue("@p_wallet_id", value.wallet_id);
                            cmd.Parameters.AddWithValue("@p_order_status_id", value.order_status_id);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    Purchase prop = new Purchase();
                                    prop.cart_id = Convert.ToInt32(dataReader["cart_id"]);
                                    prop.profile_id = Convert.ToInt32(dataReader["profile_id"]);
                                    prop.wallet_id = Convert.ToInt32(dataReader["wallet_id"]);
                                    prop.wallet_number = dataReader["wallet_number"].ToString();
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.merchant_id = Convert.ToInt32(dataReader["merchant_id"]);
                                    prop.company_name = dataReader["company_name"].ToString();
                                    prop.school_id = Convert.ToInt16(dataReader["school_id"]);
                                    prop.school_name = dataReader["school_name"].ToString();
                                    prop.recipient_id = Convert.ToInt16(dataReader["recipient_id"]);
                                    prop.recipient_name = dataReader["recipient_name"].ToString();
                                    prop.recipient_role_id = Convert.ToInt16(dataReader["recipient_role_id"]);
                                    prop.recipient_role = dataReader["recipient_role"].ToString();
                                    prop.pickup_date = Convert.ToDateTime(dataReader["pickup_date"]);
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
                                }
                                listData.Success = true;
                                listData.Code = "shopping_cart";
                                listData.Message = "Shopping cart listing.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Shopping cart is empty.";
                                listData.Data = list;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "purchase/cart");
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
        [Route("api/v2/purchase/cart-pickup-date")]
        public IHttpActionResult Post_Purchase_Order_Cart_Date([FromBody] OrderCartPickupParam value)
        {
            DateTime dt = DateTime.MinValue;
            PurchaseDataModel data = new PurchaseDataModel();
            if (value.profile_id != 0 && value.wallet_id != 0 && value.order_status_id != 0 && value.pickup_date != dt)
            {
                List<Purchase> list = new List<Purchase>();
                PurchaseModel listData = new PurchaseModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_order_cart_pickup_date", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                            cmd.Parameters.AddWithValue("@p_wallet_id", value.wallet_id);
                            cmd.Parameters.AddWithValue("@p_order_status_id", value.order_status_id);
                            cmd.Parameters.AddWithValue("@p_pickup_date", value.pickup_date);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    Purchase prop = new Purchase();
                                    prop.cart_id = Convert.ToInt32(dataReader["cart_id"]);
                                    prop.profile_id = Convert.ToInt32(dataReader["profile_id"]);
                                    prop.wallet_id = Convert.ToInt32(dataReader["wallet_id"]);
                                    prop.wallet_number = dataReader["wallet_number"].ToString();
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.merchant_id = Convert.ToInt32(dataReader["merchant_id"]);
                                    prop.company_name = dataReader["company_name"].ToString();
                                    prop.school_id = Convert.ToInt16(dataReader["school_id"]);
                                    prop.school_name = dataReader["school_name"].ToString();
                                    if (dataReader["class_id"] != DBNull.Value)
                                    {
                                        prop.class_id = Convert.ToInt16(dataReader["class_id"]);
                                        prop.class_name = dataReader["class_name"].ToString();
                                    }
                                    prop.recipient_id = Convert.ToInt16(dataReader["recipient_id"]);
                                    prop.recipient_name = dataReader["recipient_name"].ToString();
                                    prop.recipient_role_id = Convert.ToInt16(dataReader["recipient_role_id"]);
                                    prop.recipient_role = dataReader["recipient_role"].ToString();
                                    prop.pickup_date = Convert.ToDateTime(dataReader["pickup_date"]);
                                    if (dataReader["pickup_time"] != DBNull.Value) 
                                    {
                                        prop.pickup_time = dataReader.GetTimeSpan(dataReader.GetOrdinal("pickup_time"));
                                    }
                                    if (dataReader["service_method_id"] != DBNull.Value)
                                    {
                                        prop.service_method_id = Convert.ToInt32(dataReader["service_method_id"]);
                                    }
                                    prop.delivery_location = dataReader["delivery_location"].ToString();
                                    prop.product_id = Convert.ToInt32(dataReader["product_id"]);
                                    prop.product_name = dataReader["product_name"].ToString();
                                    prop.product_description = dataReader["product_description"].ToString();
                                    prop.product_photo_url = dataReader["product_photo_url"].ToString();
                                    prop.product_qty = Convert.ToInt16(dataReader["product_qty"]);
                                    prop.unit_price = Convert.ToDecimal(dataReader["unit_price"]);
                                    prop.sub_total_amount = Convert.ToDecimal(dataReader["sub_total_amount"]);
                                    prop.order_status_id = Convert.ToInt16(dataReader["order_status_id"]);
                                    prop.order_status = dataReader["order_status"].ToString();
                                    prop.order_status_bm = dataReader["order_status_bm"].ToString();
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "shopping_cart";
                                listData.Message = "Shopping cart listing.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Shopping cart is empty.";
                                listData.Data = list;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "purchase/cart");
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
        [Route("api/v2/purchase/cart-pickup-date-group")]
        public IHttpActionResult Post_Purchase_Order_Cart_Date_Group([FromBody] OrderCartParam value)
        {
            DateTime dt = DateTime.MinValue;
            PurchaseDataModel data = new PurchaseDataModel();
            if (value.profile_id != 0 && value.wallet_id != 0 && value.order_status_id != 0)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_order_cart_pickup_date_group", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                            cmd.Parameters.AddWithValue("@p_wallet_id", value.wallet_id);
                            cmd.Parameters.AddWithValue("@p_order_status_id", value.order_status_id);
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
                                listData.Code = "shopping_cart";
                                listData.Message = "Shopping cart listing.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Shopping cart is empty.";
                                listData.Data = list;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "purchase/cart");
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
        [Route("api/v2/purchase/detail-order-history-date-group")]
        public IHttpActionResult Post_Purchase_Order_History_Detail_Date_Group([FromBody] OrderHistoryDetailGroupParam value)
        {
            DateTime dt = DateTime.MinValue;
            PurchaseDataModel data = new PurchaseDataModel();
            if (value.wallet_number != null && value.reference_number != null )
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_order_detail_group", conn))
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
                                    PurchaseDate prop = new PurchaseDate();
                                    prop.pickup_date = Convert.ToDateTime(dataReader["pickup_date"]);
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "detail_order_history";
                                listData.Message = "Detail order history listing.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Detail order history is empty.";
                                listData.Data = list;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "purchase/detail-order-history-date-group");
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
        [Route("api/v2/purchase/detail-order-history-date")]
        public IHttpActionResult Post_Purchase_Order_History_Detail_Date([FromBody] OrderHistoryDetailDateParam value)
        {
            DateTime dt = DateTime.MinValue;
            PurchaseDataModel data = new PurchaseDataModel();
            if (value.wallet_number != null && value.reference_number != null && value.pickup_date != dt)
            {
                List<OrderHistoryDetail> list = new List<OrderHistoryDetail>();
                OrderHistoryDetailModel listData = new OrderHistoryDetailModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_order_detail_date", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_wallet_number", value.wallet_number);
                            cmd.Parameters.AddWithValue("@p_reference_number", value.reference_number);
                            cmd.Parameters.AddWithValue("@p_pickup_date", value.pickup_date);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    OrderHistoryDetail prop = new OrderHistoryDetail();
                                    prop.order_detail_id = Convert.ToInt32(dataReader["order_detail_id"]);
                                    prop.order_id = Convert.ToInt32(dataReader["order_id"]);
                                    prop.wallet_id = Convert.ToInt32(dataReader["wallet_id"]);
                                    prop.wallet_number = dataReader["wallet_number"].ToString();
                                    prop.reference_number = dataReader["reference_number"].ToString();
                                    prop.recipient_id = Convert.ToInt32(dataReader["recipient_id"]);
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.photo_url = dataReader["photo_url"].ToString();
                                    prop.school_id = Convert.ToInt32(dataReader["school_id"]);
                                    prop.school_name = dataReader["school_name"].ToString();
                                    if (dataReader["class_id"] != DBNull.Value)
                                    {
                                        prop.class_id = Convert.ToInt16(dataReader["class_id"]);
                                        prop.class_name = dataReader["class_name"].ToString();
                                    }
                                    prop.pickup_date = Convert.ToDateTime(dataReader["pickup_date"]);
                                    if (dataReader["pickup_time"] != DBNull.Value)
                                    {
                                        prop.pickup_time = dataReader.GetTimeSpan(dataReader.GetOrdinal("pickup_time"));
                                    }
                                    if (dataReader["service_method_id"] != DBNull.Value)
                                    {
                                        prop.service_method_id = Convert.ToInt32(dataReader["service_method_id"]);
                                    }
                                    prop.delivery_location = dataReader["delivery_location"].ToString();
                                    prop.merchant_id = Convert.ToInt32(dataReader["merchant_id"]);
                                    prop.company_name = dataReader["company_name"].ToString();
                                    prop.product_id = Convert.ToInt32(dataReader["product_id"]);
                                    prop.product_name = dataReader["product_name"].ToString();
                                    prop.product_photo_url = dataReader["product_photo_url"].ToString();
                                    prop.product_qty = Convert.ToInt32(dataReader["product_qty"]);
                                    prop.unit_price = Convert.ToDecimal(dataReader["unit_price"]);
                                    prop.sub_total_amount = Convert.ToDecimal(dataReader["sub_total_amount"]);
                                    prop.discount_amount = Convert.ToDecimal(dataReader["discount_amount"]);
                                    prop.total_amount = Convert.ToDecimal(dataReader["total_amount"]);
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "detail_order_history";
                                listData.Message = "Detail order history listing.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Detail order history is empty.";
                                listData.Data = list;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "purchase/detail-order-history-date");
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
        [Route("api/v2/purchase/order-history")]
        public IHttpActionResult Post_Purchase_Order_History([FromBody] OrderHistoryMasterParam value)
        {
            PurchaseDataModel data = new PurchaseDataModel();
            if (value.profile_id != 0 && value.wallet_id != 0)
            {
                List<OrderHistoryMaster> list = new List<OrderHistoryMaster>();
                OrderHistoryMasterModel listData = new OrderHistoryMasterModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_order_master", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                            cmd.Parameters.AddWithValue("@p_wallet_id", value.wallet_id);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    OrderHistoryMaster prop = new OrderHistoryMaster();
                                    prop.order_id = Convert.ToInt32(dataReader["order_id"]);
                                    prop.profile_id = Convert.ToInt32(dataReader["profile_id"]);
                                    prop.wallet_id = Convert.ToInt32(dataReader["wallet_id"]);
                                    prop.wallet_number = dataReader["wallet_number"].ToString();
                                    prop.reference_number = dataReader["reference_number"].ToString();
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.photo_url = dataReader["photo_url"].ToString();
                                    prop.user_role_id = Convert.ToInt16(dataReader["user_role_id"]);
                                    prop.total_amount = Convert.ToDecimal(dataReader["total_amount"]);
                                    prop.sub_total_amount = Convert.ToDecimal(dataReader["sub_total_amount"]);
                                    prop.order_status_id = Convert.ToInt16(dataReader["order_status_id"]);
                                    prop.order_status = dataReader["order_status"].ToString();
                                    prop.order_status_bm = dataReader["order_status_bm"].ToString();
                                    prop.payment_method = dataReader["payment_method"].ToString();
                                    prop.payment_method_bm = dataReader["payment_method_bm"].ToString();
                                    prop.create_at = Convert.ToDateTime(dataReader["create_at"]);
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
                        ExceptionUtility.LogException(ex, "purchase/order-history");
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
        [Route("api/v2/purchase/detail-order-history")]
        public IHttpActionResult Post_Purchase_Order_History_Detail([FromBody] OrderHistoryDetailParam value)
        {
            PurchaseDataModel data = new PurchaseDataModel();
            if (value.wallet_number != null && value.reference_number != null)
            {
                List<OrderHistoryDetail> list = new List<OrderHistoryDetail>();
                OrderHistoryDetailModel listData = new OrderHistoryDetailModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_order_detail", conn))
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
                                    OrderHistoryDetail prop = new OrderHistoryDetail();
                                    prop.order_detail_id = Convert.ToInt32(dataReader["order_detail_id"]);
                                    prop.order_id = Convert.ToInt32(dataReader["order_id"]);
                                    prop.wallet_id = Convert.ToInt32(dataReader["wallet_id"]);
                                    prop.wallet_number = dataReader["wallet_number"].ToString();
                                    prop.reference_number = dataReader["reference_number"].ToString();
                                    prop.recipient_id = Convert.ToInt32(dataReader["recipient_id"]);
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.photo_url = dataReader["photo_url"].ToString();
                                    prop.school_id = Convert.ToInt32(dataReader["school_id"]);
                                    prop.school_name = dataReader["school_name"].ToString();
                                    if (dataReader["class_id"] != DBNull.Value)
                                    {
                                        prop.class_id = Convert.ToInt16(dataReader["class_id"]);
                                        prop.class_name = dataReader["class_name"].ToString();
                                    }
                                    prop.pickup_date = Convert.ToDateTime(dataReader["pickup_date"]);
                                    if (dataReader["pickup_time"] != DBNull.Value)
                                    {
                                        prop.pickup_time = dataReader.GetTimeSpan(dataReader.GetOrdinal("pickup_time"));
                                    }
                                    if (dataReader["service_method_id"] != DBNull.Value)
                                    {
                                        prop.service_method_id = Convert.ToInt32(dataReader["service_method_id"]);
                                    }
                                    prop.delivery_location = dataReader["delivery_location"].ToString();
                                    prop.merchant_id = Convert.ToInt32(dataReader["merchant_id"]);
                                    prop.company_name = dataReader["company_name"].ToString();
                                    prop.product_id = Convert.ToInt32(dataReader["product_id"]);
                                    prop.product_name = dataReader["product_name"].ToString();
                                    prop.product_photo_url = dataReader["product_photo_url"].ToString();
                                    prop.product_qty = Convert.ToInt32(dataReader["product_qty"]);
                                    prop.unit_price = Convert.ToDecimal(dataReader["unit_price"]);
                                    prop.sub_total_amount = Convert.ToDecimal(dataReader["sub_total_amount"]);
                                    prop.discount_amount = Convert.ToDecimal(dataReader["discount_amount"]);
                                    prop.total_amount = Convert.ToDecimal(dataReader["total_amount"]);
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "detail_order_history";
                                listData.Message = "Detail order history listing.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Detail order history is empty.";
                                listData.Data = list;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "purchase/detail-order-history");
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
        [Route("api/v2/purchase/remove-order")]
        public IHttpActionResult Post_Purchase_Remove_Order_Cart([FromBody] RemoveOrderCartParam value)
        {
            PurchaseDataModel data = new PurchaseDataModel();
            if (value.order_id != 0 && value.profile_id != 0 && value.wallet_id != 0 && value.update_by != null)
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


                        using (MySqlCommand cmd2 = new MySqlCommand("sp_delete_order_master", conn))
                        {

                            cmd2.Transaction = trans;
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.Clear();
                            cmd2.Parameters.AddWithValue("@p_order_id", value.order_id);
                            cmd2.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                            cmd2.Parameters.AddWithValue("@p_wallet_id", value.wallet_id);
                            cmd2.Parameters.AddWithValue("@p_update_by", value.update_by);
                            cmd2.ExecuteNonQuery();
                            trans.Commit();

                            data.Success = true;
                            data.Code = "record_deleted";
                            data.Message = WebApiResources.OrderHistoryRemoved;

                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "purchase/remove-order");
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
        [Route("api/v2/purchase/continue-order")]
        public IHttpActionResult Post_Purchase_Continue_Order_Cart([FromBody] RemoveOrderCartParam value)
        {
            PurchaseDataModel data = new PurchaseDataModel();
            if (value.order_id != 0 && value.profile_id != 0 && value.wallet_id != 0 && value.update_by != null)
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


                        using (MySqlCommand cmd2 = new MySqlCommand("sp_update_order_master", conn))
                        {

                            cmd2.Transaction = trans;
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.Clear();
                            cmd2.Parameters.AddWithValue("@p_order_id", value.order_id);
                            cmd2.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                            cmd2.Parameters.AddWithValue("@p_wallet_id", value.wallet_id);
                            cmd2.Parameters.AddWithValue("@p_update_by", value.update_by);
                            cmd2.ExecuteNonQuery();
                            trans.Commit();

                            data.Success = true;
                            data.Code = "record_updated";
                            data.Message = WebApiResources.OrderHistoryUpdate;

                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "purchase/continue-order");
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
