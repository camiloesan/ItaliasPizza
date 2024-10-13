using Database;
using ItaliasPizza.DataAccessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ItaliasPizzaTests.DataAccessLayer
{
    [TestClass]
    public class SupplyOperationsTests
    {
        [TestMethod]
        public void GetSupplyCategoriesTest()
        {
            var result = SupplyOperations.GetSupplyCategories().Count;

            Assert.AreEqual(9, result);
        }

        [TestMethod]
        public void GetMeasurementUnitsTest()
        {
            var result = SupplyOperations.GetMeasurementUnits().Count;
            
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void SaveSupplyTest()
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

            var result = SupplyOperations.SaveSupply(supply);

            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Supply.Attach(supply);

                db.Supply.Remove(supply);
                db.SaveChanges();
            }

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void UpdateSupplyStatusTest()
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

            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Supply.Add(supply);
                db.SaveChanges();
            }

            var result = SupplyOperations.UpdateSupplyStatus(IdSupply, false);

            using (var db = new ItaliasPizzaDBEntities())
            {
                var updatedSupply = db.Supply.FirstOrDefault(s => s.IdSupply == IdSupply);
                Assert.IsNotNull(updatedSupply);
                Assert.IsFalse(updatedSupply.Status);

                db.Supply.Remove(updatedSupply);
                db.SaveChanges();
            }

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UpdateSupplyInfoTest()
        {
            var IdSupply = Guid.NewGuid();

            var supply = new Supply
            {
                IdSupply = IdSupply,
                Name = "Original Name",
                Quantity = 10,
                IdSupplyCategory = 1,
                IdMeasurementUnit = 1,
                ExpirationDate = DateTime.Today.AddDays(30),
                Status = true
            };

            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Supply.Add(supply);
                db.SaveChanges();
            }

            supply.Name = "Updated Name";
            supply.Quantity = 20;
            supply.IdSupplyCategory = 2;
            supply.IdMeasurementUnit = 2;
            supply.ExpirationDate = DateTime.Today.AddDays(60);

            var result = SupplyOperations.UpdateSupplyInfo(supply);

            using (var db = new ItaliasPizzaDBEntities())
            {
                var updatedSupply = db.Supply.FirstOrDefault(s => s.IdSupply == IdSupply);

                Assert.IsNotNull(updatedSupply);
                Assert.AreEqual("Updated Name", updatedSupply.Name);
                Assert.AreEqual(20, updatedSupply.Quantity);
                Assert.AreEqual(2, updatedSupply.IdSupplyCategory);
                Assert.AreEqual(2, updatedSupply.IdMeasurementUnit);
                Assert.AreEqual(supply.ExpirationDate, updatedSupply.ExpirationDate);

                db.Supply.Remove(updatedSupply);
                db.SaveChanges();
            }

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetSuppliesByCategoriesOfSupplierTest()
        {
            var IdSupplier = Guid.NewGuid();
            var supplier = new Supplier
            {
                IdSupplier = IdSupplier,
                Name = "TestSupplier",
                Phone = "2282739002"
            };

            var supplyCategory = new SupplyCategory
            {
                SupplyCategory1 = "Harinas"
            };

            var IdSupply = Guid.NewGuid();
            var supply = new Supply
            {
                IdSupply = IdSupply,
                Name = "Harina de trigo",
                Quantity = 10,
                IdSupplyCategory = 1,
                IdMeasurementUnit = 1,
                ExpirationDate = DateTime.Now.AddMonths(6),
                Status = true
            };

            var supplierSupplyCategory = new SupplierSupplyCategory
            {
                IdSupplier = IdSupplier,
                IdSupplyCategory = 1
            };

            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Supplier.Add(supplier);
                db.SupplyCategory.Add(supplyCategory);
                db.Supply.Add(supply);
                db.SupplierSupplyCategory.Add(supplierSupplyCategory);
                db.SaveChanges();
            }

            var result = SupplyOperations.GetSuppliesByCategoriesOfSupplier(IdSupplier);

            Assert.IsTrue(result.Any(s => s.IdSupply == IdSupply));

            using (var db = new ItaliasPizzaDBEntities())
            {
                var supplierToRemove = db.Supplier.Find(IdSupplier);
                var supplyToRemove = db.Supply.Find(IdSupply);
                var supplierCategoryToRemove = db.SupplierSupplyCategory
                    .FirstOrDefault(sc => sc.IdSupplier == IdSupplier && sc.IdSupplyCategory == 1);

                if (supplierToRemove != null)
                {
                    db.Supplier.Remove(supplierToRemove);
                }
                if (supplyToRemove != null)
                {
                    db.Supply.Remove(supplyToRemove);
                }
                if (supplierCategoryToRemove != null)
                {
                    db.SupplierSupplyCategory.Remove(supplierCategoryToRemove);
                }

                db.SaveChanges();
            }
        }

        [TestMethod]
        public void GetSupplyByIdTest()
        {
            var IdSupply = Guid.NewGuid();
            var supply = new Supply
            {
                IdSupply = IdSupply,
                Name = "Aceite de oliva",
                Quantity = 5,
                IdSupplyCategory = 1,
                IdMeasurementUnit = 1,
                ExpirationDate = DateTime.Now.AddMonths(12),
                Status = true
            };

            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Supply.Add(supply);
                db.SaveChanges();
            }

            var result = SupplyOperations.GetSupplyById(IdSupply);

            Assert.IsNotNull(result);
            Assert.AreEqual("Aceite de oliva", result.Name);
            Assert.AreEqual(IdSupply, result.IdSupply);

            using (var db = new ItaliasPizzaDBEntities())
            {
                var supplyToRemove = db.Supply.Find(IdSupply);
                if (supplyToRemove != null)
                {
                    db.Supply.Remove(supplyToRemove);
                    db.SaveChanges();
                }
            }
        }

        [TestMethod]
        public void GetSuppliesByCategoriesOfSupplierNegativeTest()
        {
            var nonExistentIdSupplier = Guid.NewGuid();

            var result = SupplyOperations.GetSuppliesByCategoriesOfSupplier(nonExistentIdSupplier);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetSupplyByIdNegativeTest()
        {
            var nonExistentIdSupply = Guid.NewGuid();

            var result = SupplyOperations.GetSupplyById(nonExistentIdSupply);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void SaveSupplyNegativeTest()
        {
            var IdSupply = Guid.NewGuid();
            var supply = new Supply
            {
                IdSupply = IdSupply,
                Name = "Test",
                Quantity = 1,
                IdSupplyCategory = 999,
                IdMeasurementUnit = 1,
                ExpirationDate = DateTime.Now,
                Status = true
            };

            try
            {
                var result = SupplyOperations.SaveSupply(supply);

                Assert.AreEqual(0, result);
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
            {
                Assert.IsTrue(ex.InnerException.InnerException.Message.Contains("FOREIGN KEY constraint"));
            }

            using (var db = new ItaliasPizzaDBEntities())
            {
                var savedSupply = db.Supply.Find(IdSupply);
                Assert.IsNull(savedSupply);
            }
        }


        [TestMethod]
        public void UpdateSupplyStatusNegativeTest()
        {
            var nonExistentIdSupply = Guid.NewGuid();

            var result = SupplyOperations.UpdateSupplyStatus(nonExistentIdSupply, false);

            Assert.IsFalse(result);

            using (var db = new ItaliasPizzaDBEntities())
            {
                var updatedSupply = db.Supply.FirstOrDefault(s => s.IdSupply == nonExistentIdSupply);
                Assert.IsNull(updatedSupply);
            }
        }

        [TestMethod]
        public void UpdateSupplyInfoNegativeTest()
        {
            var IdSupply = Guid.NewGuid();
            var supply = new Supply
            {
                IdSupply = IdSupply,
                Name = "Original Name",
                Quantity = 10,
                IdSupplyCategory = 1,
                IdMeasurementUnit = 1,
                ExpirationDate = DateTime.Today.AddDays(30),
                Status = true
            };


            supply.Name = "Updated Name";
            var result = SupplyOperations.UpdateSupplyInfo(supply);

            Assert.IsFalse(result);

            using (var db = new ItaliasPizzaDBEntities())
            {
                var updatedSupply = db.Supply.FirstOrDefault(s => s.IdSupply == IdSupply);
                Assert.IsNull(updatedSupply);
            }
        }

    }
}
