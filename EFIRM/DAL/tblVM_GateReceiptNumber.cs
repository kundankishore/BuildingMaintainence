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
    
    public partial class tblVM_GateReceiptNumber
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblVM_GateReceiptNumber()
        {
            this.tblVM_GRN_ComponentQuantity = new HashSet<tblVM_GRN_ComponentQuantity>();
            this.tblVM_GRN_PartQuantity = new HashSet<tblVM_GRN_PartQuantity>();
        }
    
        public int Id { get; set; }
        public Nullable<int> RFP_Id { get; set; }
        public string GRN_No { get; set; }
        public string PONo { get; set; }
        public string InvoiceNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public string ReceivedBy { get; set; }
        public Nullable<System.DateTime> ReceiptDate { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public Nullable<int> StoreId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblVM_GRN_ComponentQuantity> tblVM_GRN_ComponentQuantity { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblVM_GRN_PartQuantity> tblVM_GRN_PartQuantity { get; set; }
    }
}