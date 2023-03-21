using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace I3SwebAPIv2.Models
{
    public class PushMessage
    {
        public int message_from { get; set; }
        public int message_to { get; set; }
        public string push_type { get; set; }
        public string channel_id { get; set; }
        public string channel_name { get; set; }
        public int channel_type_id { get; set; }
        public string channel_photo_url { get; set; }
        public string full_name { get; set; }
        public string message { get; set; }
        public DateTime time_message { get; set; }
        public string photo_url { get; set; }
        public string attach_url { get; set; }
        public int first_id { get; set; }
        public string first_name { get; set; }
        public int second_id { get; set; }
        public string second_name { get; set; }
        public string node_path { get; set; }

    }

    public class SendMessage
    {
        public string channel_id { get; set; }
        public string channel_name { get; set; }
        public int channel_type_id { get; set; }
        public int sender_id { get; set; }
        public int receiver_id { get; set; }
        public int message_type_id { get; set; }
        public string message { get; set; }
        public string photo_base64 { get; set; }
        public string file_name { get; set; }
        public string create_by { get; set; }
    }

    public class ChatMessage
    {
        public int message_id { get; set; }
        public string channel_id { get; set; }
        public int sender_id { get; set; }
        public string sender_name { get; set; }
        public string sender_photo_url { get; set; }
        public int receiver_id { get; set; }
        public string receiver_name { get; set; }
        public string receiver_photo_url { get; set; }
        public int message_type_id { get; set; }
        public string message { get; set; }
        public string photo_url { get; set; }
        public DateTime sent_at { get; set; }
        public string create_by { get; set; }
        public DateTime create_at { get; set; }
    }



    public class ChannelUserParam
    {
        public string channel_id { get; set; }
    }
    public class ChannelUserModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ChannelUser> Data { get; set; }
    }

    public class ChannelUser
    {
        public int profile_id { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public int status_id { get; set; }
        public string status { get; set; }
        public DateTime last_seen { get; set; }
    }

    public class ChatHistoryParam
    {
        public int profile_id { get; set; }
    }

    public class ChatHistoryModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ChatHistory> Data { get; set; }
    }

    public class ChatHistory
    {
        public string channel_id { get; set; }
        public int channel_type_id { get; set; }
        public string channel_type { get; set; }
        public string channel_name { get; set; }
        public int profile_id { get; set; }
        public int sender_id { get; set; }
        public string sender_name { get; set; }
        public string sender_photo_url { get; set; }
        public int receiver_id { get; set; }
        public string receiver_name { get; set; }
        public string receiver_photo_url { get; set; }
        public string last_message { get; set; }
        public DateTime sent_at { get; set; }
        public int unread_count { get; set; }
    }

    public class JoinChannel
    {
        public string channel_id { get; set; }
        public int profile_id { get; set; }
        public string create_by { get; set; }
    }

    public class ChannelUserStatus
    {
        public string channel_id { get; set; }
        public int profile_id { get; set; }
        public int status_id { get; set; }
        public string update_by { get; set; }
    }

    public class LeaveChannel
    {
        public string channel_id { get; set; }
        public string channel_name { get; set; }
        public int profile_id { get; set; }
        public string update_by { get; set; }
    }
}