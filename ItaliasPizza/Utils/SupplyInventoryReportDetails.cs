using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizza.Utils
{
	public class SupplyInventoryReportDetails
	{
		public Guid IdSupplyInventoryReport { get; set; }
		public Guid IdInventoryReport { get; set; }
		public Guid IdSupply { get; set; }
		public string SupplyName { get; set; }
		public string MeasurementUnit { get; set; }
		public decimal ExpectedAmount { get; set; }
		public decimal ReportedAmount { get; set; }
		public string DifferingAmountReason { get; set; }
	}
}
