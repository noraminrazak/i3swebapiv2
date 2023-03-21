using I3SwebAPIv2.Class;
using I3SwebAPIv2.Models;
using I3SwebAPIv2.Resources;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.Http;

namespace I3SwebAPIv2.Controllers
{
    public class ContactV2Controller : ApiController
    {
        [HttpPost]
        [Authorize]
        [Route("api/v2/contact/class-teacher")]
        public IHttpActionResult Post_Contact_Class_Teacher([FromBody] StaffContactRequest body)
        {
            ContactDataModel data = new ContactDataModel();

            if (body.parent_id != 0)
            {
                List<SchoolClass> list = new List<SchoolClass>();
                List<StaffContact> listContact = new List<StaffContact>();
                StaffContactModel listData = new StaffContactModel();
                MySqlTransaction trans = null;

                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_parent_relationship_contact", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_parent_id", body.parent_id);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    SchoolClass prop = new SchoolClass();
                                    prop.school_id = dataReader["school_id"] as int? ?? default(int);
                                    prop.class_id = dataReader["class_id"] as int? ?? default(int);
                                    list.Add(prop);
                                }

                                dataReader.Close();
                                cmd.Dispose();

                                using (MySqlCommand cmd2 = new MySqlCommand("sp_get_staff_relationship_contact", conn))
                                {
                                    foreach (SchoolClass contact in list)
                                    {
                                        cmd2.Transaction = trans;
                                        cmd2.CommandType = CommandType.StoredProcedure;
                                        cmd2.Parameters.Clear();
                                        cmd2.Parameters.AddWithValue("@p_school_id", contact.school_id);
                                        cmd2.Parameters.AddWithValue("@p_class_id", contact.class_id);
                                        MySqlDataReader dataReader2 = cmd2.ExecuteReader();

                                        if (dataReader2.HasRows == true)
                                        {
                                            while (dataReader2.Read())
                                            {
                                                StaffContact prop = new StaffContact();
                                                prop.profile_id = dataReader2["profile_id"] as int? ?? default(int);
                                                prop.full_name = dataReader2["full_name"].ToString();
                                                prop.photo_url = dataReader2["photo_url"].ToString();
                                                prop.user_role = dataReader2["user_role"].ToString();
                                                prop.user_role_bm = dataReader2["user_role_bm"].ToString();

                                                bool alreadyExists = listContact.Any(x => x.profile_id == prop.profile_id);
                                                if (alreadyExists == false)
                                                {
                                                    listContact.Add(prop);
                                                }
                                            }
                                        }

                                        dataReader2.Close();
                                    }

                                    if (listContact.Count > 0)
                                    {
                                        listData.Success = true;
                                        listData.Code = "class_teacher_list";
                                        listData.Message = "Class teacher list.";
                                        listData.Data = listContact;
                                    }
                                    else {
                                        listData.Success = true;
                                        listData.Code = "no_record_found";
                                        listData.Message = "No record found.";
                                        listData.Data = listContact;
                                    }
                                }
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "No record found.";
                                listData.Data = listContact;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //listData.Success = false;
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "contact/class-teacher");
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
        [Route("api/v2/contact/student-parent-staff-merchant")]
        public IHttpActionResult Post_Contact_Student_Parent_Staff_Merchant([FromBody] SchoolClassRequest body)
        {
            ContactDataModel data = new ContactDataModel();

            if (body.school_id != 0 && body.class_id != null)
            {
                List<SchoolClass> list = new List<SchoolClass>();
                List<ParentContact> listContact = new List<ParentContact>();
                ParentContactModel listData = new ParentContactModel();
                MySqlTransaction trans = null;

                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_student_parent_staff_merchant_contact", conn))
                        {

                            foreach (int sid in body.class_id)
                            {
                                cmd.Transaction = trans;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@p_school_id", body.school_id);
                                cmd.Parameters.AddWithValue("@p_class_id", sid);
                                MySqlDataReader dataReader = cmd.ExecuteReader();

                                if (dataReader.HasRows == true)
                                {
                                    while (dataReader.Read())
                                    {
                                        ParentContact prop = new ParentContact();
                                        prop.profile_id = dataReader["profile_id"] as int? ?? default(int);
                                        prop.full_name = dataReader["full_name"].ToString();
                                        prop.photo_url = dataReader["photo_url"].ToString();
                                        prop.user_role = dataReader["user_role"].ToString();
                                        prop.user_role_bm = dataReader["user_role_bm"].ToString();

                                        bool alreadyExists = listContact.Any(x => x.profile_id == prop.profile_id);
                                        if (alreadyExists == false)
                                        {
                                            listContact.Add(prop);
                                        }
                                    }
                                }

                                dataReader.Close();
                            }

                            if (listContact.Count > 0)
                            {
                                listData.Success = true;
                                listData.Code = "student_parent_staff_merchant_list";
                                listData.Message = "Student parent staff merchant list.";
                                listData.Data = listContact;
                            }
                            else
                            {
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "No record found.";
                                listData.Data = listContact;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //listData.Success = false;
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "contact/student-parent-staff-merchant");
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
        [Route("api/v2/contact/school-staff-merchant")]
        public IHttpActionResult Post_Contact_School_Staff_Merchant([FromBody] SchoolStaffRequest value)
        {
            ContactDataModel data = new ContactDataModel();

            if (value.school_id != 0 && value.profile_id != 0)
            {
                List<StaffContact> list = new List<StaffContact>();
                StaffContactModel listData = new StaffContactModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_school_staff_merchant_contact", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);

                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    StaffContact prop = new StaffContact();
                                    prop.profile_id = dataReader["profile_id"] as int? ?? default(int);
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.photo_url = dataReader["photo_url"].ToString();
                                    prop.user_role = dataReader["user_role"].ToString();
                                    prop.user_role_bm = dataReader["user_role_bm"].ToString();
                                    list.Add(prop);

                                }
                                listData.Success = true;
                                listData.Code = "school_staff_list";
                                listData.Message = "School staff merchant list.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "School staff merchant could not be found.";
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
                        ExceptionUtility.LogException(ex, "contact/school-staff-merchant");
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
        [Route("api/v2/contact/school-staff")]
        public IHttpActionResult Post_Contact_School_Staff([FromBody] SchoolMerchantRequest value)
        {
            ContactDataModel data = new ContactDataModel();

            if (value.school_id != null)
            {
                List<StaffContact> list = new List<StaffContact>();
                List<StaffContact> listContact = new List<StaffContact>();
                StaffContactModel listData = new StaffContactModel();
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

                        using (MySqlCommand cmd2 = new MySqlCommand("sp_get_school_staff_contact", conn))
                        {
                            foreach (int sid in value.school_id)
                            {
                                cmd2.Transaction = trans;
                                cmd2.CommandType = CommandType.StoredProcedure;
                                cmd2.Parameters.Clear();
                                cmd2.Parameters.AddWithValue("@p_school_id", sid);
                                MySqlDataReader dataReader2 = cmd2.ExecuteReader();

                                if (dataReader2.HasRows == true)
                                {
                                    while (dataReader2.Read())
                                    {
                                        StaffContact prop = new StaffContact();
                                        prop.profile_id = dataReader2["profile_id"] as int? ?? default(int);
                                        prop.full_name = dataReader2["full_name"].ToString();
                                        prop.photo_url = dataReader2["photo_url"].ToString();
                                        prop.user_role = dataReader2["user_role"].ToString();
                                        prop.user_role_bm = dataReader2["user_role_bm"].ToString();

                                        bool alreadyExists = listContact.Any(x => x.profile_id == prop.profile_id);
                                        if (alreadyExists == false)
                                        {
                                            listContact.Add(prop);
                                        }
                                    }
                                }

                                dataReader2.Close();
                            }

                            if (listContact.Count > 0)
                            {
                                listData.Success = true;
                                listData.Code = "school_staff_list";
                                listData.Message = "School staff list.";
                                listData.Data = listContact;
                            }
                            else
                            {
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "No record found.";
                                listData.Data = listContact;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //listData.Success = false;
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "contact/school-staff");
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
        [Route("api/v2/contact/student-class")]
        public IHttpActionResult Post_Contact_Student_Class([FromBody] StaffContactRequest value)
        {
            ParentDataModel data = new ParentDataModel();
            if (value.parent_id != 0)
            {
                List<SchoolClass> list = new List<SchoolClass>();
                SchoolClassModel listData = new SchoolClassModel();
                MySqlTransaction trans = null;
                listData.Success = false;

                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_parent_relationship_contact", conn))
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
                                    SchoolClass prop = new SchoolClass();
                                    prop.school_id = dataReader["school_id"] as int? ?? default(int);
                                    prop.school_name = dataReader["school_name"].ToString();
                                    prop.class_id = dataReader["class_id"] as int? ?? default(int);
                                    prop.class_name = dataReader["class_name"].ToString();
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "student_class_list";
                                listData.Message = "Student class list.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Student class list could not be found.";
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
                        ExceptionUtility.LogException(ex, "contact/student-class");
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
