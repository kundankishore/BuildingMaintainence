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
    
    public partial class TblMachineStateReport
    {
        public int id { get; set; }
        public Nullable<int> ServiceReportId { get; set; }
        public Nullable<int> StateId { get; set; }
        public Nullable<int> userId { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<System.TimeSpan> TimeSpan { get; set; }
        public string Comments { get; set; }
    }
}
