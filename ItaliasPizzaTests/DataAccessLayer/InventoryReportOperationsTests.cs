using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Database;
using ItaliasPizza.DataAccessLayer;
using System.Collections.Generic;

namespace ItaliasPizzaTests.DataAccessLayer
{
	[TestClass]
	public class InventoryReportOperationsTests
	{

		[TestMethod]
		public void RegisterPartialInventoryReportWithSuppliesTest()
		{
			var supplies = new List<Supply>
			{
				new Supply { IdSupply = Guid.NewGuid(), Name = "TestPepperoni", Quantity = 14, IdSupplyCategory = 1, IdMeasurementUnit = 1, ExpirationDate = DateTime.Now, Status = true },
				new Supply { IdSupply = Guid.NewGuid(), Name = "TestCheese", Quantity = 14, IdSupplyCategory = 1, IdMeasurementUnit = 1, ExpirationDate = DateTime.Now, Status = true },
				new Supply { IdSupply = Guid.NewGuid(), Name = "TestTomato", Quantity = 14, IdSupplyCategory = 1, IdMeasurementUnit = 1, ExpirationDate = DateTime.Now, Status = true },
				new Supply { IdSupply = Guid.NewGuid(), Name = "TestDough", Quantity = 14, IdSupplyCategory = 1, IdMeasurementUnit = 1, ExpirationDate = DateTime.Now, Status = true }
			};

			using (var db = new ItaliasPizzaDBEntities())
			{
				foreach (var item in supplies)
				{
					db.Supply.Add(item);
				}
				db.SaveChanges();
			}

			var inventoryReport = new InventoryReport
			{
				IdInventoryReport = Guid.NewGuid(),
				Reporter = Guid.Empty,
				ReportDate = DateTime.Now,
				Status = false
			};

			foreach (var item in supplies)
			{
				inventoryReport.SupplyInventoryReport.Add(new SupplyInventoryReport
				{
					IdSupplyInventoryReport = Guid.NewGuid(),
					IdInventoryReport = inventoryReport.IdInventoryReport,
					IdSupply = item.IdSupply,
					IdMeasurementUnit = item.IdMeasurementUnit,
					ExpectedAmount = item.Quantity,
					ReportedAmount = 0,
					DifferingAmountReason = string.Empty
				});
			}

			var result = InventoryReportOperations.RegisterPartialInventoryReportWithSupplies(inventoryReport);
			InventoryReportOperations.DeleteExistingReport();

			Assert.AreEqual(1, result);
		}

		[TestMethod]
		public void GetInventoryReportTest()
		{
			var inventoryReportId = Guid.NewGuid();
			var inventoryReport = new InventoryReport
			{
				IdInventoryReport = inventoryReportId,
				Reporter = Guid.Empty,
				ReportDate = DateTime.Now,
				Status = false
			};

			var insertResult = InventoryReportOperations.RegisterPartialInventoryReportWithSupplies(inventoryReport);
			var result = InventoryReportOperations.GetInventoryReport();
			InventoryReportOperations.DeleteExistingReport();

			Assert.AreEqual(1, insertResult);
			Assert.IsNotNull(result);
			Assert.AreEqual(inventoryReportId, result.IdInventoryReport);
		}

		[TestMethod]
		public void UpdateInventoryReportTest()
		{
			var supplies = new List<Supply>
			{
				new Supply { IdSupply = Guid.NewGuid(), Name = "TestPepperoni", Quantity = 14, IdSupplyCategory = 1, IdMeasurementUnit = 1, ExpirationDate = DateTime.Now, Status = true },
				new Supply { IdSupply = Guid.NewGuid(), Name = "TestCheese", Quantity = 14, IdSupplyCategory = 1, IdMeasurementUnit = 1, ExpirationDate = DateTime.Now, Status = true },
				new Supply { IdSupply = Guid.NewGuid(), Name = "TestTomato", Quantity = 14, IdSupplyCategory = 1, IdMeasurementUnit = 1, ExpirationDate = DateTime.Now, Status = true },
				new Supply { IdSupply = Guid.NewGuid(), Name = "TestDough", Quantity = 14, IdSupplyCategory = 1, IdMeasurementUnit = 1, ExpirationDate = DateTime.Now, Status = true }
			};

			using (var db = new ItaliasPizzaDBEntities())
			{
				foreach (var item in supplies)
				{
					db.Supply.Add(item);
				}
				db.SaveChanges();
			}

			var inventoryReport = new InventoryReport
			{
				IdInventoryReport = Guid.NewGuid(),
				Reporter = Guid.Empty,
				ReportDate = DateTime.Now,
				Status = false
			};

			foreach (var item in supplies)
			{
				inventoryReport.SupplyInventoryReport.Add(new SupplyInventoryReport
				{
					IdSupplyInventoryReport = Guid.NewGuid(),
					IdInventoryReport = inventoryReport.IdInventoryReport,
					IdSupply = item.IdSupply,
					IdMeasurementUnit = item.IdMeasurementUnit,
					ExpectedAmount = item.Quantity,
					ReportedAmount = 0,
					DifferingAmountReason = string.Empty
				});
			}

			var insertResult = InventoryReportOperations.RegisterPartialInventoryReportWithSupplies(inventoryReport);
			var updatedInventoryReport = new InventoryReport
			{
				IdInventoryReport = inventoryReport.IdInventoryReport,
				Reporter = inventoryReport.Reporter,
				ReportDate = inventoryReport.ReportDate,
				Status = true
			};
			var result = InventoryReportOperations.UpdateInventoryReport(updatedInventoryReport);

			Assert.AreEqual(1, result);
		}

		[TestMethod]
		public void IsInventoryReportOpenTest()
		{
			var inventoryReportId = Guid.NewGuid();
			var inventoryReport = new InventoryReport
			{
				IdInventoryReport = inventoryReportId,
				Reporter = Guid.Empty,
				ReportDate = DateTime.Now,
				Status = false
			};

			var insertResult = InventoryReportOperations.RegisterPartialInventoryReportWithSupplies(inventoryReport);
			var result = InventoryReportOperations.IsInventoryReportOpen();
			InventoryReportOperations.DeleteExistingReport();

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void DeleteExistingReportTest()
		{
			var inventoryReportId = Guid.NewGuid();
			var inventoryReport = new InventoryReport
			{
				IdInventoryReport = inventoryReportId,
				Reporter = Guid.Empty,
				ReportDate = DateTime.Now,
				Status = false
			};

			var insertResult = InventoryReportOperations.RegisterPartialInventoryReportWithSupplies(inventoryReport);
			var result = InventoryReportOperations.DeleteExistingReport();

			Assert.AreEqual(1, insertResult);
			Assert.AreEqual(1, result);
		}
	}
}
