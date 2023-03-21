using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace I3SwebAPIv2.Models
{
    public class StaffDataModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class CreateClassParam
    {
        public string class_name { get; set; }
        public int school_id { get; set; }
        public int staff_id { get; set; }
        public int session_id { get; set; }
        public string create_by { get; set; }
    }

    public class EnrollClassParam
    {
        public int school_id { get; set; }
        public int student_id { get; set; }
        public int class_id { get; set; }
        public string update_by { get; set; }
    }

    public class UpdateStaffShiftParam
    {
        public int school_id { get; set; }
        public int staff_id { get; set; }
        public int shift_id { get; set; }
        public string update_by { get; set; }
    }

    public class RemoveClassParam
    {
        public int school_id { get; set; }
        public int student_id { get; set; }
        public int class_id { get; set; }
        public string update_by { get; set; }
    }

    public class RemoveStaffParam
    {
        public int school_id { get; set; }
        public int staff_id { get; set; }
        public string update_by { get; set; }
    }

    public class CreateClubParam
    {
        public string club_name { get; set; }
        public int school_id { get; set; }
        public int staff_id { get; set; }
        public string create_by { get; set; }
    }

    public class AddClassRelationship
    {
        public int class_id { get; set; }
        public int staff_id { get; set; }
        public string class_teacher_flag { get; set; }
        public string create_by { get; set; }
    }

    public class StaffJoinClubRelationshipParam
    {
        public int club_id { get; set; }
        public int staff_id { get; set; }
        public string create_by { get; set; }
    }

    public class StudentJoinClubRelationshipParam
    {
        public int club_id { get; set; }
        public int student_id { get; set; }
        public string create_by { get; set; }
    }

    public class ParentJoinClubRelationshipParam
    {
        public int club_id { get; set; }
        public int parent_id { get; set; }
        public string create_by { get; set; }
    }

    public class MerchantJoinClubRelationshipParam
    {
        public int club_id { get; set; }
        public int merchant_id { get; set; }
        public string create_by { get; set; }
    }

    public class StaffHandoverClubParam
    {
        public int school_id { get; set; }
        public int club_id { get; set; }
        public int current_staff_id { get; set; }
        public int new_staff_id { get; set; }
        public int new_profile_id { get; set; }
        public string update_by { get; set; }
    }

    public class RemoveClassRelationship
    {
        public int relationship_id { get; set; }
        public int staff_id { get; set; }
        public int class_id { get; set; }
        public string update_by { get; set; }
    }

    public class RemoveClubRelationship
    {
        public int relationship_id { get; set; }
        public int profile_id { get; set; }
        public int club_id { get; set; }
        public string update_by { get; set; }
    }
    public class ClassRelationshipParam
    {
        public int school_id { get; set; }
        public int staff_id { get; set; }
    }

    public class StaffClubRelationshipParam
    {
        public int school_id { get; set; }
        public int staff_id { get; set; }
    }

    public class AddClubMemberParam
    {
        public int club_id { get; set; }
        public int profile_id { get; set; }
        public int user_role_id { get; set; }
        public string create_by { get; set; }
    }

    public class AddClassStudentParam
    {
        public int school_id { get; set; }
        public int class_id { get; set; }
        public int student_id { get; set; }
        public string create_by { get; set; }
    }

    public class RemoveClubMemberParam
    {
        public int relationship_id { get; set; }
        public int profile_id { get; set; }
        public int club_id { get; set; }
        public string update_by { get; set; }
    }

    public class MerchantClubRelationshipParam
    {
        public int merchant_id { get; set; }
    }

    public class ParentClubRelationshipParam
    {
        public int parent_id { get; set; }
    }

    public class StudentClubRelationshipParam
    {
        public int school_id { get; set; }
        public int student_id { get; set; }
    }

    public class GroupRelationshipParam
    {
        public int school_id { get; set; }
        public int staff_id { get; set; }
    }

    public class ClassParam
    {
        public int school_id { get; set; }
    }

    public class SchoolMerchantParam
    {
        public int school_id { get; set; }
        //public int merchant_type_id { get; set; }
    }

    public class SchoolStaffParam
    {
        public int school_id { get; set; }
    }

    public class ClassStudentParam
    {
        public int school_id { get; set; }
        public int class_id { get; set; }
    }

    public class ClubMemberParam
    {
        public int school_id { get; set; }
        public int club_id { get; set; }
    }

    public class SessionParam
    {
        public int school_id { get; set; }
    }

    public class ClubsParam
    {
        public int[] school_id { get; set; }
        public int create_by_id { get; set; }
    }

    public class SearchParentModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<SearchParent> Data { get; set; }
    }

    public class SearchParent
    {
        public int parent_id { get; set; }
        public int profile_id { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
    }

    public class SearchMerchantModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<SearchMerchant> Data { get; set; }
    }

    public class SearchMerchant
    {
        public int merchant_id { get; set; }
        public int profile_id { get; set; }
        public string full_name { get; set; }
        public string company_name { get; set; }
        public string photo_url { get; set; }
    }

    public class SearchStaffModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<SearchStaff> Data { get; set; }
    }

    public class SearchStaff
    {
        public int staff_id { get; set; }
        public int profile_id { get; set; }
        public string full_name { get; set; }
        public string staff_number { get; set; }
        public string photo_url { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int school_type_id { get; set; }
        public string school_type { get; set; }
        public int staff_type_id { get; set; }
        public string staff_type { get; set; }
        public int card_id { get; set; }
        public string card_number { get; set; }
        public string card_status { get; set; }
        public int wallet_id { get; set; }
        public string wallet_number { get; set; }
        public int shift_id { get; set; }
        public string shift_code { get; set; }
    }

    public class SchoolStaffModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<SchoolStaff> Data { get; set; }
    }

    public class SchoolStaff
    {
        public int staff_id { get; set; }
        public int profile_id { get; set; }
        public string full_name { get; set; }
        public string staff_number { get; set; }
        public string photo_url { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int school_type_id { get; set; }
        public string school_type { get; set; }
        public int staff_type_id { get; set; }
        public string staff_type { get; set; }
        public string staff_type_bm { get; set; }
        public int shift_id { get; set; }
        public string shift_code { get; set; }
        public int wallet_id { get; set; }
        public string wallet_number { get; set; }
    }

    public class ParentSearchStudentModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ParentSearchStudent> Data { get; set; }
    }

    public class ParentSearchStudent
    {
        public int student_id { get; set; }
        public int profile_id { get; set; }
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
        public int card_id { get; set; }
        public string card_number { get; set; }
        public string card_status { get; set; }
    }

    public class StaffSearchStudentModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<StaffSearchStudent> Data { get; set; }
    }

    public class StaffSearchStudent
    {
        public int student_id { get; set; }
        public int profile_id { get; set; }
        public string student_number { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public int wallet_id { get; set; }
        public string wallet_number { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int school_type_id { get; set; }
        public string school_type { get; set; }
        public int class_id { get; set; }
        public string class_name { get; set; }
        public int card_id { get; set; }
        public string card_number { get; set; }
        public string card_status { get; set; }
    }

    public class UpdateStudentAttendanceParam
    {
        public int report_id { get; set; }
        public int school_id { get; set; }
        public int class_id { get; set; }
        public int student_id { get; set; }
        public int attendance_id { get; set; }
        public int reason_id { get; set; }
        public string update_by { get; set; }
    }

    public class UpdateClubAttendanceParam
    {
        public int report_id { get; set; }
        public int school_id { get; set; }
        public int club_id { get; set; }
        public int profile_id { get; set; }
        public int attendance_id { get; set; }
        public int reason_id { get; set; }
        public string update_by { get; set; }
    }

    public class UpdateStaffAttendanceParam
    {
        public int report_id { get; set; }
        public int school_id { get; set; }
        public int staff_id { get; set; }
        public int attendance_id { get; set; }
        public int reason_id { get; set; }
        public string update_by { get; set; }
    }

    public class StaffMonthlyAttendanceSummaryParam
    {
        public int school_id { get; set; }
        public int student_id { get; set; }
        public string entry_month { get; set; }
    }

    public class StaffMonthlyAttendanceParam
    {
        public int school_id { get; set; }
        public string entry_month { get; set; }
    }

    public class StaffDailyAttendanceParam
    {
        public int school_id { get; set; }
        public string entry_date { get; set; }
    }
    public class StaffDailyAttendanceParam2
    {
        public int school_id { get; set; }
        public int staff_id { get; set; }
        public string entry_date { get; set; }
    }

    public class StaffMonthlyAttendanceModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<StaffMonthlyAttendance> Data { get; set; }
    }

    public class StaffMonthlyAttendance
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

    public class StaffMonthlyAttendanceSummaryModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<StaffMonthlyAttendanceSummary> Data { get; set; }
    }

    public class StaffMonthlyAttendanceSummary
    {
        public int total_absent { get; set; }
        public int total_present { get; set; }
        public int total_late_in { get; set; }
        public int total_half_day { get; set; }
        public int total_attendance { get; set; }
        public int total_school_day { get; set; }
    }

    public class StaffDailyAttendanceSummaryModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<StaffDailyAttendanceSummary> Data { get; set; }
    }

    public class StaffDailyAttendanceSummary
    {
        public int total_absent { get; set; }
        public int total_present { get; set; }
        public int total_late_in { get; set; }
        public int total_half_day { get; set; }
        public int total_attendance { get; set; }
        public int total_staff { get; set; }
    }
    public class StaffAttendanceParam
    {
        public int school_id { get; set; }
        public int staff_id { get; set; }
        public string entry_month { get; set; }
    }
    public class StaffAttendanceModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<StaffAttendance> Data { get; set; }
    }

    public class StaffAttendance
    {
        public int report_id { get; set; }
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

    public class StaffOutingParam
    {
        public int profile_id { get; set; }
        public int school_id { get; set; }
        public int outing_status_id { get; set; }
        public int outing_type_id { get; set; }
        public string outing_month { get; set; }
    }

    public class StaffUpdateOutingParam
    {
        public int outing_id { get; set; }
        public int student_id { get; set; }
        public int school_id { get; set; }
        public int approve_by_id { get; set; }
        public string approve_comment { get; set; }
        public string update_by { get; set; }
    }

    public class StaffOutingMonthModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<StaffOutingMonth> Data { get; set; }
    }

    public class StaffOutingMonth {
        public int school_id { get; set; }
        public int outing_type_id { get; set; }
        public string outing_type { get; set; }
        public int outing_status_id { get; set; }
        public string outing_status { get; set; }
        public DateTime outing_month { get; set; }
    }
}