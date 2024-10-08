using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;

namespace ItaliasPizza.DataAccessLayer
{
	public class LocalOrderOperations
	{
		public static int SaveLocalOrder(LocalOrder localOrder)
		{
			using (var db = new ItaliasPizzaDBEntities())
			{
				db.LocalOrder.Add(localOrder);
				return db.SaveChanges();
			}
		}

		public static List<LocalOrder> GetLocalOrdersByStatus(OrderStatus orderStatus)
		{
			using (var db = new ItaliasPizzaDBEntities())
			{
				return db.LocalOrder.Where(o => o.IdOrderStatus == orderStatus.IdOrderStatus).ToList();
			}
		}

		//TODO: UNIT TEST
		public int UpdateLocalOrderStatus(LocalOrder localOrder, OrderStatus orderStatus)
		{
			using (var db = new ItaliasPizzaDBEntities())
			{
				var order = db.LocalOrder.FirstOrDefault(o => o.IdLocalOrder == localOrder.IdLocalOrder);
				order.IdOrderStatus = orderStatus.IdOrderStatus;
				return db.SaveChanges();
			}
		}
	}
}
