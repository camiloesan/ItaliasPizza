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
	}
}
