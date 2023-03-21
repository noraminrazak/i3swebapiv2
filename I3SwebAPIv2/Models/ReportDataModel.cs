using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace I3SwebAPIv2.Models
{
    public class ReportDataModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class ExportClassAttendanceParam
    {
        public int school_id { get; set; }
        public int class_id { get; set; }
        public string entry_month { get; set; }
        public string create_by { get; set; }

    }

    public class ExportStaffAttendanceParam
    {
        public int school_id { get; set; }
        public int shift_id { get; set; }
        public string entry_month { get; set; }
        public string create_by { get; set; }
    }

    public class ExportMerchantDailyOrderParam
    {
        public int merchant_id { get; set; }
        public int school_id { get; set; }
        public string order_date { get; set; }
        public string create_by { get; set; }
    }

    public class ExportClassDailyOrderParam
    {
        public int merchant_id { get; set; }
        public int school_id { get; set; }
        public int class_id { get; set; }
        public string order_date { get; set; }
        public string create_by { get; set; }
    }

    public class ExportStaffDailyOrderParam
    {
        public int merchant_id { get; set; }
        public int school_id { get; set; }
        public string order_date { get; set; }
        public string create_by { get; set; }
    }

    public class CustomerFeedbackRequest
    {
        public int support_category_id { get; set; }
        public int problem_type_id { get; set; }
        public int priority_type_id { get; set; }
        public string ticket_subject { get; set; }
        public string ticket_desc { get; set; }
        public int ticket_status_id { get; set; }
        public string create_by { get; set; }
    }

    public class ProblemTypeModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ProblemType> Data { get; set; }
    }

    public class ProblemType
    {
        public int problem_type_id { get; set; }
        public string problem_type { get; set; }
        public string problem_type_bm { get; set; }
    }

    public class OccupationModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<Occupation> Data { get; set; }
    }

    public class Occupation
    {
        public int occupation_id { get; set; }
        public string occupation { get; set; }
    }
}