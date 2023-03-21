using I3SwebAPIv2.Class;
using I3SwebAPIv2.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Http.Cors;
using System.Web.Http;
using System.Globalization;
using System.Threading;
using I3SwebAPIv2.Resources;

namespace I3SwebAPIv2.Controllers
{
    public class StaffsV2Controller : ApiController
    {
        [HttpPost]
        [Authorize]
        [Route("api/v2/staff/profile")]
        public IHttpActionResult Post_Staff_Profile([FromBody] ProfileParam value)
        {
            UserDataModel data = new UserDataModel();
            if (value.profile_id != 0)
            {
                List<Staff> list = new List<Staff>();
                StaffModel listData = new StaffModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_staff_profile", conn))
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
                                    Staff prop = new Staff();
                                    prop.staff_id = dataReader["staff_id"] as int? ?? default(int);
                                    prop.staff_number = dataReader["staff_number"].ToString();
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.photo_url = dataReader["photo_url"].ToString();
                                    prop.school_id = dataReader["school_id"] as int? ?? default(int);
                                    prop.school_name = dataReader["school_name"].ToString();
                                    prop.shift_id = dataReader["shift_id"] as int? ?? default(int);
                                    prop.shift_code = dataReader["shift_code"].ToString();
                                    prop.staff_type_id = dataReader["staff_type_id"] as int? ?? default(int);
                                    prop.staff_type = dataReader["staff_type"].ToString();
                                    prop.card_id = dataReader["card_id"] as int? ?? default(int);
                                    prop.card_number = dataReader["card_number"].ToString();
                                    prop.card_status_id = dataReader["card_status_id"] as int? ?? default(int);
                                    prop.card_status = dataReader["card_status"].ToString();
                                    prop.wallet_id = dataReader["wallet_id"] as int? ?? default(int);
                                    prop.wallet_number = dataReader["wallet_number"].ToString();
                                    prop.account_balance = Convert.ToDecimal(dataReader["account_balance"]);
                                    if (dataReader["status_code"].ToString() == "Aktif")
                                    {
                                        listData.Code = "active_staff";
                                        listData.Message = "Staff status is active.";
                                    }
                                    else
                                    {
                                        listData.Code = "inactive_staff";
                                        listData.Message = "Staff status is not active.";
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
                        ExceptionUtility.LogException(ex, "staff/profile");
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
        [Route("api/v2/staff/shift")]
        public IHttpActionResult Post_Staff_Shift([FromBody] StaffShiftParam value)
        {
            UserDataModel data = new UserDataModel();
            if (value.school_id != 0)
            {
                List<StaffShift> list = new List<StaffShift>();
                StaffShiftModel listData = new StaffShiftModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_staff_shift", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@SchoolId", value.school_id);
                            cmd.Parameters.AddWithValue("@ShiftId", 0);
                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    StaffShift prop = new StaffShift();
                                    prop.shift_id = Convert.ToInt16(dataReader["shift_id"]);
                                    prop.shift_code = dataReader["shift_code"].ToString();
                                    prop.start_time = dataReader["start_time"] as TimeSpan? ?? default(TimeSpan);
                                    prop.end_time = dataReader["end_time"] as TimeSpan? ?? default(TimeSpan);
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "select_staff_shift";
                                listData.Message = "Please select staff shift.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Staff shift could not be found.";
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
                        ExceptionUtility.LogException(ex, "staff/shift");
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
        [Route("api/v2/staff/create-post")]
        public IHttpActionResult Post_Staff_Create_Post(string culture, [FromBody] CreateSchoolPostParam value)
        {
            SchoolDataModel data = new SchoolDataModel();
            StaffV2Class cls = new StaffV2Class();
            if (value != null)
            {
                if (value.post_group_id != 0)
                {
                    if (value.post_group_id == 1)
                    {
                        data = cls.Insert_School_Post(culture, value);
                    }
                    else if (value.post_group_id == 2)
                    {
                        data = cls.Insert_School_Class_Post(culture, value);
                    }
                    else if (value.post_group_id == 3)
                    {
                        data = cls.Insert_School_Club_Post(culture, value);
                    }
                    return Ok(data);
                }
                else
                {
                    return BadRequest("Missing parameters.");
                }
            }

            if (data.Success == false)
                if (data.Code == "missing_params")
                {
                    return BadRequest(data.Message);
                }
                else if (data.Code == "error_occured")
                {
                    return Ok(data);
                }

            return Ok(data);
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/staff/update-post")]
        public IHttpActionResult Post_Staff_Update_Post(string culture, [FromBody] UpdateSchoolPostParam value)
        {

            SchoolDataModel data = new SchoolDataModel();
            StaffV2Class cls = new StaffV2Class();
            if (value != null)
            {
                if (value.post_group_id != 0)
                {
                    if (value.post_group_id == 1)
                    {
                        data = cls.Update_School_Post(culture, value);
                    }
                    else if (value.post_group_id == 2)
                    {
                        data = cls.Update_School_Class_Post(culture, value);
                    }
                    else if (value.post_group_id == 3)
                    {
                        data = cls.Update_School_Club_Post(culture, value);
                    }
                    return Ok(data);
                }
                else
                {
                    return BadRequest("Missing parameters.");
                }
            }

            if (data.Success == false)
                if (data.Code == "missing_params")
                {
                    return BadRequest(data.Message);
                }
                else if (data.Code == "error_occured")
                {
                    return Ok(data);
                }

            return Ok(data);
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/staff/remove-post-photo")]
        public IHttpActionResult Post_Staff_Remove_Post_Photo(string culture, [FromBody] RemovePostPhotoParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            MerchantDataModel data = new MerchantDataModel();
            if (value.post_id != 0 && value.school_id != 0 && value.post_group_id != 0 &&
                value.update_by != null && value.staff_id != 0)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_delete_post_photo", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_post_id", value.post_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_class_id", value.class_id);
                            cmd.Parameters.AddWithValue("@p_club_id", value.club_id);
                            cmd.Parameters.AddWithValue("@p_staff_id", value.staff_id);
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
                        ExceptionUtility.LogException(ex, "staff/remove-post-photo");
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
        [Route("api/v2/staff/remove-post")]
        public IHttpActionResult Post_Staff_Remove_Post(string culture, [FromBody] RemovePostPhotoParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            MerchantDataModel data = new MerchantDataModel();
            if (value.post_id != 0 && value.school_id != 0 && value.post_group_id != 0 &&
                value.update_by != null && value.staff_id != 0)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_delete_post", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_post_id", value.post_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_class_id", value.class_id);
                            cmd.Parameters.AddWithValue("@p_club_id", value.club_id);
                            cmd.Parameters.AddWithValue("@p_staff_id", value.staff_id);
                            cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            data.Success = true;
                            data.Code = "success";
                            data.Message = WebApiResources.PostSuccessRemove;
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "staff/remove-post");
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
        [Route("api/v2/staff/search-student")]
        public IHttpActionResult Post_Staff_Search_Student([FromUri] SqlFilterParam uri, [FromBody] SearchStudentParam body)
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
                List<StaffSearchStudent> list = new List<StaffSearchStudent>();
                StaffSearchStudentModel listData = new StaffSearchStudentModel();
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
                                    StaffSearchStudent prop = new StaffSearchStudent();
                                    prop.student_id = dataReader["student_id"] as int? ?? default(int);
                                    prop.profile_id = dataReader["profile_id"] as int? ?? default(int);
                                    prop.student_number = dataReader["student_number"].ToString();
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.photo_url = dataReader["photo_url"].ToString();
                                    prop.wallet_id = dataReader["wallet_id"] as int? ?? default(int);
                                    prop.wallet_number = dataReader["wallet_number"].ToString();
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
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "staff/search-student");
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
        [Route("api/v2/staff/search-student-wallet")]
        public IHttpActionResult Post_Staff_Search_Student_Wallet([FromUri] SqlFilterParam uri, [FromBody] SearchStudentParam body)
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
                List<StaffSearchStudent> list = new List<StaffSearchStudent>();
                StaffSearchStudentModel listData = new StaffSearchStudentModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_school_student_wallet_search", conn))
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
                                    StaffSearchStudent prop = new StaffSearchStudent();
                                    prop.student_id = dataReader["student_id"] as int? ?? default(int);
                                    prop.profile_id = dataReader["profile_id"] as int? ?? default(int);
                                    prop.student_number = dataReader["student_number"].ToString();
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.photo_url = dataReader["photo_url"].ToString();
                                    prop.wallet_id = dataReader["wallet_id"] as int? ?? default(int);
                                    prop.wallet_number = dataReader["wallet_number"].ToString();
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
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "staff/search-student");
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
        [Route("api/v2/staff/search-parent")]
        public IHttpActionResult Post_Staff_Search_Parent([FromUri] SqlFilterParam uri, [FromBody] SearchParentParam body)
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
                List<SearchParent> list = new List<SearchParent>();
                SearchParentModel listData = new SearchParentModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_school_parent_search", conn))
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
                                    SearchParent prop = new SearchParent();
                                    prop.parent_id = dataReader["parent_id"] as int? ?? default(int);
                                    prop.profile_id = dataReader["parent_profile_id"] as int? ?? default(int);
                                    prop.full_name = dataReader["parent_full_name"].ToString();
                                    prop.photo_url = dataReader["parent_photo_url"].ToString();
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "select_parent";
                                listData.Message = "Please select parent.";
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
                        ExceptionUtility.LogException(ex, "staff/search-parent");
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
        [Route("api/v2/staff/search-merchant")]
        public IHttpActionResult Post_Staff_Search_Merchant([FromUri] SqlFilterParam uri, [FromBody] SearchMerchantParam body)
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
                List<SearchMerchant> list = new List<SearchMerchant>();
                SearchMerchantModel listData = new SearchMerchantModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_school_merchant_search", conn))
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
                                    SearchMerchant prop = new SearchMerchant();
                                    prop.merchant_id = dataReader["merchant_id"] as int? ?? default(int);
                                    prop.profile_id = dataReader["profile_id"] as int? ?? default(int);
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.company_name = dataReader["company_name"].ToString();
                                    prop.photo_url = dataReader["photo_url"].ToString();
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "select_merchant";
                                listData.Message = "Please select merchant.";
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
                        ExceptionUtility.LogException(ex, "staff/search-merchant");
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
        [Route("api/v2/staff/search-staff")]
        public IHttpActionResult Post_Staff_Search_Staff([FromUri] SqlFilterParam uri, [FromBody] SearchStaffParam body)
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
                        ExceptionUtility.LogException(ex, "staff/search-staff");
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
        [Route("api/v2/staff/create-class")]
        public IHttpActionResult Post_Staff_Create_Class([FromBody] CreateClassParam value)
        {
            StaffDataModel data = new StaffDataModel();
            if (value.class_name != null && value.school_id != 0 && value.session_id != 0 && value.staff_id != 0 && value.create_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_insert_school_class", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@ClassName", value.class_name);
                            cmd.Parameters.AddWithValue("@SchoolId", value.school_id);
                            cmd.Parameters.AddWithValue("@SessionId", value.session_id);
                            cmd.Parameters.AddWithValue("@StaffId", value.staff_id);
                            cmd.Parameters.AddWithValue("@CreateBy", value.create_by);
                            cmd.Parameters.Add("@StatusCode", MySqlDbType.VarChar);
                            cmd.Parameters["@StatusCode"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            var status_code = cmd.Parameters["@StatusCode"].Value.ToString();

                            if (status_code == "record_saved")
                            {
                                data.Success = true;
                                data.Code = "record_saved";
                                data.Message = "School class saved successfully.";
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "record_exist";
                                data.Message = "School class already exists.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "staff/create-class");
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
        [Route("api/v2/staff/create-club")]
        public IHttpActionResult Post_Staff_Create_Club(string culture, [FromBody] CreateClubParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            StaffDataModel data = new StaffDataModel();
            if (value.club_name != null && value.school_id != 0 && value.staff_id != 0 && value.create_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_insert_school_club", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_club_name", value.club_name);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_staff_id", value.staff_id);
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
                                data.Message = WebApiResources.SchoolClubSaveSuccess;
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "record_exist";
                                data.Message = WebApiResources.SchoolClubExist;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "staff/create-club");
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
        [Route("api/v2/staff/create-club-attendance")]
        public IHttpActionResult Post_Staff_Create_Club_Attendance(string culture, [FromBody] SchoolClubGenerateAttendanceRequest body)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            SchoolDataModel data = new SchoolDataModel();
            if (body.school_id != 0 && body.club_id != 0 && body.entry_date != DateTime.MinValue && body.create_by != null)
            {
                MySqlTransaction trans = null;
                string sqlQuery = string.Empty;
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd = new MySqlCommand("sp_insert_school_club_attendance_report", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_school_id", body.school_id);
                            cmd.Parameters.AddWithValue("@p_club_id", body.club_id);
                            cmd.Parameters.AddWithValue("@p_entry_date", body.entry_date);
                            cmd.Parameters.AddWithValue("@p_create_by", body.create_by);
                            cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                            cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            var status_code = cmd.Parameters["@p_status_code"].Value.ToString();

                            if (status_code == "record_saved")
                            {
                                data.Success = true;
                                data.Code = "record_saved";
                                data.Message = WebApiResources.ClubAttCreatedSuccess;
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "record_exist";
                                data.Message = WebApiResources.ClubAttAlreadyExist;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //listData.Success = false;
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "staff/create-club-attendance");
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
        [Route("api/v2/staff/join-class")]
        public IHttpActionResult Post_Staff_Join_Class(string culture,[FromBody] AddClassRelationship value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            StaffDataModel data = new StaffDataModel();
            if (value.staff_id != 0 && value.class_id != 0 && value.class_teacher_flag != null && value.create_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_insert_staff_relationship", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_class_id", value.class_id);
                            cmd.Parameters.AddWithValue("@p_staff_id", value.staff_id);
                            cmd.Parameters.AddWithValue("@p_class_teacher_flag", value.class_teacher_flag);
                            cmd.Parameters.AddWithValue("@p_create_by", value.create_by);
                            cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                            cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd.Parameters.Add("@p_full_name", MySqlDbType.VarChar);
                            cmd.Parameters["@p_full_name"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            var status_code = cmd.Parameters["@p_status_code"].Value.ToString();

                            if (status_code == "record_saved")
                            {
                                data.Success = true;
                                data.Code = "record_saved";
                                data.Message = WebApiResources.ClassRelationshipCreateSuccess;
                            }
                            else if (status_code == "record_exist")
                            {
                                data.Success = false;
                                data.Code = "record_exist";
                                data.Message = WebApiResources.ClassRelationshipExist;
                            }
                            else if (status_code == "class_teacher_exist")
                            {
                                var full_name = cmd.Parameters["@p_full_name"].Value.ToString();

                                data.Success = false;
                                data.Code = "class_teacher_exist";
                                data.Message = WebApiResources.ClassTeacherExist.Replace("full_name", full_name);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "staff/join-class");
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
        [Route("api/v2/staff/leave-class")]
        public IHttpActionResult Post_Staff_Leave_Class(string culture, [FromBody] RemoveClassRelationship value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            StaffDataModel data = new StaffDataModel();
            if (value.relationship_id != 0 && value.staff_id != 0 && value.class_id != 0 && value.update_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_delete_staff_relationship", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_relationship_id", value.relationship_id);
                            cmd.Parameters.AddWithValue("@p_class_id", value.class_id);
                            cmd.Parameters.AddWithValue("@p_staff_id", value.staff_id);
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
                                data.Message = WebApiResources.ClassRelationshipRemoveSuccess;
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "no_record_found";
                                data.Message = WebApiResources.ClassRelationshipNotFound;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "staff/leave-class");
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
        [Route("api/v2/staff/enroll-student-class")]
        public IHttpActionResult Post_Staff_Enroll_Student_Class(string culture, [FromBody] EnrollClassParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            StaffDataModel data = new StaffDataModel();
            if (value.student_id != 0 && value.school_id != 0 && value.class_id != 0 && value.update_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_update_student_class", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_student_id", value.student_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_class_id", value.class_id);
                            cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                            cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                            cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            var status_code = cmd.Parameters["@p_status_code"].Value.ToString();

                            if (status_code == "record_update")
                            {
                                data.Success = true;
                                data.Code = "record_update";
                                data.Message = WebApiResources.StudentClassUpdateSuccess;
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "record_exist";
                                data.Message = WebApiResources.StudentClassExist;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "staff/enroll-student-class");
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
        [Route("api/v2/staff/remove-student-class")]
        public IHttpActionResult Post_Staff_Remove_Student_Class(string culture, [FromBody] RemoveClassParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            StaffDataModel data = new StaffDataModel();
            if (value.student_id != 0 && value.school_id != 0 && value.update_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_delete_student_class", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_student_id", value.student_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_class_id", value.class_id);
                            cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                            cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                            cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            var status_code = cmd.Parameters["@p_status_code"].Value.ToString();

                            if (status_code == "record_update")
                            {
                                data.Success = true;
                                data.Code = "record_update";
                                data.Message = WebApiResources.StudentClsRemoveSuccess;
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "no_record_found";
                                data.Message = WebApiResources.StudentClsNotFound;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "staff/remove-student-class");
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
        [Route("api/v2/staff/remove-staff")]
        public IHttpActionResult Post_Staff_Remove_Staff(string culture, [FromBody] RemoveStaffParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            StaffDataModel data = new StaffDataModel();
            if (value.staff_id != 0 && value.school_id != 0 && value.update_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_delete_staff", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_staff_id", value.staff_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                            cmd.Parameters.Add("@CheckStaffExists", MySqlDbType.Int32);
                            cmd.Parameters["@CheckStaffExists"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            var StaffExist = Convert.ToInt16(cmd.Parameters["@CheckStaffExists"].Value);

                            if (StaffExist == 1)
                            {
                                data.Success = true;
                                data.Code = "record_update";
                                data.Message = WebApiResources.StaffRemoveSuccess;
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "no_record_found";
                                data.Message = WebApiResources.StaffNotFound;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "staff/remove-staff");
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
        [Route("api/v2/staff/update-staff-shift")]
        public IHttpActionResult Post_Staff_Update_Staff_Shift(string culture, [FromBody] UpdateStaffShiftParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            StaffDataModel data = new StaffDataModel();
            if (value.staff_id != 0 && value.school_id != 0 && value.shift_id != 0 && value.update_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_update_staff_shift_staff_update", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_staff_id", value.staff_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_shift_id", value.shift_id);
                            cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                            cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                            cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            var status_code = cmd.Parameters["@p_status_code"].Value.ToString();

                            if (status_code == "record_update")
                            {
                                data.Success = true;
                                data.Code = "record_update";
                                data.Message = WebApiResources.StaffShiftUpdateSuccess;
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "record_exist";
                                data.Message = WebApiResources.StaffShiftExist;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "staff/update-staff-shift");
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
        [Route("api/v2/staff/class-relationship")]
        public IHttpActionResult Post_Staff_Class_Relationship([FromBody] ClassRelationshipParam value)
        {
            StaffDataModel data = new StaffDataModel();
            if (value.school_id != 0 && value.staff_id != 0)
            {
                List<ClassRelationship> list = new List<ClassRelationship>();
                ClassRelationshipModel listData = new ClassRelationshipModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_staff_relationship", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_staff_id", value.staff_id);

                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    ClassRelationship prop = new ClassRelationship();
                                    prop.relationship_id = Convert.ToInt16(dataReader["relationship_id"]);
                                    prop.class_id = Convert.ToInt16(dataReader["class_id"]);
                                    prop.class_name = dataReader["class_name"].ToString();
                                    prop.school_id = Convert.ToInt16(dataReader["school_id"]);
                                    prop.school_name = dataReader["school_name"].ToString();
                                    prop.school_type = dataReader["school_type"].ToString();
                                    prop.session_code = dataReader["session_code"].ToString();
                                    prop.class_teacher_flag = dataReader["class_teacher_flag"].ToString();
                                    prop.total_student = Convert.ToInt16(dataReader["total_student"]);
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "class_relationship_list";
                                listData.Message = "Class relationship list.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Class relationship could not be found.";
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
                        ExceptionUtility.LogException(ex, "staff/class-relationship");
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
        [Route("api/v2/staff/join-club")]
        public IHttpActionResult Post_Staff_Join_Club(string culture, [FromBody] StaffJoinClubRelationshipParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            StaffDataModel data = new StaffDataModel();
            if (value.staff_id != 0 && value.club_id != 0 && value.create_by != null)
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
                            cmd.Parameters.AddWithValue("@p_user_role_id", 8);
                            cmd.Parameters.AddWithValue("@p_staff_id", value.staff_id);
                            cmd.Parameters.AddWithValue("@p_student_id", 0);
                            cmd.Parameters.AddWithValue("@p_parent_id", 0);
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
                        ExceptionUtility.LogException(ex, "staff/join-club");
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
        [Route("api/v2/staff/handover-club")]
        public IHttpActionResult Post_Staff_Handover_Club(string culture, [FromBody] StaffHandoverClubParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            StaffDataModel data = new StaffDataModel();
            if (value.school_id != 0 && value.club_id != 0 && value.current_staff_id != 0 && value.new_staff_id != 0 && value.new_profile_id != 0 && value.update_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_update_school_club_handover", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_club_id", value.club_id);
                            cmd.Parameters.AddWithValue("@p_current_staff_id", value.current_staff_id);
                            cmd.Parameters.AddWithValue("@p_new_staff_id", value.new_staff_id);
                            cmd.Parameters.AddWithValue("@p_new_profile_id", value.new_profile_id);
                            cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                            cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                            cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            var status_code = cmd.Parameters["@p_status_code"].Value.ToString();

                            if (status_code == "record_saved")
                            {
                                data.Success = true;
                                data.Code = "record_saved";
                                data.Message = WebApiResources.ClubHandoverSuccess;
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "not_found";
                                data.Message = WebApiResources.NewChairmanMustBeMember;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "staff/handover-club");
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
        [Route("api/v2/staff/club-relationship")]
        public IHttpActionResult Post_Staff_Club_Relationship([FromBody] StaffClubRelationshipParam value)
        {
            StaffDataModel data = new StaffDataModel();
            if (value.school_id != 0 && value.staff_id != 0)
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
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_merchant_id", 0);
                            cmd.Parameters.AddWithValue("@p_staff_id", value.staff_id);
                            cmd.Parameters.AddWithValue("@p_parent_id", 0);
                            cmd.Parameters.AddWithValue("@p_student_id", 0);
                            cmd.Parameters.AddWithValue("@p_user_role_id", 8);
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
                        ExceptionUtility.LogException(ex, "staff/club-relationship");
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
        [Route("api/v2/staff/add-club-member")]
        public IHttpActionResult Post_Staff_Add_Club_Member(string culture, [FromBody] AddClubMemberParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            StaffDataModel data = new StaffDataModel();
            if (value.profile_id != 0 && value.club_id != 0 && value.user_role_id != 0 && value.create_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_insert_school_club_member", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_club_id", value.club_id);
                            cmd.Parameters.AddWithValue("@p_user_role_id", value.user_role_id);
                            cmd.Parameters.AddWithValue("@p_profile_id", value.profile_id);
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
                                data.Message = WebApiResources.NewClubMemberCreateSuccess;
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "record_exist";
                                data.Message = WebApiResources.ClubMemberExist;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "staff/add-club-member");
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
        [Route("api/v2/staff/add-class-student")]
        public IHttpActionResult Post_Staff_Add_Class_Student(string culture, [FromBody] AddClassStudentParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            StaffDataModel data = new StaffDataModel();
            if (value.school_id != 0 && value.class_id != 0 && value.student_id != 0 && value.create_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_update_student_class", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_class_id", value.class_id);
                            cmd.Parameters.AddWithValue("@p_student_id", value.student_id);
                            cmd.Parameters.AddWithValue("@p_update_by", value.create_by);
                            cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                            cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            var status_code = cmd.Parameters["@p_status_code"].Value.ToString();

                            if (status_code == "record_update")
                            {
                                data.Success = true;
                                data.Code = "record_update";
                                data.Message = WebApiResources.NewClassStudentCreateSuccess;
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "record_exist";
                                data.Message = WebApiResources.ClassStudentExist;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "staff/add-class-student");
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
        [Route("api/v2/staff/remove-club-member")]
        public IHttpActionResult Post_Staff_Remove_Club_Member(string culture, [FromBody] RemoveClubMemberParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            StaffDataModel data = new StaffDataModel();
            if (value.relationship_id != 0 && value.profile_id != 0 && value.club_id != 0 && value.update_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_delete_school_club_relationship", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_relationship_id", value.relationship_id);
                            cmd.Parameters.AddWithValue("@p_club_id", value.club_id);
                            cmd.Parameters.AddWithValue("@p_profile_id", value.profile_id);
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
                                data.Message = WebApiResources.ClubMemberRemoveSuccess;
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "no_record_found";
                                data.Message = WebApiResources.ClubMemberNotFound;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "staff/remove-club-member");
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
        [Route("api/v2/staff/update-student-attendance")]
        public IHttpActionResult Post_Staff_Update_Student_Attendance(string culture, [FromBody] UpdateStudentAttendanceParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            bool proceed = false;
            StaffDataModel data = new StaffDataModel();
            if (value.report_id != 0 && value.student_id != 0 && value.school_id != 0 && value.class_id != 0 && value.attendance_id != 0 && value.update_by != null)
            {
                if (value.attendance_id == 1)
                {
                    if (value.report_id != 0)
                    {
                        proceed = true;
                    }
                    else
                    {
                        proceed = false;
                    }
                }
                else
                {
                    proceed = true;
                }

                if (proceed == true)
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

                            using (MySqlCommand cmd = new MySqlCommand("sp_update_student_attendance_report", conn))
                            {

                                cmd.Transaction = trans;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@p_report_id", value.report_id);
                                cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                                cmd.Parameters.AddWithValue("@p_class_id", value.class_id);
                                cmd.Parameters.AddWithValue("@p_student_id", value.student_id);
                                cmd.Parameters.AddWithValue("@p_attendance_id", value.attendance_id);
                                cmd.Parameters.AddWithValue("@p_reason_id", value.reason_id);
                                cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                                cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                                cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                                cmd.ExecuteNonQuery();
                                trans.Commit();

                                var status_code = cmd.Parameters["@p_status_code"].Value.ToString();

                                if (status_code == "update_success")
                                {
                                    data.Success = true;
                                    data.Code = "record_update";
                                    data.Message = WebApiResources.StudentAttUpdateSuccess;
                                }
                                else
                                {
                                    data.Success = false;
                                    data.Code = "no_record_found";
                                    data.Message = WebApiResources.StudentAttNotFound;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            data.Success = false;
                            data.Code = "error_occured";
                            data.Message = WebApiResources.ErrorOccured;
                            ExceptionUtility.LogException(ex, "staff/update-student-attendance");
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
            else
            {
                return BadRequest("Missing parameters.");
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/staff/update-club-attendance")]
        public IHttpActionResult Post_Staff_Update_Club_Member_Attendance(string culture, [FromBody] UpdateClubAttendanceParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            bool proceed = false;
            StaffDataModel data = new StaffDataModel();
            if (value.report_id != 0 && value.profile_id != 0 && value.school_id != 0 && value.club_id != 0 && value.attendance_id != 0 && value.update_by != null)
            {
                if (value.attendance_id == 1)
                {
                    if (value.report_id != 0)
                    {
                        proceed = true;
                    }
                    else
                    {
                        proceed = false;
                    }
                }
                else
                {
                    proceed = true;
                }

                if (proceed == true)
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

                            using (MySqlCommand cmd = new MySqlCommand("sp_update_school_club_attendance_report", conn))
                            {

                                cmd.Transaction = trans;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@p_report_id", value.report_id);
                                cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                                cmd.Parameters.AddWithValue("@p_club_id", value.club_id);
                                cmd.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                                cmd.Parameters.AddWithValue("@p_attendance_id", value.attendance_id);
                                cmd.Parameters.AddWithValue("@p_reason_id", value.reason_id);
                                cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                                cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                                cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                                cmd.ExecuteNonQuery();
                                trans.Commit();

                                var status_code = cmd.Parameters["@p_status_code"].Value.ToString();

                                if (status_code == "update_success")
                                {
                                    data.Success = true;
                                    data.Code = "record_update";
                                    data.Message = WebApiResources.StudentAttUpdateSuccess;
                                }
                                else
                                {
                                    data.Success = false;
                                    data.Code = "no_record_found";
                                    data.Message = WebApiResources.StudentAttNotFound;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            data.Success = false;
                            data.Code = "error_occured";
                            data.Message = WebApiResources.ErrorOccured;
                            ExceptionUtility.LogException(ex, "staff/update-club-attendance");
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
            else
            {
                return BadRequest("Missing parameters.");
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/staff/monthly-attendance")]
        public IHttpActionResult Post_Staff_Monthly_Attendance([FromBody] StaffAttendanceParam value)
        {
            StaffDataModel data = new StaffDataModel();
            if (value.school_id != 0 && value.staff_id != 0 && value.entry_month != null)
            {
                List<StaffAttendance> list = new List<StaffAttendance>();
                StaffAttendanceModel listData = new StaffAttendanceModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_staff_attendance_report_monthly", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_staff_id", value.staff_id);
                            cmd.Parameters.AddWithValue("@p_entry_month", value.entry_month);
                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    StaffAttendance prop = new StaffAttendance();
                                    prop.report_id = dataReader["report_id"] as int? ?? default(int);
                                    prop.entry_date = dataReader["entry_date"] as DateTime? ?? default(DateTime);
                                    prop.touch_in_at = dataReader["touch_in_at"] as DateTime? ?? default(DateTime);
                                    prop.touch_out_at = dataReader["touch_out_at"] as DateTime? ?? default(DateTime);
                                    prop.attendance_id = dataReader["attendance_id"] as int? ?? default(int);
                                    prop.attendance = dataReader["attendance"].ToString();
                                    prop.attendance_bm = dataReader["attendance_bm"].ToString();
                                    prop.reason_id = dataReader["reason_id"] as int? ?? default(int);
                                    prop.reason_for_absent = dataReader["reason_for_absent"].ToString();
                                    prop.reason_for_absent_bm = dataReader["reason_for_absent_bm"].ToString();
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "staff_monthly_attendance_list";
                                listData.Message = "Staff monthly attendance report list.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Staff monthly attendance report list could not be found.";
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
                        ExceptionUtility.LogException(ex, "staff/monthly-attendance");
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
        [Route("api/v2/staff/update-staff-attendance")]
        public IHttpActionResult Post_Staff_Update_Staff_Attendance(string culture, [FromBody] UpdateStaffAttendanceParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            bool proceed = false;
            StaffDataModel data = new StaffDataModel();
            if (value.report_id != 0 && value.staff_id != 0 && value.school_id != 0 && value.attendance_id != 0 && value.update_by != null)
            {
                if (value.attendance_id == 1)
                {
                    if (value.report_id != 0)
                    {
                        proceed = true;
                    }
                    else
                    {
                        proceed = false;
                    }
                }
                else
                {
                    proceed = true;
                }

                if (proceed == true)
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

                            using (MySqlCommand cmd = new MySqlCommand("sp_update_staff_attendance_report", conn))
                            {

                                cmd.Transaction = trans;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@p_report_id", value.report_id);
                                cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                                cmd.Parameters.AddWithValue("@p_staff_id", value.staff_id);
                                cmd.Parameters.AddWithValue("@p_attendance_id", value.attendance_id);
                                cmd.Parameters.AddWithValue("@p_reason_id", value.reason_id);
                                cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                                cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                                cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                                cmd.ExecuteNonQuery();
                                trans.Commit();

                                var status_code = cmd.Parameters["@p_status_code"].Value.ToString();

                                if (status_code == "update_success")
                                {
                                    data.Success = true;
                                    data.Code = "record_update";
                                    data.Message = WebApiResources.StaffAttUpdateSuccess;
                                }
                                else
                                {
                                    data.Success = false;
                                    data.Code = "no_record_found";
                                    data.Message = WebApiResources.StaffAttNotFound;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            data.Success = false;
                            data.Code = "error_occured";
                            data.Message = WebApiResources.ErrorOccured;
                            ExceptionUtility.LogException(ex, "staff/update-staff-attendance");
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
            else
            {
                return BadRequest("Missing parameters.");
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/staff/daily-attendance-summary")]
        public IHttpActionResult Post_Staff_Daily_Attendance_Summary([FromBody] StaffDailyAttendanceParam value)
        {
            StaffDataModel data = new StaffDataModel();
            if (value.school_id != 0 && value.entry_date != null)
            {
                List<StaffDailyAttendanceSummary> list = new List<StaffDailyAttendanceSummary>();
                StaffDailyAttendanceSummaryModel listData = new StaffDailyAttendanceSummaryModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_staff_attendance_report_daily_summary", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_entry_date", value.entry_date);
                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    StaffDailyAttendanceSummary prop = new StaffDailyAttendanceSummary();
                                    prop.total_absent = Convert.ToInt32(dataReader["total_absent"]);
                                    prop.total_present = Convert.ToInt32(dataReader["total_present"]);
                                    prop.total_late_in = Convert.ToInt32(dataReader["total_late_in"]);
                                    prop.total_half_day = Convert.ToInt32(dataReader["total_half_day"]);
                                    prop.total_attendance = Convert.ToInt32(dataReader["total_attendance"]);
                                    prop.total_staff = Convert.ToInt32(dataReader["total_staff"]);
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "staff_daily_attendance_summary";
                                listData.Message = "Staff daily attendance report summary.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Staff daily attendance report summary could not be found.";
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
                        ExceptionUtility.LogException(ex, "staff/daily-attendance-summary");
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
        [Route("api/v2/staff/daily-attendance")]
        public IHttpActionResult Post_Staff_Daily_Attendance([FromBody] StaffDailyAttendanceParam2 value)
        {
            StudentDataModel data = new StudentDataModel();
            if (value.school_id != 0 && value.staff_id != 0 && value.entry_date != null)
            {
                List<StaffMonthlyAttendance> list = new List<StaffMonthlyAttendance>();
                StaffMonthlyAttendanceModel listData = new StaffMonthlyAttendanceModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_staff_attendance_report_daily_single", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_staff_id", value.staff_id);
                            cmd.Parameters.AddWithValue("@p_entry_date", value.entry_date);
                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    StaffMonthlyAttendance prop = new StaffMonthlyAttendance();
                                    prop.report_id = dataReader["report_id"] as int? ?? default(int);
                                    prop.staff_id = dataReader["staff_id"] as int? ?? default(int);
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.photo_url = dataReader["photo_url"].ToString();
                                    prop.entry_date = dataReader["entry_date"] as DateTime? ?? default(DateTime);
                                    prop.touch_in_at = dataReader["touch_in_at"] as DateTime? ?? default(DateTime);
                                    prop.touch_out_at = dataReader["touch_out_at"] as DateTime? ?? default(DateTime);
                                    prop.attendance_id = dataReader["attendance_id"] as int? ?? default(int);
                                    prop.attendance = dataReader["attendance"].ToString();
                                    prop.attendance_bm = dataReader["attendance_bm"].ToString();
                                    if (dataReader["reason_id"] is DBNull)
                                    {
                                        prop.reason_id = 0;
                                    }
                                    else
                                    {
                                        prop.reason_id = Convert.ToInt32(dataReader["reason_id"]);
                                    }
                                    prop.reason_for_absent = dataReader["reason_for_absent"].ToString();
                                    prop.reason_for_absent_bm = dataReader["reason_for_absent_bm"].ToString();
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "staff_daily_attendance";
                                listData.Message = "Staff daily attendance report list.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Staff daily attendance report list could not be found.";
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
                        ExceptionUtility.LogException(ex, "staff/daily-attendance");
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
        [Route("api/v2/staff/outing-request-month-group")]
        public IHttpActionResult Post_Staff_Outing_Request_Month_Group([FromBody] StaffOutingParam value)
        {
            StaffDataModel data = new StaffDataModel();
            if (value.school_id != 0 && value.outing_status_id != 0)
            {
                List<StudentOutingGroup> list = new List<StudentOutingGroup>();
                StudentOutingGroupModel listData = new StudentOutingGroupModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_staff_outing_month_group", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_outing_status_id", value.outing_status_id);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    StudentOutingGroup prop = new StudentOutingGroup();
                                    prop.outing_month = Convert.ToDateTime(dataReader["check_out_date"]);
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "outing_application_list";
                                listData.Message = "Outing application list.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Outing application list could not be found.";
                                listData.Data = list;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "staff/outing-request-group-month");
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
        [Route("api/v2/staff/outing-request-month")]
        public IHttpActionResult Post_Staff_Outing_Request_Month([FromBody] StaffOutingParam value)
        {
            StudentDataModel data = new StudentDataModel();
            if (value.outing_status_id != 0 && value.school_id != 0 && value.outing_month != null)
            {
                List<StaffOutingMonth> list = new List<StaffOutingMonth>();
                StaffOutingMonthModel listData = new StaffOutingMonthModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_staff_outing_month", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_outing_status_id", value.outing_status_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_outing_month", value.outing_month);
                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    StaffOutingMonth prop = new StaffOutingMonth();
                                    prop.school_id = dataReader["school_id"] as int? ?? default(int);
                                    prop.outing_type_id = dataReader["outing_type_id"] as int? ?? default(int);
                                    prop.outing_type = dataReader["outing_type"].ToString();
                                    prop.outing_status_id = dataReader["outing_status_id"] as int? ?? default(int);
                                    prop.outing_status = dataReader["outing_status"].ToString();
                                    prop.outing_month = Convert.ToDateTime(dataReader["check_out_date"]);
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "outing_application_list";
                                listData.Message = "Outing application list.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Outing application list could not be found.";
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
                        ExceptionUtility.LogException(ex, "staff/outing-request-month");
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
        [Route("api/v2/staff/outing-request")]
        public IHttpActionResult Post_Staff_Outing_Request([FromBody] StaffOutingParam value)
        {
            StudentDataModel data = new StudentDataModel();
            if (value.outing_status_id != 0 && value.outing_type_id != 0 && value.school_id != 0 && value.outing_month != null)
            {
                List<StudentOuting> list = new List<StudentOuting>();
                StudentOutingModel listData = new StudentOutingModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_staff_outing_application", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_outing_status_id", value.outing_status_id);
                            cmd.Parameters.AddWithValue("@p_outing_type_id", value.outing_type_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_outing_month", value.outing_month);
                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    StudentOuting prop = new StudentOuting();
                                    prop.outing_id = dataReader["outing_id"] as int? ?? default(int);
                                    prop.student_id = dataReader["student_id"] as int? ?? default(int);
                                    prop.profile_id = dataReader["profile_id"] as int? ?? default(int);
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.photo_url = dataReader["photo_url"].ToString();
                                    prop.school_id = dataReader["school_id"] as int? ?? default(int);
                                    prop.school_name = dataReader["school_name"].ToString();
                                    prop.outing_type_id = dataReader["outing_type_id"] as int? ?? default(int);
                                    prop.outing_type = dataReader["outing_type"].ToString();
                                    prop.check_out_date = dataReader["check_out_date"] as DateTime? ?? default(DateTime);
                                    prop.check_in_date = dataReader["check_in_date"] as DateTime? ?? default(DateTime);
                                    prop.outing_reason = dataReader["outing_reason"].ToString();
                                    prop.outing_status_id = dataReader["outing_status_id"] as int? ?? default(int);
                                    prop.outing_status = dataReader["outing_status"].ToString();
                                    prop.request_by_id = dataReader["request_by_id"] as int? ?? default(int);
                                    prop.request_by = dataReader["request_by"].ToString();
                                    prop.request_by_user_role_id = dataReader["request_by_user_role_id"] as int? ?? default(int);
                                    prop.request_by_user_role = dataReader["request_by_user_role"].ToString();
                                    prop.approve_by_id = dataReader["approve_by_id"] as int? ?? default(int);
                                    prop.approve_by = dataReader["approve_by"].ToString();
                                    prop.approve_at = dataReader["approve_at"] as DateTime? ?? default(DateTime);
                                    prop.approve_comment = dataReader["approve_comment"].ToString();
                                    prop.check_out_at = dataReader["check_out_at"] as DateTime? ?? default(DateTime);
                                    prop.check_in_at = dataReader["check_in_at"] as DateTime? ?? default(DateTime);
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "outing_application_list";
                                listData.Message = "Outing application list.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Outing application list could not be found.";
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
                        ExceptionUtility.LogException(ex, "staff/outing-request");
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
        [Route("api/v2/staff/approve-outing-request")]
        public IHttpActionResult Post_Staff_Approve_Request_Outing([FromBody] StaffUpdateOutingParam value)
        {
            DateTime dt = DateTime.MinValue;
            StudentDataModel data = new StudentDataModel();
            if (value.outing_id != 0 && value.student_id != 0 && value.school_id != 0 && value.approve_by_id != 0 && value.update_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_update_student_outing_request_approve", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_outing_id", value.outing_id);
                            cmd.Parameters.AddWithValue("@p_student_id", value.student_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_approve_by_id", value.approve_by_id);
                            cmd.Parameters.AddWithValue("@p_approve_comment", value.approve_comment);
                            cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                            cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                            cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            var status_code = cmd.Parameters["@p_status_code"].Value.ToString();

                            if (status_code == "record_updated")
                            {
                                data.Success = true;
                                data.Code = "record_updated";
                                data.Message = "Outing request has been approved.";
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "no_record_found";
                                data.Message = "Outing request could not be found.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "staff/approve-outing-request");
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
        [Route("api/v2/staff/reject-outing-request")]
        public IHttpActionResult Post_Staff_Reject_Request_Outing([FromBody] StaffUpdateOutingParam value)
        {
            DateTime dt = DateTime.MinValue;
            StudentDataModel data = new StudentDataModel();
            if (value.outing_id != 0 && value.student_id != 0 && value.school_id != 0 && value.approve_by_id != 0 && value.approve_comment != null && value.update_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_update_student_outing_request_reject", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_outing_id", value.outing_id);
                            cmd.Parameters.AddWithValue("@p_student_id", value.student_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_approve_by_id", value.approve_by_id);
                            cmd.Parameters.AddWithValue("@p_approve_comment", value.approve_comment);
                            cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                            cmd.Parameters.Add("@p_status_code", MySqlDbType.VarChar);
                            cmd.Parameters["@p_status_code"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            var status_code = cmd.Parameters["@p_status_code"].Value.ToString();

                            if (status_code == "record_updated")
                            {
                                data.Success = true;
                                data.Code = "record_updated";
                                data.Message = "Outing request has been rejected.";
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "no_record_found";
                                data.Message = "Outing request could not be found.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "staff/reject-outing-request");
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
