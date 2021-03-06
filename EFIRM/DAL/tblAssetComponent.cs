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
    
    public partial class tblAssetComponent
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblAssetComponent()
        {
            this.tblVM_GRN_ComponentQuantity = new HashSet<tblVM_GRN_ComponentQuantity>();
            this.tblVM_RFPforComponents = new HashSet<tblVM_RFPforComponents>();
        }
    
        public int Id { get; set; }
        public string AssetComponentName { get; set; }
        public Nullable<int> AssetComponentTypeId { get; set; }
        public Nullable<int> Make { get; set; }
        public Nullable<int> Model { get; set; }
        public string SerialNo { get; set; }
        public Nullable<double> Quantity { get; set; }
        public string CapacitySize { get; set; }
        public Nullable<int> AssetsId { get; set; }
        public Nullable<System.DateTime> WarantyEndDate { get; set; }
    
        public virtual tblAssetComponentType tblAssetComponentType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblVM_GRN_ComponentQuantity> tblVM_GRN_ComponentQuantity { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblVM_RFPforComponents> tblVM_RFPforComponents { get; set; }
    }
}
