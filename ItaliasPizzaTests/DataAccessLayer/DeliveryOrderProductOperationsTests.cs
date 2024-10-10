using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Database;
using ItaliasPizza.DataAccessLayer;
using System.Collections.Generic;
using System.Windows.Documents;

namespace ItaliasPizzaTests.DataAccessLayer
{
	[TestClass]
	public class DeliveryOrderProductOperationsTests
	{
		[TestMethod]
		public void GetDeliveryOrderProductsProductsByOrderId()
		{
			var idEmployee = Guid.NewGuid();
			var testEmployee = new Employee { IdEmployee = idEmployee, FirstName = "John", LastName = "Doe", Phone = "1234567890", Status = true, IdCharge = 1 };
			var accessAccount = new AccessAccount { UserName = "johndoe22", Password = "password123", IdEmployee = idEmployee, Email = "johndoe@gmail.com", Status = true };
			EmployeeOperations.SaveEmployee(testEmployee, accessAccount);

			var idClient = Guid.NewGuid();
			var testClient = new Client { IdClient = idClient, FirstName = "Jane", LastName = "Doe", Phone = "1234567890" };
			using (var db = new ItaliasPizzaDBEntities())
			{
				db.Client.Add(testClient);
				db.SaveChanges();
			}

			var orderStatus = OrderStatusOperations.GetOrderStatusByName("Listo para entregar"); 
			var deliveryOrder = new DeliveryOrder { IdDeliveryOrder = Guid.NewGuid(), IdClient = idClient, IdOrderStatus = orderStatus.IdOrderStatus, Date = DateTime.Now, Total = 120.0m, DeliveryDriver = idEmployee };
			DeliveryOrderOperations.SaveDeliveryOrder(deliveryOrder);

			var idPizza = Guid.NewGuid();
			var testPizza = new Product {IdProduct = idPizza, Name = "Pizza", IdType = 1, Price = 10.0m, Size = "Grande", Status = true };
			ProductOperations.SaveProduct(testPizza);
			
			var deliveryOrderProduct = new DeliveryOrderProduct { IdDeliveryOrderProduct = Guid.NewGuid(), IdDeliveryOrder = deliveryOrder.IdDeliveryOrder, IdProduct = testPizza.IdProduct, Quantity = 2 };
			DeliveryOrderProductOperations.SaveDeliveryOrderProduct(deliveryOrderProduct);

			List<DeliveryOrderProduct> result = DeliveryOrderProductOperations.GetDeliveryOrderProductsByOrderId(deliveryOrder.IdDeliveryOrder);

			using (var db = new ItaliasPizzaDBEntities())
			{
				db.Employee.Attach(testEmployee);
				db.AccessAccount.Attach(accessAccount);
				db.Client.Attach(testClient);
				db.DeliveryOrder.Attach(deliveryOrder);
				db.Product.Attach(testPizza);
				db.DeliveryOrderProduct.Attach(deliveryOrderProduct);

				db.Employee.Remove(testEmployee);
				db.AccessAccount.Remove(accessAccount);
				db.Client.Remove(testClient);
				db.DeliveryOrder.Remove(deliveryOrder);
				db.Product.Remove(testPizza);
				db.DeliveryOrderProduct.Remove(deliveryOrderProduct);

				db.SaveChanges();
			}
		}
	}
}
