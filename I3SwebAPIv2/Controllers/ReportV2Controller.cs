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
using System.Net;
using System.Threading;
using System.Web.Http;

namespace I3SwebAPIv2.Controllers
{
    public class ReportV2Controller : ApiController
    {
        [HttpPost]
        [Authorize]
        [Route("api/v2/report/class-attendance")]
        public IHttpActionResult Post_Report_Class_Attendance(string culture, [FromBody] ExportClassAttendanceParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            ReportV2Class cls = new ReportV2Class();
            ReportDataModel data = new ReportDataModel();
            if (value.school_id != 0 && value.class_id != 0 && value.create_by != null && value.entry_month != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_class_attendance_export_monthly", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_class_id", value.class_id);
                            cmd.Parameters.AddWithValue("@p_month_year", value.entry_month);

                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                DataTable dtData = new DataTable("Data");
                                DataTable dtSchema = new DataTable("Schema");

                                dtSchema = dataReader.GetSchemaTable();

                                foreach (DataRow schemarow in dtSchema.Rows)
                                {
                                    dtData.Columns.Add(schemarow.ItemArray[0].ToString(), System.Type.GetType(schemarow.ItemArray[11].ToString()));
                                }

                                while (dataReader.Read())
                                {
                                    object[] ColArray = new object[dataReader.FieldCount];
                                    for (int i = 0; i < dataReader.FieldCount; i++)
                                    {
                                        if (dataReader[i] != null) ColArray[i] = dataReader[i];
                                    }
                                    dtData.LoadDataRow(ColArray, true);
                                }

                                dataReader.Close();

                                string url = cls.ExportClassAttendance(dtData, Convert.ToInt32(value.entry_month.Split('-')[0]), Convert.ToInt32(value.entry_month.Split('-')[1]), value.create_by, culture);

                                if (!url.Contains("error"))
                                {
                                    data.Success = true;
                                    data.Code = "report_saved";
                                    data.Message = url;
                                }
                                else {
                                    data.Success = false;
                                    data.Code = "error";
                                    data.Message = url;
                                }
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "no_record_found";
                                data.Message = WebApiResources.ClassAttendanceReportNotFound;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "report/export-class-attendance");
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
                return BadRequest("Missing Parameters.");
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/report/staff-attendance")]
        public IHttpActionResult Post_Report_Staff_Attendance(string culture, [FromBody] ExportStaffAttendanceParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            ReportV2Class cls = new ReportV2Class();
            ReportDataModel data = new ReportDataModel();
            if (value.school_id != 0 && value.shift_id != 0 && value.create_by != null && value.entry_month != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_school_staff_attendance_export_monthly", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_shift_id", value.shift_id);
                            cmd.Parameters.AddWithValue("@p_month_year", value.entry_month);

                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                DataTable dtData = new DataTable("Data");
                                DataTable dtSchema = new DataTable("Schema");

                                dtSchema = dataReader.GetSchemaTable();

                                foreach (DataRow schemarow in dtSchema.Rows)
                                {
                                    dtData.Columns.Add(schemarow.ItemArray[0].ToString(), System.Type.GetType(schemarow.ItemArray[11].ToString()));
                                }

                                while (dataReader.Read())
                                {
                                    object[] ColArray = new object[dataReader.FieldCount];
                                    for (int i = 0; i < dataReader.FieldCount; i++)
                                    {
                                        if (dataReader[i] != null) ColArray[i] = dataReader[i];
                                    }
                                    dtData.LoadDataRow(ColArray, true);
                                }

                                dataReader.Close();

                                string url = cls.ExportStaffAttendance(dtData, Convert.ToInt32(value.entry_month.Split('-')[0]), Convert.ToInt32(value.entry_month.Split('-')[1]), value.create_by, culture);

                                if (!url.Contains("error"))
                                {
                                    data.Success = true;
                                    data.Code = "report_saved";
                                    data.Message = url;
                                }
                                else
                                {
                                    data.Success = false;
                                    data.Code = "error";
                                    data.Message = url;
                                }
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "no_record_found";
                                data.Message = WebApiResources.StaffAttendanceReportNotFound;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "report/export-staff-attendance");
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
                return BadRequest("Missing Parameters.");
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/report/product-daily-order")]
        public IHttpActionResult Post_Report_Product_Daily_Order(string culture, [FromBody] ExportMerchantDailyOrderParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            ReportV2Class cls = new ReportV2Class();
            ReportDataModel data = new ReportDataModel();
            if (value.merchant_id != 0 && value.school_id != 0 && value.order_date != null && value.create_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_order_history_product_export", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_merchant_id", value.merchant_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_pickup_date", value.order_date);

                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                DataTable dtData = new DataTable("Data");
                                DataTable dtSchema = new DataTable("Schema");

                                dtSchema = dataReader.GetSchemaTable();

                                foreach (DataRow schemarow in dtSchema.Rows)
                                {
                                    dtData.Columns.Add(schemarow.ItemArray[0].ToString(), System.Type.GetType(schemarow.ItemArray[11].ToString()));
                                }

                                while (dataReader.Read())
                                {
                                    object[] ColArray = new object[dataReader.FieldCount];
                                    for (int i = 0; i < dataReader.FieldCount; i++)
                                    {
                                        if (dataReader[i] != null) ColArray[i] = dataReader[i];
                                    }
                                    dtData.LoadDataRow(ColArray, true);
                                }

                                dataReader.Close();

                                string url = cls.ExportOrderHistoryProduct(dtData, value.order_date, value.create_by);

                                data.Success = true;
                                data.Code = "report_saved";
                                data.Message = url;
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "no_record_found";
                                data.Message = WebApiResources.ProductOrderReportNotFound;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "report/export-merchant-daily-order");
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
                return BadRequest("Missing Parameters.");
            }

        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/report/class-daily-order")]
        public IHttpActionResult Post_Report_Class_Daily_Order(string culture, [FromBody] ExportMerchantDailyOrderParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            ReportV2Class cls = new ReportV2Class();
            ReportDataModel data = new ReportDataModel();
            if (value.merchant_id != 0 && value.school_id != 0 && value.order_date != null && value.create_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_order_history_class_export", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_merchant_id", value.merchant_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_pickup_date", value.order_date);

                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                DataTable dtData = new DataTable("Data");
                                DataTable dtSchema = new DataTable("Schema");

                                dtSchema = dataReader.GetSchemaTable();

                                foreach (DataRow schemarow in dtSchema.Rows)
                                {
                                    dtData.Columns.Add(schemarow.ItemArray[0].ToString(), System.Type.GetType(schemarow.ItemArray[11].ToString()));
                                }

                                while (dataReader.Read())
                                {
                                    object[] ColArray = new object[dataReader.FieldCount];
                                    for (int i = 0; i < dataReader.FieldCount; i++)
                                    {
                                        if (dataReader[i] != null) ColArray[i] = dataReader[i];
                                    }
                                    dtData.LoadDataRow(ColArray, true);
                                }

                                dataReader.Close();

                                string url = cls.ExportOrderHistoryClass(dtData, value.order_date, value.create_by);

                                data.Success = true;
                                data.Code = "report_saved";
                                data.Message = url;
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "no_record_found";
                                data.Message = WebApiResources.ClassOrderReportNotFound;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "report/export-class-daily-order");
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
                return BadRequest("Missing Parameters.");
            }

        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/report/student-daily-order")]
        public IHttpActionResult Post_Report_Student_Daily_Order(string culture, [FromBody] ExportClassDailyOrderParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            ReportV2Class cls = new ReportV2Class();
            ReportDataModel data = new ReportDataModel();
            if (value.merchant_id != 0 && value.school_id != 0 && value.class_id != 0 && value.order_date != null && value.create_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_order_history_student_export", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_merchant_id", value.merchant_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_class_id", value.class_id);
                            cmd.Parameters.AddWithValue("@p_pickup_date", value.order_date);

                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                DataTable dtData = new DataTable("Data");
                                DataTable dtSchema = new DataTable("Schema");

                                dtSchema = dataReader.GetSchemaTable();

                                foreach (DataRow schemarow in dtSchema.Rows)
                                {
                                    dtData.Columns.Add(schemarow.ItemArray[0].ToString(), System.Type.GetType(schemarow.ItemArray[11].ToString()));
                                }

                                while (dataReader.Read())
                                {
                                    object[] ColArray = new object[dataReader.FieldCount];
                                    for (int i = 0; i < dataReader.FieldCount; i++)
                                    {
                                        if (dataReader[i] != null) ColArray[i] = dataReader[i];
                                    }
                                    dtData.LoadDataRow(ColArray, true);
                                }

                                dataReader.Close();

                                string url = cls.ExportOrderHistoryStudent(dtData, value.order_date, value.create_by);

                                data.Success = true;
                                data.Code = "report_saved";
                                data.Message = url;
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "no_record_found";
                                data.Message = WebApiResources.StudentOrderReportNotFound;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "report/export-student-daily-order");
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
                return BadRequest("Missing Parameters.");
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/report/staff-daily-order")]
        public IHttpActionResult Post_Report_Staff_Daily_Order(string culture, [FromBody] ExportStaffDailyOrderParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            ReportV2Class cls = new ReportV2Class();
            ReportDataModel data = new ReportDataModel();
            if (value.merchant_id != 0 && value.school_id != 0 && value.order_date != null && value.create_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_order_history_staff_export", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_merchant_id", value.merchant_id);
                            cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                            cmd.Parameters.AddWithValue("@p_pickup_date", value.order_date);

                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                DataTable dtData = new DataTable("Data");
                                DataTable dtSchema = new DataTable("Schema");

                                dtSchema = dataReader.GetSchemaTable();

                                foreach (DataRow schemarow in dtSchema.Rows)
                                {
                                    dtData.Columns.Add(schemarow.ItemArray[0].ToString(), System.Type.GetType(schemarow.ItemArray[11].ToString()));
                                }

                                while (dataReader.Read())
                                {
                                    object[] ColArray = new object[dataReader.FieldCount];
                                    for (int i = 0; i < dataReader.FieldCount; i++)
                                    {
                                        if (dataReader[i] != null) ColArray[i] = dataReader[i];
                                    }
                                    dtData.LoadDataRow(ColArray, true);
                                }

                                dataReader.Close();

                                string url = cls.ExportOrderHistoryStaff(dtData, value.order_date, value.create_by);

                                data.Success = true;
                                data.Code = "report_saved";
                                data.Message = url;
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "no_record_found";
                                data.Message = WebApiResources.StaffOrderReportNotFound;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "report/export-staff-daily-order");
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
                return BadRequest("Missing Parameters.");
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/v2/report/customer-feedback")]
        public IHttpActionResult Post_Report_Customer_Feedback(string culture, [FromBody] CustomerFeedbackRequest body)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            SchoolDataModel data = new SchoolDataModel();
            if (body.support_category_id != 0 && body.problem_type_id != 0 && body.ticket_status_id != 0 && body.create_by != null
                && body.ticket_subject != null && body.ticket_desc != null && body.priority_type_id != 0)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_insert_support_ticket", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_support_category_id", body.support_category_id);
                            cmd.Parameters.AddWithValue("@p_problem_type_id", body.problem_type_id);
                            cmd.Parameters.AddWithValue("@p_priority_type_id", body.priority_type_id);
                            cmd.Parameters.AddWithValue("@p_report_by", body.create_by);
                            cmd.Parameters.AddWithValue("@p_ticket_subject", body.ticket_subject);
                            cmd.Parameters.AddWithValue("@p_ticket_desc", body.ticket_desc);
                            cmd.Parameters.AddWithValue("@p_create_by", body.create_by);
                            cmd.Parameters.Add("@CheckTicketExists", MySqlDbType.Int32);
                            cmd.Parameters["@CheckTicketExists"].Direction = ParameterDirection.Output;
                            cmd.Parameters.Add("@TicketNo", MySqlDbType.Int32);
                            cmd.Parameters["@TicketNo"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            var ticket_id = Convert.ToInt32(cmd.Parameters["@TicketNo"].Value);

                            data.Success = true;
                            data.Code = ticket_id.ToString();
                            data.Message = WebApiResources.ThankYouFeedback;

                        }
                    }
                    catch (Exception ex)
                    {
                        //listData.Success = false;
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "report/customer-feedback");
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
        [Route("api/v2/report/upload-attachment")]
        public IHttpActionResult Post_Report_Upload_Attachment(string culture, [FromBody] UploadAttachmentParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            string salt = ConfigurationManager.AppSettings["passPhrase"];
            FileTransferProtocol ftp = new FileTransferProtocol();
            UserDataModel data = new UserDataModel();
            if (value.ticket_id != 0 && value.file_name != null && value.photo_base64 != null && value.create_by != null)
            {
                MySqlTransaction trans = null;
                data.Success = false;
                string sqlQuery = string.Empty;
                bool upload = false;

                string ftp_address = ConfigurationManager.AppSettings["ftpAddress"];
                string ftp_username = ConfigurationManager.AppSettings["ftpUName"];
                string ftp_password = ConfigurationManager.AppSettings["ftpPWord"];
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                string attachment_url = "/images/tickets/" + value.ticket_id + "/" + value.file_name;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd = new MySqlCommand("sp_insert_support_ticket_attachment", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_ticket_id", value.ticket_id);
                            cmd.Parameters.AddWithValue("@p_file_name", value.file_name);
                            cmd.Parameters.AddWithValue("@p_attachment_url", attachment_url);
                            cmd.Parameters.AddWithValue("@p_create_by", value.create_by);
                            MySqlDataReader dataReader = cmd.ExecuteReader();

                            if (value.file_name != null && value.photo_base64 != null)
                            {
                                var inputText = ValidateBase64EncodedString(value.photo_base64);
                                byte[] fileBytes = Convert.FromBase64String(inputText);

                                string directory = "/images/tickets/" + value.ticket_id;

                                if (ftp.Check_Directory_Exists(ftp_address, directory, ftp_username, ftp_password) == true)
                                {
                                    if (ftp.Retrieve_Delete_Directory_File(ftp_address, directory, ftp_username, ftp_password) == true)
                                    {
                                        try
                                        {
                                            //Create FTP Request.
                                            FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/tickets/" + value.ticket_id + "/" + value.file_name);
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
                                else
                                {
                                    string folder_ticketId = "/images/tickets/" + value.ticket_id;

                                    if (ftp.Create_Directory(ftp_address, folder_ticketId, ftp_username, ftp_password) == true) // ticket id
                                    {
                                        if (ftp.Check_Directory_Exists(ftp_address, folder_ticketId, ftp_username, ftp_password) == true)
                                        {
                                            try
                                            {
                                                //Create FTP Request.
                                                FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/tickets/" + value.ticket_id + "/" + value.file_name);
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

                                if (upload == true)
                                {
                                    dataReader.Close();
                                    trans.Commit();

                                    data.Success = true;
                                    data.Code = "upload_success";
                                    data.Message = WebApiResources.AttUploadSuccess;
                                }
                            }
                            else
                            {
                                return BadRequest("Missing parameters.");
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "report/upload-attachment");
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
