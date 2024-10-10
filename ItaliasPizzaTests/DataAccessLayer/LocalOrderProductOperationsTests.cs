using Database;
using ItaliasPizza.DataAccessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace ItaliasPizzaTests.DataAccessLayer
{
	[TestClass]
	public class LocalOrderProductOperationsTests
	{
		[TestMethod]
		public void SaveLocalOrderProduct()
		{
			
		}
	
		[TestMethod]
		public void GetLocalOrderProductsByOrderId()
		{
			var idEmployee = Guid.NewGuid();
			var testEmployee = new Employee { IdEmployee = idEmployee, FirstName = "John", LastName = "Doe", Phone = "1234567890", Status = true, IdCharge = 1 };
			var accessAccount = new AccessAccount { UserName = "johndoe22", Password = "password123", IdEmployee = idEmployee, Email = "johndoe@gmail.com", Status = true };
			EmployeeOperations.SaveEmployee(testEmployee, accessAccount);

			var orderStatus = OrderStatusOperations.GetOrderStatusByName("Pendiente");
			var localOrder = new LocalOrder { IdLocalOrder = Guid.NewGuid(), Waiter = idEmployee, IdOrderStatus = orderStatus.IdOrderStatus, Date = DateTime.Now, Total = 120.0m };
			LocalOrderOperations.SaveLocalOrder(localOrder);

			var idPizza = Guid.NewGuid();
			var testPizza = new Product { IdProduct = Guid.NewGuid(), Name = "Pizza", IdType = 1, Price = 10.0m, Size = "Grande", Status = true };
			ProductOperations.SaveProduct(testPizza);

			var localOrderProduct = new LocalOrderProduct { IdLocalOrderProduct = Guid.NewGuid(), IdLocalOrder = localOrder.IdLocalOrder, IdProduct = testPizza.IdProduct, Quantity = 2 };
			LocalOrderProductOperations.SaveLocalOrderProduct(localOrderProduct);

			List<LocalOrderProduct> result = LocalOrderProductOperations.GetLocalOrderProductsByOrderId(localOrder.IdLocalOrder);

			using (var db = new ItaliasPizzaDBEntities())
			{
				db.Employee.Attach(testEmployee);
				db.AccessAccount.Attach(accessAccount);
				db.LocalOrder.Attach(localOrder);
				db.Product.Attach(testPizza);
				db.LocalOrderProduct.Attach(localOrderProduct);

				db.Employee.Remove(testEmployee);
				db.AccessAccount.Remove(accessAccount);
				db.LocalOrder.Remove(localOrder);
				db.Product.Remove(testPizza);
				db.LocalOrderProduct.Remove(localOrderProduct);

				db.AccessAccount.Remove(accessAccount);
			}

			foreach (var item in result)
			{
				Assert.AreEqual(localOrderProduct.IdLocalOrderProduct, item.IdLocalOrderProduct);
			}
		}
	}
}
