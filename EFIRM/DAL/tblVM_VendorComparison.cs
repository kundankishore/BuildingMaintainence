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
    
    public partial class tblVM_VendorComparison
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblVM_VendorComparison()
        {
            this.tblVM_VComp_VendorSelection = new HashSet<tblVM_VComp_VendorSelection>();
        }
    
        public int Id { get; set; }
        public Nullable<int> RFP_Id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblVM_VComp_VendorSelection> tblVM_VComp_VendorSelection { get; set; }
        public virtual tblVMRequestForPurchase tblVMRequestForPurchase { get; set; }
    }
}