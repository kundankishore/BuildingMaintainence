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
    
    public partial class tblPPMTask
    {
        public int Id { get; set; }
        public Nullable<int> ScheduleId { get; set; }
        public Nullable<int> TaskId { get; set; }
    
        public virtual tblMaintenanceTask tblMaintenanceTask { get; set; }
        public virtual tblPPMSchedule tblPPMSchedule { get; set; }
    }
}