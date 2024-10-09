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
		//UNIT TEST
		public static List<LocalOrderProduct> GetLocalOrderProductsByOrderId(Guid localOrderId)
		{
			using (var db = new ItaliasPizzaDBEntities())
			{
				return db.LocalOrderProduct.Where(o => o.IdLocalOrder == localOrderId).ToList();
			}
		}
	}
}
