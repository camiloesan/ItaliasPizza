using Database;
using ItaliasPizza.DataAccessLayer;
using ItaliasPizza.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ItaliasPizzaTests.DataAccessLayer
{
    [TestClass]
    public class SupplierOperationsTests
    {
        [TestMethod]
        public void SaveSupplierTest()
        {
            var IdSupplier = Guid.NewGuid();
            var supplier = new Supplier
            {
                IdSupplier = IdSupplier,
                Name = "Test",
                Phone = "2282739000"
            };

            var result = SupplierOperations.SaveSupplier(supplier);

            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Supplier.Attach(supplier);

                db.Supplier.Remove(supplier);
                db.SaveChanges();
            }

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void SaveSupplierSuppliCategoryTest()
        {
            var IdSupplier = Guid.NewGuid();
            var supplier = new Supplier
            {
                IdSupplier = IdSupplier,
                Name = "Test",
                Phone = "2282739000"
            };

            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Supplier.Add(supplier);
                db.SaveChanges();
            }

            var supplierSuppliCategory = new SupplierSupplyCategory
            {
                IdSupplier = IdSupplier,
                IdSupplyCategory = 1
            };

            var result = SupplierOperations.SaveSupplierSuppliCategory(supplierSuppliCategory);

            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Supplier.Attach(supplier);

                db.SupplierSupplyCategory.Attach(supplierSuppliCategory);

                db.Supplier.Remove(supplier);
                db.SupplierSupplyCategory.Remove(supplierSuppliCategory);
                db.SaveChanges();
            }

            Assert.AreEqual(1, result);
        }


        [TestMethod]
        public void GetAllSuppliersTest()
        {
            var IdSupplier = Guid.NewGuid();
            var supplier = new Supplier
            {
                IdSupplier = IdSupplier,
                Name = "TestSupplier",
                Phone = "2282739000"
            };

            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Supplier.Add(supplier);
                db.SaveChanges();
            }

            var result = SupplierOperations.GetAllSuppliers();

            Assert.IsTrue(result.Any(s => s.IdSupplier == IdSupplier));

            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Supplier.Attach(supplier);
                db.Supplier.Remove(supplier);
                db.SaveChanges();
            }
        }


        [TestMethod]
        public void GetAllSuppliersWithCategoriesTest()
        {
            var IdSupplier = Guid.NewGuid();
            var supplier = new Supplier
            {
                IdSupplier = IdSupplier,
                Name = "TestSupplierWithCategory",
                Phone = "2282739001"
            };

            var supplierSupplyCategory = new SupplierSupplyCategory
            {
                IdSupplier = IdSupplier,
                IdSupplyCategory = 5
            };

            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Supplier.Add(supplier);
                db.SupplierSupplyCategory.Add(supplierSupplyCategory);
                db.SaveChanges();
            }

            var result = SupplierOperations.GetAllSuppliersWithCategories();

            var supplierWithCategory = result.FirstOrDefault(s => s.IdSupplier == IdSupplier);
            Assert.IsNotNull(supplierWithCategory);
            Assert.AreEqual("Vegetales", supplierWithCategory.Categories);

            using (var db = new ItaliasPizzaDBEntities())
            {
                var supplierToRemove = db.Supplier.Find(IdSupplier);
                var supplierCategoryToRemove = db.SupplierSupplyCategory
                    .FirstOrDefault(sc => sc.IdSupplier == IdSupplier && sc.IdSupplyCategory == 5);

                if (supplierCategoryToRemove != null)
                {
                    db.SupplierSupplyCategory.Remove(supplierCategoryToRemove);
                }
                if (supplierToRemove != null)
                {
                    db.Supplier.Remove(supplierToRemove);
                }

                db.SaveChanges();
            }
        }

        [TestMethod]
        public void SaveSupplierNegativeTest()
        {
            var IdSupplier = Guid.NewGuid();
            var supplier = new Supplier
            {
                IdSupplier = IdSupplier,
                Name = null,
                Phone = "2282739000"
            };

            int result = 0;
            Exception saveException = null;

            try
            {
                result = SupplierOperations.SaveSupplier(supplier);
            }
            catch (Exception ex)
            {
                saveException = ex;
            }

            Assert.AreEqual(0, result, "El proveedor no debería haberse guardado.");
            Assert.IsNotNull(saveException, "Se esperaba una excepción debido al valor nulo para el nombre.");

            using (var db = new ItaliasPizzaDBEntities())
            {
                var savedSupplier = db.Supplier.Find(IdSupplier);
                Assert.IsNull(savedSupplier, "El proveedor no debería haberse guardado en la base de datos.");
            }
        }



        [TestMethod]
        public void SaveSupplierSuppliCategoryNegativeTest()
        {
            var IdSupplier = Guid.NewGuid();
            var supplierSuppliCategory = new SupplierSupplyCategory
            {
                IdSupplier = IdSupplier,
                IdSupplyCategory = 1
            };

            int result = 0;
            Exception saveException = null;

            try
            {
                result = SupplierOperations.SaveSupplierSuppliCategory(supplierSuppliCategory);
            }
            catch (Exception ex)
            {
                saveException = ex;
            }

            Assert.AreEqual(0, result, "La relación no debería haberse guardado.");
            Assert.IsNotNull(saveException, "Se esperaba una excepción debido a la clave foránea inválida.");

            using (var db = new ItaliasPizzaDBEntities())
            {
                var savedRelation = db.SupplierSupplyCategory
                    .FirstOrDefault(sc => sc.IdSupplier == IdSupplier);
                Assert.IsNull(savedRelation, "La relación no debería haberse guardado.");
            }
        }

        [TestMethod]
        public void GetAllSuppliersNegativeTest()
        {
            var IdSupplier = Guid.NewGuid();
            var supplier = new Supplier
            {
                IdSupplier = IdSupplier,
                Name = "TestSupplier",
                Phone = "2282739000"
            };

            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Supplier.Add(supplier);
                db.SaveChanges();
            }

            Exception duplicateException = null;
            try
            {
                using (var db = new ItaliasPizzaDBEntities())
                {
                    db.Supplier.Add(supplier);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                duplicateException = ex;
            }

            Assert.IsNotNull(duplicateException, "Se esperaba una excepción por duplicar el proveedor.");

            using (var db = new ItaliasPizzaDBEntities())
            {
                var savedSupplier = db.Supplier.Find(IdSupplier);
                Assert.IsNotNull(savedSupplier, "El proveedor original debe existir.");

                db.Supplier.Remove(savedSupplier);
                db.SaveChanges();
            }
        }

        [TestMethod]
        public void GetAllSuppliersWithCategoriesNegativeTest()
        {
            var IdSupplier = Guid.NewGuid();
            var supplier = new Supplier
            {
                IdSupplier = IdSupplier,
                Name = "TestSupplierWithInvalidCategory",
                Phone = "2282739001"
            };

            var invalidSupplierSupplyCategory = new SupplierSupplyCategory
            {
                IdSupplier = IdSupplier,
                IdSupplyCategory = 999
            };

            Exception saveException = null;

            try
            {
                using (var db = new ItaliasPizzaDBEntities())
                {
                    db.Supplier.Add(supplier);
                    db.SupplierSupplyCategory.Add(invalidSupplierSupplyCategory);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                saveException = ex;
            }

            Assert.IsNotNull(saveException, "Se esperaba una excepción debido a la categoría inválida.");

            using (var db = new ItaliasPizzaDBEntities())
            {
                var supplierInDb = db.Supplier.Find(IdSupplier);
                Assert.IsNull(supplierInDb, "El proveedor no debería haberse guardado en la base de datos.");
            }
        }

        [TestMethod]
        public void GetSupplierDetailsWithCategoriesBySupplierIdTest()
        {
            var IdSupplier = Guid.NewGuid();
            var supplier = new Supplier
            {
                IdSupplier = IdSupplier,
                Name = "TestSupplier",
                Phone = "1234567890"
            };

            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Supplier.Add(supplier);
                db.SaveChanges();
            }

            var result = SupplierOperations.GetSupplierDetailsWithCategoriesBySpplierId(IdSupplier);
            Assert.IsNotNull(result);

            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Supplier.Attach(supplier);
                db.Supplier.Remove(supplier);
                db.SaveChanges();
            }
        }

        [TestMethod]
        public void UpdateSupplyInfoTest()
        {
            var IdSupplier = Guid.NewGuid();
            var supplier = new Supplier
            {
                IdSupplier = IdSupplier,
                Name = "TestSupplierToUpdateInfo",
                Phone = "2282739004"
            };

            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Supplier.Add(supplier);
                db.SaveChanges();
            }

            var supplierToUpdate = new Supplier
            {
                IdSupplier = IdSupplier,
                Name = "UpdatedName",
                Phone = "2282739999"
            };

            var result = SupplierOperations.UpdateSupplyInfo(supplierToUpdate);

            Assert.AreEqual(true, result);

            using (var db = new ItaliasPizzaDBEntities())
            {
                var supplierToDelete = db.Supplier.Find(IdSupplier);
                if (supplierToDelete != null) db.Supplier.Remove(supplierToDelete);

                db.SaveChanges();
            }
        }


        [TestMethod]
        public void GetCategoryIdsBySupplierIdTest()
        {
            var IdSupplier = Guid.NewGuid();
            var supplier = new Supplier
            {
                IdSupplier = IdSupplier,
                Name = "TestSupplier",
                Phone = "1234567890"
            };

            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Supplier.Add(supplier);
                db.SaveChanges();
            }

            var categoryIds = SupplierOperations.GetCategoryIdsBySupplierId(IdSupplier);
            Assert.IsNotNull(categoryIds);

            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Supplier.Attach(supplier);
                db.Supplier.Remove(supplier);
                db.SaveChanges();
            }
        }

        [TestMethod]
        public void DeleteSupplierSuppliCategoryTest()
        {
            var IdSupplier = Guid.NewGuid();
            var supplier = new Supplier
            {
                IdSupplier = IdSupplier,
                Name = "TestSupplierToDelete",
                Phone = "2282739002"
            };
            var supplierSupplyCategory = new SupplierSupplyCategory
            {
                IdSupplier = IdSupplier,
                IdSupplyCategory = 1
            };

            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Supplier.Add(supplier);
                db.SupplierSupplyCategory.Add(supplierSupplyCategory);
                db.SaveChanges();
            }

            var result = SupplierOperations.DeleteSupplierSuppliCategory(supplierSupplyCategory);

            Assert.AreEqual(1, result);

            using (var db = new ItaliasPizzaDBEntities())
            {
                var supplierToDelete = db.Supplier.Find(IdSupplier);
                if (supplierToDelete != null) db.Supplier.Remove(supplierToDelete);

                db.SaveChanges();
            }
        }


        [TestMethod]
        public void UpdateSupplierStatusTest()
        {
            var IdSupplier = Guid.NewGuid();
            var supplier = new Supplier
            {
                IdSupplier = IdSupplier,
                Name = "TestSupplierToUpdateStatus",
                Phone = "2282739003",
                Status = true
            };

            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Supplier.Add(supplier);
                db.SaveChanges();
            }

            var result = SupplierOperations.UpdateSupplierStatus(IdSupplier, false);

            Assert.AreEqual(true, result);

            using (var db = new ItaliasPizzaDBEntities())
            {
                var supplierToDelete = db.Supplier.Find(IdSupplier);
                if (supplierToDelete != null) db.Supplier.Remove(supplierToDelete);

                db.SaveChanges();
            }
        }


        [TestMethod]
        public void GetSupplierDetailsWithCategoriesBySupplierIdNegativeTest()
        {
            List<SupplierCategory> suppliers = new List<SupplierCategory>();

            using (var db = new ItaliasPizzaDBEntities())
            {
                suppliers = db.SupplyCategory.Select(c => new SupplierCategory
                {
                    Id = c.IdSupplyCategory,
                    Name = c.SupplyCategory1,
                    IsSelected = false
                }).ToList();
            }

            var result = SupplierOperations.GetSupplierDetailsWithCategoriesBySpplierId(Guid.NewGuid());
            Assert.AreEqual(suppliers.Count, result.Count);
        }

        [TestMethod]
        public void UpdateSupplyInfoNegativeTest()
        {
            var invalidSupplier = new Supplier
            {
                IdSupplier = Guid.NewGuid(),
                Name = "InvalidSupplier",
                Phone = "0000000000"
            };

            var result = SupplierOperations.UpdateSupplyInfo(invalidSupplier);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void GetCategoryIdsBySupplierIdNegativeTest()
        {
            var categoryIds = SupplierOperations.GetCategoryIdsBySupplierId(Guid.NewGuid());
            Assert.AreEqual(0, categoryIds.Count());
        }

        [TestMethod]
        public void DeleteSupplierSuppliCategoryNegativeTest()
        {
            var nonExistentSupplierCategory = new SupplierSupplyCategory
            {
                IdSupplier = Guid.NewGuid(),
                IdSupplyCategory = 99
            };

            int result = 0;
            Exception deleteException = null;

            try
            {
                result = SupplierOperations.DeleteSupplierSuppliCategory(nonExistentSupplierCategory);
            }
            catch (Exception ex)
            {
                deleteException = ex;
            }

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void UpdateSupplierStatusNegativeTest()
        {
            var nonExistentIdSupplier = Guid.NewGuid();

            bool result = false;
            Exception updateException = null;

            try
            {
                result = SupplierOperations.UpdateSupplierStatus(nonExistentIdSupplier, false);
            }
            catch (Exception ex)
            {
                updateException = ex;
            }

            Assert.AreEqual(false, result);
        }

    }
}
