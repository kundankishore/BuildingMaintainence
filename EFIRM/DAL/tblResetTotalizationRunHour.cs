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
    
    public partial class tblResetTotalizationRunHour
    {
        public int nRunHoursResetId { get; set; }
        public Nullable<int> AssetId { get; set; }
        public Nullable<int> nObjectListId { get; set; }
        public Nullable<System.DateTime> LastResetDateTime { get; set; }
        public Nullable<int> TotalRunHours { get; set; }
        public Nullable<int> PointComeFrom { get; set; }
    }
}
