using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;

namespace ItaliasPizza.DataAccessLayer
{
	public class DeliveryOrderProductOperations
	{
		public static int SaveDeliveryOrderProduct(DeliveryOrderProduct deliveryOrderProduct)
		{
			using (var db = new ItaliasPizzaDBEntities())
			{
				db.DeliveryOrderProduct.Add(deliveryOrderProduct);
				return db.SaveChanges();
			}
		}

		public static List<DeliveryOrderProduct> GetDeliveryOrderProductsByOrderId(Guid deliveryOrderId)
		{
			using (var db = new ItaliasPizzaDBEntities())
			{
				return db.DeliveryOrderProduct.Where(o => o.IdDeliveryOrder == deliveryOrderId).ToList();
			}
		}
	}
}
