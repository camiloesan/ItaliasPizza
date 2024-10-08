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
                                              CategoryName = category.SupplyCategory1
                                          })
                                          .ToList();

                return supplierCategories
                             .GroupBy(x => x.Supplier)
                             .Select(g => new SupplierDetails
                             {
                                 IdSupplier = g.Key.IdSupplier,
                                 Name = g.Key.Name,
                                 Phone = g.Key.Phone,
                                 Categories = string.Join(", ", g.Select(c => c.CategoryName))
                             })
                             .ToList();
            }
        }



    }
}
