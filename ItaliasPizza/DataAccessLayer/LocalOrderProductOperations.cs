using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;

namespace ItaliasPizza.DataAccessLayer
{
	public class LocalOrderProductOperations
	{
		public static int SaveLocalOrderProduct(LocalOrderProduct localOrderProduct)
		{
			using (var db = new ItaliasPizzaDBEntities())
			{
				db.LocalOrderProduct.Add(localOrderProduct);
				return db.SaveChanges();
			}
		}
		
		public static List<LocalOrderProduct> GetLocalOrderProductsByOrderId(Guid localOrderId)
		{
			using (var db = new ItaliasPizzaDBEntities())
			{
				return db.LocalOrderProduct.Where(o => o.IdLocalOrder == localOrderId).ToList();
			}
		}
	}
}
