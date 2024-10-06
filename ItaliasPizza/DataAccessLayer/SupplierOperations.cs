using Database;
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
    }
}
