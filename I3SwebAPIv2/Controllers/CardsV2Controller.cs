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
using i3sAuth;
using System.Web.Http.Cors;

namespace I3SwebAPIv2.Controllers
{
    //[EnableCors(origins: "https://program.i-3s.com.my", headers: "*", methods: "*")]
    public class CardsV2Controller : ApiController
    {
        [HttpPost]
        [Authorize]
        [Route("api/v2/card/search-assignment")]
        public IHttpActionResult Post_Card_Search_Assignment([FromBody] CardSearchNameParam value)
        {
            CardDataModel data = new CardDataModel();
            if (value.user_role_id != 0 && value.school_id != 0 && value.search_name != null)
            {
                List<CardSearch> list = new List<CardSearch>();
                CardSearchModel listData = new CardSearchModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_card_search_name", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_user_role_id", value.user_role_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_search_name", value.search_name);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    CardSearch prop = new CardSearch();
                                    prop.profile_id = dataReader["profile_id"] as int? ?? default(int);
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.photo_url = dataReader["photo_url"].ToString();
                                    prop.school_name = dataReader["school_name"].ToString();
                                    prop.class_name = dataReader["class_name"].ToString();
                                    prop.card_id = dataReader["card_id"] as int? ?? default(int);
                                    prop.card_number = dataReader["card_number"].ToString();
                                    prop.card_status_id = dataReader["card_status_id"] as int? ?? default(int);
                                    prop.card_status = dataReader["card_status"].ToString();
                                    list.Add(prop);
                                }

                                listData.Success = true;
                                listData.Code = "assignment_list";
                                listData.Message = "Card assignment listing.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Card assignment could not be found.";
                                listData.Data = list;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "card/search-assignment");
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
        [Route("api/v2/card/update-status-blacklist")]
        public IHttpActionResult Post_Card_Update_Status_Blacklist(string culture, [FromBody] UpdateCardParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            CardDataModel data = new CardDataModel();
            if (value.card_id != 0 && value.school_id != 0 && value.update_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_update_card_status_blacklist", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_card_id", value.card_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                            cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                            cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            var status_code = (string)cmd.Parameters["@p_status_code"].Value;

                            if (status_code == "update_success")
                            {
                                data.Success = true;
                                data.Code = status_code;
                                data.Message = WebApiResources.CardStatusUpdateSuccess;
                            }
                            else if (status_code == "Blacklist")
                            {
                                data.Success = false;
                                data.Code = "card_already_blacklist";
                                data.Message = WebApiResources.CardStatusAlreadyBlacklist;
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "card_not_active";
                                data.Message = WebApiResources.CardStatusNotActive;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "card/update-status-blacklist");
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
        [Route("api/v2/card/daily-limit")]
        public IHttpActionResult Post_Card_Daily_Limit([FromBody] CardLimitParam value)
        {
            CardDataModel data = new CardDataModel();
            if (value.card_id != 0)
            {
                List<CardDailyLimit> list = new List<CardDailyLimit>();
                CardDailyLimitModel listData = new CardDailyLimitModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_card_daily_limit", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_card_id", value.card_id);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    CardDailyLimit prop = new CardDailyLimit();
                                    prop.card_id = dataReader["card_id"] as int? ?? default(int);
                                    prop.card_number = dataReader["card_number"].ToString();
                                    prop.daily_limit = dataReader["daily_limit"] as decimal? ?? default(decimal);
                                    prop.card_status_id = dataReader["card_status_id"] as int? ?? default(int);
                                    prop.card_status = dataReader["card_status"].ToString();
                                    list.Add(prop);
                                }

                                listData.Success = true;
                                listData.Code = "daily_limit";
                                listData.Message = "Card daily limit.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Card could not be found.";
                                listData.Data = list;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "card/daily-limit");
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
        [Route("api/v2/card/update-daily-limit")]
        public IHttpActionResult Post_Card_Update_Daily_Limit(string culture, [FromBody] UpdateCardLimitParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            CardDataModel data = new CardDataModel();
            if (value.card_id != 0 && value.school_id != 0 && value.daily_limit >= 0 && value.update_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_update_card_daily_limit", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_card_id", value.card_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_daily_limit", value.daily_limit);
                            cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                            cmd.ExecuteNonQuery();
                            trans.Commit();


                            data.Success = true;
                            data.Code = "update_success";
                            data.Message = WebApiResources.CardLimitUpdateSuccess;

                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "card/update-daily-limit");
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
        [Route("api/v2/card/search-status")]
        public IHttpActionResult Post_Card_Search_By_Status([FromBody] CardSearchStatusParam value)
        {
            CardDataModel data = new CardDataModel();
            if (value.card_number != null && value.card_status_id != 0)
            {
                List<Card> list = new List<Card>();
                CardModel listData = new CardModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_card_search_extra", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_card_number", value.card_number);
                            cmd.Parameters.AddWithValue("@p_card_status_id", value.card_status_id);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    Card prop = new Card();
                                    prop.card_id = dataReader["card_id"] as int? ?? default(int);
                                    prop.card_number = dataReader["card_number"].ToString();
                                    prop.card_status_id = dataReader["card_status_id"] as int? ?? default(int);
                                    prop.card_status = dataReader["card_status"].ToString();
                                    list.Add(prop);
                                }

                                listData.Success = true;
                                listData.Code = "select_card";
                                listData.Message = "Card listing.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Card could not be found.";
                                listData.Data = list;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "card/search-status");
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
        [Route("api/v2/card/replacement")]
        public IHttpActionResult Post_Card_Replacement(string culture, [FromBody] CardReplacementParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            CardDataModel data = new CardDataModel();
            if (value.user_role_id != 0 && value.old_card_id != 0 && value.old_card_status_id != 0 && !string.IsNullOrEmpty(value.new_card_number) && value.school_id != 0 && value.create_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_update_card_replacement", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_user_role_id", value.user_role_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_old_card_id", value.old_card_id);
                            cmd.Parameters.AddWithValue("@p_old_card_status_id", value.old_card_status_id);
                            cmd.Parameters.AddWithValue("@p_new_card_number", value.new_card_number);
                            cmd.Parameters.AddWithValue("@p_create_by", value.create_by);
                            cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                            cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            var status_code = (string)cmd.Parameters["@p_status_code"].Value;

                            if (status_code == "update_success")
                            {
                                data.Success = true;
                                data.Code = "update_success";
                                data.Message = WebApiResources.CardReplaceSuccess;
                            }
                            else if (status_code == "card_not_blacklist")
                            {
                                data.Success = false;
                                data.Code = status_code;
                                data.Message = WebApiResources.InCaseLostStolenCard;
                            }
                            else if (status_code == "card_not_active")
                            {
                                data.Success = false;
                                data.Code = status_code;
                                data.Message = WebApiResources.CardStatusNotActive;
                            }
                            else if (status_code == "card_not_found")
                            {
                                data.Success = false;
                                data.Code = status_code;
                                data.Message = WebApiResources.NewCardNotFound;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "card/replacement");
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
        [Route("api/v2/card/transaction")]
        public IHttpActionResult Post_Card_Transaction([FromBody] CardSearchNameParam value)
        {
            CardDataModel data = new CardDataModel();
            if (value.user_role_id != 0 && value.school_id != 0 && value.search_name != null)
            {
                List<CardSearch> list = new List<CardSearch>();
                CardSearchModel listData = new CardSearchModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_card_search_name", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_user_role_id", value.user_role_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_search_name", value.search_name);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    CardSearch prop = new CardSearch();
                                    prop.profile_id = dataReader["profile_id"] as int? ?? default(int);
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.photo_url = dataReader["photo_url"].ToString();
                                    prop.school_name = dataReader["school_name"].ToString();
                                    prop.class_name = dataReader["class_name"].ToString();
                                    prop.card_id = dataReader["card_id"] as int? ?? default(int);
                                    prop.card_number = dataReader["card_number"].ToString();
                                    prop.card_status_id = dataReader["card_status_id"] as int? ?? default(int);
                                    prop.card_status = dataReader["card_status"].ToString();
                                    list.Add(prop);
                                }

                                listData.Success = true;
                                listData.Code = "assignment_list";
                                listData.Message = "Card assignment listing.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Card assignment could not be found.";
                                listData.Data = list;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "card/search-assignment");
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
        [Route("api/v2/card/decrypt")]
        public IHttpActionResult Post_Card_Hash([FromBody] CardHashParam value)
        {
            string salt = ConfigurationManager.AppSettings["passPhrase"];
            Rijndael auth = new Rijndael();
            CardDataModel data = new CardDataModel();
            if (!string.IsNullOrEmpty(value.card_pin))
            {
                string cardPin = auth.DecryptRijndael(value.card_pin, salt);

                data.Success = true;
                data.Code = "OK";
                data.Message = cardPin;
            }
            else
            {
                return BadRequest("Missing parameters.");
            }

            return Ok(data);
        }
    }
}
