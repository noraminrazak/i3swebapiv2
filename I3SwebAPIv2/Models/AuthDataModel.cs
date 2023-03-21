using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace I3SwebAPIv2.Models
{
    public class AuthDataModel
    {
        public bool Success { get; set; }
        //public string Code { get; set; }
        public string Message { get; set; }
    }

    public class LookupDataModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class UserDataModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class LoginDataModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class RegisterDataModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }
}