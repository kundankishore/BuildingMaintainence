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
    
    public partial class tblReportSoftServicesPPM
    {
        public int nReportSoftServicesPPMId { get; set; }
        public string sTemplateName { get; set; }
        public Nullable<System.DateTime> dtSavedDate { get; set; }
        public Nullable<int> nFacilityId { get; set; }
        public Nullable<int> nBuildingId { get; set; }
        public Nullable<int> nFloorId { get; set; }
        public Nullable<int> nYearVal { get; set; }
        public Nullable<int> nMonthVal { get; set; }
        public string sSoftServiesIds { get; set; }
        public string sFrequencyTypeIds { get; set; }
        public string sMaintenanceTeamIds { get; set; }
        public string sMaintenanceTypeIds { get; set; }
        public Nullable<int> nScheduleFromDate { get; set; }
        public Nullable<int> nScheduleToDate { get; set; }
    }
}
