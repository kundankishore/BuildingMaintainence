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
    
    public partial class tblVM_RFPforComponents
    {
        public int Id { get; set; }
        public Nullable<int> RFP_Id { get; set; }
        public Nullable<int> AssetComponentTypeId { get; set; }
        public Nullable<int> AssetComponentId { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<double> PriceValue { get; set; }
    
        public virtual tblAssetComponent tblAssetComponent { get; set; }
        public virtual tblAssetComponentType tblAssetComponentType { get; set; }
    }
}
