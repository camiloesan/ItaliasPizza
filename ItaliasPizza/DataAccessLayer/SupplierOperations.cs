using Database;
using ItaliasPizza.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizza.DataAccessLayer
{
    public class SupplierOperations
    {
        public static int SaveSupplier(Supplier supplier)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Supplier.Add(supplier);
                return db.SaveChanges();
            }
        }

        public static List<Supplier> GetAllSuppliers()
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                return db.Supplier.ToList();
            }
        }

        public static int SaveSupplierSuppliCategory(SupplierSupplyCategory supplierSupplyCategory)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                db.SupplierSupplyCategory.Add(supplierSupplyCategory);
                return db.SaveChanges();
            }
        }

        public static List<SupplierDetails> GetAllSuppliersWithCategories()
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                var supplierCategories = (from supplier in db.Supplier
                                          join supplierCategory in db.SupplierSupplyCategory
                                          on supplier.IdSupplier equals supplierCategory.IdSupplier
                                          join category in db.SupplyCategory
                                          on supplierCategory.IdSupplyCategory equals category.IdSupplyCategory
                                          select new
                                          {
                                              Supplier = supplier,
                                              CategoryName = category.SupplyCategory1,
                                          })
                                          .ToList();

                return supplierCategories
                             .GroupBy(x => x.Supplier)
                             .Select(g => new SupplierDetails
                             {
                                 IdSupplier = g.Key.IdSupplier,
                                 Name = g.Key.Name,
                                 Phone = g.Key.Phone,
                                 Categories = string.Join(", ", g.Select(c => c.CategoryName)),
                                 Status = g.Key.Status ? "Disponible" : "No disponible"
                             })
                             .ToList();
            }
        }

        public static Supplier GetSupplierById(Guid guid)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                return db.Supplier.FirstOrDefault(s => s.IdSupplier == guid);
            }
        }

        public static List<SupplierCategory> GetSupplierDetailsWithCategoriesBySpplierId(Guid supplierId)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                var allCategoryIds = db.SupplyCategory.Select(c => new SupplierCategory
                {
                    Id = c.IdSupplyCategory,
                    Name = c.SupplyCategory1,
                    IsSelected = false
                }).ToList();

                var supplierCategoryIds = db.SupplierSupplyCategory
                    .Where(s => s.IdSupplier == supplierId)
                    .Select(s => s.IdSupplyCategory)
                    .ToList();

                foreach (var category in allCategoryIds)
                {
                    if (supplierCategoryIds.Contains(category.Id))
                    {
                        category.IsSelected = true;
                    }
                }

                return allCategoryIds;
            }
        }

        public static List<SupplierCategory> GetAllSupplierCategories()
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                return db.SupplyCategory.Select(c => new SupplierCategory
                {
                    Id = c.IdSupplyCategory,
                    Name = c.SupplyCategory1,
                    IsSelected = false
                }).ToList();
            }
        }

        public static bool UpdateSupplyInfo(Supplier supplier)
        {
            bool result = false;

            using (var db = new ItaliasPizzaDBEntities())
            {
                var existingSupplier = db.Supplier.FirstOrDefault(s => s.IdSupplier == supplier.IdSupplier);

                if (existingSupplier != null)
                {
                    existingSupplier.Name = supplier.Name;
                    existingSupplier.Phone = supplier.Phone;
                    if (db.SaveChanges() != 0)
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        public static List<int> GetCategoryIdsBySupplierId(Guid supplierId)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                return db.SupplierSupplyCategory
                    .Where(s => s.IdSupplier == supplierId)
                    .Select (s => s.IdSupplyCategory)
                    .ToList();
            }
        }

        public static int DeleteSupplierSuppliCategory(SupplierSupplyCategory supplierSupplyCategory)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                var existingSupplierCategory = db.SupplierSupplyCategory
                    .FirstOrDefault(s => s.IdSupplier == supplierSupplyCategory.IdSupplier
                    && s.IdSupplyCategory == supplierSupplyCategory.IdSupplyCategory);
                if (existingSupplierCategory != null)
                {
                    db.SupplierSupplyCategory.Remove(existingSupplierCategory);
                }

                return db.SaveChanges();
            }
        }

        public static bool UpdateSupplierStatus(Guid id, bool status)
        {
            bool result = false;

            using (var db = new ItaliasPizzaDBEntities())
            {
                var existingSupplier = db.Supplier.FirstOrDefault(s => s.IdSupplier == id);

                if (existingSupplier != null)
                {
                    existingSupplier.Status = status;
                    if (db.SaveChanges() != 0)
                    {
                        result = true;
                    }
                }
            }

            return result;
        }
    }
}
