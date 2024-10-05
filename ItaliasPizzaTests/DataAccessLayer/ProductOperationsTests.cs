using Database;
using ItaliasPizza.DataAccessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItaliasPizzaTests.DataAccessLayer
{
	/// <summary>
	/// Summary description for ProductOperationsTests
	/// </summary>
	[TestClass]
	public class ProductOperationsTests
	{
		public ProductOperationsTests()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		[TestMethod]
		public void GetACtiveProductsTest()
		{
			var expected = new List<Product>
			{
				// Los GUID varian en cada base de datos, por lo que se deben cambiar
				// TODO: constructor de datos de prueba para que los GUID sean iguales
				new Product
				{
					IdProduct = Guid.Parse("e4021288-5e41-4385-9b26-3b4f294fa5eb"),
					Name = "Pizza Margarita",
					IdType = 1,
					Price = 100,
					Status = true
				},
				new Product
				{
					IdProduct = Guid.Parse("e378aeca-3c97-4ca1-ad86-c81f47fa6e4d"),
					Name = "Pizza Pepperoni",
					IdType = 1,
					Price = 120,
					Status = true
				},
				new Product
				{
					IdProduct = Guid.Parse("d401dbf2-cc35-4abd-9156-e67076f3708a"),
					Name = "Pizza Mexicana",
					IdType = 1,
					Price = 140,
					Status = true
				},
				new Product
				{
					IdProduct = Guid.Parse("6fb66d23-7e34-4572-ab34-ea5030def8ac"),
					Name = "Pizza Vegetariana",
					IdType = 1,
					Price = 110,
					Status = true
				},
				new Product
				{
					IdProduct = Guid.Parse("ba7aafa9-d418-40fd-af76-eea03f4d979c"),
					Name = "Pizza Hawaiana",
					IdType = 1,
					Price = 130,
					Status = true
				}
			};

			var result = ProductOperations.GetActiveProducts().ToList();

			//Assert.AreEqual(expected, result);
			for (int i = 0; i < expected.Count; i++)
			{
				Assert.AreEqual(expected[i].IdProduct, result[i].IdProduct);
				Assert.AreEqual(expected[i].Name, result[i].Name);
				Assert.AreEqual(expected[i].IdType, result[i].IdType);
				Assert.AreEqual(expected[i].Price, result[i].Price);
				Assert.AreEqual(expected[i].Status, result[i].Status);
			}
		}

		[TestMethod]
		public void GetPreparableProductQuantityTest()
		{
			// Los GUID varian en cada base de datos, por lo que se deben cambiar
			// TODO: constructor de datos de prueba para que los GUID sean igualess
			var stringIdProduct = "e4021288-5e41-4385-9b26-3b4f294fa5eb";
			var idProduct = Guid.Parse(stringIdProduct);

			Product product = new Product
			{
				IdProduct = idProduct
			};

			var expected = 100;
			var result = ProductOperations.GetPreparableProductQuantity(product);
			Assert.AreEqual(expected, result);
		}
	}
}
