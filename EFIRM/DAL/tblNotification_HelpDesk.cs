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
    
    public partial class tblNotification_HelpDesk
    {
        public int Id { get; set; }
        public string FormName { get; set; }
        public Nullable<int> UserRoleId { get; set; }
        public string EmailNotification { get; set; }
        public string SMSNotification { get; set; }
        public Nullable<int> MaintenanceType { get; set; }
    }
}
