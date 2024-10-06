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

        public static Supply GetSupplyById(Guid id)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                return db.Supply.FirstOrDefault(s => s.IdSupply == id);
            }
        }

        public static bool UpdateSupplyStatus(Guid id, bool status)
        {
            bool result = false;

            using (var db = new ItaliasPizzaDBEntities())
            {
                var existingSupply = db.Supply.FirstOrDefault(s => s.IdSupply == id);

                if (existingSupply != null)
                {
                    existingSupply.Status = status;
                    if (db.SaveChanges() == 1)
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        public static bool UpdateSupplyInfo(Supply supply)
        {
            bool result = false;

            using (var db = new ItaliasPizzaDBEntities())
            {
                var existingSupply = db.Supply.FirstOrDefault(s => s.IdSupply == supply.IdSupply);

                if (existingSupply != null)
                {
                    existingSupply.Name = supply.Name;
                    existingSupply.Quantity = supply.Quantity;
                    existingSupply.IdSupplyCategory = supply.IdSupplyCategory;
                    existingSupply.IdMeasurementUnit = supply.IdMeasurementUnit;
                    existingSupply.ExpirationDate = supply.ExpirationDate;
                    if (db.SaveChanges() == 1)
                    {
                        result = true;
                    }
                }
            }

            return result;
        }
    }
}
