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
    
    public partial class tblFacilityWorkingSchedule
    {
        public int Id { get; set; }
        public Nullable<int> FacilityId { get; set; }
        public string Day { get; set; }
        public Nullable<bool> IsWorking { get; set; }
        public Nullable<System.TimeSpan> FromTime { get; set; }
        public Nullable<System.TimeSpan> ToTime { get; set; }
    }
}
