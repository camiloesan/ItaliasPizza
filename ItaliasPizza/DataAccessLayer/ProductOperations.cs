using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;

namespace ItaliasPizza.DataAccessLayer
{
	public class ProductOperations
	{
		public static List<Product> GetActiveProducts()
		{
			using (var db = new ItaliasPizzaDBEntities())
			{
				return db.Product.Where(p => p.Status == true).ToList();
			}
		}

		public static int GetPreparableProductQuantity(Product product)
		{
			using (var db = new ItaliasPizzaDBEntities())
			{
				Guid idProduct = product.IdProduct;
				return db.Database.SqlQuery<int>("SELECT dbo.fn_CalculateMaxProducts(@idProduct)", new System.Data.SqlClient.SqlParameter("@idProduct", idProduct)).FirstOrDefault();
			}
		}
        
        public static int SaveProduct(Product product)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Product.Add(product);
                return db.SaveChanges();
            }
        }

        public static int UpdateProduct(Product product)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                return db.SaveChanges();
            }
        }

        public static bool IsProductNameDuplicated(string name)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                return db.Product.Any(p => p.Name == name);
            }
        }

        public static Product GetProductById(Guid idProduct)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                return db.Product.Find(idProduct);
            }
        }
    }
}
