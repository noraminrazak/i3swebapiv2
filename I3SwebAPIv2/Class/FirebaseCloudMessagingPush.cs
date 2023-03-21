using I3SwebAPIv2.Models;
using MySql.Data.MySqlClient;
using Nancy.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;

namespace I3SwebAPIv2.Class
{
    public class FirebaseCloudMessagingPush
    {
        private const string WEB_ADDRESS = "https://fcm.googleapis.com/fcm/send";
        private const string SENDER_ID = "459292767069";
        private const string SERVER_KEY = "AAAAau_7c10:APA91bEP9YeSl-wscYnFkMytOAGddjbc0v9uqRf2sCp-vogzDoi9uAihdlOQy31wfCAsR5Fft16ycsAmLrmN2GWEs9cFQN4b4csQz7MPN1U3jFnTPFKiNlcTj1tNjhZBp7Oa5zpWWBBU";

        public void StaffPostSendNotification(int user_group_id, int staff_id, int first_id, string message, int second_id = 0, string full_name = "") 
        {
            List<UserDevice> list = new List<UserDevice>();
            MySqlTransaction trans = null;

            string procsName = string.Empty;

            if (user_group_id == 1)
            {
                procsName = "sp_get_school_user_device";
            }
            else if (user_group_id == 2)
            {
                procsName = "sp_get_school_class_user_device";
            }
            else if (user_group_id == 3)
            {
                procsName = "sp_get_school_club_user_device";
            }
            else if (user_group_id == 4)
            {
                procsName = "sp_get_user_device";
            }

            string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(constr))
            {
                try
                {
                    conn.Open();
                    trans = conn.BeginTransaction();

                    using (MySqlCommand cmd = new MySqlCommand(procsName, conn))
                    {
                        cmd.Transaction = trans;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();

                        if (user_group_id == 1)
                        {
                            cmd.Parameters.AddWithValue("@p_school_id", first_id);
                            cmd.Parameters.AddWithValue("@p_staff_id", staff_id);
                        }
                        else if (user_group_id == 2)
                        {
                            cmd.Parameters.AddWithValue("@p_school_id", first_id);
                            cmd.Parameters.AddWithValue("@p_class_id", second_id);
                            cmd.Parameters.AddWithValue("@p_staff_id", staff_id);
                        }
                        else if (user_group_id == 3)
                        {
                            cmd.Parameters.AddWithValue("@p_school_id", first_id);
                            cmd.Parameters.AddWithValue("@p_club_id", second_id);
                            cmd.Parameters.AddWithValue("@p_staff_id", staff_id);
                        }
                        else if (user_group_id == 4)
                        {
                            cmd.Parameters.AddWithValue("@p_profile_id", first_id);
                        }

                        MySqlDataReader dataReader = cmd.ExecuteReader();

                        if (dataReader.HasRows == true)
                        {
                            while (dataReader.Read())
                            {
                                UserDevice prop = new UserDevice();
                                prop.device_platform_id = dataReader["device_platform_id"] as int? ?? default(int);
                                prop.device_token = dataReader["device_token"].ToString();

                                if (user_group_id == 1)
                                {
                                    prop.group_name = dataReader["school_name"].ToString();
                                }
                                else if (user_group_id == 2)
                                {
                                    prop.group_name = dataReader["class_name"].ToString();
                                }
                                else if (user_group_id == 3)
                                {
                                    prop.group_name = dataReader["club_name"].ToString();
                                }

                                list.Add(prop);
                            }

                            foreach (UserDevice r in list)
                            {
                                if (r.device_platform_id == 1)
                                {
                                    SendNotification(r.device_token, r.group_name, message, "high", 0);
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtility.LogException(ex, "chat/send");
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public void ChatSendNotification(SendMessage value)
        {
            FirebaseCloudMessagingPush fcm = new FirebaseCloudMessagingPush();
            List<UserDevice> list = new List<UserDevice>();
            MySqlTransaction trans = null;

            string channelName = string.Empty;
            string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(constr))
            {
                try
                {
                    conn.Open();
                    trans = conn.BeginTransaction();

                    using (MySqlCommand cmd = new MySqlCommand("sp_get_channel_user_device", conn))
                    {
                        cmd.Transaction = trans;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@p_channel_id", value.channel_id);
                        cmd.Parameters.AddWithValue("@p_profile_id", value.sender_id);
                        MySqlDataReader dataReader = cmd.ExecuteReader();

                        if (dataReader.HasRows == true)
                        {
                            while (dataReader.Read())
                            {
                                UserDevice prop = new UserDevice();
                                prop.device_platform_id = dataReader["device_platform_id"] as int? ?? default(int);
                                prop.device_token = dataReader["device_token"].ToString();
                                list.Add(prop);
                            }

                            if (value.channel_type_id == 1)
                            {
                                channelName = value.create_by;
                            }
                            else
                            {
                                channelName = value.channel_name;
                            }

                            foreach (UserDevice r in list)
                            {
                                if (r.device_platform_id == 1)
                                {
                                    fcm.SendNotification(r.device_token, channelName, value.message, "high", 0);
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtility.LogException(ex, "chat/send");
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public void PurchaseOrderStatusNotification(int profile_id, string title, string message, string full_name = "")
        {
            List<UserDevice> list = new List<UserDevice>();
            MySqlTransaction trans = null;

            string constr = ConfigurationManager.ConnectionStrings["i3sConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(constr))
            {
                try
                {
                    conn.Open();
                    trans = conn.BeginTransaction();

                    using (MySqlCommand cmd = new MySqlCommand("sp_get_user_device", conn))
                    {
                        cmd.Transaction = trans;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@p_profile_id", profile_id);

                        MySqlDataReader dataReader = cmd.ExecuteReader();

                        if (dataReader.HasRows == true)
                        {
                            while (dataReader.Read())
                            {
                                UserDevice prop = new UserDevice();
                                prop.device_platform_id = dataReader["device_platform_id"] as int? ?? default(int);
                                prop.device_token = dataReader["device_token"].ToString();
                                list.Add(prop);
                            }

                            foreach (UserDevice r in list)
                            {
                                if (r.device_platform_id == 1)
                                {
                                    SendNotification(r.device_token, title, message, "high", 0);
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionUtility.LogException(ex, "purchase/order-received");
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public string SendNotification(string deviceToken, string title, string message, string priority = "high", int badge = 0, List<Tuple<string, string>> parameters = null)
        {
            var result = "-1";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(WEB_ADDRESS);

            parameters = parameters ?? new List<Tuple<string, string>>();

            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", SERVER_KEY));
            httpWebRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));
            httpWebRequest.Method = "POST";

            if (title.Length > 100)
                title = title.Substring(0, 95) + "...";

            //Message cannot exceed 100
            if (message.Length > 100)
                message = message.Substring(0, 95) + "...";

            JObject jObject = new JObject();
            jObject.Add("to", deviceToken);
            jObject.Add("priority", priority);
            jObject.Add("content_available", true);

            JObject jObjNotification = new JObject();
            jObjNotification.Add("click_action", ".MainActivity");
            jObjNotification.Add("body", message);
            jObjNotification.Add("title", title);

            jObject.Add("notification", jObjNotification);

            JObject jObjData = new JObject();

            foreach (Tuple<string, string> parameter in parameters)
            {
                jObjData.Add(parameter.Item1, parameter.Item2);
            }

            jObject.Add("data", jObjData);

            var serializer = new JavaScriptSerializer();
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = jObject.ToString();
                streamWriter.Write(json);
                streamWriter.Flush();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            return result;
        }
    }
}