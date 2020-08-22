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
    
    public partial class tblHelpDeskRedirectFlow
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblHelpDeskRedirectFlow()
        {
            this.tblHelpDeskRedirectAssets = new HashSet<tblHelpDeskRedirectAsset>();
            this.tblHelpDeskRedirectResources = new HashSet<tblHelpDeskRedirectResource>();
        }
    
        public int Id { get; set; }
        public string RedirectComment { get; set; }
        public Nullable<int> AssetGroupId { get; set; }
        public Nullable<int> CategoryTypeId { get; set; }
        public string ApprovalRejectComment { get; set; }
        public Nullable<int> WorkorderId { get; set; }
        public string RedirectStatus { get; set; }
        public string SubmittedUserId { get; set; }
        public string ApproverId { get; set; }
    
        public virtual tblAssetGroup tblAssetGroup { get; set; }
        public virtual tblAssetType tblAssetType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblHelpDeskRedirectAsset> tblHelpDeskRedirectAssets { get; set; }
        public virtual tblWorkOrder tblWorkOrder { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblHelpDeskRedirectResource> tblHelpDeskRedirectResources { get; set; }
    }
}