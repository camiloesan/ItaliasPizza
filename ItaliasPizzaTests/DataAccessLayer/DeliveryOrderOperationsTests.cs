using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Database;
using ItaliasPizza.DataAccessLayer;

namespace ItaliasPizzaTests.DataAccessLayer
{
	[TestClass]
	public class DeliveryOrderOperationsTests
	{
		[TestMethod]
		public void GetDeliveryOrdersByStatusTest()
		{
			var orderStatus = OrderStatusOperations.GetOrderStatusByName("Pendiente");

			var result = DeliveryOrderOperations.GetDeliveryOrdersByStatus(orderStatus);

			foreach (var order in result)
			{
				Assert.AreEqual(order.IdOrderStatus, orderStatus.IdOrderStatus);
			}
		}
	}
}
