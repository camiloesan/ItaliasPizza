﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using ItaliasPizza.DataAccessLayer;
using System;
using Database;
using System.Collections.Generic;
using System.Text;

namespace ItaliasPizzaTests.DataAccessLayer
{
	[TestClass]
	public class LocalOrderOperationsTests
	{
		[TestMethod]
		public void GetLocalOrdersByStatusTest()
		{
			var orderStatus = OrderStatusOperations.GetOrderStatusByName("Pendiente");

			List<LocalOrder> result = LocalOrderOperations.GetLocalOrdersByStatus(orderStatus);

			foreach (var order in result)
			{
				Assert.AreEqual(order.IdOrderStatus, orderStatus.IdOrderStatus);
			}
		}
	}
}
