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
    
    public partial class Supply
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Supply()
        {
            this.OrderedSupply = new HashSet<OrderedSupply>();
            this.RecipeSupply = new HashSet<RecipeSupply>();
            this.SupplyInventoryReport = new HashSet<SupplyInventoryReport>();
        }
    
        public System.Guid IdSupply { get; set; }
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public int IdSupplyCategory { get; set; }
        public int IdMeasurementUnit { get; set; }
        public System.DateTime ExpirationDate { get; set; }
        public bool Status { get; set; }
    
        public virtual MeasurementUnit MeasurementUnit { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderedSupply> OrderedSupply { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RecipeSupply> RecipeSupply { get; set; }
        public virtual SupplyCategory SupplyCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupplyInventoryReport> SupplyInventoryReport { get; set; }
    }
}
