using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizza.DataAccessLayer
{
    public class SupplyOperations
    {
        public static List<SupplyCategory> GetSupplyCategoriesNames()
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                return db.SupplyCategory.ToList();
            }
        }

        public static int SaveSupply(Supply supply)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Supply.Add(supply);
                return db.SaveChanges();
            }
        }
    }
}
