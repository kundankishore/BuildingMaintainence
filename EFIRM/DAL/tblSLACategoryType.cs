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
    
    public partial class tblSLACategoryType
    {
        public int Id { get; set; }
        public Nullable<int> SLAId { get; set; }
        public Nullable<int> RequestTypeID { get; set; }
        public Nullable<int> CategoryTypeID { get; set; }
    
        public virtual tblSLA tblSLA { get; set; }
    }
}
