//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class InventoryReport
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InventoryReport()
        {
            this.SupplyInventoryReport = new HashSet<SupplyInventoryReport>();
        }
    
        public System.Guid IdInventoryReport { get; set; }
        public System.Guid Reporter { get; set; }
        public string Observations { get; set; }
        public System.DateTime ReportDate { get; set; }
        public bool Status { get; set; }
    
        public virtual Employee Employee { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupplyInventoryReport> SupplyInventoryReport { get; set; }
    }
}
