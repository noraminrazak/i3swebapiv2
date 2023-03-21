using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace I3SwebAPIv2.Models
{
    public class StudentDataModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class StudentMonthlyAttendanceSummaryParam
    {
        public int school_id { get; set; }
        public int class_id { get; set; }
        public int student_id { get; set; }
        public string entry_month { get; set; }
    }

    public class StudentMonthlyAttendanceParam
    {
        public int school_id { get; set; }
        public int class_id { get; set; }
        public int student_id { get; set; }
        public string entry_month { get; set; }
    }

    public class ClubMemberMonthlyAttendanceParam
    {
        public int school_id { get; set; }
        public int club_id { get; set; }
        public int profile_id { get; set; }
        public string entry_month { get; set; }
    }

    public class StudentDailyAttendanceParam
    {
        public int school_id { get; set; }
        public int class_id { get; set; }
        public int student_id { get; set; }
        public string entry_date { get; set; }
    }

    public class ClubMemberDailyAttendanceParam
    {
        public int school_id { get; set; }
        public int club_id { get; set; }
        public int profile_id { get; set; }
        public string entry_date { get; set; }
    }

    public class StudentMonthlyAttendanceSummaryModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<StudentMonthlyAttendanceSummary> Data { get; set; }
    }

    public class StudentMonthlyAttendanceSummary
    {
        public int total_absent { get; set; }
        public int total_present { get; set; }
        public int total_late_in { get; set; }
        public int total_half_day { get; set; }
        public int total_attendance { get; set; }
        public int total_school_day { get; set; }
    }

    public class StudentMonthlyAttendanceModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<StudentMonthlyAttendance> Data { get; set; }
    }

    public class StudentMonthlyAttendance
    {
        public int report_id { get; set; }
        public DateTime entry_date { get; set; }
        public int attendance_id { get; set; }
        public string attendance { get; set; }
        public string attendance_bm { get; set; }
        public int reason_id { get; set; }
        public string reason_for_absent { get; set; }
        public string reason_for_absent_bm { get; set; }
    }

    public class ClubMemberMonthlyAttendanceModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ClubMemberMonthlyAttendance> Data { get; set; }
    }

    public class ClubMemberMonthlyAttendance
    {
        public int report_id { get; set; }
        public DateTime entry_date { get; set; }
        public int attendance_id { get; set; }
        public string attendance { get; set; }
        public string attendance_bm { get; set; }
        public int reason_id { get; set; }
        public string reason_for_absent { get; set; }
        public string reason_for_absent_bm { get; set; }
    }

    public class StudentRequestOutingParam
    {
        public int student_id { get; set; }
        public int school_id { get; set; }
        public int outing_type_id { get; set; }
        public DateTime check_out_date { get; set; }
        public DateTime check_in_date { get; set; }
        public string outing_reason { get; set; }
        public int request_by_id { get; set; }
        public int request_by_user_role_id { get; set; }
        public string create_by { get; set; }
    }

    public class StudentUpdateOutingParam
    {
        public int outing_id { get; set; }
        public int student_id { get; set; }
        public int school_id { get; set; }
        public int outing_type_id { get; set; }
        public DateTime check_out_date { get; set; }
        public DateTime check_in_date { get; set; }
        public string outing_reason { get; set; }
        public int request_by_id { get; set; }
        public int request_by_user_role_id { get; set; }
        public string update_by { get; set; }
    }

    public class StudentSubmitOutingParam
    {
        public int outing_id { get; set; }
        public int student_id { get; set; }
        public int school_id { get; set; }
        public int outing_type_id { get; set; }
        public DateTime check_out_date { get; set; }
        public DateTime check_in_date { get; set; }
        public string outing_reason { get; set; }
        public int request_by_id { get; set; }
        public int request_by_user_role_id { get; set; }
        public string create_by { get; set; }
    }

    public class StudentOutingParam 
    {
        public int student_id { get; set; }
        public int school_id { get; set; }
        public string outing_month { get; set; }
    }

    public class StudentOutingModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<StudentOuting> Data { get; set; }
    }

    public class StudentOuting
    {
        public int outing_id { get; set; }
        public int student_id { get; set; }
        public int profile_id { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int outing_type_id { get; set; }
        public string outing_type { get; set; }
        public DateTime check_out_date { get; set; }
        public DateTime check_in_date { get; set; }
        public string outing_reason { get; set; }
        public int outing_status_id { get; set; }
        public string outing_status { get; set; }
        public int request_by_id { get; set; }
        public string request_by { get; set; }
        public int request_by_user_role_id { get; set; }
        public string request_by_user_role { get; set; }
        public int approve_by_id { get; set; }
        public string approve_by { get; set; }
        public DateTime approve_at { get; set; }
        public string approve_comment { get; set; }
        public DateTime check_out_at { get; set; }
        public DateTime check_in_at { get; set; }
    }

    public class StudentOutingGroupModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<StudentOutingGroup> Data { get; set; }
    }

    public class StudentOutingGroup
    {
        public DateTime outing_month { get; set; }
    }
}