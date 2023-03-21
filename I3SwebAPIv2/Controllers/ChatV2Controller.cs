using I3SwebAPIv2.Class;
using I3SwebAPIv2.Database;
using I3SwebAPIv2.Models;
using I3SwebAPIv2.Resources;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
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
    public class ChatV2Controller : ApiController
    {
        FirebaseDB firebaseDB = new FirebaseDB("https://i-3s-512d2.firebaseio.com");
        FirebaseCloudMessagingPush fcm = new FirebaseCloudMessagingPush();

        [HttpPost]
        [Route("api/v2/chat/send")]
        public IHttpActionResult Post_Chat_Send(string culture, [FromBody] SendMessage value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            string photo_url = string.Empty;
            bool proceed = false;
            FileTransferProtocol ftp = new FileTransferProtocol();
            UserDataModel data = new UserDataModel();

            if (value.channel_id != null && value.channel_type_id != 0 && value.channel_name != null &&
                value.sender_id != 0 && value.receiver_id != 0 &&  value.message_type_id != 0 && value.create_by != null)
            {

                if (value.message_type_id == 1)
                {
                    if (value.message != null)
                    {
                        proceed = true;
                    }
                }
                else 
                {
                    if (value.photo_base64 != null && value.file_name != null)
                    {
                        photo_url = "/images/chats/" + value.channel_id + "/" + value.file_name;
                        proceed = true;
                    }
                }

                if (proceed == true)
                {
                    MySqlTransaction trans = null;
                    data.Success = false;
                    string sqlQuery = string.Empty;
                    bool upload = false;

                    string ftp_address = ConfigurationManager.AppSettings["ftpAddress"];
                    string ftp_username = ConfigurationManager.AppSettings["ftpUName"];
                    string ftp_password = ConfigurationManager.AppSettings["ftpPWord"];
                    string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                    using (MySqlConnection conn = new MySqlConnection(constr))
                    {
                        try
                        {
                            conn.Open();
                            trans = conn.BeginTransaction();

                            using (MySqlCommand cmd = new MySqlCommand("sp_insert_message", conn))
                            {
                                cmd.Transaction = trans;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@p_channel_id", value.channel_id);
                                cmd.Parameters.AddWithValue("@p_channel_type_id", value.channel_type_id);
                                cmd.Parameters.AddWithValue("@p_channel_name", value.channel_name);
                                cmd.Parameters.AddWithValue("@p_message_type_id", value.message_type_id);
                                cmd.Parameters.AddWithValue("@p_sender_id", value.sender_id);
                                cmd.Parameters.AddWithValue("@p_receiver_id", value.receiver_id);

                                if (value.message_type_id == 1)
                                {
                                    cmd.Parameters.AddWithValue("@p_message", value.message);
                                    cmd.Parameters.AddWithValue("@p_photo_url", null);
                                    cmd.Parameters.AddWithValue("@p_file_name", null);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@p_message", value.file_name);
                                    cmd.Parameters.AddWithValue("@p_photo_url", photo_url);
                                    cmd.Parameters.AddWithValue("@p_file_name", value.file_name);
                                }

                                cmd.Parameters.AddWithValue("@p_create_by", value.create_by);
                                cmd.Parameters.Add("@p_message_id", MySqlDbType.Int16);
                                cmd.Parameters["@p_message_id"].Direction = ParameterDirection.Output;
                                cmd.Parameters.Add("@p_sender_name", MySqlDbType.VarChar);
                                cmd.Parameters["@p_sender_name"].Direction = ParameterDirection.Output;
                                cmd.Parameters.Add("@p_sender_photo_url", MySqlDbType.VarChar);
                                cmd.Parameters["@p_sender_photo_url"].Direction = ParameterDirection.Output;
                                cmd.Parameters.Add("@p_receiver_name", MySqlDbType.VarChar);
                                cmd.Parameters["@p_receiver_name"].Direction = ParameterDirection.Output;
                                cmd.Parameters.Add("@p_receiver_photo_url", MySqlDbType.VarChar);
                                cmd.Parameters["@p_receiver_photo_url"].Direction = ParameterDirection.Output;
                                cmd.ExecuteNonQuery();
                                trans.Commit();

                                var message_id = Convert.ToInt16(cmd.Parameters["@p_message_id"].Value);
                                var sender_name = cmd.Parameters["@p_sender_name"].Value.ToString();
                                var sender_photo_url = string.Empty;

                                if (cmd.Parameters["@p_sender_photo_url"].Value != DBNull.Value) 
                                {
                                    sender_photo_url = cmd.Parameters["@p_sender_photo_url"].Value.ToString();
                                }

                                var receiver_name = cmd.Parameters["@p_receiver_name"].Value.ToString();
                                var receiver_photo_url = string.Empty;

                                if (cmd.Parameters["@p_receiver_photo_url"].Value != DBNull.Value)
                                {
                                    receiver_photo_url = cmd.Parameters["@p_receiver_photo_url"].Value.ToString();
                                }

                                if (value.message_type_id == 2) 
                                {
                                    byte[] fileBytes = Convert.FromBase64String(value.photo_base64);
                                    string directory = "/images/chats/" + value.channel_id;

                                    if (ftp.Check_Directory_Exists(ftp_address, directory, ftp_username, ftp_password) == true)
                                    {
                                        if (ftp.Retrieve_Delete_Directory_File(ftp_address, directory, ftp_username, ftp_password) == true)
                                        {
                                            try
                                            {
                                                //Create FTP Request.
                                                FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/chats/" + value.channel_id + "/" + value.file_name);
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
                                        if (ftp.Create_Directory(ftp_address, directory, ftp_username, ftp_password) == true)
                                        {
                                            if (ftp.Check_Directory_Exists(ftp_address, directory, ftp_username, ftp_password) == true)
                                            {
                                                if (ftp.Retrieve_Delete_Directory_File(ftp_address, directory, ftp_username, ftp_password) == true)
                                                {
                                                    try
                                                    {
                                                        //Create FTP Request.
                                                        FtpWebRequest ftprequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + ftp_address + "/images/chats/" + value.channel_id + "/" + value.file_name);
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

                                if (value.message_type_id == 2)
                                {
                                    if (upload == true)
                                    {
                                        data.Success = true;
                                        data.Code = "record_saved";
                                        data.Message = "Message successfuly sent.";
                                    }
                                    else 
                                    {
                                        data.Success = false;
                                        data.Code = "fail";
                                        data.Message = "Images upload failed.";
                                    }
                                }
                                else 
                                {
                                    data.Success = true;
                                    data.Code = "record_saved";
                                    data.Message = "Message successfuly sent.";
                                }

                                FirebaseDB firebaseDBTeams = firebaseDB.NodePath("chats/" + value.channel_id + "");
                                var message = new ChatMessage()
                                {
                                    message_id = message_id,
                                    channel_id = value.channel_id,
                                    message_type_id = value.message_type_id,
                                    sender_id = value.sender_id,
                                    sender_name = sender_name,
                                    sender_photo_url = sender_photo_url,
                                    receiver_id = value.receiver_id,
                                    receiver_name = receiver_name,
                                    receiver_photo_url = receiver_photo_url,
                                    message = value.message,
                                    photo_url = photo_url,
                                    sent_at = DateTime.Now,
                                    create_by = value.create_by,
                                    create_at = DateTime.Now
                                };

                                FirebaseResponse postResponse = firebaseDBTeams.Post(JsonConvert.SerializeObject(message));
                            }
                        }
                        catch (Exception ex)
                        {
                            data.Success = false;
                            data.Code = "error_occured";
                            data.Message = WebApiResources.ErrorOccured;
                            ExceptionUtility.LogException(ex, "chat/send");
                        }
                        finally
                        {
                            fcm.ChatSendNotification(value);
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
        [Route("api/v2/chat/join-channel")]
        public IHttpActionResult Post_Chat_Join_Channel(string culture, [FromBody] JoinChannel value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }
            ChatDataModel data = new ChatDataModel();
            if (value.profile_id != 0 && value.channel_id != null && value.create_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_insert_channel_user", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                            cmd.Parameters.AddWithValue("@p_channel_id", value.channel_id);
                            cmd.Parameters.AddWithValue("@p_create_by", value.create_by);
                            cmd.Parameters.Add("@p_user_exists", MySqlDbType.Int16);
                            cmd.Parameters["@p_user_exists"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            var exist = Convert.ToInt16(cmd.Parameters["@p_user_exists"].Value);

                            if (exist == 0)
                            {
                                data.Success = true;
                                data.Code = "record_saved";
                                data.Message = WebApiResources.ChannelJoinSuccess;
                            }
                            else
                            {
                                data.Success = false;
                                data.Code = "record_exist";
                                data.Message = WebApiResources.ChannelAlreadyJoin;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "chat/join-channel");
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
        [Route("api/v2/chat/leave-channel")]
        public IHttpActionResult Post_Chat_Leave_Channel(string culture, [FromBody] LeaveChannel value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            ChatDataModel data = new ChatDataModel();
            if (value.profile_id != 0 && value.channel_id != null && value.update_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_delete_channel_user", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                            cmd.Parameters.AddWithValue("@p_channel_id", value.channel_id);
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            data.Success = true;
                            data.Code = "record_deleted";
                            data.Message = WebApiResources.ChannelLeaveSuccess;

                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "chat/leave-channel");
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
        [Route("api/v2/chat/user-status")]
        public IHttpActionResult Post_Chat_User_Status(string culture, [FromBody] ChannelUserStatus value)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var language = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = language;
            }

            ChatDataModel data = new ChatDataModel();
            if (value.channel_id != null && value.profile_id != 0 && value.status_id != 0 && value.update_by != null)
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_update_channel_user_status", conn))
                        {

                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_profile_id", value.profile_id);
                            cmd.Parameters.AddWithValue("@p_channel_id", value.channel_id);
                            cmd.Parameters.AddWithValue("@p_status_id", value.status_id);
                            cmd.Parameters.AddWithValue("@p_update_by", value.update_by);
                            cmd.ExecuteNonQuery();
                            trans.Commit();

                            data.Success = true;
                            data.Code = "record_updated";
                            data.Message = WebApiResources.StatusUpdateSuccess;

                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "chat/user-status");
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
        [Route("api/v2/chat/history")]
        public IHttpActionResult Post_Chat_History([FromBody] ChatHistoryParam value)
        {
            ParentDataModel data = new ParentDataModel();

            if (value.profile_id != 0)
            {
                List<ChatHistory> list = new List<ChatHistory>();
                ChatHistoryModel listData = new ChatHistoryModel();
                MySqlTransaction trans = null;
                listData.Success = false;
                string sqlQuery = string.Empty;
                string status_Code = string.Empty;
                string password = string.Empty;
                int channelTypeId = 0;
                string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_chat_history", conn))
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
                                    ChatHistory prop = new ChatHistory();
                                    prop.channel_id = dataReader["channel_id"].ToString();
                                    prop.channel_type_id = dataReader["channel_type_id"] as int? ?? default(int);
                                    prop.channel_type = dataReader["channel_type"].ToString();
                                    prop.channel_name = dataReader["channel_name"].ToString();
                                    prop.profile_id = Convert.ToInt16(dataReader["profile_id"]);
                                    prop.sender_id = Convert.ToInt16(dataReader["sender_id"]);
                                    prop.sender_name = dataReader["sender_name"].ToString();
                                    prop.sender_photo_url = dataReader["sender_photo_url"].ToString();
                                    prop.receiver_id = Convert.ToInt16(dataReader["receiver_id"]);
                                    prop.receiver_name = dataReader["receiver_name"].ToString();
                                    prop.receiver_photo_url = dataReader["receiver_photo_url"].ToString();
                                    prop.last_message = dataReader["last_message"].ToString();
                                    prop.sent_at = Convert.ToDateTime(dataReader["sent_at"].ToString());
                                    prop.unread_count = Convert.ToInt16(dataReader["unread_count"]);
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "chat_history";
                                listData.Message = "Chat history list.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Chat history could not be found.";
                                listData.Data = list;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "chat/history");
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
        [Route("api/v2/chat/channel-user")]
        public IHttpActionResult Post_Chat_Channel_Member([FromBody] ChannelUserParam value)
        {
            ChatDataModel data = new ChatDataModel();
            if (value.channel_id != null)
            {
                List<ChannelUser> list = new List<ChannelUser>();
                ChannelUserModel listData = new ChannelUserModel();
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

                        using (MySqlCommand cmd = new MySqlCommand("sp_get_channel_user", conn))
                        {
                            cmd.Transaction = trans;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p_channel_id", value.channel_id);
                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows == true)
                            {
                                while (dataReader.Read())
                                {
                                    ChannelUser prop = new ChannelUser();
                                    prop.profile_id = Convert.ToInt16(dataReader["profile_id"]);
                                    prop.full_name = dataReader["full_name"].ToString();
                                    prop.photo_url = dataReader["photo_url"].ToString();
                                    prop.status_id = Convert.ToInt16(dataReader["status_id"]);
                                    prop.status = dataReader["status"].ToString();
                                    prop.last_seen = Convert.ToDateTime(dataReader["last_seen"]);
                                    list.Add(prop);
                                }
                                listData.Success = true;
                                listData.Code = "channel_user_list";
                                listData.Message = "Channel user list.";
                                listData.Data = list;
                            }
                            else
                            {
                                //listData.Success = false;
                                listData.Success = true;
                                listData.Code = "no_record_found";
                                listData.Message = "Channel user could not be found.";
                                listData.Data = list;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        data.Success = false;
                        data.Code = "error_occured";
                        data.Message = WebApiResources.ErrorOccured;
                        ExceptionUtility.LogException(ex, "chat/channel-user");
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
