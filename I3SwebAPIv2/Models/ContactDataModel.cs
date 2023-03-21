using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace I3SwebAPIv2.Models
{
    public class ContactDataModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }
    public class StaffContactRequest
    {
        public int parent_id { get; set; }
    }
    public class StaffContactModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<StaffContact> Data { get; set; }
    }
    public class StaffContact
    {
        public int profile_id { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public string user_role { get; set; }
        public string user_role_bm { get; set; }
    }
    public class SchoolClassModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<SchoolClass> Data { get; set; }
    }
    public class SchoolClass
    {
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int class_id { get; set; }
        public string class_name { get; set; }
    }

    public class SchoolClassRequest
    {
        public int school_id { get; set; }
        public int[] class_id { get; set; }
    }

    public class ParentContactModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ParentContact> Data { get; set; }
    }
    public class ParentContact
    {
        public int profile_id { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public string user_role { get; set; }
        public string user_role_bm { get; set; }
    }

    public class SchoolStaffRequest
    {
        public int profile_id { get; set; }
        public int school_id { get; set; }
    }

    public class SchoolMerchantRequest
    {
        public int[] school_id { get; set; }
    }
}