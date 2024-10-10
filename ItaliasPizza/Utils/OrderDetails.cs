using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizza.Utils
{
	public sealed class OrderDetails
	{
		public Guid OrderId { get; set; }
		public string Client { get; set; }
		public string Table { get; set; }
		public string TotalPrice { get; set; }
		public string Status { get; set; }
		public string OrderType { get; set; }
		public DateTime OrderDate { get; set; }
	}
}
