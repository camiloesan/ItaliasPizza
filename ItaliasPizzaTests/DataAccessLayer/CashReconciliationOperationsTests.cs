using Database;
using ItaliasPizza.DataAccessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity.Validation;
using System.Linq;

namespace ItaliasPizzaTests.DataAccessLayer
{
    [TestClass]
    public class CashReconciliationOperationsTests
    {
        [TestMethod]
        public void GetCashClosingDetails_Success()
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                Guid cashReconciliationId = Guid.NewGuid();
                Guid saleTransactionId = Guid.NewGuid();
                Guid expenseTransactionId = Guid.NewGuid();
                Guid employeeId = Guid.NewGuid();

                try
                {
                    db.Employee.Add(new Employee
                    {
                        IdEmployee = employeeId,
                        FirstName = "Test",
                        LastName = "Employee",
                        Phone = "1234567890",
                        Status = true,
                        IdCharge = 4
                    });
                    db.CashReconciliation.Add(new CashReconciliation
                    {
                        IdCashReconciliation = cashReconciliationId,
                        OpeningDate = DateTime.Today.AddDays(-1),
                        ClosingDate = DateTime.Today,
                        StartingAmount = 1000,
                        FinishingAmount = 1200,
                        SalesAmount = 500,
                        SpendingsAmount = 300,
                        CashDifference = 200,
                        Observations = "Test closing",
                        RegisteredBy = employeeId
                    });

                    db.Transaction.Add(new Transaction
                    {
                        IdTransaction = saleTransactionId,
                        IdTransactionType = 1,
                        Date = DateTime.Now,
                        Amount = 200,
                        Description = "Test sale",
                        RegisteredBy = employeeId
                    });

                    db.Transaction.Add(new Transaction
                    {
                        IdTransaction = expenseTransactionId,
                        IdTransactionType = 2,
                        Date = DateTime.Now,
                        Amount = 50,
                        Description = "Test expense",
                        RegisteredBy = employeeId
                    });
                    db.SaveChanges();

                    var details = CashReconciliationOperations.GetCashClosingDetails();
                    Assert.AreEqual(1200, details.PreviousDayCash);
                    Assert.AreEqual(200, details.TotalSalesCash);
                    Assert.AreEqual(50, details.TotalSpentCash);
                }
                finally
                {
                    db.Transaction.Remove(db.Transaction.Find(saleTransactionId));
                    db.Transaction.Remove(db.Transaction.Find(expenseTransactionId));
                    db.CashReconciliation.Remove(db.CashReconciliation.Find(cashReconciliationId));
                    db.Employee.Remove(db.Employee.Find(employeeId));
                    db.SaveChanges();
                }
            }
        }

        [TestMethod]
        public void GetCashClosingDetails_NoData()
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                var details = CashReconciliationOperations.GetCashClosingDetails();

                Assert.AreEqual(0, details.PreviousDayCash);
                Assert.AreEqual(0, details.TotalSalesCash);
                Assert.AreEqual(0, details.TotalSpentCash);
            }
        }

        [TestMethod]
        public void IsCashReconciliationRegisteredToday_Success()
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                Guid cashReconciliationId = Guid.NewGuid();
                Guid employeeId = Guid.NewGuid();

                try
                {
                    db.Employee.Add(new Employee
                    {
                        IdEmployee = employeeId,
                        FirstName = "Test",
                        LastName = "Employee",
                        Phone = "1234567890",
                        Status = true,
                        IdCharge = 4
                    });
                    db.CashReconciliation.Add(new CashReconciliation
                    {
                        IdCashReconciliation = cashReconciliationId,
                        OpeningDate = DateTime.Today,
                        ClosingDate = DateTime.Today,
                        StartingAmount = 1000,
                        FinishingAmount = 1200,
                        SalesAmount = 500,
                        SpendingsAmount = 300,
                        CashDifference = 200,
                        Observations = "Test closing",
                        RegisteredBy = employeeId
                    });
                    db.SaveChanges();

                    Assert.IsTrue(CashReconciliationOperations.IsCashReconciliationRegisteredToday());
                }
                finally
                {
                    db.CashReconciliation.Remove(db.CashReconciliation.Find(cashReconciliationId));
                    db.Employee.Remove(db.Employee.Find(employeeId));
                    db.SaveChanges();
                }
            }
        }

        [TestMethod]
        public void IsCashReconciliationRegisteredToday_NoData()
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                Assert.IsFalse(CashReconciliationOperations.IsCashReconciliationRegisteredToday());
            }
        }

        [TestMethod]
        public void SaveCashReconciliation_Success()
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                Guid cashReconciliationId = Guid.NewGuid();
                Guid employeeId = Guid.NewGuid();

                try
                {
                    db.Employee.Add(new Employee
                    {
                        IdEmployee = employeeId,
                        FirstName = "Test",
                        LastName = "Employee",
                        Phone = "1234567890",
                        Status = true,
                        IdCharge = 4
                    });
                    db.SaveChanges();
                    var cashReconciliation = new CashReconciliation
                    {
                        IdCashReconciliation = cashReconciliationId,
                        OpeningDate = DateTime.Today,
                        ClosingDate = DateTime.Today,
                        StartingAmount = 1000,
                        FinishingAmount = 1200,
                        SalesAmount = 500,
                        SpendingsAmount = 300,
                        CashDifference = 200,
                        Observations = "Test save",
                        RegisteredBy = employeeId
                    };

                    var result = CashReconciliationOperations.SaveCashReconciliation(cashReconciliation);
                    Assert.AreEqual(1, result);

                    var savedRecord = db.CashReconciliation.FirstOrDefault(cr => cr.IdCashReconciliation == cashReconciliationId);
                    Assert.IsNotNull(savedRecord);
                }
                finally
                {
                    db.Employee.Remove(db.Employee.Find(employeeId));
                    var recordToDelete = db.CashReconciliation.Find(cashReconciliationId);
                    if (recordToDelete != null)
                    {
                        db.CashReconciliation.Remove(recordToDelete);
                        db.SaveChanges();
                    }
                }
            }
        }


        [TestMethod]
        public void SaveCashReconciliation_InvalidData()
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                try
                {
                    var invalidCashReconciliation = new CashReconciliation
                    {
                        IdCashReconciliation = Guid.NewGuid(),
                        OpeningDate = DateTime.Today,
                        FinishingAmount = 0,
                        SalesAmount = 500,
                        SpendingsAmount = 300,
                        RegisteredBy = Guid.NewGuid()
                    };

                    var result = CashReconciliationOperations.SaveCashReconciliation(invalidCashReconciliation);
                    Assert.AreEqual(0, result);
                }
                catch (DbEntityValidationException)
                {
                    Assert.IsNotNull("La entidad no pasó la validación.");
                }
            }
        }


    }
}
