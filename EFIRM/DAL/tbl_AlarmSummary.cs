//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EFIRM.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_AlarmSummary
    {
        public int nAlarmSummaryId { get; set; }
        public Nullable<int> nPointId { get; set; }
        public Nullable<System.DateTime> dtOccuranceTime { get; set; }
        public Nullable<System.DateTime> dtNormalizeTime { get; set; }
        public Nullable<System.DateTime> dtAckTime { get; set; }
        public Nullable<bool> bAlarmStatus { get; set; }
        public string sAlarmType { get; set; }
        public Nullable<float> rValue { get; set; }
        public Nullable<int> userLoginID { get; set; }
        public Nullable<int> Severity { get; set; }
        public string Status { get; set; }
        public string SAlarmMessage { get; set; }
    }
}
