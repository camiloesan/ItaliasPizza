using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Database;
using ItaliasPizza.DataAccessLayer;

namespace ItaliasPizzaTests.DataAccessLayer
{
	[TestClass]
	public class ClientOperationsTests
	{
		[TestMethod]
		public void SaveClientTest()
		{
			var idClient = Guid.NewGuid();
			var client = new Client
			{
				IdClient = idClient,
				Phone = "1234567890",
				FirstName = "John",
				LastName = "Doe",
			};

			var idAddress = Guid.NewGuid();
			var address = new Address
			{
				Street = "Test",
				Number = 123,
				Colony = "Test",
				PostalCode = "12345",
				Reference = "Test",
				Status = true,
				IdClient = idClient
			};

			var expected = 2;
			var actual = ClientOperations.SaveClient(client, address);

			using (var db = new ItaliasPizzaDBEntities())
			{
				db.Client.Attach(client);
				db.Address.Attach(address);

				db.Client.Remove(client);
				db.Address.Remove(address);
				db.SaveChanges();
			}

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void IsPhoneRegisteredIsRegisteredTest()
		{
			var idClient = Guid.NewGuid();
			var client = new Client
			{
				IdClient = idClient,
				Phone = "1234567890",
				FirstName = "John",
				LastName = "Doe",
			};

			var idAddress = Guid.NewGuid();
			var address = new Address
			{
				Street = "Test",
				Number = 123,
				Colony = "Test",
				PostalCode = "12345",
				Reference = "Test",
				Status = true,
				IdClient = idClient
			};

			ClientOperations.SaveClient(client, address);

			bool result = ClientOperations.IsPhoneRegistered(client.Phone);

			using (var db = new ItaliasPizzaDBEntities())
			{
				db.Client.Attach(client);
				db.Address.Attach(address);

				db.Client.Remove(client);
				db.Address.Remove(address);
				db.SaveChanges();
			}

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void GetClientByDeliveryOrderTest()
		{
			var idClient = Guid.NewGuid();
			var client = new Client
			{
				IdClient = idClient,
				Phone = "1234567890",
				FirstName = "John",
				LastName = "Doe",
			};

			var idAddress = Guid.NewGuid();
			var address = new Address
			{
				Street = "Test",
				Number = 123,
				Colony = "Test",
				PostalCode = "12345",
				Reference = "Test",
				Status = true,
				IdClient = idClient
			};
			ClientOperations.SaveClient(client,address);

			var idDeliveryOrder = Guid.NewGuid();
			var deliveryOrder = new DeliveryOrder
			{
				IdDeliveryOrder = idDeliveryOrder,
				IdClient = idClient,
				Date = DateTime.Now,
				IdOrderStatus = OrderStatusOperations.GetOrderStatusByName("Pendiente").IdOrderStatus
			};
			DeliveryOrderOperations.SaveDeliveryOrder(deliveryOrder);

			var result = ClientOperations.GetClientByDeliveryOrder(deliveryOrder);

			using (var db = new ItaliasPizzaDBEntities())
			{
				db.Client.Attach(client);
				db.Address.Attach(address);
				db.DeliveryOrder.Attach(deliveryOrder);

				db.Client.Remove(client);
				db.Address.Remove(address);
				db.DeliveryOrder.Remove(deliveryOrder);
				db.SaveChanges();
			}

			Assert.AreEqual(client.IdClient, result.IdClient);
		}

        [TestMethod]
        public void UpdateClientTest()
        {
            var idClient = Guid.NewGuid();
            var client = new Client
            {
                IdClient = idClient,
                Phone = "1234567890",
                FirstName = "John",
                LastName = "Doe",
            };

			// save client with linq
			using (var db = new ItaliasPizzaDBEntities())
			{
                db.Client.Add(client);
                db.SaveChanges();
            }

            client.FirstName = "Jane";
            var result = ClientOperations.UpdateClient(client);

            //teardown
            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Client.Attach(client);
                db.Client.Remove(client);
                db.SaveChanges();
            }

            Assert.AreEqual(1, result);
        }
    }
}
