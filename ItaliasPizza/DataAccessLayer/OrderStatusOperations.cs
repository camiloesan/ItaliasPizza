using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;

namespace ItaliasPizza.DataAccessLayer
{
	public class OrderStatusOperations
	{
		public static OrderStatus GetOrderStatusByName(string status)
		{
			using (var db = new ItaliasPizzaDBEntities())
			{
				return db.OrderStatus.Where(o => o.Status == status).FirstOrDefault();
			}
		}
	}
}
