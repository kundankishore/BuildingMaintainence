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
    
    public partial class tblMonthlyLogger
    {
        public int Id { get; set; }
        public Nullable<int> HistoryId { get; set; }
        public Nullable<int> UnitId { get; set; }
        public Nullable<int> MonthNo { get; set; }
        public Nullable<int> LogYear { get; set; }
        public Nullable<double> MonthlyValue { get; set; }
    }
}
