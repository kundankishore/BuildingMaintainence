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
    
    public partial class tblRoomRecurringBooking
    {
        public int Id { get; set; }
        public int RoomBookingId { get; set; }
        public Nullable<System.DateTime> BookingDate { get; set; }
        public Nullable<System.TimeSpan> StartTime { get; set; }
        public Nullable<System.TimeSpan> EndTime { get; set; }
        public Nullable<int> RoomId { get; set; }
        public string RecurringBookingStatus { get; set; }
    
        public virtual tblRoomBookingMaster tblRoomBookingMaster { get; set; }
    }
}
