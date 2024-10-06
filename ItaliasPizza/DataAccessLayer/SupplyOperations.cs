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
        public static List<SupplyCategory> GetSupplyCategories()
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                return db.SupplyCategory.ToList();
            }
        }
        
        public static List<MeasurementUnit> GetMeasurementUnits()
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                return db.MeasurementUnit.ToList();
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
