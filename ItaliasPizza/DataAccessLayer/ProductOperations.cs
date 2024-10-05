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
		public static ICollection<Product> GetActiveProducts()
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
	}
}
