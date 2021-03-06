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
    
    public partial class tblPart
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblPart()
        {
            this.tblInventoryParts = new HashSet<tblInventoryPart>();
            this.tblVM_GRN_PartQuantity = new HashSet<tblVM_GRN_PartQuantity>();
            this.tblVM_RFPforParts = new HashSet<tblVM_RFPforParts>();
        }
    
        public int Id { get; set; }
        public string PartName { get; set; }
        public Nullable<int> PartTypeId { get; set; }
        public Nullable<int> Model { get; set; }
        public string Rating { get; set; }
        public string RecorderLevel { get; set; }
        public string MinOrderQuantity { get; set; }
        public string MaximumLevel { get; set; }
        public string PartNumber { get; set; }
        public Nullable<int> Make { get; set; }
        public string Capacity { get; set; }
        public Nullable<int> ShelfLife { get; set; }
        public Nullable<int> LeadTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblInventoryPart> tblInventoryParts { get; set; }
        public virtual tblPartsType tblPartsType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblVM_GRN_PartQuantity> tblVM_GRN_PartQuantity { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblVM_RFPforParts> tblVM_RFPforParts { get; set; }
    }
}
