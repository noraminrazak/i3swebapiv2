using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace I3SwebAPIv2.Models
{
    public class ChatDataModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }
    public class ChatChannelParam
    {
        public int channel_type_id { get; set; }
        public string channel_name { get; set; }
        public string node_path { get; set; }
        public string create_by { get; set; }
    }
    public class UserChannelParam
    {
        public int channel_id { get; set; }
        public int profile_id { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public string is_online { get; set; }
    }
    public class ChatChannelModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ChatChannel> Data { get; set; }
    }

    public class ChatChannel
    {
        public int channel_id { get; set; }
        public int channel_type_id { get; set; }
        public string channel_name { get; set; }
        public string node_path { get; set; }
        public string create_by { get; set; }
    }
    public class UserDeviceModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<UserDevice> Data { get; set; }
    }

    public class UserDevice
    {
        public int device_platform_id { get; set; }
        public string device_token { get; set; }
        public string group_name { get; set; }
    }
}