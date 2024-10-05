using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;

namespace ItaliasPizza.DataAccessLayer
{
	internal class LocalOrderOperations
	{
		public static int SaveOrder(LocalOrder localOrder)
		{
			using (var db = new ItaliasPizzaDBEntities())
			{
				db.LocalOrder.Add(localOrder);
				return db.SaveChanges();
			}
		}	
	}
}
