using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizza.Utils
{
    public class SupplierOrderDetails
    {
        public Guid OrderIdentifier { get; set; }
        public Guid IdSupplier { get; set; }
        public string SupplierName { get; set; }
        public string Supplies { get; set; }
        public string OrderDate { get; set; }
        public string ExpectedDate { get; set; }
        public string ArrivalDate { get; set; }
        public string Status { get; set; }
    }
}
