using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Database;
using ItaliasPizza.DataAccessLayer;
using System.Collections.Generic;

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
		public void UpdateDeliveryOrderStatusTest()
		{
			var idEmployee = Guid.NewGuid();
			var testDeliveryDriver = new Employee { IdEmployee = idEmployee, FirstName = "John", LastName = "Doe", Phone = "1234567890", Status = true, IdCharge = 1};
			var accessAccount = new AccessAccount { UserName = "johndoe22", Password = "password123", IdEmployee = idEmployee, Email = "johndoe@gmail.com", Status = true };
			EmployeeOperations.SaveEmployee(testDeliveryDriver, accessAccount);

			var idClient = Guid.NewGuid();
			var testClient = new Client { IdClient = idClient, FirstName = "Jane", LastName = "Doe", Phone = "1234567890" };
			var testAddress = new Address { IdAddress = Guid.NewGuid(), IdClient = idClient, Street = "123 Main St", Number = 123, PostalCode = "1234", Colony = "Main", Reference = "Some reference" };

			using (var db = new ItaliasPizzaDBEntities())
			{
				db.Client.Add(testClient);
				db.Address.Add(testAddress);
				db.SaveChanges();
			}


			var defaultStatus = OrderStatusOperations.GetOrderStatusByName("Listo para entregar");
			var deliveryOrder = new DeliveryOrder { IdDeliveryOrder = Guid.NewGuid(), IdClient = idClient, IdOrderStatus = defaultStatus.IdOrderStatus, Date = DateTime.Now, Total = 120.0m, DeliveryDriver = idEmployee, IdClientAddress = testAddress.IdAddress };
			DeliveryOrderOperations.SaveDeliveryOrder(deliveryOrder);

			var newStatus = OrderStatusOperations.GetOrderStatusByName("Entregado");
			var result = DeliveryOrderOperations.UpdateDeliveryOrderStatus(deliveryOrder, newStatus);

			using (var db = new ItaliasPizzaDBEntities())
			{
				db.Employee.Attach(testDeliveryDriver);
				db.AccessAccount.Attach(accessAccount);
				db.Client.Attach(testClient);
				db.Address.Attach(testAddress);
				db.DeliveryOrder.Attach(deliveryOrder);
				
				db.Employee.Remove(testDeliveryDriver);
				db.AccessAccount.Remove(accessAccount);
				db.Client.Remove(testClient);
				db.Address.Remove(testAddress);
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
			var testAddress = new Address { IdAddress = Guid.NewGuid(), IdClient = idClient, Street = "123 Main St", Number = 123, PostalCode = "1234", Colony = "Main", Reference = "Some reference" };
			using (var db = new ItaliasPizzaDBEntities())
			{
				db.Client.Add(testClient);
				db.Address.Add(testAddress);
				db.SaveChanges();
			}

			var defaultStatus = OrderStatusOperations.GetOrderStatusByName("Listo para entregar");
			var deliveryOrder = new DeliveryOrder { IdDeliveryOrder = Guid.NewGuid(), IdClient = idClient, IdOrderStatus = defaultStatus.IdOrderStatus, Date = DateTime.Now, Total = 120.0m, DeliveryDriver = idEmployee, IdClientAddress = testAddress.IdAddress };
			DeliveryOrderOperations.SaveDeliveryOrder(deliveryOrder);

			var result = DeliveryOrderOperations.GetDeliveryOrderById(deliveryOrder.IdDeliveryOrder);

			using (var db = new ItaliasPizzaDBEntities())
			{
				db.Employee.Attach(testDeliveryDriver);
				db.AccessAccount.Attach(accessAccount);
				db.Client.Attach(testClient);
				db.Address.Attach(testAddress);
				db.DeliveryOrder.Attach(deliveryOrder);

				db.Employee.Remove(testDeliveryDriver);
				db.AccessAccount.Remove(accessAccount);
				db.Client.Remove(testClient);
				db.Address.Remove(testAddress);
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
			var testAddress = new Address { IdAddress = Guid.NewGuid(), IdClient = idClient, Street = "123 Main St", Number = 123, PostalCode = "1234", Colony = "Main", Reference = "Some reference" };
			using (var db = new ItaliasPizzaDBEntities())
			{
				db.Client.Add(testClient);
				db.Address.Add(testAddress);
				db.SaveChanges();
			}

			var defaultStatus = OrderStatusOperations.GetOrderStatusByName("Listo para entregar");
			var deliveryOrder = new DeliveryOrder { IdDeliveryOrder = Guid.NewGuid(), IdClient = idClient, IdOrderStatus = defaultStatus.IdOrderStatus, Date = DateTime.Now, Total = 120.0m, DeliveryDriver = idEmployee, IdClientAddress = testAddress.IdAddress };
			DeliveryOrderOperations.SaveDeliveryOrder(deliveryOrder);

			var result = DeliveryOrderOperations.SetNotDeliveredReason(deliveryOrder, "No se encontró la dirección");

			using (var db = new ItaliasPizzaDBEntities())
			{
				db.Employee.Attach(testDeliveryDriver);
				db.AccessAccount.Attach(accessAccount);
				db.Client.Attach(testClient);
				db.Address.Attach(testAddress);
				db.DeliveryOrder.Attach(deliveryOrder);

				db.Employee.Remove(testDeliveryDriver);
				db.AccessAccount.Remove(accessAccount);
				db.Client.Remove(testClient);
				db.Address.Remove(testAddress);
				db.DeliveryOrder.Remove(deliveryOrder);

				db.SaveChanges();
			}

			Assert.AreEqual(1, result);
		}

		[TestMethod]
		public void SaveDeliveryOrderWithProductsTest()
		{
			var idEmployee = Guid.NewGuid();
			var testDeliveryDriver = new Employee { IdEmployee = idEmployee, FirstName = "John", LastName = "Doe", Phone = "1234567890", Status = true, IdCharge = 1 };
			var accessAccount = new AccessAccount { UserName = "johndoe22", Password = "password123", IdEmployee = idEmployee, Email = "johndoe@gmail.com", Status = true };
			EmployeeOperations.SaveEmployee(testDeliveryDriver, accessAccount);

			var idClient = Guid.NewGuid();
			var testClient = new Client { IdClient = idClient, FirstName = "Jane", LastName = "Doe", Phone = "1234567890" };
			var testAddress = new Address { IdAddress = Guid.NewGuid(), IdClient = idClient, Street = "123 Main St", Number = 123, PostalCode = "1234", Colony = "Main", Reference = "Some reference" };

			var defaultStatus = OrderStatusOperations.GetOrderStatusByName("Listo para entregar");
			var deliveryOrder = new DeliveryOrder { IdDeliveryOrder = Guid.NewGuid(), IdClient = idClient, IdOrderStatus = defaultStatus.IdOrderStatus, Date = DateTime.Now, Total = 120.0m, DeliveryDriver = idEmployee, IdClientAddress = testAddress.IdAddress };

			var product1 = new Product { IdProduct = Guid.NewGuid(), Name = "Pizza Margherita", IdType = 1, Price = 50.0m, Size = "Grande", Status = true };
			var product2 = new Product { IdProduct = Guid.NewGuid(), Name = "Pizza Pepperoni", IdType = 1, Price = 70.0m, Size = "Grande", Status = true };
			var orderProduct1 = new DeliveryOrderProduct { IdDeliveryOrderProduct = Guid.NewGuid(), IdDeliveryOrder = deliveryOrder.IdDeliveryOrder, IdProduct = product1.IdProduct, Quantity = 1 };
			var orderProduct2 = new DeliveryOrderProduct { IdDeliveryOrderProduct = Guid.NewGuid(), IdDeliveryOrder = deliveryOrder.IdDeliveryOrder, IdProduct = product2.IdProduct, Quantity = 1 };

			using (var db = new ItaliasPizzaDBEntities())
			{
				db.Client.Add(testClient);
				db.Address.Add(testAddress);
				db.Product.Add(product1);
				db.Product.Add(product2);
				db.SaveChanges();
			}

			var insertResult = DeliveryOrderOperations.SaveDeliveryOrderWithProducts(deliveryOrder, new List<DeliveryOrderProduct> { orderProduct1, orderProduct2 });

			var result = DeliveryOrderOperations.GetDeliveryOrderById(deliveryOrder.IdDeliveryOrder);

			using (var db = new ItaliasPizzaDBEntities())
			{
				db.Employee.Attach(testDeliveryDriver);
				db.AccessAccount.Attach(accessAccount);
				db.Client.Attach(testClient);
				db.Address.Attach(testAddress);
				db.DeliveryOrder.Attach(deliveryOrder);
				db.Product.Attach(product1);
				db.Product.Attach(product2);
				db.DeliveryOrderProduct.Attach(orderProduct1);
				db.DeliveryOrderProduct.Attach(orderProduct2);

				db.Employee.Remove(testDeliveryDriver);
				db.AccessAccount.Remove(accessAccount);
				db.Client.Remove(testClient);
				db.Address.Remove(testAddress);
				db.DeliveryOrder.Remove(deliveryOrder);
				db.Product.Remove(product1);
				db.Product.Remove(product2);
				db.DeliveryOrderProduct.Remove(orderProduct1);
				db.DeliveryOrderProduct.Remove(orderProduct2);

				db.SaveChanges();
			}

			Assert.AreEqual(3, insertResult);
			Assert.IsNotNull(result);
			Assert.AreEqual(deliveryOrder.IdDeliveryOrder, result.IdDeliveryOrder);
		}
	}
}
