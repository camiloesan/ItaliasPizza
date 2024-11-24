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

		public static int UpdateInventoryReport(InventoryReport inventoryReport)
		{
			return 0;
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
