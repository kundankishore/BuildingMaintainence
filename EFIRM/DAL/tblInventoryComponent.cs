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
    
    public partial class tblInventoryComponent
    {
        public int Id { get; set; }
        public Nullable<int> StoreId { get; set; }
        public string Rack { get; set; }
        public Nullable<int> ComponentId { get; set; }
        public Nullable<System.DateTime> WarrentyEndDate { get; set; }
        public string Status { get; set; }
        public Nullable<double> Quantity { get; set; }
        public string Serial { get; set; }
    }
}
