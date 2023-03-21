using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace I3SwebAPIv2.Models
{ 
    public class StaffShiftParam
    {
        public int school_id { get; set; }
    }
    public class CreateSchoolPostParam
    {
        public int school_id { get; set; }
        public int class_id { get; set; }
        public int club_id { get; set; }
        public int post_group_id { get; set; }
        public int staff_id { get; set; }
        public string post_message { get; set; }
        public string file_name { get; set; }
        public string photo_base64 { get; set; }
        public string photo_url { get; set; }
        public DateTime start_at { get; set; }
        public DateTime end_at { get; set; }
        public string create_by { get; set; }
        public DateTime create_at { get; set; }
    }

    public class UpdateSchoolPostParam
    {
        public int post_id { get; set; }
        public int school_id { get; set; }
        public int class_id { get; set; }
        public int club_id { get; set; }
        public int post_group_id { get; set; }
        public int staff_id { get; set; }
        public string post_message { get; set; }
        public string file_name { get; set; }
        public string photo_base64 { get; set; }
        public string photo_url { get; set; }
        public DateTime start_at { get; set; }
        public DateTime end_at { get; set; }
        public string update_by { get; set; }
        public DateTime update_at { get; set; }
    }

    public class SchoolPostRequest
    {
        public int[] school_id { get; set; }
    }

    public class SchoolClassPostRequest
    {
        public int school_id { get; set; }
        public int class_id { get; set; }
    }
    public class SchoolClubPostRequest
    {
        public int school_id { get; set; }
        public int club_id { get; set; }
    }

    public class SchoolClubGenerateAttendanceRequest
    {
        public int school_id { get; set; }
        public int club_id { get; set; }
        public DateTime entry_date { get; set; }
        public string create_by { get; set; }
    }

    public class StaffTypeModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<StaffType> Data { get; set; }
    }

    public class StaffType
    {
        public int staff_type_id { get; set; }
        public string staff_type { get; set; }
        public string staff_type_bm { get; set; }
    }

    public class StaffShiftModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<StaffShift> Data { get; set; }
    }

    public class StaffShift
    {
        public int shift_id { get; set; }
        public string shift_code { get; set; }
        public TimeSpan start_time { get; set; }
        public TimeSpan end_time { get; set; }
    }

    public class SchoolDataModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class SchoolPostModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<SchoolPost> Data { get; set; }
    }


    public class SchoolPost
    {
        public int post_id { get; set; }
        public string school_name { get; set; }
        public string school_type { get; set; }
        public string post_message { get; set; }
        public string date_from { get; set; }
        public string date_to { get; set; }
        public string post_photo_url { get; set; }
        public string create_by { get; set; }
        public string create_by_photo_url { get; set; }
        public DateTime create_at { get; set; }
    }

    public class SchoolClassPostModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<SchoolClassPost> Data { get; set; }
    }


    public class SchoolClassPost
    {
        public int post_id { get; set; }
        public string school_name { get; set; }
        public string school_type { get; set; }
        public string class_name { get; set; }
        public string post_message { get; set; }
        public string date_from { get; set; }
        public string date_to { get; set; }
        public string post_photo_url { get; set; }
        public string create_by { get; set; }
        public string create_by_photo_url { get; set; }
        public DateTime create_at { get; set; }
    }

    public class SchoolClubPostModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<SchoolClubPost> Data { get; set; }
    }


    public class SchoolClubPost
    {
        public int post_id { get; set; }
        public string school_name { get; set; }
        public string school_type { get; set; }
        public string club_name { get; set; }
        public string post_message { get; set; }
        public string date_from { get; set; }
        public string date_to { get; set; }
        public string post_photo_url { get; set; }
        public string create_by { get; set; }
        public string create_by_photo_url { get; set; }
        public DateTime create_at { get; set; }
    }

    public class SqlFilterParam {
        public int? limit { get; set; }
        public int? offset { get; set; }
    }

    public class CultureFilterParam
    {
        public string culture { get; set; }
    }

    //public class UpdateStaffAttendanceParam
    //{
    //    public int report_id { get; set; }
    //    public int staff_id { get; set; }
    //    public int card_id { get; set; }
    //    public int attendance_id { get; set; }
    //    public int reason_id { get; set; }
    //    public string update_by { get; set; }
    //}

    //public class UpdateStudentAttendanceParam
    //{
    //    public int report_id { get; set; }
    //    public int student_id { get; set; }
    //    public int card_id { get; set; }
    //    public int attendance_id { get; set; }
    //    public int reason_id { get; set; }
    //    public string update_by { get; set; }
    //}

    public class ClassesModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<Classes> Data { get; set; }
    }

    public class Classes
    {
        public int class_id { get; set; }
        public string class_name { get; set; }
        public string school_name { get; set; }
        public string session_code { get; set; }
        public int total_student { get; set; }
    }

    public class ClubsModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<Clubs> Data { get; set; }
    }

    public class Clubs
    {
        public int club_id { get; set; }
        public string club_name { get; set; }
        public string school_name { get; set; }
        public int staff_id { get; set; }
        public string full_name { get; set; }
        public int total_member { get; set; }
    }

    public class SessionModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<Session> Data { get; set; }
    }

    public class Session
    {
        public int session_id { get; set; }
        public string session_code { get; set; }
        public TimeSpan start_time { get; set; }
        public TimeSpan late_in_time { get; set; }
        public TimeSpan recess_end_time { get; set; }
    }

    public class ClassRelationshipModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ClassRelationship> Data { get; set; }
    }

    public class ClassRelationship
    {
        public int relationship_id { get; set; }
        public int class_id { get; set; }
        public string class_name { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public string school_type { get; set; }
        public string session_code { get; set; }
        public string class_teacher_flag { get; set; }
        public int total_student { get; set; }
    }

    public class ClubRelationshipModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ClubRelationship> Data { get; set; }
    }

    public class ClubRelationship
    {
        public int relationship_id { get; set; }
        public int club_id { get; set; }
        public string club_name { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public string school_type { get; set; }
        public int total_member { get; set; }
        public int create_by_staff_id { get; set; }
    }

    public class SchoolClassDailyAttendanceParam
    {
        public int school_id { get; set; }
        public int class_id { get; set; }
        public string entry_date { get; set; }
    }

    public class SchoolClubDailyAttendanceParam
    {
        public int school_id { get; set; }
        public int club_id { get; set; }
        public string entry_date { get; set; }
    }
    public class SchoolStaffDailyAttendanceParam
    {
        public int school_id { get; set; }
        public int shift_id { get; set; }
        public string entry_date { get; set; }
    }
    public class SchoolStaffDailyAttendancePercentParam
    {
        public int school_id { get; set; }
        public string entry_month { get; set; }
    }

    public class SchoolClassDailyAttendanceSummaryModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<SchoolClassDailyAttendanceSummary> Data { get; set; }
    }

    public class SchoolClassDailyAttendanceSummary
    {
        public int total_absent { get; set; }
        public int total_present { get; set; }
        public int total_late_in { get; set; }
        public int total_half_day { get; set; }
        public int total_attendance { get; set; }
        public int total_student { get; set; }
    }

    public class SchoolClubDailyAttendanceSummaryModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<SchoolClubDailyAttendanceSummary> Data { get; set; }
    }

    public class SchoolClubDailyAttendanceSummary
    {
        public int total_absent { get; set; }
        public int total_present { get; set; }
        public int total_late_in { get; set; }
        public int total_half_day { get; set; }
        public int total_attendance { get; set; }
        public int total_member { get; set; }
    }

    public class SchoolClassDailyAttendancePercentageModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<SchoolClassDailyAttendancePercentage> Data { get; set; }
    }

    public class SchoolClassDailyAttendancePercentage
    {
        public int school_id { get; set; }
        public int class_id { get; set; }
        public DateTime entry_date { get; set; }
        public int total_attendance { get; set; }
        public int total_student { get; set; }
        public decimal total_percentage { get; set; }
    }

    public class SchoolClubDailyAttendancePercentageModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<SchoolClubDailyAttendancePercentage> Data { get; set; }
    }

    public class SchoolClubDailyAttendancePercentage
    {
        public int school_id { get; set; }
        public int club_id { get; set; }
        public DateTime entry_date { get; set; }
        public int total_attendance { get; set; }
        public int total_member { get; set; }
        public decimal total_percentage { get; set; }
    }
    public class SchoolStaffDailyAttendancePercentageModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<SchoolStaffDailyAttendancePercentage> Data { get; set; }
    }

    public class SchoolStaffDailyAttendancePercentage
    {
        public int school_id { get; set; }
        public DateTime entry_date { get; set; }
        public int total_attendance { get; set; }
        public int total_staff { get; set; }
        public decimal total_percentage { get; set; }
    }


    public class SchoolStaffDailyAttendanceSummaryModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<SchoolStaffDailyAttendanceSummary> Data { get; set; }
    }

    public class SchoolStaffDailyAttendanceSummary
    {
        public int total_absent { get; set; }
        public int total_present { get; set; }
        public int total_late_in { get; set; }
        public int total_half_day { get; set; }
        public int total_attendance { get; set; }
        public int total_staff { get; set; }
    }

    public class SchoolClassDailyAttendanceModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<SchoolClassDailyAttendance> Data { get; set; }
    }

    public class SchoolClassDailyAttendance
    {
        public int report_id { get; set; }
        public int student_id { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public DateTime entry_date { get; set; }
        public int attendance_id { get; set; }
        public string attendance { get; set; }
        public string attendance_bm { get; set; }
        public int reason_id { get; set; }
        public string reason_for_absent { get; set; }
        public string reason_for_absent_bm { get; set; }
    }

    public class SchoolClubDailyAttendanceModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<SchoolClubDailyAttendance> Data { get; set; }
    }

    public class SchoolClubDailyAttendance
    {
        public int report_id { get; set; }
        public int profile_id { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public int user_role_id { get; set; }
        public string user_role { get; set; }
        public DateTime entry_date { get; set; }
        public int attendance_id { get; set; }
        public string attendance { get; set; }
        public string attendance_bm { get; set; }
        public int reason_id { get; set; }
        public string reason_for_absent { get; set; }
        public string reason_for_absent_bm { get; set; }
    }

    public class SchoolStaffDailyAttendanceModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<SchoolStaffDailyAttendance> Data { get; set; }
    }

    public class SchoolStaffDailyAttendance
    {
        public int report_id { get; set; }
        public int staff_id { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public DateTime entry_date { get; set; }
        public DateTime touch_in_at { get; set; }
        public DateTime touch_out_at { get; set; }
        public int attendance_id { get; set; }
        public string attendance { get; set; }
        public string attendance_bm { get; set; }
        public int reason_id { get; set; }
        public string reason_for_absent { get; set; }
        public string reason_for_absent_bm { get; set; }
    }

    public class SearchSchoolParam
    {
        public int state_id { get; set; }
        public string school_name { get; set; }
    }

    public class SearchSchoolModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<SearchSchool> Data { get; set; }
    }

    public class SearchSchool
    {
        public int school_id { get; set; }
        public string school_name { get; set; }
        public string school_code { get; set; }
        public string school_type { get; set; }
        public string address { get; set; }
        public string postcode { get; set; }
        public string city { get; set; }
        public string state_name { get; set; }
        public string country_name { get; set; }
        public string status_code { get; set; }
    }

    public class SchoolInfoParam
    {
        public int school_id { get; set; }
    }

    public class SchoolInfoModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<SchoolInfo> Data { get; set; }
    }

    public class SchoolInfo
    {
        public int school_id { get; set; }
        public string school_name { get; set; }
        public string school_code { get; set; }
        public int school_type_id { get; set; }
        public string school_type { get; set; }
        public string school_website { get; set; }
        public string school_result_url { get; set; }
        public string city { get; set; }
        public string state_name { get; set; }
        public string email { get; set; }
        public int total_staff { get; set; }

    }
}