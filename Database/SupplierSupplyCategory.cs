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
    
    public partial class SupplierSupplyCategory
    {
        public int IdSupplierSupplyCategory { get; set; }
        public System.Guid IdSupplier { get; set; }
        public int IdSupplyCategory { get; set; }
    
        public virtual Supplier Supplier { get; set; }
        public virtual SupplyCategory SupplyCategory { get; set; }
    }
}
