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
    
    public partial class tblRoomBookingMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblRoomBookingMaster()
        {
            this.tblRoomBookingHistories = new HashSet<tblRoomBookingHistory>();
            this.tblRoomRecurringBookings = new HashSet<tblRoomRecurringBooking>();
        }
    
        public int Id { get; set; }
        public string RoomBookingNo { get; set; }
        public Nullable<int> RoomId { get; set; }
        public Nullable<bool> BMSControl { get; set; }
        public Nullable<System.DateTime> BookingDate { get; set; }
        public Nullable<System.DateTime> BookingEndDate { get; set; }
        public Nullable<System.TimeSpan> StratTime { get; set; }
        public Nullable<System.TimeSpan> EndTime { get; set; }
        public string Reason { get; set; }
        public Nullable<int> BookBy { get; set; }
        public Nullable<int> BookOnBehalf { get; set; }
        public Nullable<bool> IsRecurringBooking { get; set; }
        public Nullable<int> BookingFrequency { get; set; }
        public string BookingStatus { get; set; }
        public string ApprovalStatus { get; set; }
        public Nullable<double> Duration { get; set; }
        public Nullable<double> RatePerHour { get; set; }
        public Nullable<double> TotalAmount { get; set; }
        public string InvoiceCreated { get; set; }
        public Nullable<System.Guid> RoomBookGuid { get; set; }
        public Nullable<int> nProgramId { get; set; }
        public Nullable<int> nCourseId { get; set; }
        public Nullable<int> nModuleId { get; set; }
        public string OverallBookingStatus { get; set; }
    
        public virtual tblOccupant tblOccupant { get; set; }
        public virtual tblRoom tblRoom { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblRoomBookingHistory> tblRoomBookingHistories { get; set; }
        public virtual tblUserLoginInfo tblUserLoginInfo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblRoomRecurringBooking> tblRoomRecurringBookings { get; set; }
    }
}
