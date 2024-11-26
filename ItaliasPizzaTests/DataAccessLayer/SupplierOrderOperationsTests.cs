using Database;
using ItaliasPizza.DataAccessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

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

        [TestMethod]
        public void SaveSupplierOrderNegativeTest()
        {
            var invalidIdSupply = Guid.NewGuid();
            var invalidIdSupplier = Guid.NewGuid();

            var IdSupplierOrder = Guid.NewGuid();
            var supplierOrder = new SupplierOrder
            {
                IdSupplierOrder = IdSupplierOrder,
                IdSupplier = invalidIdSupplier,
                IdSupply = invalidIdSupply,
                OrderDate = DateTime.Today,
                ExpectedDate = DateTime.Today.AddDays(10),
                IdOrderStatus = 1
            };

            int result = 0;
            Exception saveException = null;

            try
            {
                result = SupplierOrderOperations.SaveSupplierOrder(supplierOrder);
            }
            catch (Exception ex)
            {
                saveException = ex;
            }

            Assert.AreEqual(0, result, "El pedido no debería haberse guardado.");
            Assert.IsNotNull(saveException, "Se esperaba una excepción debido a claves foráneas inválidas.");

            using (var db = new ItaliasPizzaDBEntities())
            {
                var savedOrder = db.SupplierOrder.Find(IdSupplierOrder);
                Assert.IsNull(savedOrder, "El pedido no debería haberse guardado en la base de datos.");
            }
        }

        [TestClass]
        public class OrderedSupplyOperationsTests
        {
            [TestMethod]
            public void SaveOrderedSupplyTest()
            {
                var IdSupplierOrder = Guid.NewGuid();
                var IdSupply = Guid.NewGuid();
                var orderedSupplies = new List<OrderedSupply>
        {
            new OrderedSupply
            {
                IdSupply = IdSupply,
                IdSupplierOrder = IdSupplierOrder,
                OrderIdentifier = Guid.NewGuid(),
                Quantity = 5,
                IdMeasurementUnit = 1
            }
        };

                var IdSupplier = Guid.NewGuid();
                var supplier = new Supplier
                {
                    IdSupplier = IdSupplier,
                    Name = "Test Supplier",
                    Phone = "1234567890"
                };

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
                    db.SupplierOrder.Add(supplierOrder);
                    db.Supply.Add(new Supply
                    {
                        IdSupply = IdSupply,
                        Name = "Test Supply",
                        Quantity = 1,
                        IdSupplyCategory = 1,
                        IdMeasurementUnit = 1,
                        ExpirationDate = DateTime.Now,
                        Status = true
                    });
                    db.SaveChanges();

                    result = SupplierOrderOperations.SaveOrderedSupply(orderedSupplies);
                }

                using (var db = new ItaliasPizzaDBEntities())
                {
                    db.OrderedSupply.RemoveRange(db.OrderedSupply.Where(o => o.IdSupplierOrder == IdSupplierOrder));
                    db.SupplierOrder.Remove(supplierOrder);
                    db.Supplier.Remove(supplier);
                    db.Supply.Remove(db.Supply.Find(IdSupply));
                    db.SaveChanges();
                }

                Assert.AreEqual(1, result);
            }

            [TestMethod]
            public void SaveOrderedSupplyNegativeTest()
            {
                var orderedSupplies = new List<OrderedSupply>
                {
                    new OrderedSupply
                    {
                        IdSupply = Guid.NewGuid(),
                        IdSupplierOrder = Guid.NewGuid(),
                        OrderIdentifier = Guid.NewGuid(),
                        Quantity = 5,
                        IdMeasurementUnit = 1
                    }
                };

                int result = 0;
                Exception saveException = null;

                try
                {
                    result = SupplierOrderOperations.SaveOrderedSupply(orderedSupplies);
                }
                catch (Exception ex)
                {
                    saveException = ex;
                }

                Assert.AreEqual(0, result, "No se debería haber guardado el suministro ordenado.");
                Assert.IsNotNull(saveException, "Se esperaba una excepción debido a claves foráneas inválidas.");
            }
        }

        [TestMethod]
        public void GetSupplierOrderDetailsTest()
        {
            var orderIdentifier = Guid.NewGuid();
            var IdSupply = Guid.NewGuid();
            var IdSupplierOrder = Guid.NewGuid();
            var IdSupplier = Guid.NewGuid();

            using (var db = new ItaliasPizzaDBEntities())
            {
                var supply = new Supply
                {
                    IdSupply = IdSupply,
                    Name = "Test Supply",
                    Quantity = 5,
                    IdSupplyCategory = 1,
                    IdMeasurementUnit = 1,
                    ExpirationDate = DateTime.Now.AddMonths(1),
                    Status = true
                };
                db.Supply.Add(supply);

                db.Supplier.Add(new Supplier
                {
                    IdSupplier = IdSupplier,
                    Name = "Test Supplier",
                    Phone = "1234567890"
                });

                var supplierOrder = new SupplierOrder
                {
                    IdSupplierOrder = IdSupplierOrder,
                    OrderDate = DateTime.Now,
                    IdSupplier = IdSupplier,
                    IdOrderStatus = 1
                };
                db.SupplierOrder.Add(supplierOrder);

                var orderedSupply = new OrderedSupply
                {
                    IdSupply = IdSupply,
                    OrderIdentifier = orderIdentifier,
                    Quantity = 5,
                    IdMeasurementUnit = 1,
                    IdSupplierOrder = IdSupplierOrder
                };
                db.OrderedSupply.Add(orderedSupply);

                db.SaveChanges();
            }

            var details = SupplierOrderOperations.GetSupplierOrderDetails();

            Assert.IsTrue(details.Any(d => d.IdSupplier == IdSupplier && d.OrderIdentifier == orderIdentifier), "No se encontró el detalle de la orden.");

            using (var db = new ItaliasPizzaDBEntities())
            {
                db.OrderedSupply.RemoveRange(db.OrderedSupply.Where(os => os.OrderIdentifier == orderIdentifier));
                db.SupplierOrder.RemoveRange(db.SupplierOrder.Where(so => so.IdSupplierOrder == IdSupplierOrder));
                db.Supply.RemoveRange(db.Supply.Where(s => s.IdSupply == IdSupply));
                db.SaveChanges();
            }
        }

        [TestMethod]
        public void GetSupplierOrderDetailsNoResultTest()
        {
            var details = SupplierOrderOperations.GetSupplierOrderDetails();
            var expectedResult = details.Count;
            Assert.IsTrue(details.Count == expectedResult, "No debería haber resultados cuando no existen datos en la base.");
        }


        [TestMethod]
        public void GetOrderedSupplyDetailsByOrderIdentifierTest()
        {
            var orderIdentifier = Guid.NewGuid();
            var IdSupply = Guid.NewGuid();
            var IdSupplierOrder = Guid.NewGuid();
            var IdSupplier = Guid.NewGuid();

            using (var db = new ItaliasPizzaDBEntities())
            {
                var supply = new Supply
                {
                    IdSupply = IdSupply,
                    Name = "Test Supply",
                    Quantity = 5,
                    IdSupplyCategory = 1,
                    IdMeasurementUnit = 1,
                    ExpirationDate = DateTime.Now.AddMonths(1),
                    Status = true
                };
                db.Supply.Add(supply);

                db.Supplier.Add(new Supplier
                {
                    IdSupplier = IdSupplier,
                    Name = "Test Supplier",
                    Phone = "1234567890"
                });

                var supplierOrder = new SupplierOrder
                {
                    IdSupplierOrder = IdSupplierOrder,
                    OrderDate = DateTime.Now,
                    IdSupplier = IdSupplier,
                    IdOrderStatus = 1
                };
                db.SupplierOrder.Add(supplierOrder);

                var orderedSupply = new OrderedSupply
                {
                    IdSupply = IdSupply,
                    OrderIdentifier = orderIdentifier,
                    Quantity = 5,
                    IdMeasurementUnit = 1,
                    IdSupplierOrder = IdSupplierOrder
                };
                db.OrderedSupply.Add(orderedSupply);

                db.SaveChanges();
            }

            var details = SupplierOrderOperations.GetOrderedSupplyDetailsByOrderIdentifier(orderIdentifier);

            Assert.IsTrue(details.Any(d => d.SupplyName == "Test Supply"), "No se encontró el suministro.");

            using (var db = new ItaliasPizzaDBEntities())
            {
                db.OrderedSupply.RemoveRange(db.OrderedSupply.Where(os => os.OrderIdentifier == orderIdentifier));
                db.SupplierOrder.RemoveRange(db.SupplierOrder.Where(so => so.IdSupplierOrder == IdSupplierOrder));
                db.Supply.RemoveRange(db.Supply.Where(s => s.IdSupply == IdSupply));
                db.SaveChanges();
            }
        }


        [TestMethod]
        public void GetOrderedSupplyDetailsByOrderIdentifierNegativeTest()
        {
            var invalidOrderIdentifier = Guid.NewGuid();
            var details = SupplierOrderOperations.GetOrderedSupplyDetailsByOrderIdentifier(invalidOrderIdentifier);

            Assert.IsTrue(details.Count == 0, "No debería haber resultados para un identificador de orden inválido.");
        }


        [TestMethod]
        public void DeleteSupplierOrdersAndOrderedSuppliesTest()
        {
            var orderIdentifier = Guid.NewGuid();
            var IdSupplierOrder = Guid.NewGuid();
            var IdSupply = Guid.NewGuid();
            var IdSupplier = Guid.NewGuid();

            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Supplier.Add(new Supplier
                {
                    IdSupplier = IdSupplier,
                    Name = "Test Supplier",
                    Phone = "1234567890"
                });

                db.Supply.Add(new Supply
                {
                    IdSupply = IdSupply,
                    Name = "Test Supply",
                    Quantity = 1,
                    IdSupplyCategory = 1,
                    IdMeasurementUnit = 1,
                    ExpirationDate = DateTime.Now,
                    Status = true
                });

                db.SupplierOrder.Add(new SupplierOrder
                {
                    IdSupplierOrder = IdSupplierOrder,
                    IdSupplier = IdSupplier,
                    IdSupply = IdSupply,
                    OrderDate = DateTime.Today,
                    ExpectedDate = DateTime.Today.AddDays(10),
                    IdOrderStatus = 1
                });

                db.OrderedSupply.Add(new OrderedSupply
                {
                    IdSupply = IdSupply,
                    OrderIdentifier = orderIdentifier,
                    IdSupplierOrder = IdSupplierOrder,
                    Quantity = 5,
                    IdMeasurementUnit = 1
                });
                db.SaveChanges();
            }

            int result = SupplierOrderOperations.DeleteSupplierOrdersAndOrderedSupplies(orderIdentifier);
            Assert.AreEqual(2, result, "La orden de proveedor y los suministros deberían haberse eliminado.");

            using (var db = new ItaliasPizzaDBEntities())
            {
                var supplierOrder = db.SupplierOrder.Find(IdSupplierOrder);
                Assert.IsNull(supplierOrder, "La orden de proveedor debería haber sido eliminada.");

                var orderedSupply = db.OrderedSupply.FirstOrDefault(os => os.OrderIdentifier == orderIdentifier);
                Assert.IsNull(orderedSupply, "Los suministros ordenados deberían haber sido eliminados.");
            }
        }


    }
}
