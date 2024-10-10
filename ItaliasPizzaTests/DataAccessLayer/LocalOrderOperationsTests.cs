using Microsoft.VisualStudio.TestTools.UnitTesting;
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
			var idEmployee = Guid.NewGuid();
			var testEmployee = new Employee { IdEmployee = idEmployee, FirstName = "John", LastName = "Doe", Phone = "1234567890", Status = true, IdCharge = 1 };
			var accessAccount = new AccessAccount { UserName = "johndoe22", Password = "password123", IdEmployee = idEmployee, Email = "johndoe@gmail.com", Status = true };
			EmployeeOperations.SaveEmployee(testEmployee, accessAccount);

			var orderStatus = OrderStatusOperations.GetOrderStatusByName("Pendiente");

			var localOrder1 = new LocalOrder
			{
				IdLocalOrder = Guid.NewGuid(),
				Waiter = idEmployee,
				IdOrderStatus = orderStatus.IdOrderStatus,
				Date = DateTime.Now,
				Total = 120.0m
			};
			LocalOrderOperations.SaveLocalOrder(localOrder1);

			List<LocalOrder> result = LocalOrderOperations.GetLocalOrdersByStatus(orderStatus);

			using (var db = new ItaliasPizzaDBEntities())
			{
				db.Employee.Attach(testEmployee);
				db.AccessAccount.Attach(accessAccount);
				db.LocalOrder.Attach(localOrder1);

				db.Employee.Remove(testEmployee);
				db.AccessAccount.Remove(accessAccount);
				db.LocalOrder.Remove(localOrder1);

				db.SaveChanges();
			}

			foreach (var order in result)
			{
				Assert.AreEqual(order.IdOrderStatus, orderStatus.IdOrderStatus);
			}
		}

		[TestMethod]
		public void UpdateLocalOrderStatusTest()
		{
			var idEmployee = Guid.NewGuid();
			var testEmployee = new Employee { IdEmployee = idEmployee, FirstName = "John", LastName = "Doe", Phone = "1234567890", Status = true, IdCharge = 1 };
			var accessAccount = new AccessAccount { UserName = "johndoe22", Password = "password123", IdEmployee = idEmployee, Email = "johndoe@gmail.com", Status = true };
			EmployeeOperations.SaveEmployee(testEmployee, accessAccount);

			var defaultStatus = OrderStatusOperations.GetOrderStatusByName("Pendiente");
			var localOrder1 = new LocalOrder
			{ IdLocalOrder = Guid.NewGuid(), Waiter = idEmployee, IdOrderStatus = defaultStatus.IdOrderStatus, Date = DateTime.Now, Total = 120.0m };
			LocalOrderOperations.SaveLocalOrder(localOrder1);

			var newStatus = OrderStatusOperations.GetOrderStatusByName("Cancelado");
			var result = LocalOrderOperations.UpdateLocalOrderStatus(localOrder1, newStatus);

			using (var db = new ItaliasPizzaDBEntities())
			{
				db.Employee.Attach(testEmployee);
				db.AccessAccount.Attach(accessAccount);
				db.LocalOrder.Attach(localOrder1);

				db.Employee.Remove(testEmployee);
				db.AccessAccount.Remove(accessAccount);
				db.LocalOrder.Remove(localOrder1);

				db.SaveChanges();
			}

			Assert.AreEqual(1, result);
		}

		[TestMethod]
		public void GetLocalOrderById()
		{
			var idEmployee = Guid.NewGuid();
			var testEmployee = new Employee
			{
				IdEmployee = idEmployee,
				FirstName = "John",
				LastName = "Doe",
				Phone = "1234567890",
				Status = true,
				IdCharge = 1
			};
			var accessAccount = new AccessAccount
			{
				UserName = "johndoe22",
				Password = "password123",
				IdEmployee = idEmployee,
				Email = "johndoe@gmail.com",
				Status = true,
			};
			EmployeeOperations.SaveEmployee(testEmployee, accessAccount);

			var defaultStatus = OrderStatusOperations.GetOrderStatusByName("Pendiente");
			var localOrderTest = new LocalOrder
			{
				IdLocalOrder = Guid.NewGuid(),
				Waiter = idEmployee,
				IdOrderStatus = defaultStatus.IdOrderStatus,
				Date = DateTime.Now,
				Total = 120.0m
			};
			LocalOrderOperations.SaveLocalOrder(localOrderTest);

			var result = LocalOrderOperations.GetLocalOrderById(localOrderTest.IdLocalOrder);

			using (var db = new ItaliasPizzaDBEntities())
			{
				db.Employee.Attach(testEmployee);
				db.AccessAccount.Attach(accessAccount);
				db.LocalOrder.Attach(localOrderTest);

				db.Employee.Remove(testEmployee);
				db.AccessAccount.Remove(accessAccount);
				db.LocalOrder.Remove(localOrderTest);

				db.SaveChanges();
			}

			Assert.AreEqual(localOrderTest.IdLocalOrder, result.IdLocalOrder);
		}
	}
}
