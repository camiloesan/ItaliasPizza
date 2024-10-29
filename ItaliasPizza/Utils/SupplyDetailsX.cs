using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizza.Utils
{
    public class SupplyDetailsX
    {
        public Guid IdSupply { get; set; }
        public string Name { get; set; }
        public string Quantity { get; set; }
        public string MeasurementUnit { get; set; }
        public string ExpirationDate { get; set; }
        public int IdSupplyCategory { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
    }
}
