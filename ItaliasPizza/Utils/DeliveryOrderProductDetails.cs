using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizza.Utils
{
	public class DeliveryOrderProductDetails
	{
		public Guid IdDeliveryOrder { get; set; }
		public Guid IdProduct { get; set; }
		public string Name { get; set; }
		public int Quantity { get; set; }
		public decimal SubTotal { get; set; }
	}
}
