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
    
    public partial class tblReportPPMSchedule
    {
        public int Id { get; set; }
        public string TemplateName { get; set; }
        public Nullable<System.DateTime> SavedDate { get; set; }
        public Nullable<int> FacilityId { get; set; }
        public Nullable<int> BuildingId { get; set; }
        public Nullable<int> FloorId { get; set; }
        public Nullable<int> YearVal { get; set; }
        public Nullable<int> MonthVal { get; set; }
        public string AssetTypeStr { get; set; }
        public string AssetStr { get; set; }
        public string FrequenciesStr { get; set; }
        public string MaintenanceTeamStr { get; set; }
        public string MaintenanceTypeStr { get; set; }
        public Nullable<int> ScheduleFromDate { get; set; }
        public Nullable<int> ScheduleToDate { get; set; }
    }
}
