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
    
    public partial class tblKPI
    {
        public int Id { get; set; }
        public string KPIName { get; set; }
        public string KPIDesc { get; set; }
        public int AssetTypeId { get; set; }
    
        public virtual tblAssetType tblAssetType { get; set; }
    }
}
