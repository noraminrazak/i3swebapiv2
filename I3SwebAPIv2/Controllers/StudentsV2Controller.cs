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
using System.Web.Http.Cors;

namespace I3SwebAPIv2.Controllers
{
    public class StudentsV2Controller : ApiController
    {
        [HttpPost]
        [Authorize]
        [Route("api/v2/student/profile")]
        public IHttpActionResult Post_Student_Profile([FromBody]StudentParam value)
        {
            StudentDataModel data = new StudentDataModel();
            if (value.profile_id != 0)
            {
                List<Student> list = new List<Student>();
                StudentModel listData = new StudentModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_student_profile", conn))
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
                                    Student prop = new Student();
                                    prop.student_id = Convert.ToInt16(dataReader["student_id"]);
                                    prop.student_number = dataReader["student_number"].ToString();
                                    prop.profile_id = Convert.ToInt16(dataReader["profile_id"]);
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.photo_url = dataReader["photo_url"].ToString();
                                    prop.school_id = Convert.ToInt16(dataReader["school_id"]);
                                    prop.school_name = dataReader["school_name"].ToString();
                                    prop.class_id = dataReader["class_id"] as int? ?? default(int);
                                    prop.class_name = dataReader["class_name"].ToString();
                                    if (DBNull.Value != dataReader["card_id"])
                                    {
                                        prop.card_id = Convert.ToInt16(dataReader["card_id"]);
                                        prop.card_number = dataReader["card_number"].ToString();
                                        prop.hex_id = dataReader["hex_id"].ToString();
                                        prop.card_status_id = Convert.ToInt16(dataReader["card_status_id"]);
                                        prop.card_status = dataReader["card_status"].ToString();
                                    }
                                    else {
                                        prop.card_id = 0;
                                        prop.card_number = "";
                                        prop.hex_id = "";
                                        prop.card_status_id = 0;
                                        prop.card_status = "";
                                    }

                                    prop.wallet_id = Convert.ToInt16(dataReader["wallet_id"]);
                                    prop.wallet_number = dataReader["wallet_number"].ToString();
                                    prop.account_balance = Convert.ToDecimal(dataReader["account_balance"]);
                                    if (dataReader["status_code"].ToString() == "Aktif")
                                    {
                                        listData.Code = "active_student";
                                        listData.Message = "Student status is active.";
                                    }
                                    else
                                    {
                                        listData.Code = "inactive_student";
                                        listData.Message = "Student status is not active.";
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
                        ExceptionUtility.LogException(ex, "student/profile");
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
        [Route("api/v2/student/club-relationship")]
        public IHttpActionResult Post_Student_Club_Relationship([FromBody]StudentClubRelationshipParam value)
        {
            StudentDataModel data = new StudentDataModel();
            if (value.school_id != 0 && value.student_id != 0)
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
                            cmd.Parameters.AddWithValue("@p_staff_id", 0);
                            cmd.Parameters.AddWithValue("@p_parent_id", 0);
                            cmd.Parameters.AddWithValue("@p_student_id", value.student_id);
                            cmd.Parameters.AddWithValue("@p_user_role_id", 10);
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
                        ExceptionUtility.LogException(ex, "student/club-relationship");
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
        [Route("api/v2/student/join-club")]
        public IHttpActionResult Post_Student_Join_Club(string culture, [FromBody] StudentJoinClubRelationshipParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            StaffDataModel data = new StaffDataModel();
            if (value.student_id != 0 && value.club_id != 0 && value.create_by != null)
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
                            cmd.Parameters.AddWithValue("@p_user_role_id", 10);
                            cmd.Parameters.AddWithValue("@p_staff_id", 0);
                            cmd.Parameters.AddWithValue("@p_student_id", value.student_id);
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
                        ExceptionUtility.LogException(ex, "student/join-club");
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
        [Route("api/v2/student/monthly-attendance-summary")]
        public IHttpActionResult Post_Student_Monthly_Attendance_Summary([FromBody]StudentMonthlyAttendanceSummaryParam value)
        {
            StudentDataModel data = new StudentDataModel();
            if (value.school_id != 0 && value.class_id != 0 && value.student_id != 0 && value.entry_month != null)
            {
                List<StudentMonthlyAttendanceSummary> list = new List<StudentMonthlyAttendanceSummary>();
                StudentMonthlyAttendanceSummaryModel listData = new StudentMonthlyAttendanceSummaryModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_student_attendance_report_monthly_summary", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_class_id", value.class_id);
                            cmd.Parameters.AddWithValue("@p_student_id", value.student_id);
                            cmd.Parameters.AddWithValue("@p_entry_month", value.entry_month);
                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    StudentMonthlyAttendanceSummary prop = new StudentMonthlyAttendanceSummary();
                                    prop.total_absent = Convert.ToInt16(dataReader["total_absent"]);
                                    prop.total_present = Convert.ToInt16(dataReader["total_present"]);
                                    prop.total_late_in = Convert.ToInt16(dataReader["total_late_in"]);
                                    prop.total_half_day = Convert.ToInt16(dataReader["total_half_day"]);
                                    prop.total_attendance = Convert.ToInt16(dataReader["total_attendance"]);
                                    prop.total_school_day = Convert.ToInt16(dataReader["total_school_day"]);
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "student_monthly_attendance_summary";
                                listData.Message = "Student monthly attendance report summary";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Student monthly attendance report summary could not be found.";
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
                        ExceptionUtility.LogException(ex, "student/monthly-attendance-summary");
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
        [Route("api/v2/student/monthly-attendance")]
        public IHttpActionResult Post_Student_Monthly_Attendance([FromBody]StudentMonthlyAttendanceParam value)
        {
            StudentDataModel data = new StudentDataModel();
            if (value.school_id != 0 && value.class_id != 0 && value.student_id != 0 && value.entry_month != null)
            {
                List<StudentMonthlyAttendance> list = new List<StudentMonthlyAttendance>();
                StudentMonthlyAttendanceModel listData = new StudentMonthlyAttendanceModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_student_attendance_report_monthly", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_class_id", value.class_id);
                            cmd.Parameters.AddWithValue("@p_student_id", value.student_id);
                            cmd.Parameters.AddWithValue("@p_entry_month", value.entry_month);
                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    StudentMonthlyAttendance prop = new StudentMonthlyAttendance();
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
                                listData.Code = "student_monthly_attendance_list";
                                listData.Message = "Student monthly attendance report list.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Student monthly attendance report list could not be found.";
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
                        ExceptionUtility.LogException(ex, "student/monthly-attendance");
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
        [Route("api/v2/student/daily-attendance")]
        public IHttpActionResult Post_Student_Daily_Attendance([FromBody]StudentDailyAttendanceParam value)
        {
            StudentDataModel data = new StudentDataModel();
            if (value.school_id != 0 && value.class_id != 0 && value.student_id != 0 && value.entry_date != null)
            {
                List<StudentMonthlyAttendance> list = new List<StudentMonthlyAttendance>();
                StudentMonthlyAttendanceModel listData = new StudentMonthlyAttendanceModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_student_attendance_report_daily", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_class_id", value.class_id);
                            cmd.Parameters.AddWithValue("@p_student_id", value.student_id);
                            cmd.Parameters.AddWithValue("@p_entry_date", value.entry_date);
                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    StudentMonthlyAttendance prop = new StudentMonthlyAttendance();
                                    prop.report_id = dataReader["report_id"] as int? ?? default(int);
                                    prop.entry_date = dataReader["entry_date"] as DateTime? ?? default(DateTime);
                                    prop.attendance_id = dataReader["attendance_id"] as int? ?? default(int);
                                    prop.attendance = dataReader["attendance"].ToString();
                                    prop.attendance_bm = dataReader["attendance_bm"].ToString();
                                    if (dataReader["reason_id"] is DBNull)
                                    {
                                        prop.reason_id = 0;
                                    }
                                    else {
                                        prop.reason_id = Convert.ToInt32(dataReader["reason_id"]);
                                    }
                                    prop.reason_for_absent = dataReader["reason_for_absent"].ToString();
                                    prop.reason_for_absent_bm = dataReader["reason_for_absent_bm"].ToString();
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "student_daily_attendance";
                                listData.Message = "Student daily attendance report.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Student daily attendance report could not be found.";
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
                        ExceptionUtility.LogException(ex, "student/daily-attendance");
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
        [Route("api/v2/student/submit-outing-request")]
        public IHttpActionResult Post_Student_Submit_Request_Outing([FromBody] StudentSubmitOutingParam value)
        {
            int _outing_id = 0;
            DateTime dt = DateTime.MinValue;
            StudentDataModel data = new StudentDataModel();
            if (value.student_id != 0 && value.school_id != 0 && value.outing_type_id != 0 && value.check_out_date != dt && value.check_in_date != dt
                && value.outing_reason != null && value.request_by_id != 0 && value.request_by_user_role_id != 0 && value.create_by != null)
            {
                MySqlTransaction trans = null;
                data.Success = false;
                string sqlQuery = string.Empty;
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                if (value.outing_id > 0)
                {
                    _outing_id = value.outing_id;
                }

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd = new MySqlCommand("sp_insert_student_outing_request_submit", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_outing_id", _outing_id);
                            cmd.Parameters.AddWithValue("@p_student_id", value.student_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_outing_type_id", value.outing_type_id);
                            cmd.Parameters.AddWithValue("@p_check_out_date", value.check_out_date);
                            cmd.Parameters.AddWithValue("@p_check_in_date", value.check_in_date);
                            cmd.Parameters.AddWithValue("@p_outing_reason", value.outing_reason);
                            cmd.Parameters.AddWithValue("@p_request_by_id", value.request_by_id);
                            cmd.Parameters.AddWithValue("@p_request_by_user_role_id", value.request_by_user_role_id);
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
                                data.Message = "Outing request successfully submitted.";
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
                        ExceptionUtility.LogException(ex, "student/submit-outing-request");
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
        [Route("api/v2/student/save-outing-request")]
        public IHttpActionResult Post_Student_Save_Request_Outing([FromBody] StudentRequestOutingParam value)
        {
            DateTime dt = DateTime.MinValue;
            StudentDataModel data = new StudentDataModel(); 
            if (value.student_id != 0 && value.school_id != 0 && value.outing_type_id != 0 && value.check_out_date != dt && value.check_in_date != dt
                && value.outing_reason != null  && value.request_by_id != 0 && value.request_by_user_role_id != 0 && value.create_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_insert_student_outing", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_student_id", value.student_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_outing_type_id", value.outing_type_id);
                            cmd.Parameters.AddWithValue("@p_check_out_date", value.check_out_date);
                            cmd.Parameters.AddWithValue("@p_check_in_date", value.check_in_date);
                            cmd.Parameters.AddWithValue("@p_outing_reason", value.outing_reason);
                            cmd.Parameters.AddWithValue("@p_request_by_id", value.request_by_id);
                            cmd.Parameters.AddWithValue("@p_request_by_user_role_id", value.request_by_user_role_id);
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
                                data.Message = "Outing request saved successfully.";
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "record_exist";
                                data.Message = "Outing request already exists.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "student/save-outing-request");
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
        [Route("api/v2/student/update-outing-request")]
        public IHttpActionResult Post_Student_Update_Request_Outing([FromBody] StudentUpdateOutingParam value)
        {
            DateTime dt = DateTime.MinValue;
            StudentDataModel data = new StudentDataModel();
            if (value.outing_id != 0 && value.student_id != 0 && value.school_id != 0 && value.outing_type_id != 0 && value.check_out_date != dt && value.check_in_date != dt
                && value.outing_reason != null && value.request_by_id != 0 && value.request_by_user_role_id != 0 && value.update_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_update_student_outing", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_outing_id", value.outing_id);
                            cmd.Parameters.AddWithValue("@p_student_id", value.student_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_outing_type_id", value.outing_type_id);
                            cmd.Parameters.AddWithValue("@p_check_out_date", value.check_out_date);
                            cmd.Parameters.AddWithValue("@p_check_in_date", value.check_in_date);
                            cmd.Parameters.AddWithValue("@p_outing_reason", value.outing_reason);
                            cmd.Parameters.AddWithValue("@p_request_by_id", value.request_by_id);
                            cmd.Parameters.AddWithValue("@p_request_by_user_role_id", value.request_by_user_role_id);
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
                                data.Message = "Outing request has been updated.";
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
                        ExceptionUtility.LogException(ex, "student/update-outing-request");
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
        [Route("api/v2/student/cancel-outing-request")]
        public IHttpActionResult Post_Student_Cancel_Request_Outing([FromBody] StudentUpdateOutingParam value)
        {
            DateTime dt = DateTime.MinValue;
            StudentDataModel data = new StudentDataModel();
            if (value.outing_id != 0 && value.student_id != 0 && value.school_id != 0 && value.update_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_update_student_outing_request_cancel", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_outing_id", value.outing_id);
                            cmd.Parameters.AddWithValue("@p_student_id", value.student_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
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
                                data.Message = "Outing request has been canceled.";
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
                        ExceptionUtility.LogException(ex, "student/cancel-outing-request");
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
        [Route("api/v2/student/outing-request-month")]
        public IHttpActionResult Post_Student_Outing_Request_Month([FromBody] StudentOutingParam value)
        {
            StudentDataModel data = new StudentDataModel();
            if (value.student_id != 0 && value.school_id != 0 && value.outing_month != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_student_outing_month", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_student_id", value.student_id);
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
                                listData.Code = "student_outing_list";
                                listData.Message = "Student outing list.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Student outing list could not be found.";
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
                        ExceptionUtility.LogException(ex, "student/outing-request-month");
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
        [Route("api/v2/student/outing-request-month-group")]
        public IHttpActionResult Post_Student_Outing_Request_Month_Group([FromBody] StudentOutingParam value)
        {
            StudentDataModel data = new StudentDataModel();
            if (value.student_id != 0 && value.school_id != 0)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_student_outing_month_group", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_student_id", value.student_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
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
                                listData.Code = "student_outing_list";
                                listData.Message = "Student outing list.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Student outing list could not be found.";
                                listData.Data = list;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "student/outing-request-group-month");
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
        [Route("api/v2/student/parent-relationship")]
        public IHttpActionResult Post_Student_Parent_Relationship([FromBody] StudentParentParam value)
        {
            StudentDataModel data = new StudentDataModel();
            if (value.student_id != 0)
            {
                List<StudentParent> list = new List<StudentParent>();
                StudentParentModel listData = new StudentParentModel();
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
                            cmd.Parameters.AddWithValue("@p_parent_id", 0);
                            cmd.Parameters.AddWithValue("@p_student_id", value.student_id);
                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    StudentParent prop = new StudentParent();
                                    prop.parent_id = dataReader["parent_id"] as int? ?? default(int);
                                    prop.profile_id = dataReader["parent_profile_id"] as int? ?? default(int);
                                    prop.full_name = dataReader["parent_full_name"].ToString();
                                    prop.photo_url = dataReader["parent_photo_url"].ToString();
                                    prop.mobile_number = dataReader["parent_mobile_number"].ToString();
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "student_parent_relationship";
                                listData.Message = "Student parent relationship";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Student parent relationship could not be found.";
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
                        ExceptionUtility.LogException(ex, "student/parent-relationship");
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
