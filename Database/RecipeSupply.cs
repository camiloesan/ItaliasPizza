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
    
    public partial class RecipeSupply
    {
        public System.Guid IdRecipeSupply { get; set; }
        public System.Guid IdRecipe { get; set; }
        public System.Guid IdSupply { get; set; }
        public decimal SupplyAmount { get; set; }
        public int IdMeasurementUnit { get; set; }
    
        public virtual MeasurementUnit MeasurementUnit { get; set; }
        public virtual Recipe Recipe { get; set; }
        public virtual Supply Supply { get; set; }
    }
}
