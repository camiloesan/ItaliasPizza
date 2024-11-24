using Database;
using ItaliasPizza.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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

        public static List<Supply> GetSuppliesByCategoriesOfSupplier(Guid IdSupplier)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                return db.Supply.Where(s => db.SupplierSupplyCategory
                                .Where(ssc => ssc.IdSupplier == IdSupplier)
                                .Select(ssc => ssc.IdSupplyCategory)
                                .Contains(s.IdSupplyCategory))
                                .ToList();
            }
        }
		//UNIT TEST
		public static List<Supply> GetSupplies()
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                return db.Supply.ToList();
			}
        }

        public static MeasurementUnit GetMeasurementUnitById(int id)
		{
			using (var db = new ItaliasPizzaDBEntities())
			{
				return db.MeasurementUnit.FirstOrDefault(mu => mu.IdMeasurementUnit == id);
			}
		}

        public static List<SupplyDetails> GetSupplyDetailsXes()
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                var result = db.Supply
                .Select(s => new SupplyDetails
                {
                    IdSupply = s.IdSupply,
                    Name = s.Name,
                    MeasurementUnit = s.MeasurementUnit.MeasurementUnit1,
					RawQuantity = s.Quantity.ToString(),
					Quantity = s.Quantity.ToString() + " " + s.MeasurementUnit.MeasurementUnit1,
                    Category = s.SupplyCategory.SupplyCategory1,
                    IdSupplyCategory = s.IdSupplyCategory,
                    ExpirationDate = s.ExpirationDate.ToString(),
                    Status = s.Status ? "Disponible" : "No disponible"
                })
                .ToList();

            return result;
            }

        }
    }
}
