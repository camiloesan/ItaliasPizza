using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizza.DataAccessLayer
{
    public class ProductTypeOperations
    {
        public static List<ProductType> GetProductTypes()
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                return db.ProductType.ToList();
            }
        }
    }
}
