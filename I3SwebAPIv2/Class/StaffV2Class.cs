using I3SwebAPIv2.Models;
using I3SwebAPIv2.Resources;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading;

namespace I3SwebAPIv2.Class
{
    public class StaffV2Class
    {
        public SchoolDataModel Insert_School_Post(string culture, CreateSchoolPostParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            bool upload = false;
            FirebaseCloudMessagingPush fcm = new FirebaseCloudMessagingPush();
            FileTransferProtocol ftp = new FileTransferProtocol();
            SchoolDataModel data = new SchoolDataModel();
            if (value != null)
            {
                if (value.school_id != 0 && value.staff_id != 0 && value.post_message != null && value.start_at != null && value.end_at != null && value.create_by != null)
                {
                    MySqlTransaction trans = null;
                    MySqlTransaction trans2 = null;
                    data.Success = false;
                    string sqlQuery = string.Empty;

                    int post_id;
                    string password = string.Empty;
                    string ftp_address = ConfigurationManager.AppSettings["ftpAddress"];
                    string ftp_username = ConfigurationManager.AppSettings["ftpUName"];
                    string ftp_password = ConfigurationManager.AppSettings["ftpPWord"];
                    string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;
                    DateTime dt = DateTime.Now;
                    string time_stamp = dt.ToString("yyyy_MM_dd");
                    string photo_url = "/images/schools/" + value.school_id + "/" + time_stamp + "/" + value.file_name;

                    using (MySqlConnection conn = new MySqlConnection(constr))
                    {
                        try
                        {
                            conn.Open();
                            trans = conn.BeginTransaction();

                            using (MySqlCommand cmd = new MySqlCommand("sp_insert_school_post", conn))
                            {
                                cmd.Transaction = trans;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                                cmd.Parameters.AddWithValue("@p_staff_id", value.staff_id);
                                cmd.Parameters.AddWithValue("@p_post_message", value.post_message);
                                cmd.Parameters.AddWithValue("@p_start_at", value.start_at);
                                cmd.Parameters.AddWithValue("@p_end_at", value.end_at);
                                cmd.Parameters.AddWithValue("@p_create_by", value.create_by);
                                cmd.Parameters.Add("@PostId", MySqlDbType.Int16);
                                cmd.Parameters["@PostId"].Direction = ParameterDirection.Output;
                                MySqlDataReader dataReader = cmd.ExecuteReader();
                                dataReader.Close();
                                trans.Commit();

                                post_id = Convert.ToInt16(cmd.Parameters["@PostId"].Value);

                                if (value.file_name != null && value.photo_base64 != null)
                                {
                                    var inputText = ValidateBase64EncodedString(value.photo_base64);
                                    byte[] fileBytes = Convert.FromBase64String(inputText);
                                    string directory = "/images/schools/" + value.school_id + "/" + time_stamp;
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
                                        string folder_school_id = "/images/schools/" + value.school_id;
                                        string folder_timestamp = "/images/schools/" + value.school_id + "/" + time_stamp;

                                        if (ftp.Check_Directory_Exists(ftp_address, folder_school_id, ftp_username, ftp_password) == true) //school id
                                        {
                                            if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true) //time stamp
                                            {
                                                try
                                                {
                                                    //Create FTP Request.
                                                    FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/" + time_stamp + "/" + value.file_name);
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
                                                if (ftp.Create_Directory(ftp_address, folder_timestamp, ftp_username, ftp_password) == true) // time stamp
                                                {
                                                    if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                    {
                                                        try
                                                        {
                                                            //Create FTP Request.
                                                            FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/" + time_stamp + "/" + value.file_name);
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
                                        else
                                        {
                                            if (ftp.Create_Directory(ftp_address, folder_school_id, ftp_username, ftp_password) == true)
                                            {
                                                if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                {
                                                    try
                                                    {
                                                        //Create FTP Request.
                                                        FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/" + time_stamp + "/" + value.file_name);
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
                                                    if (ftp.Create_Directory(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                    {
                                                        if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                        {
                                                            try
                                                            {
                                                                //Create FTP Request.
                                                                FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/" + time_stamp + "/" + value.file_name);
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

                                    if (upload == true)
                                    {
                                        try
                                        {
                                            using (MySqlCommand cmd2 = new MySqlCommand("sp_update_school_post_photo", conn))
                                            {
                                                trans2 = conn.BeginTransaction();
                                                cmd2.Transaction = trans2;
                                                cmd2.CommandType = CommandType.StoredProcedure;
                                                cmd2.Parameters.Clear();
                                                cmd2.Parameters.AddWithValue("@p_post_id", post_id);
                                                cmd2.Parameters.AddWithValue("@p_school_id", value.school_id);
                                                cmd2.Parameters.AddWithValue("@p_file_name", value.file_name);
                                                cmd2.Parameters.AddWithValue("@p_photo_url", photo_url);
                                                MySqlDataReader dataReader2 = cmd2.ExecuteReader();
                                                dataReader2.Close();
                                                trans2.Commit();
                                            }

                                            data.Success = true;
                                            data.Code = "record_saved";
                                            data.Message = WebApiResources.NewPostCreateSuccess;
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
                                    data.Message = WebApiResources.NewPostCreateSuccess;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            data.Success = false;
                            data.Code = "error_occured";
                            data.Message = WebApiResources.ErrorOccured;
                            ExceptionUtility.LogException(ex, "staff/create-school-post");
                        }
                        finally
                        {
                            fcm.StaffPostSendNotification(1, value.staff_id, value.school_id, value.post_message, 0, value.create_by);
                            conn.Close();
                        }
                    }
                }
                else
                {
                    data.Success = false;
                    data.Code = "missing_params";
                    data.Message = "Missing parameters.";

                    return data;
                }
                return data;
            }
            else
            {
                data.Success = false;
                data.Code = "missing_params";
                data.Message = "Missing parameters.";

                return data;
            }
        }

        public SchoolDataModel Update_School_Post(string culture, UpdateSchoolPostParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            bool upload = false;
            FirebaseCloudMessagingPush fcm = new FirebaseCloudMessagingPush();
            FileTransferProtocol ftp = new FileTransferProtocol();
            SchoolDataModel data = new SchoolDataModel();
            if (value != null)
            {
                if (value.post_id != 0 && value.school_id != 0 && value.staff_id != 0 && value.post_message != null && value.start_at != null && value.end_at != null && value.update_by != null)
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
                    string time_stamp = dt.ToString("yyyy_MM_dd");
                    string photo_url = "/images/schools/" + value.school_id + "/" + time_stamp + "/" + value.file_name;

                    using (MySqlConnection conn = new MySqlConnection(constr))
                    {
                        try
                        {
                            conn.Open();
                            trans = conn.BeginTransaction();

                            using (MySqlCommand cmd = new MySqlCommand("sp_update_school_post", conn))
                            {
                                cmd.Transaction = trans;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@p_post_id", value.post_id);
                                cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                                cmd.Parameters.AddWithValue("@p_staff_id", value.staff_id);
                                cmd.Parameters.AddWithValue("@p_post_message", value.post_message);
                                cmd.Parameters.AddWithValue("@p_start_at", value.start_at);
                                cmd.Parameters.AddWithValue("@p_end_at", value.end_at);
                                cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                                MySqlDataReader dataReader = cmd.ExecuteReader();
                                dataReader.Close();
                                trans.Commit();

                                if (value.file_name != null && value.photo_base64 != null)
                                {
                                    var inputText = ValidateBase64EncodedString(value.photo_base64);
                                    byte[] fileBytes = Convert.FromBase64String(inputText);
                                    string directory = "/images/schools/" + value.school_id + "/" + time_stamp;
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
                                        string folder_school_id = "/images/schools/" + value.school_id;
                                        string folder_timestamp = "/images/schools/" + value.school_id + "/" + time_stamp;

                                        if (ftp.Check_Directory_Exists(ftp_address, folder_school_id, ftp_username, ftp_password) == true) //school id
                                        {
                                            if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true) //time stamp
                                            {
                                                try
                                                {
                                                    //Create FTP Request.
                                                    FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/" + time_stamp + "/" + value.file_name);
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
                                                if (ftp.Create_Directory(ftp_address, folder_timestamp, ftp_username, ftp_password) == true) // time stamp
                                                {
                                                    if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                    {
                                                        try
                                                        {
                                                            //Create FTP Request.
                                                            FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/" + time_stamp + "/" + value.file_name);
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
                                        else
                                        {
                                            if (ftp.Create_Directory(ftp_address, folder_school_id, ftp_username, ftp_password) == true)
                                            {
                                                if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                {
                                                    try
                                                    {
                                                        //Create FTP Request.
                                                        FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/" + time_stamp + "/" + value.file_name);
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
                                                    if (ftp.Create_Directory(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                    {
                                                        if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                        {
                                                            try
                                                            {
                                                                //Create FTP Request.
                                                                FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/" + time_stamp + "/" + value.file_name);
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

                                    if (upload == true)
                                    {
                                        try
                                        {
                                            using (MySqlCommand cmd2 = new MySqlCommand("sp_update_school_post_photo", conn))
                                            {
                                                trans2 = conn.BeginTransaction();
                                                cmd2.Transaction = trans2;
                                                cmd2.CommandType = CommandType.StoredProcedure;
                                                cmd2.Parameters.Clear();
                                                cmd2.Parameters.AddWithValue("@p_post_id", value.post_id);
                                                cmd2.Parameters.AddWithValue("@p_school_id", value.school_id);
                                                cmd2.Parameters.AddWithValue("@p_file_name", value.file_name);
                                                cmd2.Parameters.AddWithValue("@p_photo_url", photo_url);
                                                MySqlDataReader dataReader2 = cmd2.ExecuteReader();
                                                dataReader2.Close();
                                                trans2.Commit();
                                            }

                                            data.Success = true;
                                            data.Code = "record_saved";
                                            data.Message = WebApiResources.PostUpdateSuccess;
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
                                    data.Message = WebApiResources.PostUpdateSuccess;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            data.Success = false;
                            data.Code = "error_occured";
                            data.Message = WebApiResources.ErrorOccured;
                            ExceptionUtility.LogException(ex, "staff/update-school-post");
                        }
                        finally
                        {
                            fcm.StaffPostSendNotification(1, value.staff_id, value.school_id, value.post_message, 0, value.update_by);
                            conn.Close();
                        }
                    }
                }
                else
                {
                    data.Success = false;
                    data.Code = "missing_params";
                    data.Message = "Missing parameters.";

                    return data;
                }
                return data;
            }
            else
            {
                data.Success = false;
                data.Code = "missing_params";
                data.Message = "Missing parameters.";

                return data;
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
        public SchoolDataModel Insert_School_Class_Post(string culture, CreateSchoolPostParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;

            }

            bool upload = false;
            FirebaseCloudMessagingPush fcm = new FirebaseCloudMessagingPush();
            FileTransferProtocol ftp = new FileTransferProtocol();
            SchoolDataModel data = new SchoolDataModel();
            if (value != null)
            {
                if (value.school_id != 0 && value.class_id != 0 && value.staff_id != 0 && value.post_message != null && value.start_at != null && value.end_at != null && value.create_by != null)
                {
                    MySqlTransaction trans = null;
                    MySqlTransaction trans2 = null;
                    data.Success = false;
                    string sqlQuery = string.Empty;

                    int post_id;
                    string password = string.Empty;
                    string ftp_address = ConfigurationManager.AppSettings["ftpAddress"];
                    string ftp_username = ConfigurationManager.AppSettings["ftpUName"];
                    string ftp_password = ConfigurationManager.AppSettings["ftpPWord"];
                    string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;
                    DateTime dt = DateTime.Now;
                    string time_stamp = dt.ToString("yyyy_MM_dd");
                    string photo_url = "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp + "/" + value.file_name;

                    using (MySqlConnection conn = new MySqlConnection(constr))
                    {
                        try
                        {
                            conn.Open();
                            trans = conn.BeginTransaction();
                            using (MySqlCommand cmd = new MySqlCommand("sp_insert_school_class_post", conn))
                            {
                                cmd.Transaction = trans;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                                cmd.Parameters.AddWithValue("@p_class_id", value.class_id);
                                cmd.Parameters.AddWithValue("@p_staff_id", value.staff_id);
                                cmd.Parameters.AddWithValue("@p_post_message", value.post_message);
                                cmd.Parameters.AddWithValue("@p_start_at", value.start_at);
                                cmd.Parameters.AddWithValue("@p_end_at", value.end_at);
                                cmd.Parameters.AddWithValue("@p_create_by", value.create_by);
                                cmd.Parameters.Add("@PostId", MySqlDbType.Int16);
                                cmd.Parameters["@PostId"].Direction = ParameterDirection.Output;
                                MySqlDataReader dataReader = cmd.ExecuteReader();
                                dataReader.Close();
                                trans.Commit();

                                post_id = Convert.ToInt16(cmd.Parameters["@PostId"].Value);

                                if (value.file_name != null && value.photo_base64 != null)
                                {
                                    var inputText = ValidateBase64EncodedString(value.photo_base64);
                                    byte[] fileBytes = Convert.FromBase64String(inputText);
                                    string directory = "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp;
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
                                        string folder_school_id = "/images/schools/" + value.school_id;
                                        string folder_school_class = "/images/schools/" + value.school_id + "/class";
                                        string folder_school_class_id = "/images/schools/" + value.school_id + "/class/" + value.class_id;
                                        string folder_timestamp = "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp;

                                        if (ftp.Check_Directory_Exists(ftp_address, folder_school_id, ftp_username, ftp_password) == true) //school id
                                        {
                                            if (ftp.Check_Directory_Exists(ftp_address, folder_school_class, ftp_username, ftp_password) == true) //class 
                                            {
                                                if (ftp.Check_Directory_Exists(ftp_address, folder_school_class_id, ftp_username, ftp_password) == true) // class id
                                                {
                                                    if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true) // time stamp
                                                    {
                                                        try
                                                        {
                                                            //Create FTP Request.
                                                            FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp + "/" + value.file_name);
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
                                                        if (ftp.Create_Directory(ftp_address, folder_timestamp, ftp_username, ftp_password) == true) // create time stamp
                                                        {
                                                            if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true) // check time stamp
                                                            {
                                                                try
                                                                {
                                                                    //Create FTP Request.
                                                                    FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp + "/" + value.file_name);
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
                                                else
                                                {
                                                    if (ftp.Create_Directory(ftp_address, folder_school_class_id, ftp_username, ftp_password) == true)
                                                    {
                                                        if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                        {
                                                            try
                                                            {
                                                                //Create FTP Request.
                                                                FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp + "/" + value.file_name);
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
                                                            if (ftp.Create_Directory(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)// create time stamp
                                                            {
                                                                try
                                                                {
                                                                    //Create FTP Request.
                                                                    FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp + "/" + value.file_name);
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
                                                if (ftp.Create_Directory(ftp_address, folder_school_class, ftp_username, ftp_password) == true)
                                                {
                                                    if (ftp.Check_Directory_Exists(ftp_address, folder_school_class_id, ftp_username, ftp_password) == true)
                                                    {
                                                        if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                        {
                                                            try
                                                            {
                                                                //Create FTP Request.
                                                                FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp + "/" + value.file_name);
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
                                                            if (ftp.Create_Directory(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)// create time stamp
                                                            {
                                                                try
                                                                {
                                                                    //Create FTP Request.
                                                                    FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp + "/" + value.file_name);
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
                                                        if (ftp.Create_Directory(ftp_address, folder_school_class_id, ftp_username, ftp_password) == true)
                                                        {
                                                            if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                            {
                                                                try
                                                                {
                                                                    //Create FTP Request.
                                                                    FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp + "/" + value.file_name);
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
                                                                if (ftp.Create_Directory(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                                {
                                                                    try
                                                                    {
                                                                        //Create FTP Request.
                                                                        FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp + "/" + value.file_name);
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
                                        else
                                        {
                                            if (ftp.Create_Directory(ftp_address, folder_school_id, ftp_username, ftp_password) == true)
                                            {
                                                if (ftp.Check_Directory_Exists(ftp_address, folder_school_class, ftp_username, ftp_password) == true)
                                                {
                                                    if (ftp.Check_Directory_Exists(ftp_address, folder_school_class_id, ftp_username, ftp_password) == true)
                                                    {
                                                        if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                        {
                                                            try
                                                            {
                                                                //Create FTP Request.
                                                                FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp + "/" + value.file_name);
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

                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (ftp.Create_Directory(ftp_address, folder_school_class_id, ftp_username, ftp_password) == true)
                                                        {
                                                            if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                            {
                                                                try
                                                                {
                                                                    //Create FTP Request.
                                                                    FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp + "/" + value.file_name);
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
                                                                if (ftp.Create_Directory(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                                {
                                                                    try
                                                                    {
                                                                        //Create FTP Request.
                                                                        FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp + "/" + value.file_name);
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
                                                    if (ftp.Create_Directory(ftp_address, folder_school_class, ftp_username, ftp_password) == true)
                                                    {
                                                        if (ftp.Check_Directory_Exists(ftp_address, folder_school_class_id, ftp_username, ftp_password) == true)
                                                        {
                                                            if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                            {
                                                                try
                                                                {
                                                                    //Create FTP Request.
                                                                    FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp + "/" + value.file_name);
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
                                                                if (ftp.Create_Directory(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                                {
                                                                    try
                                                                    {
                                                                        //Create FTP Request.
                                                                        FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp + "/" + value.file_name);
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
                                                            if (ftp.Create_Directory(ftp_address, folder_school_class_id, ftp_username, ftp_password) == true)
                                                            {
                                                                if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                                {
                                                                    try
                                                                    {
                                                                        //Create FTP Request.
                                                                        FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp + "/" + value.file_name);
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
                                                                    if (ftp.Create_Directory(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                                    {
                                                                        try
                                                                        {
                                                                            //Create FTP Request.
                                                                            FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp + "/" + value.file_name);
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
                                        }
                                    }

                                    if (upload == true)
                                    {
                                        try
                                        {
                                            using (MySqlCommand cmd2 = new MySqlCommand("sp_update_school_class_post_photo", conn))
                                            {
                                                trans2 = conn.BeginTransaction();
                                                cmd2.Transaction = trans2;
                                                cmd2.CommandType = CommandType.StoredProcedure;
                                                cmd2.Parameters.Clear();
                                                cmd2.Parameters.AddWithValue("@p_post_id", post_id);
                                                cmd2.Parameters.AddWithValue("@p_school_id", value.school_id);
                                                cmd2.Parameters.AddWithValue("@p_class_id", value.class_id);
                                                cmd2.Parameters.AddWithValue("@p_file_name", value.file_name);
                                                cmd2.Parameters.AddWithValue("@p_photo_url", photo_url);
                                                MySqlDataReader dataReader2 = cmd2.ExecuteReader();
                                                dataReader2.Close();
                                                trans2.Commit();
                                            }

                                            data.Success = true;
                                            data.Code = "record_saved";
                                            data.Message = WebApiResources.NewPostCreateSuccess;
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
                                    data.Message = WebApiResources.NewPostCreateSuccess;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            data.Success = false;
                            data.Code = "error_occured";
                            data.Message = WebApiResources.ErrorOccured;
                            ExceptionUtility.LogException(ex, "staff/create-class-post");
                        }
                        finally
                        {
                            fcm.StaffPostSendNotification(2, value.staff_id, value.school_id, value.post_message, value.class_id, value.create_by);
                            conn.Close();
                        }
                    }
                }
                else
                {
                    data.Success = false;
                    data.Code = "missing_params";
                    data.Message = "Missing parameters.";

                    return data;
                }
                return data;
            }
            else
            {
                data.Success = false;
                data.Code = "missing_params";
                data.Message = "Missing parameters.";

                return data;
            }
        }

        public SchoolDataModel Update_School_Class_Post(string culture, UpdateSchoolPostParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            bool upload = false;
            FirebaseCloudMessagingPush fcm = new FirebaseCloudMessagingPush();
            FileTransferProtocol ftp = new FileTransferProtocol();
            SchoolDataModel data = new SchoolDataModel();
            if (value != null)
            {
                if (value.post_id != 0 && value.school_id != 0 && value.class_id != 0 && value.staff_id != 0 && value.post_message != null && value.start_at != null && value.end_at != null && value.update_by != null)
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
                    string time_stamp = dt.ToString("yyyy_MM_dd");
                    string photo_url = "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp + "/" + value.file_name;

                    using (MySqlConnection conn = new MySqlConnection(constr))
                    {
                        try
                        {
                            conn.Open();
                            trans = conn.BeginTransaction();
                            using (MySqlCommand cmd = new MySqlCommand("sp_update_school_class_post", conn))
                            {
                                cmd.Transaction = trans;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@p_post_id", value.post_id);
                                cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                                cmd.Parameters.AddWithValue("@p_class_id", value.class_id);
                                cmd.Parameters.AddWithValue("@p_staff_id", value.staff_id);
                                cmd.Parameters.AddWithValue("@p_post_message", value.post_message);
                                cmd.Parameters.AddWithValue("@p_start_at", value.start_at);
                                cmd.Parameters.AddWithValue("@p_end_at", value.end_at);
                                cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                                MySqlDataReader dataReader = cmd.ExecuteReader();
                                dataReader.Close();
                                trans.Commit();


                                if (value.file_name != null && value.photo_base64 != null)
                                {
                                    var inputText = ValidateBase64EncodedString(value.photo_base64);
                                    byte[] fileBytes = Convert.FromBase64String(inputText);
                                    string directory = "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp;
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
                                        string folder_school_id = "/images/schools/" + value.school_id;
                                        string folder_school_class = "/images/schools/" + value.school_id + "/class";
                                        string folder_school_class_id = "/images/schools/" + value.school_id + "/class/" + value.class_id;
                                        string folder_timestamp = "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp;

                                        if (ftp.Check_Directory_Exists(ftp_address, folder_school_id, ftp_username, ftp_password) == true) //school id
                                        {
                                            if (ftp.Check_Directory_Exists(ftp_address, folder_school_class, ftp_username, ftp_password) == true) //class 
                                            {
                                                if (ftp.Check_Directory_Exists(ftp_address, folder_school_class_id, ftp_username, ftp_password) == true) // class id
                                                {
                                                    if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true) // time stamp
                                                    {
                                                        try
                                                        {
                                                            //Create FTP Request.
                                                            FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp + "/" + value.file_name);
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
                                                        if (ftp.Create_Directory(ftp_address, folder_timestamp, ftp_username, ftp_password) == true) // create time stamp
                                                        {
                                                            if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true) // check time stamp
                                                            {
                                                                try
                                                                {
                                                                    //Create FTP Request.
                                                                    FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp + "/" + value.file_name);
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
                                                else
                                                {
                                                    if (ftp.Create_Directory(ftp_address, folder_school_class_id, ftp_username, ftp_password) == true)
                                                    {
                                                        if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                        {
                                                            try
                                                            {
                                                                //Create FTP Request.
                                                                FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp + "/" + value.file_name);
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
                                                            if (ftp.Create_Directory(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)// create time stamp
                                                            {
                                                                try
                                                                {
                                                                    //Create FTP Request.
                                                                    FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp + "/" + value.file_name);
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
                                                if (ftp.Create_Directory(ftp_address, folder_school_class, ftp_username, ftp_password) == true)
                                                {
                                                    if (ftp.Check_Directory_Exists(ftp_address, folder_school_class_id, ftp_username, ftp_password) == true)
                                                    {
                                                        if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                        {
                                                            try
                                                            {
                                                                //Create FTP Request.
                                                                FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp + "/" + value.file_name);
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
                                                            if (ftp.Create_Directory(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)// create time stamp
                                                            {
                                                                try
                                                                {
                                                                    //Create FTP Request.
                                                                    FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp + "/" + value.file_name);
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
                                                        if (ftp.Create_Directory(ftp_address, folder_school_class_id, ftp_username, ftp_password) == true)
                                                        {
                                                            if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                            {
                                                                try
                                                                {
                                                                    //Create FTP Request.
                                                                    FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp + "/" + value.file_name);
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
                                                                if (ftp.Create_Directory(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                                {
                                                                    try
                                                                    {
                                                                        //Create FTP Request.
                                                                        FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp + "/" + value.file_name);
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
                                        else
                                        {
                                            if (ftp.Create_Directory(ftp_address, folder_school_id, ftp_username, ftp_password) == true)
                                            {
                                                if (ftp.Check_Directory_Exists(ftp_address, folder_school_class, ftp_username, ftp_password) == true)
                                                {
                                                    if (ftp.Check_Directory_Exists(ftp_address, folder_school_class_id, ftp_username, ftp_password) == true)
                                                    {
                                                        if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                        {
                                                            try
                                                            {
                                                                //Create FTP Request.
                                                                FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp + "/" + value.file_name);
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

                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (ftp.Create_Directory(ftp_address, folder_school_class_id, ftp_username, ftp_password) == true)
                                                        {
                                                            if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                            {
                                                                try
                                                                {
                                                                    //Create FTP Request.
                                                                    FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp + "/" + value.file_name);
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
                                                                if (ftp.Create_Directory(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                                {
                                                                    try
                                                                    {
                                                                        //Create FTP Request.
                                                                        FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp + "/" + value.file_name);
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
                                                    if (ftp.Create_Directory(ftp_address, folder_school_class, ftp_username, ftp_password) == true)
                                                    {
                                                        if (ftp.Check_Directory_Exists(ftp_address, folder_school_class_id, ftp_username, ftp_password) == true)
                                                        {
                                                            if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                            {
                                                                try
                                                                {
                                                                    //Create FTP Request.
                                                                    FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp + "/" + value.file_name);
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
                                                                if (ftp.Create_Directory(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                                {
                                                                    try
                                                                    {
                                                                        //Create FTP Request.
                                                                        FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp + "/" + value.file_name);
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
                                                            if (ftp.Create_Directory(ftp_address, folder_school_class_id, ftp_username, ftp_password) == true)
                                                            {
                                                                if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                                {
                                                                    try
                                                                    {
                                                                        //Create FTP Request.
                                                                        FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp + "/" + value.file_name);
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
                                                                    if (ftp.Create_Directory(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                                    {
                                                                        try
                                                                        {
                                                                            //Create FTP Request.
                                                                            FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/class/" + value.class_id + "/" + time_stamp + "/" + value.file_name);
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
                                        }
                                    }

                                    if (upload == true)
                                    {
                                        try
                                        {
                                            using (MySqlCommand cmd2 = new MySqlCommand("sp_update_school_class_post_photo", conn))
                                            {
                                                trans2 = conn.BeginTransaction();
                                                cmd2.Transaction = trans2;
                                                cmd2.CommandType = CommandType.StoredProcedure;
                                                cmd2.Parameters.Clear();
                                                cmd2.Parameters.AddWithValue("@p_post_id", value.post_id);
                                                cmd2.Parameters.AddWithValue("@p_school_id", value.school_id);
                                                cmd2.Parameters.AddWithValue("@p_class_id", value.class_id);
                                                cmd2.Parameters.AddWithValue("@p_file_name", value.file_name);
                                                cmd2.Parameters.AddWithValue("@p_photo_url", photo_url);
                                                MySqlDataReader dataReader2 = cmd2.ExecuteReader();
                                                dataReader2.Close();
                                                trans2.Commit();
                                            }

                                            data.Success = true;
                                            data.Code = "record_saved";
                                            data.Message = WebApiResources.PostUpdateSuccess;
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
                                    data.Message = WebApiResources.PostUpdateSuccess;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            data.Success = false;
                            data.Code = "error_occured";
                            data.Message = WebApiResources.ErrorOccured;
                            ExceptionUtility.LogException(ex, "staff/update-class-post");
                        }
                        finally
                        {
                            fcm.StaffPostSendNotification(2, value.staff_id, value.school_id, value.post_message, value.class_id, value.update_by);
                            conn.Close();
                        }
                    }
                }
                else
                {
                    data.Success = false;
                    data.Code = "missing_params";
                    data.Message = "Missing parameters.";

                    return data;
                }
                return data;
            }
            else
            {
                data.Success = false;
                data.Code = "missing_params";
                data.Message = "Missing parameters.";

                return data;
            }
        }

        public SchoolDataModel Insert_School_Club_Post(string culture, CreateSchoolPostParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;

            }
            bool upload = false;
            FirebaseCloudMessagingPush fcm = new FirebaseCloudMessagingPush();
            FileTransferProtocol ftp = new FileTransferProtocol();
            SchoolDataModel data = new SchoolDataModel();
            if (value != null)
            {
                if (value.school_id != 0 && value.club_id != 0 && value.staff_id != 0 && value.post_message != null && value.start_at != null && value.end_at != null && value.create_by != null)
                {
                    MySqlTransaction trans = null;
                    MySqlTransaction trans2 = null;
                    data.Success = false;
                    string sqlQuery = string.Empty;

                    int post_id;
                    string password = string.Empty;
                    string ftp_address = ConfigurationManager.AppSettings["ftpAddress"];
                    string ftp_username = ConfigurationManager.AppSettings["ftpUName"];
                    string ftp_password = ConfigurationManager.AppSettings["ftpPWord"];
                    string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;
                    DateTime dt = DateTime.Now;
                    string time_stamp = dt.ToString("yyyy_MM_dd");
                    string photo_url = "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp + "/" + value.file_name;

                    using (MySqlConnection conn = new MySqlConnection(constr))
                    {
                        try
                        {
                            conn.Open();
                            trans = conn.BeginTransaction();
                            using (MySqlCommand cmd = new MySqlCommand("sp_insert_school_club_post", conn))
                            {
                                cmd.Transaction = trans;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                                cmd.Parameters.AddWithValue("@p_club_id", value.club_id);
                                cmd.Parameters.AddWithValue("@p_staff_id", value.staff_id);
                                cmd.Parameters.AddWithValue("@p_post_message", value.post_message);
                                cmd.Parameters.AddWithValue("@p_start_at", value.start_at);
                                cmd.Parameters.AddWithValue("@p_end_at", value.end_at);
                                cmd.Parameters.AddWithValue("@p_create_by", value.create_by);
                                cmd.Parameters.Add("@PostId", MySqlDbType.Int16);
                                cmd.Parameters["@PostId"].Direction = ParameterDirection.Output;
                                MySqlDataReader dataReader = cmd.ExecuteReader();
                                dataReader.Close();
                                trans.Commit();

                                post_id = Convert.ToInt16(cmd.Parameters["@PostId"].Value);

                                if (value.file_name != null && value.photo_base64 != null)
                                {
                                    var inputText = ValidateBase64EncodedString(value.photo_base64);
                                    byte[] fileBytes = Convert.FromBase64String(inputText);
                                    string directory = "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp;
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
                                        string folder_school_id = "/images/schools/" + value.school_id;
                                        string folder_school_club = "/images/schools/" + value.school_id + "/clubs";
                                        string folder_school_club_id = "/images/schools/" + value.school_id + "/clubs/" + value.club_id;
                                        string folder_timestamp = "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp;

                                        if (ftp.Check_Directory_Exists(ftp_address, folder_school_id, ftp_username, ftp_password) == true) //school id
                                        {
                                            if (ftp.Check_Directory_Exists(ftp_address, folder_school_club, ftp_username, ftp_password) == true) //clubs 
                                            {
                                                if (ftp.Check_Directory_Exists(ftp_address, folder_school_club_id, ftp_username, ftp_password) == true) // club id
                                                {
                                                    if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true) // time stamp
                                                    {
                                                        try
                                                        {
                                                            //Create FTP Request.
                                                            FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp + "/" + value.file_name);
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
                                                        if (ftp.Create_Directory(ftp_address, folder_timestamp, ftp_username, ftp_password) == true) // create time stamp
                                                        {
                                                            if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true) // check time stamp
                                                            {
                                                                try
                                                                {
                                                                    //Create FTP Request.
                                                                    FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp + "/" + value.file_name);
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
                                                else
                                                {
                                                    if (ftp.Create_Directory(ftp_address, folder_school_club_id, ftp_username, ftp_password) == true)
                                                    {
                                                        if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                        {
                                                            try
                                                            {
                                                                //Create FTP Request.
                                                                FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp + "/" + value.file_name);
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
                                                            if (ftp.Create_Directory(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)// create time stamp
                                                            {
                                                                try
                                                                {
                                                                    //Create FTP Request.
                                                                    FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp + "/" + value.file_name);
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
                                                if (ftp.Create_Directory(ftp_address, folder_school_club, ftp_username, ftp_password) == true)
                                                {
                                                    if (ftp.Check_Directory_Exists(ftp_address, folder_school_club_id, ftp_username, ftp_password) == true)
                                                    {
                                                        if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                        {
                                                            try
                                                            {
                                                                //Create FTP Request.
                                                                FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp + "/" + value.file_name);
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
                                                            if (ftp.Create_Directory(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)// create time stamp
                                                            {
                                                                try
                                                                {
                                                                    //Create FTP Request.
                                                                    FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp + "/" + value.file_name);
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
                                                        if (ftp.Create_Directory(ftp_address, folder_school_club_id, ftp_username, ftp_password) == true)
                                                        {
                                                            if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                            {
                                                                try
                                                                {
                                                                    //Create FTP Request.
                                                                    FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp + "/" + value.file_name);
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
                                                                if (ftp.Create_Directory(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                                {
                                                                    try
                                                                    {
                                                                        //Create FTP Request.
                                                                        FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp + "/" + value.file_name);
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
                                        else
                                        {
                                            if (ftp.Create_Directory(ftp_address, folder_school_id, ftp_username, ftp_password) == true)
                                            {
                                                if (ftp.Check_Directory_Exists(ftp_address, folder_school_club, ftp_username, ftp_password) == true)
                                                {
                                                    if (ftp.Check_Directory_Exists(ftp_address, folder_school_club_id, ftp_username, ftp_password) == true)
                                                    {
                                                        if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                        {
                                                            try
                                                            {
                                                                //Create FTP Request.
                                                                FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp + "/" + value.file_name);
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

                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (ftp.Create_Directory(ftp_address, folder_school_club_id, ftp_username, ftp_password) == true)
                                                        {
                                                            if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                            {
                                                                try
                                                                {
                                                                    //Create FTP Request.
                                                                    FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp + "/" + value.file_name);
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
                                                                if (ftp.Create_Directory(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                                {
                                                                    try
                                                                    {
                                                                        //Create FTP Request.
                                                                        FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp + "/" + value.file_name);
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
                                                    if (ftp.Create_Directory(ftp_address, folder_school_club, ftp_username, ftp_password) == true)
                                                    {
                                                        if (ftp.Check_Directory_Exists(ftp_address, folder_school_club_id, ftp_username, ftp_password) == true)
                                                        {
                                                            if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                            {
                                                                try
                                                                {
                                                                    //Create FTP Request.
                                                                    FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp + "/" + value.file_name);
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
                                                                if (ftp.Create_Directory(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                                {
                                                                    try
                                                                    {
                                                                        //Create FTP Request.
                                                                        FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp + "/" + value.file_name);
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
                                                            if (ftp.Create_Directory(ftp_address, folder_school_club_id, ftp_username, ftp_password) == true)
                                                            {
                                                                if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                                {
                                                                    try
                                                                    {
                                                                        //Create FTP Request.
                                                                        FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp + "/" + value.file_name);
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
                                                                    if (ftp.Create_Directory(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                                    {
                                                                        try
                                                                        {
                                                                            //Create FTP Request.
                                                                            FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp + "/" + value.file_name);
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
                                        }
                                    }

                                    if (upload == true)
                                    {
                                        try
                                        {
                                            using (MySqlCommand cmd2 = new MySqlCommand("sp_update_school_club_post_photo", conn))
                                            {
                                                trans2 = conn.BeginTransaction();
                                                cmd2.Transaction = trans2;
                                                cmd2.CommandType = CommandType.StoredProcedure;
                                                cmd2.Parameters.Clear();
                                                cmd2.Parameters.AddWithValue("@p_post_id", post_id);
                                                cmd2.Parameters.AddWithValue("@p_school_id", value.school_id);
                                                cmd2.Parameters.AddWithValue("@p_club_id", value.club_id);
                                                cmd2.Parameters.AddWithValue("@p_file_name", value.file_name);
                                                cmd2.Parameters.AddWithValue("@p_photo_url", photo_url);
                                                MySqlDataReader dataReader2 = cmd2.ExecuteReader();
                                                dataReader2.Close();
                                                trans2.Commit();
                                            }

                                            data.Success = true;
                                            data.Code = "record_saved";
                                            data.Message = WebApiResources.NewPostCreateSuccess;
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
                                    data.Message = WebApiResources.NewPostCreateSuccess;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            data.Success = false;
                            data.Code = "error_occured";
                            data.Message = WebApiResources.ErrorOccured;
                            ExceptionUtility.LogException(ex, "staff/create-club-post");
                        }
                        finally
                        {
                            fcm.StaffPostSendNotification(3, value.staff_id, value.school_id, value.post_message, value.club_id, value.create_by);
                            conn.Close();
                        }
                    }
                }
                else
                {
                    data.Success = false;
                    data.Code = "missing_params";
                    data.Message = "Missing parameters.";

                    return data;
                }
                return data;
            }
            else
            {
                data.Success = false;
                data.Code = "missing_params";
                data.Message = "Missing parameters.";

                return data;
            }
        }

        public SchoolDataModel Update_School_Club_Post(string culture, UpdateSchoolPostParam value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            bool upload = false;
            FirebaseCloudMessagingPush fcm = new FirebaseCloudMessagingPush();
            FileTransferProtocol ftp = new FileTransferProtocol();
            SchoolDataModel data = new SchoolDataModel();
            if (value != null)
            {
                if (value.post_id != 0 && value.school_id != 0 && value.club_id != 0 && value.staff_id != 0 && value.post_message != null && value.start_at != null && value.end_at != null && value.update_by != null)
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
                    string time_stamp = dt.ToString("yyyy_MM_dd");
                    string photo_url = "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp + "/" + value.file_name;

                    using (MySqlConnection conn = new MySqlConnection(constr))
                    {
                        try
                        {
                            conn.Open();
                            trans = conn.BeginTransaction();
                            using (MySqlCommand cmd = new MySqlCommand("sp_update_school_club_post", conn))
                            {
                                cmd.Transaction = trans;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@p_post_id", value.post_id);
                                cmd.Parameters.AddWithValue("@p_school_id", value.school_id);
                                cmd.Parameters.AddWithValue("@p_club_id", value.club_id);
                                cmd.Parameters.AddWithValue("@p_staff_id", value.staff_id);
                                cmd.Parameters.AddWithValue("@p_post_message", value.post_message);
                                cmd.Parameters.AddWithValue("@p_start_at", value.start_at);
                                cmd.Parameters.AddWithValue("@p_end_at", value.end_at);
                                cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                                MySqlDataReader dataReader = cmd.ExecuteReader();
                                dataReader.Close();
                                trans.Commit();

                                if (value.file_name != null && value.photo_base64 != null)
                                {
                                    var inputText = ValidateBase64EncodedString(value.photo_base64);
                                    byte[] fileBytes = Convert.FromBase64String(inputText);
                                    string directory = "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp;
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
                                        string folder_school_id = "/images/schools/" + value.school_id;
                                        string folder_school_club = "/images/schools/" + value.school_id + "/clubs";
                                        string folder_school_club_id = "/images/schools/" + value.school_id + "/clubs/" + value.club_id;
                                        string folder_timestamp = "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp;

                                        if (ftp.Check_Directory_Exists(ftp_address, folder_school_id, ftp_username, ftp_password) == true) //school id
                                        {
                                            if (ftp.Check_Directory_Exists(ftp_address, folder_school_club, ftp_username, ftp_password) == true) //clubs 
                                            {
                                                if (ftp.Check_Directory_Exists(ftp_address, folder_school_club_id, ftp_username, ftp_password) == true) // club id
                                                {
                                                    if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true) // time stamp
                                                    {
                                                        try
                                                        {
                                                            //Create FTP Request.
                                                            FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp + "/" + value.file_name);
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
                                                        if (ftp.Create_Directory(ftp_address, folder_timestamp, ftp_username, ftp_password) == true) // create time stamp
                                                        {
                                                            if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true) // check time stamp
                                                            {
                                                                try
                                                                {
                                                                    //Create FTP Request.
                                                                    FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp + "/" + value.file_name);
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
                                                else
                                                {
                                                    if (ftp.Create_Directory(ftp_address, folder_school_club_id, ftp_username, ftp_password) == true)
                                                    {
                                                        if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                        {
                                                            try
                                                            {
                                                                //Create FTP Request.
                                                                FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp + "/" + value.file_name);
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
                                                            if (ftp.Create_Directory(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)// create time stamp
                                                            {
                                                                try
                                                                {
                                                                    //Create FTP Request.
                                                                    FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp + "/" + value.file_name);
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
                                                if (ftp.Create_Directory(ftp_address, folder_school_club, ftp_username, ftp_password) == true)
                                                {
                                                    if (ftp.Check_Directory_Exists(ftp_address, folder_school_club_id, ftp_username, ftp_password) == true)
                                                    {
                                                        if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                        {
                                                            try
                                                            {
                                                                //Create FTP Request.
                                                                FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp + "/" + value.file_name);
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
                                                            if (ftp.Create_Directory(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)// create time stamp
                                                            {
                                                                try
                                                                {
                                                                    //Create FTP Request.
                                                                    FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp + "/" + value.file_name);
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
                                                        if (ftp.Create_Directory(ftp_address, folder_school_club_id, ftp_username, ftp_password) == true)
                                                        {
                                                            if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                            {
                                                                try
                                                                {
                                                                    //Create FTP Request.
                                                                    FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp + "/" + value.file_name);
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
                                                                if (ftp.Create_Directory(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                                {
                                                                    try
                                                                    {
                                                                        //Create FTP Request.
                                                                        FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp + "/" + value.file_name);
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
                                        else
                                        {
                                            if (ftp.Create_Directory(ftp_address, folder_school_id, ftp_username, ftp_password) == true)
                                            {
                                                if (ftp.Check_Directory_Exists(ftp_address, folder_school_club, ftp_username, ftp_password) == true)
                                                {
                                                    if (ftp.Check_Directory_Exists(ftp_address, folder_school_club_id, ftp_username, ftp_password) == true)
                                                    {
                                                        if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                        {
                                                            try
                                                            {
                                                                //Create FTP Request.
                                                                FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp + "/" + value.file_name);
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

                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (ftp.Create_Directory(ftp_address, folder_school_club_id, ftp_username, ftp_password) == true)
                                                        {
                                                            if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                            {
                                                                try
                                                                {
                                                                    //Create FTP Request.
                                                                    FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp + "/" + value.file_name);
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
                                                                if (ftp.Create_Directory(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                                {
                                                                    try
                                                                    {
                                                                        //Create FTP Request.
                                                                        FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp + "/" + value.file_name);
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
                                                    if (ftp.Create_Directory(ftp_address, folder_school_club, ftp_username, ftp_password) == true)
                                                    {
                                                        if (ftp.Check_Directory_Exists(ftp_address, folder_school_club_id, ftp_username, ftp_password) == true)
                                                        {
                                                            if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                            {
                                                                try
                                                                {
                                                                    //Create FTP Request.
                                                                    FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp + "/" + value.file_name);
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
                                                                if (ftp.Create_Directory(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                                {
                                                                    try
                                                                    {
                                                                        //Create FTP Request.
                                                                        FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp + "/" + value.file_name);
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
                                                            if (ftp.Create_Directory(ftp_address, folder_school_club_id, ftp_username, ftp_password) == true)
                                                            {
                                                                if (ftp.Check_Directory_Exists(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                                {
                                                                    try
                                                                    {
                                                                        //Create FTP Request.
                                                                        FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp + "/" + value.file_name);
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
                                                                    if (ftp.Create_Directory(ftp_address, folder_timestamp, ftp_username, ftp_password) == true)
                                                                    {
                                                                        try
                                                                        {
                                                                            //Create FTP Request.
                                                                            FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/schools/" + value.school_id + "/clubs/" + value.club_id + "/" + time_stamp + "/" + value.file_name);
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
                                        }
                                    }

                                    if (upload == true)
                                    {
                                        try
                                        {
                                            using (MySqlCommand cmd2 = new MySqlCommand("sp_update_school_club_post_photo", conn))
                                            {
                                                trans2 = conn.BeginTransaction();
                                                cmd2.Transaction = trans2;
                                                cmd2.CommandType = CommandType.StoredProcedure;
                                                cmd2.Parameters.Clear();
                                                cmd2.Parameters.AddWithValue("@p_post_id", value.post_id);
                                                cmd2.Parameters.AddWithValue("@p_school_id", value.school_id);
                                                cmd2.Parameters.AddWithValue("@p_club_id", value.club_id);
                                                cmd2.Parameters.AddWithValue("@p_file_name", value.file_name);
                                                cmd2.Parameters.AddWithValue("@p_photo_url", photo_url);
                                                MySqlDataReader dataReader2 = cmd2.ExecuteReader();
                                                dataReader2.Close();
                                                trans2.Commit();
                                            }

                                            data.Success = true;
                                            data.Code = "record_saved";
                                            data.Message = WebApiResources.PostUpdateSuccess;
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
                                    data.Message = WebApiResources.PostUpdateSuccess;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            data.Success = false;
                            data.Code = "error_occured";
                            data.Message = WebApiResources.ErrorOccured;
                            ExceptionUtility.LogException(ex, "staff/update-club-post");
                        }
                        finally
                        {
                            fcm.StaffPostSendNotification(3, value.staff_id, value.school_id, value.post_message, value.club_id, value.update_by);
                            conn.Close();
                        }
                    }
                }
                else
                {
                    data.Success = false;
                    data.Code = "missing_params";
                    data.Message = "Missing parameters.";

                    return data;
                }
                return data;
            }
            else
            {
                data.Success = false;
                data.Code = "missing_params";
                data.Message = "Missing parameters.";

                return data;
            }
        }
    }
}