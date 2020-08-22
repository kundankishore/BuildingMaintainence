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
    
    public partial class tblMRMailroomInbound
    {
        public int nMailroomInboundId { get; set; }
        public string sStatus { get; set; }
        public Nullable<System.DateTime> dtInDate { get; set; }
        public string sInTrackingNumber { get; set; }
        public Nullable<int> nCourierSerivceTypeId { get; set; }
        public Nullable<int> nModeOfDeliveryId { get; set; }
        public string sInOrigin { get; set; }
        public string sInDestination { get; set; }
        public string sInItems { get; set; }
        public string sInDescription { get; set; }
        public Nullable<int> nWorkforceId { get; set; }
        public string sInReceiverEmailId { get; set; }
        public string sInReceiverContactNumber { get; set; }
        public string sInReceiverDesignation { get; set; }
        public string sInSenderName { get; set; }
        public string sInSenderEmailId { get; set; }
        public string sInSenderCompanyName { get; set; }
        public string sInSenderPhoneNumber { get; set; }
    
        public virtual tblMRCourierServiceProvider tblMRCourierServiceProvider { get; set; }
        public virtual tblMRModeOfDelivery tblMRModeOfDelivery { get; set; }
        public virtual tblWorkForce tblWorkForce { get; set; }
    }
}
