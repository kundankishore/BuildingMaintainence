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
    
    public partial class tblMaintenanceTeam
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblMaintenanceTeam()
        {
            this.tblWorkForceToMaintenanceTeamMappings = new HashSet<tblWorkForceToMaintenanceTeamMapping>();
        }
    
        public int Id { get; set; }
        public string Team { get; set; }
        public string Description { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblWorkForceToMaintenanceTeamMapping> tblWorkForceToMaintenanceTeamMappings { get; set; }
    }
}
