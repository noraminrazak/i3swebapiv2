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
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace I3SwebAPIv2.Controllers
{
    public class ParentsV2Controller : ApiController
    {
        MPayWallet mpay = new MPayWallet();

        [HttpPost]
        [Authorize]
        [Route("api/v2/parent/profile")]
        public IHttpActionResult Post_Parent_Profile([FromBody]ProfileParam value)
        {
            ParentDataModel data = new ParentDataModel();
            if (value.profile_id != 0)
            {
                List<Parent> list = new List<Parent>();
                ParentModel listData = new ParentModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_parent_profile", conn))
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
                                    Parent prop = new Parent();
                                    prop.parent_id = dataReader["parent_id"] as int? ?? default(int);
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.photo_url = dataReader["photo_url"].ToString();
                                    prop.wallet_id = dataReader["wallet_id"] as int? ?? default(int);
                                    prop.wallet_number = dataReader["wallet_number"].ToString();
                                    prop.account_balance = Convert.ToDecimal(dataReader["account_balance"]);
                                    if (dataReader["status_code"].ToString() == "Aktif")
                                    {
                                        listData.Code = "active_parent";
                                        listData.Message = "Parent status is active.";
                                    }
                                    else
                                    {
                                        listData.Code = "inactive_parent";
                                        listData.Message = "Parent status is not active.";
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
                                listData.Message = "Parent could not be found.";
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
                        ExceptionUtility.LogException(ex, "parent/profile");
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
        [Route("api/v2/parent/school-relationship")]
        public IHttpActionResult Post_Parent_School_Relationship([FromBody]ParentParam value)
        {
            ParentDataModel data = new ParentDataModel();
            if (value.parent_id != 0)
            {
                List<ParentSchool> list = new List<ParentSchool>();
                ParentSchoolModel listData = new ParentSchoolModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_parent_school_relationship", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_parent_id", value.parent_id);
                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    ParentSchool prop = new ParentSchool();
                                    prop.school_id = Convert.ToInt16(dataReader["school_id"]);
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
                                listData.Code = "parent_school_list";
                                listData.Message = "Parent school relationship list.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Parent school relationship could not be found.";
                                listData.Data = list;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "parent/school-relationship");
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
        [Route("api/v2/parent/search-student")]
        public IHttpActionResult Post_Parent_Search_Student([FromUri]SqlFilterParam uri, [FromBody]SearchStudentParam body)
        {

            ParentDataModel data = new ParentDataModel();
            int? limit = 0;
            int? offset = 0;
            if (uri != null)
            {
                if (uri.limit == null)
                {
                    limit = 0;
                }
                else {
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
            else {
                limit = 0;
                offset = 0;
            }

            if (body.school_id != 0 && body.search_name != null)
            {
                List<ParentSearchStudent> list = new List<ParentSearchStudent>();
                ParentSearchStudentModel listData = new ParentSearchStudentModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_school_student_search", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_limit", limit);
                            cmd.Parameters.AddWithValue("@p_offset", offset);
                            cmd.Parameters.AddWithValue("@p_school_id", body.school_id);
                            cmd.Parameters.AddWithValue("@p_search_name", body.search_name);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    ParentSearchStudent prop = new ParentSearchStudent();
                                    prop.student_id = dataReader["student_id"] as int? ?? default(int);
                                    prop.student_number = dataReader["student_number"].ToString();
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.nric = dataReader["nric"].ToString();
                                    prop.photo_url = dataReader["photo_url"].ToString();
                                    prop.school_id = dataReader["school_id"] as int? ?? default(int);
                                    prop.school_name = dataReader["school_name"].ToString();
                                    prop.school_type_id = dataReader["school_type_id"] as int? ?? default(int);
                                    prop.school_type = dataReader["school_type"].ToString();
                                    prop.class_id = dataReader["class_id"] as int? ?? default(int);
                                    prop.class_name = dataReader["class_name"].ToString();
                                    prop.card_id = dataReader["card_id"] as int? ?? default(int);
                                    prop.card_number = dataReader["card_number"].ToString();
                                    prop.card_status = dataReader["card_status"].ToString();
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "select_student";
                                listData.Message = "Please select student.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Student could not be found.";
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
                        ExceptionUtility.LogException(ex, "parent/search-student");
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
        [Route("api/v2/parent/search-staff")]
        public IHttpActionResult Post_Parent_Search_Staff([FromUri] SqlFilterParam uri, [FromBody] SearchStaffParam body)
        {
            StaffDataModel data = new StaffDataModel();
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
            if (body.school_id != 0 && body.search_name != null)
            {
                List<SearchStaff> list = new List<SearchStaff>();
                SearchStaffModel listData = new SearchStaffModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_school_staff_search", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_limit", limit);
                            cmd.Parameters.AddWithValue("@p_offset", offset);
                            cmd.Parameters.AddWithValue("@p_school_id", body.school_id);
                            cmd.Parameters.AddWithValue("@p_search_name", body.search_name);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    SearchStaff prop = new SearchStaff();
                                    prop.staff_id = dataReader["staff_id"] as int? ?? default(int);
                                    prop.profile_id = dataReader["profile_id"] as int? ?? default(int);
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.staff_number = dataReader["staff_number"].ToString();
                                    prop.photo_url = dataReader["photo_url"].ToString();
                                    prop.school_id = dataReader["school_id"] as int? ?? default(int);
                                    prop.school_name = dataReader["school_name"].ToString();
                                    prop.school_type_id = dataReader["school_type_id"] as int? ?? default(int);
                                    prop.school_type = dataReader["school_type"].ToString();
                                    prop.staff_type_id = dataReader["staff_type_id"] as int? ?? default(int);
                                    prop.staff_type = dataReader["staff_type"].ToString();
                                    prop.card_id = dataReader["card_id"] as int? ?? default(int);
                                    prop.card_number = dataReader["card_number"].ToString();
                                    prop.card_status = dataReader["card_status"].ToString();
                                    prop.wallet_id = dataReader["wallet_id"] as int? ?? default(int);
                                    prop.wallet_number = dataReader["wallet_number"].ToString();
                                    prop.shift_id = dataReader["shift_id"] as int? ?? default(int);
                                    prop.shift_code = dataReader["shift_code"].ToString();
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "select_staff";
                                listData.Message = "Please select staff.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Staff could not be found.";
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
                        ExceptionUtility.LogException(ex, "parent/search-staff");
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
        [Route("api/v2/parent/search-staff-wallet")]
        public IHttpActionResult Post_Parent_Search_Staff_Wallet([FromUri] SqlFilterParam uri, [FromBody] SearchStaffParam body)
        {
            StaffDataModel data = new StaffDataModel();
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
            if (body.school_id != 0 && body.search_name != null)
            {
                List<SearchStaff> list = new List<SearchStaff>();
                SearchStaffModel listData = new SearchStaffModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_school_staff_wallet_search", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_limit", limit);
                            cmd.Parameters.AddWithValue("@p_offset", offset);
                            cmd.Parameters.AddWithValue("@p_school_id", body.school_id);
                            cmd.Parameters.AddWithValue("@p_search_name", body.search_name);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    SearchStaff prop = new SearchStaff();
                                    prop.staff_id = dataReader["staff_id"] as int? ?? default(int);
                                    prop.profile_id = dataReader["profile_id"] as int? ?? default(int);
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.staff_number = dataReader["staff_number"].ToString();
                                    prop.photo_url = dataReader["photo_url"].ToString();
                                    prop.school_id = dataReader["school_id"] as int? ?? default(int);
                                    prop.school_name = dataReader["school_name"].ToString();
                                    prop.school_type_id = dataReader["school_type_id"] as int? ?? default(int);
                                    prop.school_type = dataReader["school_type"].ToString();
                                    prop.staff_type_id = dataReader["staff_type_id"] as int? ?? default(int);
                                    prop.staff_type = dataReader["staff_type"].ToString();
                                    prop.card_id = dataReader["card_id"] as int? ?? default(int);
                                    prop.card_number = dataReader["card_number"].ToString();
                                    prop.card_status = dataReader["card_status"].ToString();
                                    prop.wallet_id = dataReader["wallet_id"] as int? ?? default(int);
                                    prop.wallet_number = dataReader["wallet_number"].ToString();
                                    prop.shift_id = dataReader["shift_id"] as int? ?? default(int);
                                    prop.shift_code = dataReader["shift_code"].ToString();
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "select_staff";
                                listData.Message = "Please select staff.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Staff could not be found.";
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
                        ExceptionUtility.LogException(ex, "parent/search-staff");
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
        //[Route("api/v2/parent/create-student-relationship")]
        //public async Task<IHttpActionResult> Post_Parent_Create_Student_Relationship(string culture, [FromBody]RegisterStudentParam value)
        //{
        //    if (!string.IsNullOrEmpty(culture))
        //    {
        //        var language = new CultureInfo(culture);
        //        Thread.CurrentThread.CurrentUICulture = language;
        //    }
        //    ParentDataModel data = new ParentDataModel();
        //    if (value.parent_id != 0 && value.student_id != 0 && value.school_id != 0 && value.class_id != 0 && value.create_by != null)
        //    {
        //        List<RegisterStudent> list = new List<RegisterStudent>();
        //        RegisterStudentModel listData = new RegisterStudentModel();
        //        MySqlTransaction trans = null;
        //        data.Success = false;
        //        int uid = 0;
        //        string sqlQuery = string.Empty;
        //        string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

        //        using (MySqlConnection conn = new MySqlConnection(constr))
        //        {
        //            try
        //            {
        //                conn.Open();
        //                trans = conn.BeginTransaction();

        //                using (MySqlCommand cmd = new MySqlCommand("sp_insert_parent_register_student", conn))
        //                {

        //                    cmd.Transaction = trans;
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    cmd.Parameters.Clear();
        //                    cmd.Parameters.AddWithValue("@p_parent_id", value.parent_id);
        //                    cmd.Parameters.AddWithValue("@p_student_id", value.student_id);
        //                    cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
        //                    cmd.Parameters.AddWithValue("@p_class_id", value.class_id);
        //                    cmd.Parameters.AddWithValue("@p_create_by", value.create_by);
        //                    cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
        //                    cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
        //                    cmd.Parameters.Add("@p_mpay_uid", MySqlDbType.Int32);
        //                    cmd.Parameters["@p_mpay_uid"].Direction = ParameterDirection.Output;
        //                    cmd.Parameters.Add("@p_wallet_id", MySqlDbType.Int32); //student wallet_id
        //                    cmd.Parameters["@p_wallet_id"].Direction = ParameterDirection.Output;
        //                    cmd.ExecuteNonQuery();
        //                    trans.Commit();

        //                    var status_code = (string)cmd.Parameters["@p_status_code"].Value;
        //                    if (DBNull.Value != cmd.Parameters["@p_mpay_uid"].Value) 
        //                    {
        //                        uid = Convert.ToInt32(cmd.Parameters["@p_mpay_uid"].Value);
        //                    }

        //                    var wallet_id = Convert.ToInt32(cmd.Parameters["@p_wallet_id"].Value);

        //                    if (status_code == "record_saved")
        //                    {
        //                        AddVirtualBalanceParam prop = new AddVirtualBalanceParam();
        //                        prop.uid = uid.ToString();

        //                        var result = mpay.PostAddVirtualBalance(prop);
        //                        string jsonStr = await result;
        //                        AddVirtualBalanceModel response = JsonConvert.DeserializeObject<AddVirtualBalanceModel>(jsonStr);

        //                        if (response.Header.status == "00")
        //                        {
        //                            CardInfo card = new CardInfo();
        //                            foreach (CardInfo sl in response.Body.cardinfo)
        //                            {
        //                                card.card_id = sl.card_id;
        //                                card.mask_cardno = sl.mask_cardno;
        //                                card.cardtoken = sl.cardtoken;
        //                                card.cardtype = sl.cardtype;
        //                                card.cardGroup = sl.cardGroup;
        //                            }

        //                            using (MySqlCommand cmd2 = new MySqlCommand("sp_update_student_wallet_info", conn))
        //                            {
        //                                try
        //                                {
        //                                    cmd2.Transaction = trans;
        //                                    cmd2.CommandType = CommandType.StoredProcedure;
        //                                    cmd2.Parameters.AddWithValue("@p_wallet_id", wallet_id);
        //                                    cmd2.Parameters.AddWithValue("@p_mpay_mask_cardno", card.mask_cardno);
        //                                    cmd2.Parameters.AddWithValue("@p_mpay_card_token", card.cardtoken);
        //                                    cmd2.Parameters.AddWithValue("@p_mpay_card_type", card.cardtype);
        //                                    cmd2.Parameters.AddWithValue("@p_mpay_card_group", card.cardGroup);
        //                                    cmd2.Parameters.AddWithValue("@p_update_by", value.create_by);
        //                                    cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
        //                                    cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
        //                                    cmd2.ExecuteNonQuery();
        //                                    trans.Commit();

        //                                    var code_status = (string)cmd.Parameters["@p_status_code"].Value;

        //                                    if (code_status == "record_saved" || code_status == "token_exists")
        //                                    {
        //                                        data.Success = true;
        //                                        data.Code = code_status;
        //                                        data.Message = WebApiResources.ParentStudentRelationshipCreateSuccess;
        //                                    }
        //                                    else 
        //                                    {
        //                                        data.Success = false;
        //                                        data.Code = code_status;
        //                                        data.Message = WebApiResources.NoRecordFoundText;
        //                                    }
        //                                }
        //                                catch (Exception ex)
        //                                {
        //                                    trans.Rollback();
        //                                    data.Success = false;
        //                                    data.Code = "error_occured";
        //                                    data.Message = WebApiResources.ErrorOccured;
        //                                    ExceptionUtility.LogException(ex, "parent/register-student");
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            data.Success = false;
        //                            data.Code = response.Header.status;
        //                            data.Message = response.Header.message;
        //                        }
        //                    }
        //                    else 
        //                    {
        //                        data.Success = false;
        //                        data.Code = status_code;
        //                        data.Message = WebApiResources.ParentStudentRelationshipExist;
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                trans.Rollback();
        //                data.Success = false;
        //                data.Code = "error_occured";
        //                data.Message = WebApiResources.ErrorOccured;
        //                ExceptionUtility.LogException(ex, "parent/register-student");
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
        [Route("api/v2/parent/remove-student-relationship")]
        public IHttpActionResult Post_Parent_Remove_Student_Relationship(string culture, [FromBody]RemoveStudentRelationship value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            ParentDataModel data = new ParentDataModel();
            if (value.parent_id != 0 && value.student_id != 0 && value.update_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_delete_parent_relationship", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_parent_id", value.parent_id);
                            cmd.Parameters.AddWithValue("@p_student_id", value.student_id);
                            cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                            cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                            cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            var status_code = cmd.Parameters["@p_status_code"].Value.ToString();

                            if (status_code == "delete_success")
                            {
                                data.Success = true;
                                data.Code = "delete_success";
                                data.Message = WebApiResources.StudentRelationshipRemoveSuccess;
                            }
                            else if (status_code == "clear_balance")
                            {
                                data.Success = false;
                                data.Code = "clear_balance";
                                data.Message = WebApiResources.PlsClearWalletBalance;
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "no_record_found";
                                data.Message = WebApiResources.StudentRelationshipNotFound;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "parent/remove-student-relationship");
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
        [Route("api/v2/parent/student-relationship")]
        public IHttpActionResult Post_Parent_Student_Relationship([FromBody]ParentParam value)
        {
            ParentDataModel data = new ParentDataModel();
            if (value.parent_id != 0)
            {
                List<ParentStudent> list = new List<ParentStudent>();
                ParentStudentModel listData = new ParentStudentModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_parent_relationship", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_parent_id", value.parent_id);
                            cmd.Parameters.AddWithValue("@p_student_id", 0);
                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    ParentStudent prop = new ParentStudent();
                                    prop.student_id = dataReader["student_id"] as int? ?? default(int);
                                    prop.profile_id = dataReader["student_profile_id"] as int? ?? default(int);
                                    prop.full_name = dataReader["student_full_name"].ToString();
                                    prop.photo_url = dataReader["student_photo_url"].ToString();
                                    prop.school_id = dataReader["school_id"] as int? ?? default(int);
                                    prop.school_name = dataReader["school_name"].ToString();
                                    prop.school_type_id = dataReader["school_type_id"] as int? ?? default(int);
                                    prop.school_type = dataReader["school_type"].ToString();
                                    prop.class_id = dataReader["class_id"] as int? ?? default(int);
                                    prop.class_name = dataReader["class_name"].ToString();
                                    prop.card_id = dataReader["card_id"] as int? ?? default(int);
                                    prop.card_number = dataReader["card_number"].ToString();
                                    prop.wallet_id = dataReader["student_wallet_id"] as int? ?? default(int);
                                    prop.wallet_number = dataReader["student_wallet_number"].ToString();
                                    prop.account_balance = Convert.ToDecimal(dataReader["student_account_balance"]);
                                    prop.card_status_id = dataReader["card_status_id"] as int? ?? default(int);
                                    prop.card_status = dataReader["card_status"].ToString();
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "parent_student_relationship";
                                listData.Message = "Parent student relationship";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Parent student relationship could not be found.";
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
                        ExceptionUtility.LogException(ex, "parent/student-relationship");
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
        [Route("api/v2/parent/club-relationship")]
        public IHttpActionResult Post_Parent_Club_Relationship([FromBody]ParentClubRelationshipParam value)
        {
            ParentDataModel data = new ParentDataModel();
            if (value.parent_id != 0)
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
                            cmd.Parameters.AddWithValue("@p_merchant_id", 0);
                            cmd.Parameters.AddWithValue("@p_staff_id", 0);
                            cmd.Parameters.AddWithValue("@p_parent_id", value.parent_id);
                            cmd.Parameters.AddWithValue("@p_student_id", 0);
                            cmd.Parameters.AddWithValue("@p_user_role_id", 9);
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
                        ExceptionUtility.LogException(ex, "parent/club-relationship");
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
        [Route("api/v2/parent/join-club")]
        public IHttpActionResult Post_Parent_Join_Club(string culture, [FromBody] ParentJoinClubRelationshipParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            StaffDataModel data = new StaffDataModel();
            if (value.parent_id != 0 && value.club_id != 0 && value.create_by != null)
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
                            cmd.Parameters.AddWithValue("@p_user_role_id", 9);
                            cmd.Parameters.AddWithValue("@p_staff_id", 0);
                            cmd.Parameters.AddWithValue("@p_student_id", 0);
                            cmd.Parameters.AddWithValue("@p_parent_id", value.parent_id);
                            cmd.Parameters.AddWithValue("@p_merchant_id", 0);
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
                        ExceptionUtility.LogException(ex, "parent/join-club");
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
