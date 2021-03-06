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
    
    public partial class tblHelpDeskChangeRTDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblHelpDeskChangeRTDetail()
        {
            this.tblHelpDeskChangeRTAssets = new HashSet<tblHelpDeskChangeRTAsset>();
            this.tblHelpDeskChangeRTResources = new HashSet<tblHelpDeskChangeRTResource>();
        }
    
        public int Id { get; set; }
        public int ServiceRequestId { get; set; }
        public Nullable<int> RequestTypeId { get; set; }
        public Nullable<int> AssetGroupId { get; set; }
        public Nullable<int> CategoryTypeId { get; set; }
        public Nullable<int> LeadTechnicianId { get; set; }
        public string RequestorComment { get; set; }
        public string ApprovalRejectComment { get; set; }
        public Nullable<int> RequestSubmittedUserLoginId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblHelpDeskChangeRTAsset> tblHelpDeskChangeRTAssets { get; set; }
        public virtual tblServiceRequest tblServiceRequest { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblHelpDeskChangeRTResource> tblHelpDeskChangeRTResources { get; set; }
    }
}
