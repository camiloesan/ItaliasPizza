using Database;
using ItaliasPizza.DataAccessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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

    }
}
