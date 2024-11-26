using Database;
using ItaliasPizza.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizza.DataAccessLayer
{
	public class InventoryReportOperations
	{
		public static int RegisterPartialInventoryReportWithSupplies(InventoryReport inventoryReport)
		{
			int result = 0;

			using (var db = new ItaliasPizzaDBEntities())
			{
				using (var transacion = db.Database.BeginTransaction())
				{
					try
					{
						db.InventoryReport.Add(inventoryReport);

						foreach (var inventoryReportSupply in inventoryReport.SupplyInventoryReport)
						{
							db.SupplyInventoryReport.Add(inventoryReportSupply);
						}
						db.SaveChanges();

						transacion.Commit();

						result = 1;
					}
					catch (Exception e)
					{
						transacion.Rollback();
						result = -1;
					}
				}
			}

			return result;
		}

		public static InventoryReport GetInventoryReport()
		{
			InventoryReport inventoryReport = null;

			using (var db = new ItaliasPizzaDBEntities())
			{
				inventoryReport = db.InventoryReport
					.Include("SupplyInventoryReport")
					.Include("SupplyInventoryReport.Supply")
					.Include("SupplyInventoryReport.MeasurementUnit")
					.FirstOrDefault(ir => ir.Status == false);
			}

			return inventoryReport;
		}

		public static int UpdateInventoryReport(InventoryReport inventoryReport)
		{
			int result = 0;

			using (var db = new ItaliasPizzaDBEntities())
			{
				var existingReport = db.InventoryReport
					.Include("SupplyInventoryReport")
					.FirstOrDefault(ir => ir.IdInventoryReport == inventoryReport.IdInventoryReport);

				if (existingReport != null)
				{
					existingReport.Status = inventoryReport.Status;
					existingReport.Observations = inventoryReport.Observations;

					foreach (var supplyReport in inventoryReport.SupplyInventoryReport)
					{
						var existingSupplyReport = existingReport.SupplyInventoryReport
							.FirstOrDefault(sir => sir.IdSupplyInventoryReport == supplyReport.IdSupplyInventoryReport);

						if (existingSupplyReport != null)
						{
							existingSupplyReport.ReportedAmount = supplyReport.ReportedAmount;
							existingSupplyReport.DifferingAmountReason = supplyReport.DifferingAmountReason;
						}
					}

					result = db.SaveChanges();
				}
			}

			return result;
		}

		public static bool IsInventoryReportOpen()
		{
			bool isOpen = false;

			using (var db = new ItaliasPizzaDBEntities())
			{
				isOpen = db.InventoryReport.Any(ir => ir.Status == false);
			}

			return isOpen;
		}
		

		public static int DeleteExistingReport() {
			int result = 0;

			using (var db = new ItaliasPizzaDBEntities())
			{
				using (var transaction = db.Database.BeginTransaction())
				{
					try
					{
						var inventoryReport = db.InventoryReport.FirstOrDefault(ir => ir.Status == false);
						if (inventoryReport != null)
						{
							var supplyInventoryReports = db.SupplyInventoryReport.Where(sir => sir.IdInventoryReport == inventoryReport.IdInventoryReport);
							foreach (var supplyInventoryReport in supplyInventoryReports)
							{
								db.SupplyInventoryReport.Remove(supplyInventoryReport);
							}
							db.InventoryReport.Remove(inventoryReport);
							db.SaveChanges();
							transaction.Commit();
							result = 1;
						}
					}
					catch (Exception e)
					{
						transaction.Rollback();
						result = -1;
					}
				}
			}

			return result;
		} 
	}
}
