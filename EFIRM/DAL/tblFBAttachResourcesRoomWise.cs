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
    
    public partial class tblFBAttachResourcesRoomWise
    {
        public int Id { get; set; }
        public Nullable<int> RoomId { get; set; }
        public Nullable<int> ResourceId { get; set; }
        public Nullable<double> Quantity { get; set; }
    
        public virtual tblResourceMakeModel tblResourceMakeModel { get; set; }
        public virtual tblRoom tblRoom { get; set; }
    }
}