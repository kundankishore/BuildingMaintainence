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
    
    public partial class tbl_Devices
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Devices()
        {
            this.tbl_ObjectList = new HashSet<tbl_ObjectList>();
        }
    
        public int nDevicesId { get; set; }
        public string sDeviceName { get; set; }
        public string sDeviceNumber { get; set; }
        public string sIPAddress { get; set; }
        public Nullable<int> nPortNo { get; set; }
        public Nullable<int> nVendorId { get; set; }
        public Nullable<int> nNetwork { get; set; }
        public Nullable<int> nSegmentationSupported { get; set; }
        public Nullable<int> nNoOfObjects { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ObjectList> tbl_ObjectList { get; set; }
    }
}
