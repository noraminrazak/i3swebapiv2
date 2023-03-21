using System.Collections.Generic;

namespace I3SwebAPIv2.Models
{
    public class RegisterAccountParam
    {
        public int user_role_id { get; set; }
        public string full_name { get; set; }
        public int id_type { get; set; }
        public string id_number { get; set; } //idno
        public int nationality_id { get; set; }
        public string date_of_birth { get; set; } //dob
        public string mobile_number { get; set; } //mobileno
        public string email { get; set; } //loginid
        public string password { get; set; }
        public int marketing_flag { get; set; }
    }

    public class PasswordCheckParam 
    {
        public string password { get; set; }
    }
    public class RegisterAccountDataModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class RegisterAccountDataListModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<RegisterAccountData> Data { get; set; }
    }

    public class RegisterAccountData
    {
        public int account_id { get; set; }
    }
    public class AccEkycParam
    {
        public int account_id { get; set; }
    }

    public class DoEKYCDataModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class DoEKYCDataListModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<DoEKYCData> Data { get; set; }
    }

    public class DoEKYCData
    {
        public int account_id { get; set; }
    }

    public class UploadImageParam
    {
        public string id_number { get; set; }
        public int image_type_id { get; set; }
        public string file_name { get; set; }
        public string photo_base64 { get; set; }
        public string create_by { get; set; }
    }

    public class DoCaptchaModel
    {
        public Header Header { get; set; }
        public BodyCaptcha Body { get; set; }
    }

    public class BodyCaptcha
    {
        public string recaptcha_URL { get; set; }
    }

    public class AccountDataModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class AccountVerifyParam
    {
        public int account_id { get; set; }
        public string otp { get; set; }
    }

    public class AccDoEKYCModel
    {
        public Header Header { get; set; }
        public BodyAccDoEkyc Body { get; set; }
    }

    public class BodyAccDoEkyc
    {
        public AccDoEkycInfo EKYC_info { get; set; }

    }

    public class AccDoEkycInfo
    {
        public string face_match_status { get; set; }
        public string uid { get; set; }
        public string landmark_check_status { get; set; }
        public string EKYC_id { get; set; }
        public string landmark_text_pass_count { get; set; }
        public string landmark_pass_count { get; set; }
        public string landmark_text_treshold { get; set; }
        public string message { get; set; }
        public string landmark_treshold { get; set; }
        public OCRData OCR_data { get; set; }
        public string status { get; set; }
    }

    public class OCRData
    {
        public string id_number { get; set; }
        public string extra_passport_data { get; set; }
        public string address { get; set; }
        public string sex { get; set; }
        public string id_type_name { get; set; }
        public string id_type_id { get; set; }
        public string uuid { get; set; }
        public string religion { get; set; }
        public string colour { get; set; }
        public string nationality { get; set; }
        public string dob { get; set; }
        public string id_exp_date { get; set; }
        public string region { get; set; }

    }

    public class AccountCSInfoParam
    {
        public int profile_id { get; set; }
        public int user_role_id { get; set; }
    }

    public class AccountCSInfoModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<AccountCSInfo> Data { get; set; }
    }

    public class AccountCSInfo
    {
        public string full_name { get; set; }
        public string email { get; set; }
        public string school_name { get; set; }
        public string coordinate { get; set; }
    }
}