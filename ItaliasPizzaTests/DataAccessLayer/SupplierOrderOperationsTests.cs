using Database;
using ItaliasPizza.DataAccessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ItaliasPizzaTests.DataAccessLayer
{
    [TestClass]
    public class SupplierOrderOperationsTests
    {
        [TestMethod]
        public void SaveSupplierOrderTest()
        {
            var IdSupply = Guid.NewGuid();
            var supply = new Supply
            {
                IdSupply = IdSupply,
                Name = "Test",
                Quantity = 1,
                IdSupplyCategory = 1,
                IdMeasurementUnit = 1,
                ExpirationDate = DateTime.Now,
                Status = true
            };

            var IdSupplier = Guid.NewGuid();
            var supplier = new Supplier
            {
                IdSupplier = IdSupplier,
                Name = "Test",
                Phone = "2282739000"
            };

            var IdSupplierOrder = Guid.NewGuid();
            var supplierOrder = new SupplierOrder
            {
                IdSupplierOrder = IdSupplierOrder,
                IdSupplier = IdSupplier,
                IdSupply = IdSupply,
                OrderDate = DateTime.Today,
                ExpectedDate = DateTime.Today.AddDays(10),
                IdOrderStatus = 1
            };

            int result;

            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Supplier.Add(supplier);
                db.Supply.Add(supply);
                db.SaveChanges();

                result = SupplierOrderOperations.SaveSupplierOrder(supplierOrder);
            }

            using (var db = new ItaliasPizzaDBEntities())
            {
                db.SupplierOrder.Attach(supplierOrder);
                db.SupplierOrder.Remove(supplierOrder);

                db.Supply.Attach(supply);
                db.Supply.Remove(supply);

                db.Supplier.Attach(supplier);
                db.Supplier.Remove(supplier);

                db.SaveChanges();
            }

            Assert.AreEqual(1, result);
        }

    }
}
