using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizza.DataAccessLayer
{
    public class SupplierOrderOperations
    {
        public static int SaveSupplierOrder(SupplierOrder supplierOrder)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                db.SupplierOrder.Add(supplierOrder);
                return db.SaveChanges();
            }
        }
    }
}
