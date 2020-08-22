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
    
    public partial class tblUserLoginInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblUserLoginInfo()
        {
            this.tblFBAnnouncementMngts = new HashSet<tblFBAnnouncementMngt>();
            this.tblQuickLinks = new HashSet<tblQuickLink>();
            this.tblRoomBookingMasters = new HashSet<tblRoomBookingMaster>();
        }
    
        public int Id { get; set; }
        public Nullable<int> WorkforceId { get; set; }
        public Nullable<int> UserRoleId { get; set; }
        public Nullable<int> FacilityId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Nullable<int> LanguageId { get; set; }
        public string Email_id { get; set; }
        public string Contact_No { get; set; }
        public string LoginTypeId { get; set; }
        public Nullable<bool> IsAllowRoomBooking { get; set; }
        public string UserSignature { get; set; }
    
        public virtual tblFacility tblFacility { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblFBAnnouncementMngt> tblFBAnnouncementMngts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblQuickLink> tblQuickLinks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblRoomBookingMaster> tblRoomBookingMasters { get; set; }
        public virtual tblUserRole tblUserRole { get; set; }
    }
}
