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
    
    public partial class tblVMRequestForPurchase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblVMRequestForPurchase()
        {
            this.tblVM_VendorComparison = new HashSet<tblVM_VendorComparison>();
        }
    
        public int Id { get; set; }
        public string RFP_No { get; set; }
        public string RFPTitle { get; set; }
        public string RequestType { get; set; }
        public string Purpose { get; set; }
        public string RFPTemplate { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<bool> IsSkipVendorComparison { get; set; }
        public string DetailsOfPartsComp { get; set; }
        public Nullable<System.DateTime> ExpecDeliveryDateOfPartsComp { get; set; }
        public string DeliveryLocation { get; set; }
        public string DetailsOfManpower { get; set; }
        public string LocationOfManpower { get; set; }
        public Nullable<int> ApprovedByUserId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblVM_VendorComparison> tblVM_VendorComparison { get; set; }
    }
}
