using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizza.Utils
{
	public class OrderProductDetails
	{
		public Guid OrderProductId { get; set; }
		public string ProductName { get; set; }
		public string Quantity { get; set; }
		public decimal UnitPrice { get; set; }
		public decimal SubTotal{ get; set; }
	}
}
