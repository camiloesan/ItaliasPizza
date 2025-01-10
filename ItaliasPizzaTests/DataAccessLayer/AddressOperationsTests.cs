using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Database;
using ItaliasPizza.DataAccessLayer;

namespace ItaliasPizzaTests.DataAccessLayer
{
	[TestClass]
	public class AddressOperationsTests
	{
		[TestMethod]
		public void SaveAddressTest()
		{
			var testClient = new Client { IdClient = Guid.NewGuid(), FirstName = "Jane", LastName = "Doe", Phone = "1234567890" };
			using (var db = new ItaliasPizzaDBEntities())
			{
				db.Client.Add(testClient);
				db.SaveChanges();
			}

			var testAddress = new Address { IdAddress = Guid.NewGuid(), IdClient = testClient.IdClient, Street = "123 Main St", Number = 123, PostalCode = "1234", Colony = "Main", Reference = "Some reference" };

			var result = AddressOperations.SaveAddress(testAddress);

			using (var db = new ItaliasPizzaDBEntities())
			{
				db.Client.Attach(testClient);
				db.Address.Attach(testAddress);

				db.Client.Remove(testClient);
				db.Address.Remove(testAddress);
				db.SaveChanges();
			}

			Assert.AreEqual(result, 1);
		}

		[TestMethod]
		public void SaveAddressTestWithException()
		{
			var testAddress = new Address { IdAddress = Guid.NewGuid(), IdClient = Guid.NewGuid(), Street = "123 Main St", Number = 123, PostalCode = "1234", Colony = "Main", Reference = "Some reference" };
			var result = AddressOperations.SaveAddress(testAddress);
			Assert.AreEqual(result, -1);
		}

		[TestMethod]
		public void GetClientAddressesTest()
		{
			var testClient = new Client { IdClient = Guid.NewGuid(), FirstName = "Jane", LastName = "Doe", Phone = "1234567890" };
			var testAddress1 = new Address { IdAddress = Guid.NewGuid(), IdClient = testClient.IdClient, Street = "123 Main St", Number = 123, PostalCode = "1234", Colony = "Main", Reference = "Some reference" };
			var testAddress2 = new Address { IdAddress = Guid.NewGuid(), IdClient = testClient.IdClient, Street = "456 Elm St", Number = 456, PostalCode = "5678", Colony = "Elm", Reference = "Another reference" };
			
			using (var db = new ItaliasPizzaDBEntities())
			{
				db.Client.Add(testClient);
				db.Address.Add(testAddress1);
				db.Address.Add(testAddress2);
				db.SaveChanges();
			}

			var resultAddresses = AddressOperations.GetClientAddresses(testClient);
			
			using (var db = new ItaliasPizzaDBEntities())
			{
				db.Client.Attach(testClient);
				db.Address.Attach(testAddress1);
				db.Address.Attach(testAddress2);

				db.Client.Remove(testClient);
				db.Address.Remove(testAddress1);
				db.Address.Remove(testAddress2);

				db.SaveChanges();
			}
			Assert.AreEqual(resultAddresses.Count, 2);
		}

		[TestMethod]
		public void GetAddressByIdTest()
		{
			var testClient = new Client { IdClient = Guid.NewGuid(), FirstName = "Jane", LastName = "Doe", Phone = "1234567890" };
			var testAddress = new Address { IdAddress = Guid.NewGuid(), IdClient = testClient.IdClient, Street = "123 Main St", Number = 123, PostalCode = "1234", Colony = "Main", Reference = "Some reference" };

			using (var db = new ItaliasPizzaDBEntities())
			{
				db.Client.Add(testClient);
				db.Address.Add(testAddress);
				db.SaveChanges();
			}
			var resultAddress = AddressOperations.GetAddressById(testAddress.IdAddress);

			using (var db = new ItaliasPizzaDBEntities())
			{
				db.Client.Attach(testClient);
				db.Address.Attach(testAddress);
				db.Client.Remove(testClient);
				db.Address.Remove(testAddress);
				db.SaveChanges();
			}
			Assert.AreEqual(resultAddress.Street, "123 Main St");
		}
	}
}
