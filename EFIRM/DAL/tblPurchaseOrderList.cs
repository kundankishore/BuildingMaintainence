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
    
    public partial class tblPurchaseOrderList
    {
        public int Id { get; set; }
        public Nullable<int> PurchaseRequisitionId { get; set; }
        public Nullable<int> PartCompId { get; set; }
        public string ItemName { get; set; }
        public string ItemUnit { get; set; }
        public string ReqQty { get; set; }
        public Nullable<int> TypeId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Capacity { get; set; }
        public string Rating_Parts { get; set; }
        public string SerialNo { get; set; }
    
        public virtual tblPurchaseRequisition tblPurchaseRequisition { get; set; }
    }
}
