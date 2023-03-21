using I3SwebAPIv2.Class;
using I3SwebAPIv2.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Http.Cors;
using System.Web.Http;
using I3SwebAPIv2.Resources;

namespace I3SwebAPIv2.Controllers
{
    public class SchoolsV2Controller : ApiController
    {
        [HttpPost]
        [Authorize]
        [Route("api/v2/school/session")]
        public IHttpActionResult Post_School_Session([FromBody]SessionParam value)
        {
            SchoolDataModel data = new SchoolDataModel();
            if (value.school_id != 0)
            {
                List<Session> list = new List<Session>();
                SessionModel listData = new SessionModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_school_session", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@SchoolId", value.school_id);
                            cmd.Parameters.AddWithValue("@SessionId", 0);

                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    Session prop = new Session();
                                    prop.session_id = dataReader["session_id"] as int? ??default(int);
                                    prop.session_code = dataReader["session_code"].ToString();
                                    prop.start_time = dataReader["start_time"] as TimeSpan? ??default(TimeSpan);
                                    prop.late_in_time = dataReader["late_in_time"] as TimeSpan? ?? default(TimeSpan);
                                    prop.recess_end_time = dataReader["recess_end_time"] as TimeSpan? ?? default(TimeSpan);
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "select_session";
                                listData.Message = "Please select session.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "School session could not be found.";
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
                        ExceptionUtility.LogException(ex, "school/session");
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
        [Route("api/v2/school/class")]
        public IHttpActionResult Post_School_Class([FromBody]ClassParam value)
        {
            SchoolDataModel data = new SchoolDataModel();
            if (value.school_id != 0)
            {
                List<Classes> list = new List<Classes>();
                ClassesModel listData = new ClassesModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_school_class", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@SchoolId", value.school_id);
                            cmd.Parameters.AddWithValue("@ClassId", 0);

                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    Classes prop = new Classes();
                                    prop.class_id = Convert.ToInt16(dataReader["class_id"]);
                                    prop.class_name = dataReader["class_name"].ToString();
                                    prop.school_name = dataReader["school_name"].ToString();
                                    prop.session_code = dataReader["session_code"].ToString();
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "select_class";
                                listData.Message = "Please select class.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "School classes could not be found.";
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
                        ExceptionUtility.LogException(ex, "school/class");
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
        [Route("api/v2/school/merchant")]
        public IHttpActionResult Post_School_Merchant([FromBody] SchoolMerchantParam value)
        {
            MerchantDataModel data = new MerchantDataModel();
            if (value.school_id != 0)
            {
                List<Merchant> list = new List<Merchant>();
                MerchantModel listData = new MerchantModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_school_merchant", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    Merchant prop = new Merchant();
                                    prop.merchant_id = Convert.ToInt16(dataReader["merchant_id"]);
                                    prop.company_name = dataReader["company_name"].ToString();
                                    prop.merchant_type_id = Convert.ToInt16(dataReader["merchant_type_id"]);
                                    prop.merchant_type = dataReader["merchant_type"].ToString();
                                    prop.full_name = dataReader["full_name"].ToString();
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
                                listData.Message = "School merchant could not be found.";
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
                        ExceptionUtility.LogException(ex, "school/merchant");
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
        [Route("api/v2/school/info")]
        public IHttpActionResult Post_School_Info([FromBody]SchoolInfoParam value)
        {
            //SchoolInfoModel data = new SchoolInfoModel();
            if (value.school_id != 0)
            {
                List<SchoolInfo> list = new List<SchoolInfo>();
                SchoolInfoModel listData = new SchoolInfoModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_school", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@SchoolId", value.school_id);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    SchoolInfo prop = new SchoolInfo();
                                    prop.school_id = dataReader["school_id"] as int? ?? default(int);
                                    prop.school_name = dataReader["school_name"].ToString();
                                    prop.school_code = dataReader["school_code"].ToString();
                                    prop.school_type_id = dataReader["school_type_id"] as int? ?? default(int);
                                    prop.school_type = dataReader["school_type"].ToString();
                                    prop.school_website = dataReader["school_website"].ToString();
                                    prop.school_result_url = dataReader["school_result_url"].ToString();
                                    prop.email = dataReader["email"].ToString();
                                    prop.city = dataReader["city"].ToString();
                                    prop.state_name = dataReader["state_name"].ToString();
                                    prop.total_staff = Convert.ToInt16(dataReader["total_staff"]);

                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "select_school";
                                listData.Message = "Please select school.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "School could not be found.";
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
                        ExceptionUtility.LogException(ex, "school");
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

        [HttpPost]
        [Authorize]
        [Route("api/v2/school/staff")]
        public IHttpActionResult Post_School_Staff([FromUri]SqlFilterParam uri, [FromBody]SchoolStaffParam value)
        {
            SchoolDataModel data = new SchoolDataModel();
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
            if (value.school_id != 0)
            {
                List<SchoolStaff> list = new List<SchoolStaff>();
                SchoolStaffModel listData = new SchoolStaffModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_school_staff", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_limit", limit);
                            cmd.Parameters.AddWithValue("@p_offset", offset);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);

                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    SchoolStaff prop = new SchoolStaff();
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
                                    prop.staff_type_bm = dataReader["staff_type_bm"].ToString();
                                    prop.shift_id = dataReader["shift_id"] as int? ?? default(int);
                                    prop.shift_code = dataReader["shift_code"].ToString();
                                    prop.wallet_id = dataReader["wallet_id"] as int? ?? default(int);
                                    prop.wallet_number = dataReader["wallet_number"].ToString();
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "school_staff_list";
                                listData.Message = "School staff list.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "School staff could not be found.";
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
                        ExceptionUtility.LogException(ex, "school/staff");
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
        [Route("api/v2/school/class-student")]
        public IHttpActionResult Post_School_Class_Student([FromUri]SqlFilterParam uri, [FromBody]ClassStudentParam value)
        {
            SchoolDataModel data = new SchoolDataModel();
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
            if (value.school_id != 0 && value.class_id != 0)
            {
                List<ClassStudent> list = new List<ClassStudent>();
                ClassStudentModel listData = new ClassStudentModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_school_class_student", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_limit", limit);
                            cmd.Parameters.AddWithValue("@p_offset", offset);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_class_id", value.class_id);

                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    ClassStudent prop = new ClassStudent();
                                    prop.student_id = Convert.ToInt16(dataReader["student_id"]);
                                    prop.profile_id = Convert.ToInt16(dataReader["profile_id"]);
                                    prop.student_number = dataReader["student_number"].ToString();
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.photo_url = dataReader["photo_url"].ToString();
                                    prop.school_id = Convert.ToInt16(dataReader["school_id"]);
                                    prop.school_name = dataReader["school_name"].ToString();
                                    prop.class_id = Convert.ToInt16(dataReader["class_id"]);
                                    prop.class_name = dataReader["class_name"].ToString();
                                    prop.status_code = dataReader["status_code"].ToString();
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "class_student_list";
                                listData.Message = "School class student list.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "School class student could not be found.";
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
                        ExceptionUtility.LogException(ex, "school/class-student");
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

                //if (listData.Success == true)
                //    return Ok(listData);

                return Ok(listData);
            }
            else
            {
                return BadRequest("Missing Parameters.");
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/school/club")]
        public IHttpActionResult Post_School_Club([FromBody]ClubsParam value)
        {
            SchoolDataModel data = new SchoolDataModel();
            if (value.school_id != null && value.create_by_id != 0)
            {
                List<Clubs> list = new List<Clubs>();
                ClubsModel listData = new ClubsModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_create_school_club_temp", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                            using (MySqlCommand cmd2 = new MySqlCommand("sp_insert_school_club_temp", conn))
                            {
                                //trans2 = conn.BeginTransaction();

                                foreach (int sid in value.school_id)
                                {
                                    cmd2.Transaction = trans;
                                    cmd2.CommandType = CommandType.StoredProcedure;
                                    cmd2.Parameters.Clear();
                                    cmd2.Parameters.AddWithValue("@p_school_id", sid);
                                    cmd2.Parameters.AddWithValue("@p_create_by_id", value.create_by_id);
                                    cmd2.ExecuteNonQuery();
                                    cmd2.Dispose();
                                }

                                using (MySqlCommand cmd3 = new MySqlCommand("sp_get_school_club_temp", conn))
                                {

                                    cmd3.Transaction = trans;
                                    cmd3.CommandType = CommandType.StoredProcedure;
                                    MySqlDataReader dataReader = cmd3.ExecuteReader();

                                    if (dataReader.HasRows == true)
                                    {
                                        while (dataReader.Read())
                                        {
                                            Clubs prop = new Clubs();
                                            prop.club_id = Convert.ToInt16(dataReader["club_id"]);
                                            prop.club_name = dataReader["club_name"].ToString();
                                            prop.school_name = dataReader["school_name"].ToString();
                                            prop.staff_id = Convert.ToInt16(dataReader["staff_id"]);
                                            prop.full_name = dataReader["full_name"].ToString();
                                            prop.total_member = Convert.ToInt32(dataReader["total_member"]);
                                            list.Add(prop);
                                        }
                                        listData.Success = true;
                                        listData.Code = "select_club";
                                        listData.Message = "Please select club.";
                                        listData.Data = list;
                                    }
                                    else
                                    {
                                        //listData.Success = false;
                                        listData.Success = true;
                                        listData.Code = "no_record_found";
                                        listData.Message = "School clubs could not be found.";
                                        listData.Data = list;
                                    }

                                    dataReader.Close();
                                    cmd3.Dispose();

                                    using (MySqlCommand cmd4 = new MySqlCommand("sp_drop_school_club_temp", conn))
                                    {
                                        cmd4.Transaction = trans;
                                        cmd4.CommandType = CommandType.StoredProcedure;
                                        cmd4.ExecuteNonQuery();
                                        cmd4.Dispose();
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //listData.Success = false;
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "school/clubs");
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
        [Route("api/v2/school/club-member")]
        public IHttpActionResult Post_School_Club_Member([FromUri]SqlFilterParam uri, [FromBody]ClubMemberParam value)
        {
            SchoolDataModel data = new SchoolDataModel();
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
            if (value.school_id != 0 && value.club_id != 0)
            {
                List<ClubMember> list = new List<ClubMember>();
                ClubMemberModel listData = new ClubMemberModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_school_club_member", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_limit", limit);
                            cmd.Parameters.AddWithValue("@p_offset", offset);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_club_id", value.club_id);

                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    ClubMember prop = new ClubMember();
                                    prop.relationship_id = Convert.ToInt16(dataReader["relationship_id"]);
                                    prop.profile_id = Convert.ToInt16(dataReader["profile_id"]);
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.photo_url = dataReader["photo_url"].ToString();
                                    prop.user_role_id = Convert.ToInt16(dataReader["user_role_id"]);
                                    prop.user_role = dataReader["user_role"].ToString();
                                    prop.status_code = dataReader["status_code"].ToString();
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "club_member_list";
                                listData.Message = "School club member list.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "School club member could not be found.";
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
                        ExceptionUtility.LogException(ex, "school/club-member");
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

                //if (listData.Success == true)
                //    return Ok(listData);

                return Ok(listData);
            }
            else
            {
                return BadRequest("Missing Parameters.");
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/school/post")]
        public IHttpActionResult Post_School_Post([FromUri]SqlFilterParam uri, [FromBody]SchoolPostRequest body)
        {
            SchoolDataModel data = new SchoolDataModel();
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
            if (body.school_id != null)
            {
                List<SchoolPost> list = new List<SchoolPost>();
                SchoolPostModel listData = new SchoolPostModel();
                MySqlTransaction trans = null;
                //MySqlTransaction trans2 = null;
                //MySqlTransaction trans3 = null;
                listData.Success = false;
                string sqlQuery = string.Empty;
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd = new MySqlCommand("sp_create_school_post_temp", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();


                            using (MySqlCommand cmd2 = new MySqlCommand("sp_insert_school_post_temp", conn))
                            {
                                //trans2 = conn.BeginTransaction();

                                foreach (int sid in body.school_id)
                                {
                                    cmd2.Transaction = trans;
                                    cmd2.CommandType = CommandType.StoredProcedure;
                                    cmd2.Parameters.Clear();
                                    cmd2.Parameters.AddWithValue("@p_school_id", sid);
                                    cmd2.ExecuteNonQuery();
                                    cmd2.Dispose();
                                }

                                //trans3 = conn.BeginTransaction();

                                using (MySqlCommand cmd3 = new MySqlCommand("sp_get_school_post_temp", conn))
                                {
                                    cmd3.Transaction = trans;
                                    cmd3.CommandType = CommandType.StoredProcedure;
                                    cmd3.Parameters.Clear();
                                    cmd3.Parameters.AddWithValue("@p_limit", limit);
                                    cmd3.Parameters.AddWithValue("@p_offset", offset);
                                    MySqlDataReader dataReader = cmd3.ExecuteReader();

                                    if (dataReader.HasRows == true)
                                    {
                                        while (dataReader.Read())
                                        {
                                            SchoolPost prop = new SchoolPost();
                                            prop.post_id = dataReader["post_id"] as int? ?? default(int);
                                            prop.school_name = dataReader["school_name"].ToString();
                                            prop.school_type = dataReader["school_type"].ToString();
                                            prop.post_message = dataReader["post_message"].ToString();
                                            prop.date_from = Convert.ToDateTime(dataReader["start_at"]).ToString("yyyy-MM-dd");
                                            prop.date_to = Convert.ToDateTime(dataReader["end_at"]).ToString("yyyy-MM-dd");
                                            prop.post_photo_url = dataReader["post_photo_url"].ToString();
                                            prop.create_by = dataReader["create_by"].ToString();
                                            prop.create_by_photo_url = dataReader["create_by_photo_url"].ToString();
                                            prop.create_at = dataReader["create_at"] as DateTime? ?? default(DateTime);
                                            list.Add(prop);
                                        }
                                        listData.Success = true;
                                        listData.Code = "school_post";
                                        listData.Message = "School post listing.";
                                        listData.Data = list;

                                    }
                                    else
                                    {
                                        //listData.Success = false;
                                        listData.Success = true;
                                        listData.Code = "no_record_found";
                                        listData.Message = "No record found.";
                                        listData.Data = list;
                                    }

                                    dataReader.Close();
                                    cmd3.Dispose();

                                    using (MySqlCommand cmd4 = new MySqlCommand("sp_drop_school_post_temp", conn))
                                    {
                                        cmd4.Transaction = trans;
                                        cmd4.CommandType = CommandType.StoredProcedure;
                                        cmd4.ExecuteNonQuery();
                                        cmd4.Dispose();
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //listData.Success = false;
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "school/post");
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
        [Route("api/v2/school/class-post")]
        public IHttpActionResult Post_School_Class_Post([FromUri]SqlFilterParam uri, [FromBody]SchoolClassPostRequest body)
        {
            SchoolDataModel data = new SchoolDataModel();
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
            if (body.school_id != 0 && body.class_id != 0)
            {
                List<SchoolClassPost> list = new List<SchoolClassPost>();
                SchoolClassPostModel listData = new SchoolClassPostModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_school_class_post", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_limit", limit);
                            cmd.Parameters.AddWithValue("@p_offset", offset);
                            cmd.Parameters.AddWithValue("@p_school_id", body.school_id);
                            cmd.Parameters.AddWithValue("@p_class_id", body.class_id);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    SchoolClassPost prop = new SchoolClassPost();
                                    prop.post_id = dataReader["post_id"] as int? ?? default(int);
                                    prop.school_name = dataReader["school_name"].ToString();
                                    prop.school_type = dataReader["school_type"].ToString();
                                    prop.class_name = dataReader["class_name"].ToString();
                                    prop.post_message = dataReader["post_message"].ToString();
                                    prop.date_from = Convert.ToDateTime(dataReader["start_at"]).ToString("yyyy-MM-dd");
                                    prop.date_to = Convert.ToDateTime(dataReader["end_at"]).ToString("yyyy-MM-dd");
                                    prop.post_photo_url = dataReader["post_photo_url"].ToString();
                                    prop.create_by = dataReader["create_by"].ToString();
                                    prop.create_by_photo_url = dataReader["create_by_photo_url"].ToString();
                                    prop.create_at = dataReader["create_at"] as DateTime? ?? default(DateTime);
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "school_class_post";
                                listData.Message = "School class post listing.";
                                listData.Data = list;

                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
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
                        ExceptionUtility.LogException(ex, "school/class-post");
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
        [Route("api/v2/school/club-post")]
        public IHttpActionResult Post_School_Club_Post([FromUri]SqlFilterParam uri, [FromBody]SchoolClubPostRequest body)
        {
            SchoolDataModel data = new SchoolDataModel();
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
            if (body.school_id != 0 && body.club_id != 0)
            {
                List<SchoolClubPost> list = new List<SchoolClubPost>();
                SchoolClubPostModel listData = new SchoolClubPostModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_school_club_post", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_limit", limit);
                            cmd.Parameters.AddWithValue("@p_offset", offset);
                            cmd.Parameters.AddWithValue("@p_school_id", body.school_id);
                            cmd.Parameters.AddWithValue("@p_club_id", body.club_id);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    SchoolClubPost prop = new SchoolClubPost();
                                    prop.post_id = dataReader["post_id"] as int? ?? default(int);
                                    prop.school_name = dataReader["school_name"].ToString();
                                    prop.school_type = dataReader["school_type"].ToString();
                                    prop.club_name = dataReader["club_name"].ToString();
                                    prop.post_message = dataReader["post_message"].ToString();
                                    prop.date_from = Convert.ToDateTime(dataReader["start_at"]).ToString("yyyy-MM-dd");
                                    prop.date_to = Convert.ToDateTime(dataReader["end_at"]).ToString("yyyy-MM-dd");
                                    prop.post_photo_url = dataReader["post_photo_url"].ToString();
                                    prop.create_by = dataReader["create_by"].ToString();
                                    prop.create_by_photo_url = dataReader["create_by_photo_url"].ToString();
                                    prop.create_at = dataReader["create_at"] as DateTime? ?? default(DateTime);
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "school_club_post";
                                listData.Message = "School club post listing.";
                                listData.Data = list;

                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
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
                        ExceptionUtility.LogException(ex, "school/club-post");
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
        [Route("api/v2/school/class-daily-attendance-summary")]
        public IHttpActionResult Post_School_Class_Daily_Attendance_Summary([FromBody]SchoolClassDailyAttendanceParam value)
        {
            StaffDataModel data = new StaffDataModel();
            if (value.school_id != 0 && value.class_id != 0 &&  value.entry_date != null)
            {
                List<SchoolClassDailyAttendanceSummary> list = new List<SchoolClassDailyAttendanceSummary>();
                SchoolClassDailyAttendanceSummaryModel listData = new SchoolClassDailyAttendanceSummaryModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_class_attendance_report_daily_summary", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_class_id", value.class_id);
                            cmd.Parameters.AddWithValue("@p_entry_date", value.entry_date);
                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    SchoolClassDailyAttendanceSummary prop = new SchoolClassDailyAttendanceSummary();
                                    prop.total_absent = Convert.ToInt16(dataReader["total_absent"]);
                                    prop.total_present = Convert.ToInt16(dataReader["total_present"]);
                                    prop.total_late_in = Convert.ToInt16(dataReader["total_late_in"]);
                                    prop.total_half_day = Convert.ToInt16(dataReader["total_half_day"]);
                                    prop.total_attendance = Convert.ToInt16(dataReader["total_attendance"]);
                                    prop.total_student = Convert.ToInt16(dataReader["total_student"]);
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "school_class_daily_attendance_summary";
                                listData.Message = "School class daily attendance report summary.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "School class daily attendance report summary could not be found.";
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
                        ExceptionUtility.LogException(ex, "school/class-daily-attendance-summary");
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
        [Route("api/v2/school/class-daily-attendance-percentage")]
        public IHttpActionResult Post_School_Class_Daily_Attendance_Percentage([FromBody] SchoolClassDailyAttendanceParam value)
        {
            StaffDataModel data = new StaffDataModel();
            if (value.school_id != 0 && value.class_id != 0 && value.entry_date != null)
            {
                List<SchoolClassDailyAttendancePercentage> list = new List<SchoolClassDailyAttendancePercentage>();
                SchoolClassDailyAttendancePercentageModel listData = new SchoolClassDailyAttendancePercentageModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_class_attendance_report_daily_percentage", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_class_id", value.class_id);
                            cmd.Parameters.AddWithValue("@p_entry_date", value.entry_date);
                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    SchoolClassDailyAttendancePercentage prop = new SchoolClassDailyAttendancePercentage();
                                    prop.school_id = Convert.ToInt16(dataReader["school_id"]);
                                    prop.class_id = Convert.ToInt16(dataReader["class_id"]);
                                    prop.entry_date = dataReader["entry_date"] as DateTime? ?? default(DateTime);
                                    prop.total_attendance = Convert.ToInt16(dataReader["total_attendance"]);
                                    prop.total_student = Convert.ToInt16(dataReader["total_student"]);
                                    prop.total_percentage = Convert.ToDecimal(dataReader["total_percentage"]);
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "school_class_daily_attendance_percentage";
                                listData.Message = "School class daily attendance report percentage.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "School class daily attendance report percentage could not be found.";
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
                        ExceptionUtility.LogException(ex, "school/class-daily-attendance-percentage");
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
        [Route("api/v2/school/club-daily-attendance-percentage")]
        public IHttpActionResult Post_School_Club_Daily_Attendance_Percentage([FromBody] SchoolClubDailyAttendanceParam value)
        {
            StaffDataModel data = new StaffDataModel();
            if (value.school_id != 0 && value.club_id != 0 && value.entry_date != null)
            {
                List<SchoolClubDailyAttendancePercentage> list = new List<SchoolClubDailyAttendancePercentage>();
                SchoolClubDailyAttendancePercentageModel listData = new SchoolClubDailyAttendancePercentageModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_club_attendance_report_daily_percentage", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_club_id", value.club_id);
                            cmd.Parameters.AddWithValue("@p_entry_date", value.entry_date);
                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    SchoolClubDailyAttendancePercentage prop = new SchoolClubDailyAttendancePercentage();
                                    prop.school_id = Convert.ToInt16(dataReader["school_id"]);
                                    prop.club_id = Convert.ToInt16(dataReader["club_id"]);
                                    prop.entry_date = dataReader["entry_date"] as DateTime? ?? default(DateTime);
                                    prop.total_attendance = Convert.ToInt16(dataReader["total_attendance"]);
                                    prop.total_member = Convert.ToInt16(dataReader["total_member"]);
                                    prop.total_percentage = Convert.ToDecimal(dataReader["total_percentage"]);
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "school_club_daily_attendance_percentage";
                                listData.Message = "School club daily attendance report percentage.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "School club daily attendance report percentage could not be found.";
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
                        ExceptionUtility.LogException(ex, "school/club-daily-attendance-percentage");
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
        [Route("api/v2/school/club-daily-attendance-summary")]
        public IHttpActionResult Post_School_Club_Daily_Attendance_Summary([FromBody] SchoolClubDailyAttendanceParam value)
        {
            StaffDataModel data = new StaffDataModel();
            if (value.school_id != 0 && value.club_id != 0 && value.entry_date != null)
            {
                List<SchoolClubDailyAttendanceSummary> list = new List<SchoolClubDailyAttendanceSummary>();
                SchoolClubDailyAttendanceSummaryModel listData = new SchoolClubDailyAttendanceSummaryModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_club_attendance_report_daily_summary", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_club_id", value.club_id);
                            cmd.Parameters.AddWithValue("@p_entry_date", value.entry_date);
                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    SchoolClubDailyAttendanceSummary prop = new SchoolClubDailyAttendanceSummary();
                                    prop.total_absent = Convert.ToInt16(dataReader["total_absent"]);
                                    prop.total_present = Convert.ToInt16(dataReader["total_present"]);
                                    prop.total_late_in = Convert.ToInt16(dataReader["total_late_in"]);
                                    prop.total_half_day = Convert.ToInt16(dataReader["total_half_day"]);
                                    prop.total_attendance = Convert.ToInt16(dataReader["total_attendance"]);
                                    prop.total_member = Convert.ToInt16(dataReader["total_member"]);
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "school_club_daily_attendance_summary";
                                listData.Message = "School club daily attendance report summary.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "School club daily attendance report summary could not be found.";
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
                        ExceptionUtility.LogException(ex, "school/club-daily-attendance-summary");
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
        [Route("api/v2/school/club-member-monthly-attendance")]
        public IHttpActionResult Post_School_Club_Member_Monthly_Attendance([FromBody] ClubMemberMonthlyAttendanceParam value)
        {
            StudentDataModel data = new StudentDataModel();
            if (value.school_id != 0 && value.club_id != 0 && value.profile_id != 0 && value.entry_month != null)
            {
                List<ClubMemberMonthlyAttendance> list = new List<ClubMemberMonthlyAttendance>();
                ClubMemberMonthlyAttendanceModel listData = new ClubMemberMonthlyAttendanceModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_club_member_attendance_report_monthly", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_club_id", value.club_id);
                            cmd.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                            cmd.Parameters.AddWithValue("@p_entry_month", value.entry_month);
                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    ClubMemberMonthlyAttendance prop = new ClubMemberMonthlyAttendance();
                                    prop.report_id = dataReader["report_id"] as int? ?? default(int);
                                    prop.entry_date = dataReader["entry_date"] as DateTime? ?? default(DateTime);
                                    prop.attendance_id = dataReader["attendance_id"] as int? ?? default(int);
                                    prop.attendance = dataReader["attendance"].ToString();
                                    prop.attendance_bm = dataReader["attendance_bm"].ToString();
                                    prop.reason_id = dataReader["reason_id"] as int? ?? default(int);
                                    prop.reason_for_absent = dataReader["reason_for_absent"].ToString();
                                    prop.reason_for_absent_bm = dataReader["reason_for_absent_bm"].ToString();
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "club_member_monthly_attendance_list";
                                listData.Message = "Club member monthly attendance report list.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Club member monthly attendance report list could not be found.";
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
                        ExceptionUtility.LogException(ex, "school/club-member-monthly-attendance");
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
        [Route("api/v2/school/club-member-daily-attendance")]
        public IHttpActionResult Post_School_Club_Member_Daily_Attendance([FromBody] ClubMemberDailyAttendanceParam value)
        {
            StudentDataModel data = new StudentDataModel();
            if (value.school_id != 0 && value.club_id != 0 && value.profile_id != 0 && value.entry_date != null)
            {
                List<ClubMemberMonthlyAttendance> list = new List<ClubMemberMonthlyAttendance>();
                ClubMemberMonthlyAttendanceModel listData = new ClubMemberMonthlyAttendanceModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_club_member_attendance_report_daily", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_club_id", value.club_id);
                            cmd.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                            cmd.Parameters.AddWithValue("@p_entry_date", value.entry_date);
                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    ClubMemberMonthlyAttendance prop = new ClubMemberMonthlyAttendance();
                                    prop.report_id = dataReader["report_id"] as int? ?? default(int);
                                    prop.entry_date = dataReader["entry_date"] as DateTime? ?? default(DateTime);
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
                                listData.Code = "club_member_daily_attendance";
                                listData.Message = "Club member daily attendance report.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Club member daily attendance report could not be found.";
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
                        ExceptionUtility.LogException(ex, "school/club-member-daily-attendance");
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
        [Route("api/v2/school/staff-daily-attendance-summary")]
        public IHttpActionResult Post_School_Staff_Daily_Attendance_Summary([FromBody]SchoolStaffDailyAttendanceParam value)
        {
            StaffDataModel data = new StaffDataModel();
            if (value.school_id != 0 && value.shift_id != 0 && value.entry_date != null)
            {
                List<SchoolStaffDailyAttendanceSummary> list = new List<SchoolStaffDailyAttendanceSummary>();
                SchoolStaffDailyAttendanceSummaryModel listData = new SchoolStaffDailyAttendanceSummaryModel();
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
                            cmd.Parameters.AddWithValue("@p_shift_id", value.shift_id);
                            cmd.Parameters.AddWithValue("@p_entry_date", value.entry_date);
                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    SchoolStaffDailyAttendanceSummary prop = new SchoolStaffDailyAttendanceSummary();
                                    prop.total_absent = Convert.ToInt16(dataReader["total_absent"]);
                                    prop.total_present = Convert.ToInt16(dataReader["total_present"]);
                                    prop.total_late_in = Convert.ToInt16(dataReader["total_late_in"]);
                                    prop.total_half_day = Convert.ToInt16(dataReader["total_half_day"]);
                                    prop.total_attendance = Convert.ToInt16(dataReader["total_attendance"]);
                                    prop.total_staff = Convert.ToInt16(dataReader["total_staff"]);
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "school_staff_daily_attendance_summary";
                                listData.Message = "School staff daily attendance report summary.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "School staff daily attendance report summary could not be found.";
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
                        ExceptionUtility.LogException(ex, "school/staff-daily-attendance-summary");
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
        [Route("api/v2/school/staff-daily-attendance-percentage")]
        public IHttpActionResult Post_School_Staff_Daily_Attendance_Percentage([FromBody] SchoolStaffDailyAttendanceParam value)
        {
            StaffDataModel data = new StaffDataModel();
            if (value.school_id != 0 && value.shift_id != 0 && value.entry_date != null)
            {
                List<SchoolStaffDailyAttendancePercentage> list = new List<SchoolStaffDailyAttendancePercentage>();
                SchoolStaffDailyAttendancePercentageModel listData = new SchoolStaffDailyAttendancePercentageModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_staff_attendance_report_daily_percentage", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_shift_id", value.shift_id);
                            cmd.Parameters.AddWithValue("@p_entry_date", value.entry_date);
                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    SchoolStaffDailyAttendancePercentage prop = new SchoolStaffDailyAttendancePercentage();
                                    prop.school_id = Convert.ToInt16(dataReader["school_id"]);
                                    prop.entry_date = dataReader["entry_date"] as DateTime? ?? default(DateTime);
                                    prop.total_attendance = Convert.ToInt16(dataReader["total_attendance"]);
                                    prop.total_staff = Convert.ToInt16(dataReader["total_staff"]);
                                    prop.total_percentage = Convert.ToDecimal(dataReader["total_percentage"]);
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "school_staff_daily_attendance_percentage";
                                listData.Message = "School staff daily attendance report percentage.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "School staff daily attendance report percentage could not be found.";
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
                        ExceptionUtility.LogException(ex, "school/staff-daily-attendance-percentage");
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
        [Route("api/v2/school/class-daily-attendance")]
        public IHttpActionResult Post_School_Class_Daily_Attendance([FromBody]SchoolClassDailyAttendanceParam value)
        {
            StudentDataModel data = new StudentDataModel();
            if (value.school_id != 0 && value.class_id != 0 && value.entry_date != null)
            {
                List<SchoolClassDailyAttendance> list = new List<SchoolClassDailyAttendance>();
                SchoolClassDailyAttendanceModel listData = new SchoolClassDailyAttendanceModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_class_attendance_report_daily", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_class_id", value.class_id);
                            cmd.Parameters.AddWithValue("@p_entry_date", value.entry_date);
                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    SchoolClassDailyAttendance prop = new SchoolClassDailyAttendance();
                                    prop.report_id = dataReader["report_id"] as int? ?? default(int);
                                    prop.student_id = dataReader["student_id"] as int? ?? default(int);
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.photo_url = dataReader["photo_url"].ToString();
                                    prop.entry_date = dataReader["entry_date"] as DateTime? ?? default(DateTime);
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
                                listData.Code = "school_class_daily_attendance_list";
                                listData.Message = "School class daily attendance list report.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "School class daily attendance report list could not be found.";
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
                        ExceptionUtility.LogException(ex, "school/class-daily-attendance");
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
        [Route("api/v2/school/club-daily-attendance")]
        public IHttpActionResult Post_School_Club_Daily_Attendance([FromBody] SchoolClubDailyAttendanceParam value)
        {
            StudentDataModel data = new StudentDataModel();
            if (value.school_id != 0 && value.club_id != 0 && value.entry_date != null)
            {
                List<SchoolClubDailyAttendance> list = new List<SchoolClubDailyAttendance>();
                SchoolClubDailyAttendanceModel listData = new SchoolClubDailyAttendanceModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_club_attendance_report_daily", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_club_id", value.club_id);
                            cmd.Parameters.AddWithValue("@p_entry_date", value.entry_date);
                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    SchoolClubDailyAttendance prop = new SchoolClubDailyAttendance();
                                    prop.report_id = dataReader["report_id"] as int? ?? default(int);
                                    prop.profile_id = dataReader["profile_id"] as int? ?? default(int);
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.photo_url = dataReader["photo_url"].ToString();
                                    prop.user_role_id = dataReader["user_role_id"] as int? ?? default(int);
                                    prop.user_role = dataReader["user_role"].ToString();
                                    prop.entry_date = dataReader["entry_date"] as DateTime? ?? default(DateTime);
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
                                listData.Code = "school_club_daily_attendance_list";
                                listData.Message = "School club daily attendance list report.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "School club daily attendance report list could not be found.";
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
                        ExceptionUtility.LogException(ex, "school/club-daily-attendance");
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
        [Route("api/v2/school/staff-daily-attendance")]
        public IHttpActionResult Post_School_Staff_Daily_Attendance([FromBody]SchoolStaffDailyAttendanceParam value)
        {
            StudentDataModel data = new StudentDataModel();
            if (value.school_id != 0 && value.shift_id != 0 && value.entry_date != null)
            {
                List<SchoolStaffDailyAttendance> list = new List<SchoolStaffDailyAttendance>();
                SchoolStaffDailyAttendanceModel listData = new SchoolStaffDailyAttendanceModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_staff_attendance_report_daily", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_shift_id", value.shift_id);
                            cmd.Parameters.AddWithValue("@p_entry_date", value.entry_date);
                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    SchoolStaffDailyAttendance prop = new SchoolStaffDailyAttendance();
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
                                listData.Code = "school_staff_daily_attendance_list";
                                listData.Message = "School staff daily attendance list report.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "School staff daily attendance report list could not be found.";
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
                        ExceptionUtility.LogException(ex, "school/staff-daily-attendance");
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
        [Route("api/v2/school/search")]
        public IHttpActionResult Post_School_Search([FromUri] SqlFilterParam uri, [FromBody]SearchSchoolParam body)
        {
            SchoolDataModel data = new SchoolDataModel();
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
            if (body.state_id != 0 && body.school_name != null)
            {
                List<SearchSchool> list = new List<SearchSchool>();
                SearchSchoolModel listData = new SearchSchoolModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_school_search", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_limit", limit);
                            cmd.Parameters.AddWithValue("@p_offset", offset);
                            cmd.Parameters.AddWithValue("@p_state_id", body.state_id);
                            cmd.Parameters.AddWithValue("@p_school_name", body.school_name);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    SearchSchool prop = new SearchSchool();
                                    prop.school_id = dataReader["school_id"] as int? ?? default(int);
                                    prop.school_name = dataReader["school_name"].ToString();
                                    prop.school_code = dataReader["school_code"].ToString();
                                    prop.school_type = dataReader["school_type"].ToString();
                                    prop.address = dataReader["address"].ToString();
                                    prop.postcode = dataReader["postcode"].ToString();
                                    prop.city = dataReader["city"].ToString();
                                    prop.state_name = dataReader["state_name"].ToString();
                                    prop.country_name = dataReader["country_name"].ToString();
                                    prop.status_code = dataReader["status_code"].ToString();
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "select_school";
                                listData.Message = "Please select school.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "School could not be found.";
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
                        ExceptionUtility.LogException(ex, "school/search");
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
    }
}
