using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Database;
using ItaliasPizza.DataAccessLayer;

namespace ItaliasPizzaTests.DataAccessLayer
{
	[TestClass]
	public class OrderStatusOperationsTests
	{
		[TestMethod]
		public void GetOrderStatusByNameTest()
		{
			var expectedStatus = "Pendiente";

			var orderStatus = OrderStatusOperations.GetOrderStatusByName(expectedStatus);

			Console.WriteLine(orderStatus.IdOrderStatus+" "+orderStatus.Status);

			Assert.AreEqual(expectedStatus, orderStatus.Status);
		}
	}
}
