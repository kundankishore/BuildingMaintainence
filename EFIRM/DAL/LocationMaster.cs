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
    
    public partial class LocationMaster
    {
        public int Id { get; set; }
        public Nullable<int> FacilityId { get; set; }
        public string LocationName { get; set; }
        public string ServerName { get; set; }
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
    
        public virtual tblFacility tblFacility { get; set; }
    }
}