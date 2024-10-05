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
				new Product
				{
					IdProduct = Guid.Parse("92cfdaed-6426-450f-85de-abd572136f7d"),
					Name = "Pizza Margarita",
					IdType = 1,
					Price = 100,
					Status = true
				},
				new Product
				{
					IdProduct = Guid.Parse("13ef1c10-de14-4e8e-a738-cf5f3f2202e8"),
					Name = "Pizza Pepperoni",
					IdType = 1,
					Price = 120,
					Status = true
				},
				new Product
				{
					IdProduct = Guid.Parse("6d1a7b07-72aa-4cfd-afec-d660f6edc08c"),
					Name = "Pizza Mexicana",
					IdType = 1,
					Price = 140,
					Status = true
				},
				new Product
				{
					IdProduct = Guid.Parse("d83c28df-9151-4c33-ba52-e5a9e40f2d57"),
					Name = "Pizza Vegetariana",
					IdType = 1,
					Price = 110,
					Status = true
				},
				new Product
				{
					IdProduct = Guid.Parse("4d318789-288b-4d5d-aefc-f8d6ae8f621d"),
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
			var stringIdProduct = "92CFDAED-6426-450F-85DE-ABD572136F7D";
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
