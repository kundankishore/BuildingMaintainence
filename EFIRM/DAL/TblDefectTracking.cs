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
    
    public partial class TblDefectTracking
    {
        public int Id { get; set; }
        public string MRNnumber { get; set; }
        public string PartCompName { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public Nullable<int> ReturnQuantity { get; set; }
        public string Comments { get; set; }
        public Nullable<int> ReturningPerson { get; set; }
        public Nullable<System.DateTime> ReturnDate { get; set; }
        public Nullable<int> PartNo { get; set; }
        public Nullable<int> purchaseRequistionId { get; set; }
        public Nullable<int> PartCompType { get; set; }
    }
}
