using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizza.Utils
{
    public class OrderedSupplyDetails
    {
        public Guid IdSupply { get; set; }
        public string SupplyName { get; set; }
        public decimal SupplyAmount { get; set; }
        public string MeasurementUnit { get; set; }
        public int IdMeasurementUnit { get; set; }
    }
}
