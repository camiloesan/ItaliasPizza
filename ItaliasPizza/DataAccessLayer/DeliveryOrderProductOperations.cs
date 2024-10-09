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
		//UNIT TEST
		public static List<DeliveryOrderProduct> GetDeliveryOrderProductsByOrderId(Guid deliveryOrderId)
		{
			using (var db = new ItaliasPizzaDBEntities())
			{
				return db.DeliveryOrderProduct.Where(o => o.IdDeliveryOrder == deliveryOrderId).ToList();
			}
		}
	}
}
