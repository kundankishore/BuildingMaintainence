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
    
    public partial class tblPointSlouse
    {
        public int PointSliceID { get; set; }
        public int PointID { get; set; }
        public bool IsRawData { get; set; }
        public Nullable<int> RollUpInterval { get; set; }
        public Nullable<int> RollUpIntervalUomID { get; set; }
        public Nullable<int> DemandInterval { get; set; }
        public Nullable<int> DemandIntervalUomID { get; set; }
        public System.DateTime LowTimeMarker { get; set; }
        public System.DateTime HighTimeMarker { get; set; }
        public Nullable<int> FunctionTypeID { get; set; }
    }
}
