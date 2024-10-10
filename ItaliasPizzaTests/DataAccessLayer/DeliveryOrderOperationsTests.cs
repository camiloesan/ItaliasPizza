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

		[TestMethod]
		public void UpdateDeliveryOrderStatusTesst()
		{
			var idEmployee = Guid.NewGuid();
			var testDeliveryDriver = new Employee { IdEmployee = idEmployee, FirstName = "John", LastName = "Doe", Phone = "1234567890", Status = true, IdCharge = 1};
			var accessAccount = new AccessAccount { UserName = "johndoe22", Password = "password123", IdEmployee = idEmployee, Email = "johndoe@gmail.com", Status = true };
			EmployeeOperations.SaveEmployee(testDeliveryDriver, accessAccount);

			var idClient = Guid.NewGuid();
			var testClient = new Client { IdClient = idClient, FirstName = "Jane", LastName = "Doe", Phone = "1234567890" };
			using (var db = new ItaliasPizzaDBEntities())
			{
				db.Client.Add(testClient);
				db.SaveChanges();
			}

			var defaultStatus = OrderStatusOperations.GetOrderStatusByName("Listo para entregar");
			var deliveryOrder = new DeliveryOrder { IdDeliveryOrder = Guid.NewGuid(), IdClient = idClient, IdOrderStatus = defaultStatus.IdOrderStatus, Date = DateTime.Now, Total = 120.0m, DeliveryDriver = idEmployee };
			DeliveryOrderOperations.SaveDeliveryOrder(deliveryOrder);

			var newStatus = OrderStatusOperations.GetOrderStatusByName("Entregado");
			var result = DeliveryOrderOperations.UpdateDeliveryOrderStatus(deliveryOrder, newStatus);

			using (var db = new ItaliasPizzaDBEntities())
			{
				db.Employee.Attach(testDeliveryDriver);
				db.AccessAccount.Attach(accessAccount);
				db.Client.Attach(testClient);
				db.DeliveryOrder.Attach(deliveryOrder);
				
				db.Employee.Remove(testDeliveryDriver);
				db.AccessAccount.Remove(accessAccount);
				db.Client.Remove(testClient);
				db.DeliveryOrder.Remove(deliveryOrder);

				db.SaveChanges();
			}

			Assert.AreEqual(1, result);
		}

		[TestMethod]
		public void GetDeliveryOrderByIdTest()
		{
			var idEmployee = Guid.NewGuid();
			var testDeliveryDriver = new Employee { IdEmployee = idEmployee, FirstName = "John", LastName = "Doe", Phone = "1234567890", Status = true, IdCharge = 1 };
			var accessAccount = new AccessAccount { UserName = "johndoe22", Password = "password123", IdEmployee = idEmployee, Email = "johndoe@gmail.com", Status = true };
			EmployeeOperations.SaveEmployee(testDeliveryDriver, accessAccount);

			var idClient = Guid.NewGuid();
			var testClient = new Client { IdClient = idClient, FirstName = "Jane", LastName = "Doe", Phone = "1234567890" };
			using (var db = new ItaliasPizzaDBEntities())
			{
				db.Client.Add(testClient);
				db.SaveChanges();
			}

			var defaultStatus = OrderStatusOperations.GetOrderStatusByName("Listo para entregar");
			var deliveryOrder = new DeliveryOrder { IdDeliveryOrder = Guid.NewGuid(), IdClient = idClient, IdOrderStatus = defaultStatus.IdOrderStatus, Date = DateTime.Now, Total = 120.0m, DeliveryDriver = idEmployee };
			DeliveryOrderOperations.SaveDeliveryOrder(deliveryOrder);

			var result = DeliveryOrderOperations.GetDeliveryOrderById(deliveryOrder.IdDeliveryOrder);

			using (var db = new ItaliasPizzaDBEntities())
			{
				db.Employee.Attach(testDeliveryDriver);
				db.AccessAccount.Attach(accessAccount);
				db.Client.Attach(testClient);
				db.DeliveryOrder.Attach(deliveryOrder);

				db.Employee.Remove(testDeliveryDriver);
				db.AccessAccount.Remove(accessAccount);
				db.Client.Remove(testClient);
				db.DeliveryOrder.Remove(deliveryOrder);

				db.SaveChanges();
			}

			Assert.AreEqual(deliveryOrder.IdDeliveryOrder, result.IdDeliveryOrder);
		}

		[TestMethod]
		public void SetNotDeliveredReasonTest()
		{
			var idEmployee = Guid.NewGuid();
			var testDeliveryDriver = new Employee { IdEmployee = idEmployee, FirstName = "John", LastName = "Doe", Phone = "1234567890", Status = true, IdCharge = 1 };
			var accessAccount = new AccessAccount { UserName = "johndoe22", Password = "password123", IdEmployee = idEmployee, Email = "johndoe@gmail.com", Status = true };
			EmployeeOperations.SaveEmployee(testDeliveryDriver, accessAccount);

			var idClient = Guid.NewGuid();
			var testClient = new Client { IdClient = idClient, FirstName = "Jane", LastName = "Doe", Phone = "1234567890" };
			using (var db = new ItaliasPizzaDBEntities())
			{
				db.Client.Add(testClient);
				db.SaveChanges();
			}

			var defaultStatus = OrderStatusOperations.GetOrderStatusByName("Listo para entregar");
			var deliveryOrder = new DeliveryOrder { IdDeliveryOrder = Guid.NewGuid(), IdClient = idClient, IdOrderStatus = defaultStatus.IdOrderStatus, Date = DateTime.Now, Total = 120.0m, DeliveryDriver = idEmployee };
			DeliveryOrderOperations.SaveDeliveryOrder(deliveryOrder);

			var result = DeliveryOrderOperations.SetNotDeliveredReason(deliveryOrder, "No se encontró la dirección");

			using (var db = new ItaliasPizzaDBEntities())
			{
				db.Employee.Attach(testDeliveryDriver);
				db.AccessAccount.Attach(accessAccount);
				db.Client.Attach(testClient);
				db.DeliveryOrder.Attach(deliveryOrder);

				db.Employee.Remove(testDeliveryDriver);
				db.AccessAccount.Remove(accessAccount);
				db.Client.Remove(testClient);
				db.DeliveryOrder.Remove(deliveryOrder);

				db.SaveChanges();
			}

			Assert.AreEqual(1, result);
		}
	}
}
