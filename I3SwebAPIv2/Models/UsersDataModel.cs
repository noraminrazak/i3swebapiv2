using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace I3SwebAPIv2.Models
{
    public class LoginParam
    {
        public string username { get; set; }
    }

    public class RegisterCheckParam
    {
        public string username { get; set; }
        public int user_role_id { get; set; }
    }

    public class QuickRegisterParam
    {
        public string username { get; set; }
    }

    public class PasswordParam
    {
        public string password { get; set; }
        public string auth { get; set; }
    }

    public class SystemNotifyParam
    {
        public int profile_id { get; set; }
    }

    public class SystemNotifyModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<SystemNotify> Data { get; set; }
    }

    public class SystemNotify
    {
        public int notify_id { get; set; }
        public string notify_subject { get; set; }
        public string notify_message { get; set; }
        public string notify_photo_url { get; set; }
        public string notify_link { get; set; }
        public string notify_link_text { get; set; }
        public string notify_link_param { get; set; }
        public string read_flag { get; set; }
        public string create_at { get; set; }
    }
    public class UpdateSystemNotifyParam
    {
        public int notify_id { get; set; }
        public int profile_id { get; set; }
        public string update_by { get; set; }
    }

    public class CreatePasswordParam
    {
        public string mobile_number { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string confirm_password { get; set; }
    }
    public class ResubmitParam 
    {
        public string username { get; set; }
    }

    public class NewKYCParam
    {
        public string username { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string postcode { get; set; }
        public string state_id { get; set; }
        public string province { get; set; }
        public string country_id { get; set; }
        public string mother_maiden_name { get; set; }
        public string occupation { get; set; }
        public string employer_name { get; set; }
    }

    public class LRCParam 
    {
        public string lrc { get; set; }
    }
    public class RegisterParam
    {
        public string full_name { get; set; }
        public string mother_maiden_name { get; set; }
        public int card_type_id { get; set; }
        //public string photo_name { get; set; }
        //public string photo_id_name { get; set; }
        public int nationality_id { get; set; }
        public string identity_number { get; set; } //idno
        public string date_of_birth { get; set; } //dob
        public string mobile_number { get; set; } //mobileno
        public string email { get; set; } //loginid
        public string address { get; set; }
        public int state_id { get; set; }
        //public string province { get; set; }
        public string postcode { get; set; }
        public string city { get; set; }
        public int country_id { get; set; }
        public string occupation { get; set; }
        public string employer_name { get; set; }
        public string password { get; set; }
        public int marketing_flag { get; set; }
        public int user_role_id { get; set; }
    }

    public class VerifyAccountParam
    {
        public string mobile_number { get; set; }
        public string username { get; set; }
        public string otp { get; set; }
    }

    public class ChangePasswordParam
    {
        public int profile_id { get; set; }
        public string update_by { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string new_password { get; set; }
    }

    public class ChangePinParam
    {
        public string username { get; set; }
        public string new_pin { get; set; }
        public string update_by { get; set; }
    }

    public class ResetPasswordParam
    {
        public int user_id { get; set; }
        public string email { get; set; }
        public string token { get; set; }
        public string password { get; set; }
        public string confirm_password { get; set; }
    }

    public class ForgetPasswordParam
    {
        public string username { get; set; }
        public string email { get; set; }
    }

    public class DeleteAccountParam
    {
        public string username { get; set; }
    }

    public class ProfileParam
    {
        public int profile_id { get; set; }
    }

    public class UpdateProfileParam
    {
        public int profile_id { get; set; }
        public string full_name { get; set; }
        public string nric { get; set; }
        public string mobile_number { get; set; }
        public string email { get; set; }
        public int user_race_id { get; set; }
        public int card_type_id { get; set; }
        public string date_of_birth { get; set; }
        public string address { get; set; }
        public string postcode { get; set; }
        public string city { get; set; }
        public int state_id { get; set; }
        public int country_id { get; set; }
        public string update_by { get; set; }
    }

    public class UpdateProfileInfoParam
    {
        public int profile_id { get; set; }
        public string full_name { get; set; }
        public string nric { get; set; }
        public string mobile_number { get; set; }
        public string email { get; set; }
        public int user_race_id { get; set; }
        public int card_type_id { get; set; }
        public string date_of_birth { get; set; }
        public string address { get; set; }
        public string postcode { get; set; }
        public string city { get; set; }
        public int state_id { get; set; }
        public string state_name { get; set; }
        public int country_id { get; set; }
        public string mother_maiden_name { get; set; }
        public string occupation { get; set; }
        public string employer_name { get; set; }
        public string update_by { get; set; }
    }

    public class UpdateDeviceToken {
        public int profile_id { get; set; }
        public string device_token { get; set; }
        public int device_platform_id { get; set; }
        public string update_by { get; set; }
    }

    public class UpdatePhotoParam {
        public int profile_id { get; set; }
        public string file_name { get; set; }
        public string photo_base64 { get; set; }
        public string update_by { get; set; }
    }

    public class RemovePhotoParam
    {
        public int profile_id { get; set; }
        public string update_by { get; set; }
    }

    public class UploadPhotoParam
    {
        public string identity_number { get; set; }
        public int image_type_id { get; set; }
        public string file_name { get; set; }
        public string photo_base64 { get; set; }
        public string create_by { get; set; }
    }

    public class UploadAttachmentParam
    {
        public int ticket_id { get; set; }
        public string file_name { get; set; }
        public string photo_base64 { get; set; }
        public string create_by { get; set; }
    }

    public class ParentParam
    {
        public int parent_id { get; set; }
    }


    public class RegisterStudentParam
    {
        public int parent_id { get; set; }
        public int student_id { get; set; }
        public int school_id { get; set; }
        public int class_id { get; set; }
        public string create_by { get; set; }
    }

    public class ParentVirtualBalanceParam
    {
        public int parent_profile_id { get; set; }
        public int student_profile_id { get; set; }
        public string create_by { get; set; }
    }


    public class SearchStudentParam
    {
        public int school_id { get; set; }
        public string search_name { get; set; }
    }

    public class SearchParentParam
    {
        public int school_id { get; set; }
        public string search_name { get; set; }
    }

    public class SearchStaffParam
    {
        public int school_id { get; set; }
        public string search_name { get; set; }
    }

    public class SearchMerchantParam
    {
        public int school_id { get; set; }
        public string search_name { get; set; }
    }

    public class StudentParam
    {
        public int profile_id { get; set; }
    }

    public class StudentParentParam
    {
        public int student_id { get; set; }
    }

    public class RoleModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public int profile_id { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public List<Role> Data { get; set; }
    }

    public class Role
    {
        public int user_role_id { get; set; }
        public string user_role { get; set; }
    }

    public class StaffModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<Staff> Data { get; set; }
    }

    public class Staff
    {
        public int staff_id { get; set; }
        public string staff_number { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int shift_id { get; set; }
        public string shift_code { get; set; }
        public int staff_type_id { get; set; }
        public string staff_type { get; set; }
        public int wallet_id { get; set; }
        public string wallet_number { get; set; }
        public decimal account_balance { get; set; }
        public int card_id { get; set; }
        public string card_number { get; set; }
        public int card_status_id { get; set; }
        public string card_status { get; set; }
    }

    public class StudentModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<Student> Data { get; set; }
    }

    public class Student
    {
        public int student_id { get; set; }
        public string student_number { get; set; }
        public int profile_id { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int class_id { get; set; }
        public string class_name { get; set; }
        public int wallet_id { get; set; }
        public string wallet_number { get; set; }
        public decimal account_balance { get; set; }
        public int card_id { get; set; }
        public string hex_id { get; set; }
        public string card_number { get; set; }
        public int card_status_id { get; set; }
        public string card_status { get; set; }
    }

    public class ClassStudentModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ClassStudent> Data { get; set; }
    }

    public class ClassStudent
    {
        public int student_id { get; set; }
        public int profile_id { get; set; }
        public string student_number { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int class_id { get; set; }
        public string class_name { get; set; }
        public string status_code { get; set; }
    }

    public class ClubMemberModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ClubMember> Data { get; set; }
    }

    public class ClubMember
    {
        public int relationship_id { get; set; }
        public int profile_id { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public int user_role_id { get; set; }
        public string user_role { get; set; }
        public string status_code { get; set; }
    }

    public class RegisterStudentModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<RegisterStudent> Data { get; set; }
    }

    public class RegisterStudent
    {
        public int student_id { get; set; }
        public string student_number { get; set; }
        public string full_name { get; set; }
        public string nric { get; set; }
        public string photo_url { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int school_type_id { get; set; }
        public string school_type { get; set; }
        public int class_id { get; set; }
        public string class_name { get; set; }
    }

    public class ParentModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<Parent> Data { get; set; }
    }

    public class Parent
    {
        public int parent_id { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public int wallet_id { get; set; }
        public string wallet_number { get; set; }
        public decimal account_balance { get; set; }
    }
    public class ProfileModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<Profile> Data { get; set; }
    }

    public class Profile
    {
        public int profile_id { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public string email { get; set; }
        public string mobile_number { get; set; }
        public int user_race_id { get; set; }
        public string user_race { get; set; }
        public int card_type_id { get; set; }
        public string card_type { get; set; }
        public string nric { get; set; }
        public string date_of_birth { get; set; }
        public string address { get; set; }
        public string postcode { get; set; }
        public string city { get; set; }
        public int state_id { get; set; }
        public string state_name { get; set; }
        public int country_id { get; set; }
        public string country_name { get; set; }
        public string mother_maiden_name { get; set; }
        public string occupation { get; set; }
        public string employer_name { get; set; }
    }

    public class StateModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<State> Data { get; set; }
    }

    public class State
    {
        public int state_id { get; set; }
        public string state_name { get; set; }
    }

    public class CityModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<City> Data { get; set; }
    }

    public class StateParam
    {
        public int state_id { get; set; }
    }

    public class City
    {
        public int city_id { get; set; }
        public string city_name { get; set; }
    }

    public class CardTypeModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<CardType> Data { get; set; }
    }

    public class CardType
    {
        public int card_type_id { get; set; }
        public string card_type { get; set; }
        public string card_type_bm { get; set; }
    }

    public class CardStatusModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<CardStatus> Data { get; set; }
    }

    public class CardStatus
    {
        public int card_status_id { get; set; }
        public string card_status { get; set; }
        public string card_status_bm { get; set; }
    }

   
    public class CountryModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<Country> Data { get; set; }
    }

    public class Country
    {
        public int country_id { get; set; }
        public string country_name { get; set; }
        public string locale_code { get; set; }
        public string country_code { get; set; }
    }

    public class PlatformVersionModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<PlatformVersion> Data { get; set; }
    }

    public class PlatformVersion
    {
        public string platform_name { get; set; }
        public string version_number { get; set; }
        public string build_number { get; set; }
        public string release { get; set; }
    }
    public class ReasonModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<Reason> Data { get; set; }
    }



    public class Reason
    {
        public int reason_id { get; set; }
        public string reason_for_absent { get; set; }
        public string reason_for_absent_bm { get; set; }
    }

    public class AttendanceModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<Attendance> Data { get; set; }
    }

    public class Attendance
    {
        public int attendance_id { get; set; }
        public string attendance_code { get; set; }
        public string attendance { get; set; }
    }

    public class UserRaceModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<UserRace> Data { get; set; }
    }

    public class UserRace
    {
        public int user_race_id { get; set; }
        public string user_race { get; set; }
        public string user_race_bm { get; set; }
    }

    public class ParentSchoolModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ParentSchool> Data { get; set; }
    }

    public class ParentSchool
    {
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int school_type_id { get; set; }
        public string school_type { get; set; }
        public string city { get; set; }
        public string state_name { get; set; }
        public string country_name { get; set; }
        public string status_code { get; set; }
    }

    public class ParentStudentModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ParentStudent> Data { get; set; }
    }

    public class ParentStudent
    {
        public int student_id { get; set; }
        public int profile_id { get; set; }
        public string student_number { get; set; }
        public string photo_url { get; set; }
        public string full_name { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int school_type_id { get; set; }
        public string school_type { get; set; }
        public int class_id { get; set; }
        public string class_name { get; set; }
        public int card_id { get; set; }
        public string card_number { get; set; }
        public decimal account_balance { get; set; }
        public int card_status_id { get; set; }
        public string card_status { get; set; }
        public int wallet_id { get; set; }
        public string wallet_number { get; set; }
    }

    public class StudentParentModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<StudentParent> Data { get; set; }
    }

    public class StudentParent
    {
        public int parent_id { get; set; }
        public int profile_id { get; set; }
        public string photo_url { get; set; }
        public string full_name { get; set; }
        public string mobile_number { get; set; }
    }

    public class MoceanModel
    {
        public List<Mocean> messages { get; set; }
    }
    public class Mocean
    {
        public string status { get; set; }
        public string receiver { get; set; }
        public string msgid { get; set; }
        public string err_msg { get; set; }
    }

    public class TopupParam
    {
        public string status { get; set; }
        public string receiver { get; set; }
        public string msgid { get; set; }
        public string err_msg { get; set; }
    }

    public class UserDetail
    {
        public string connection_id { get; set; }
        public int profile_id { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
    }
    public class MessageDetail
    {
        public int from_profile_id { get; set; }
        public string from_full_name { get; set; }
        public string from_photo_url { get; set; }
        public int to_profile_id { get; set; }
        public string to_full_name { get; set; }
        public string to_photo_url { get; set; }
        public string message { get; set; }
    }
}