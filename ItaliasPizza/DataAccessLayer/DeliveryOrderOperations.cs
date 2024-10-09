using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;

namespace ItaliasPizza.DataAccessLayer
{
	public class DeliveryOrderOperations
	{
		public static int SaveDeliveryOrder(DeliveryOrder deliveryOrder)
		{
			using (var db = new ItaliasPizzaDBEntities())
			{
				db.DeliveryOrder.Add(deliveryOrder);
				return db.SaveChanges();
			}
		}

		public static List<DeliveryOrder> GetDeliveryOrdersByStatus(OrderStatus orderStatus)
		{
			using (var db = new ItaliasPizzaDBEntities())
			{
				return db.DeliveryOrder.Where(o => o.IdOrderStatus == orderStatus.IdOrderStatus ).ToList();
			}
		}

		//TODO: UNIT TEST
		public static int UpdateDeliveryOrderStatus(DeliveryOrder deliveryOrder, OrderStatus orderStatus)
		{
			using (var db = new ItaliasPizzaDBEntities())
			{
				var order = db.DeliveryOrder.FirstOrDefault(o => o.IdDeliveryOrder == deliveryOrder.IdDeliveryOrder);
				order.IdOrderStatus = orderStatus.IdOrderStatus;
				return db.SaveChanges();
			}
		}

		public static DeliveryOrder GetDeliveryOrderById(Guid id)
		{
			using (var db = new ItaliasPizzaDBEntities())
			{
				return db.DeliveryOrder.Find(id);
			}
		}

		public static int SetNotDeliveredReason(DeliveryOrder deliveryOrder, string reason)
		{
			using (var db = new ItaliasPizzaDBEntities())
			{
				var order = db.DeliveryOrder.FirstOrDefault(o => o.IdDeliveryOrder == deliveryOrder.IdDeliveryOrder);
				order.NotDeliveredReason = reason;
				return db.SaveChanges();
			}
		}
	}
}
