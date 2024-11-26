using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizza.Utils
{
    public class ProductDetails
    {
        public Guid IdProduct { get; set; }
        public string Name { get; set; }
        public Decimal Price { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }
}
