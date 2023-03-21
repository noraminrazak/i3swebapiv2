using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace I3SwebAPIv2.Models
{
    public class ParentDataModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class RemoveStudentRelationship
    {
        public int parent_id { get; set; }
        public int student_id { get; set; }
        public string update_by { get; set; }
    }
}