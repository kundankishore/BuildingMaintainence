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
    
    public partial class tblVM_VComp_VendorSelection
    {
        public int Id { get; set; }
        public Nullable<int> VendorCompId { get; set; }
        public Nullable<int> VendorId { get; set; }
        public Nullable<double> TotalValue { get; set; }
        public string Quotation { get; set; }
    
        public virtual tblVendorMasterGeneral tblVendorMasterGeneral { get; set; }
        public virtual tblVM_VendorComparison tblVM_VendorComparison { get; set; }
    }
}
