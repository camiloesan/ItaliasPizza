using Database;
using ItaliasPizza.DataAccessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace ItaliasPizzaTests.DataAccessLayer
{
    [TestClass]
    public class TransactionOperationsTests
    {
        [TestMethod]
        public void SaveTransaction_Success()
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                Guid transactionId = Guid.NewGuid();
                Guid employeeId = Guid.NewGuid();

                try
                {
                    // Insertar un empleado necesario para la clave foránea
                    db.Employee.Add(new Employee
                    {
                        IdEmployee = employeeId,
                        FirstName = "Test",
                        LastName = "Employee",
                        Phone = "1234567890",
                        Status = true,
                        IdCharge = 4 // ID válido en la tabla Charge (p.e., "Cajero")
                    });
                    db.SaveChanges();

                    var transaction = new Transaction
                    {
                        IdTransaction = transactionId,
                        IdTransactionType = 1, // ID válido en la tabla TransactionType (p.e., "Venta")
                        Date = DateTime.Now,
                        Amount = 100.0m,
                        Description = "Test transaction",
                        RegisteredBy = employeeId
                    };

                    var result = TransactionOperations.SaveTransaction(transaction);
                    Assert.AreEqual(1, result);

                    var savedTransaction = db.Transaction.FirstOrDefault(t => t.IdTransaction == transactionId);
                    Assert.IsNotNull(savedTransaction);
                }
                finally
                {
                    db.Transaction.Remove(db.Transaction.Find(transactionId));
                    db.Employee.Remove(db.Employee.Find(employeeId));
                    db.SaveChanges();
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(DbUpdateException))]
        public void SaveTransaction_Failure_InvalidForeignKey()
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                var transaction = new Transaction
                {
                    IdTransaction = Guid.NewGuid(),
                    IdTransactionType = -1,
                    Date = DateTime.Now,
                    Amount = 100.0m,
                    Description = "Invalid transaction",
                    RegisteredBy = Guid.NewGuid()
                };

                TransactionOperations.SaveTransaction(transaction);
            }
        }

    }
}
