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

		public static int SaveLocalOrderWithProducts(LocalOrder localOrder, List<LocalOrderProduct> localOrderProducts)
		{
			int result = 0;

			using (var db = new ItaliasPizzaDBEntities())
			{
				using (var transaction = db.Database.BeginTransaction())
				{
					try
					{
						db.LocalOrder.Add(localOrder);

						foreach (var localOrderProduct in localOrderProducts)
						{
							db.LocalOrderProduct.Add(localOrderProduct);
						}

						db.SaveChanges();

						transaction.Commit();

						result = localOrderProducts.Count + 1;
					}
					catch (Exception e)
					{
						transaction.Rollback();
					}
				}
			}
			return result;
		}

		public static List<LocalOrder> GetLocalOrdersByStatus(OrderStatus orderStatus)
		{
			using (var db = new ItaliasPizzaDBEntities())
			{
				return db.LocalOrder.Where(o => o.IdOrderStatus == orderStatus.IdOrderStatus).ToList();
			}
		}

		public static int UpdateLocalOrderStatus(LocalOrder localOrder, OrderStatus orderStatus)
		{
			using (var db = new ItaliasPizzaDBEntities())
			{
				var order = db.LocalOrder.FirstOrDefault(o => o.IdLocalOrder == localOrder.IdLocalOrder);
				order.IdOrderStatus = orderStatus.IdOrderStatus;
				return db.SaveChanges();
			}
		}

		public static LocalOrder GetLocalOrderById(Guid id)
		{
			using (var db = new ItaliasPizzaDBEntities())
			{
				return db.LocalOrder.Find(id);
			}
		}
	}
}
