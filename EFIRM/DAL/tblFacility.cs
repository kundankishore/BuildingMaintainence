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
    
    public partial class tblFacility
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblFacility()
        {
            this.LocationMasters = new HashSet<LocationMaster>();
            this.tblAreas = new HashSet<tblArea>();
            this.tblBuildings = new HashSet<tblBuilding>();
            this.tblFBAnnouncementMngts = new HashSet<tblFBAnnouncementMngt>();
            this.tblFBBookingSlots = new HashSet<tblFBBookingSlot>();
            this.tblInventoryIssueReturns = new HashSet<tblInventoryIssueReturn>();
            this.tblKeys = new HashSet<tblKey>();
            this.tblOccupants = new HashSet<tblOccupant>();
            this.tblPurchaseRequisitions = new HashSet<tblPurchaseRequisition>();
            this.tblRooms = new HashSet<tblRoom>();
            this.tblServiceReports = new HashSet<tblServiceReport>();
            this.tblUserLoginInfoes = new HashSet<tblUserLoginInfo>();
            this.tblWorkForces = new HashSet<tblWorkForce>();
        }
    
        public int Id { get; set; }
        public Nullable<int> PortfolioId { get; set; }
        public string FacilityName { get; set; }
        public string FacilityAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PinCode { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonEmail { get; set; }
        public string ContactPersonNumber { get; set; }
        public Nullable<int> CurrencyId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string IfcId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LocationMaster> LocationMasters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblArea> tblAreas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBuilding> tblBuildings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblFBAnnouncementMngt> tblFBAnnouncementMngts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblFBBookingSlot> tblFBBookingSlots { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblInventoryIssueReturn> tblInventoryIssueReturns { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblKey> tblKeys { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblOccupant> tblOccupants { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblPurchaseRequisition> tblPurchaseRequisitions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblRoom> tblRooms { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblServiceReport> tblServiceReports { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblUserLoginInfo> tblUserLoginInfoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblWorkForce> tblWorkForces { get; set; }
    }
}
