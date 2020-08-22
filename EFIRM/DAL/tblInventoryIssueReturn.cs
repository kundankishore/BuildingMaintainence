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
    
    public partial class tblInventoryIssueReturn
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblInventoryIssueReturn()
        {
            this.tblIssueReturnItemsLists = new HashSet<tblIssueReturnItemsList>();
            this.tblIssueReturnItemsListComponents = new HashSet<tblIssueReturnItemsListComponent>();
        }
    
        public int Id { get; set; }
        public Nullable<int> TransactionTypeId { get; set; }
        public Nullable<int> FacilityId { get; set; }
        public Nullable<int> BuildingId { get; set; }
        public Nullable<int> StoreId { get; set; }
        public Nullable<int> WorkOrdersId { get; set; }
        public string IssuerName { get; set; }
        public Nullable<System.DateTime> IssueReturnDate { get; set; }
        public Nullable<int> PurchaseReqId { get; set; }
    
        public virtual tblFacility tblFacility { get; set; }
        public virtual tblInventoryIssueReturn tblInventoryIssueReturn1 { get; set; }
        public virtual tblInventoryIssueReturn tblInventoryIssueReturn2 { get; set; }
        public virtual tblPurchaseRequisition tblPurchaseRequisition { get; set; }
        public virtual tblStore tblStore { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblIssueReturnItemsList> tblIssueReturnItemsLists { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblIssueReturnItemsListComponent> tblIssueReturnItemsListComponents { get; set; }
    }
}