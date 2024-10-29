using Database;
using ItaliasPizza.DataAccessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
			var idMargarita = Guid.NewGuid();
			var idPiPepperoni = Guid.NewGuid();
			var idMexicana = Guid.NewGuid();
			var idVegetariana = Guid.NewGuid();
			var idHawaiana = Guid.NewGuid();

			var margarita = new Product { IdProduct = idMargarita, Name = "Pizza Margarita", IdType = 1, Price = 100, Size = "Chica", Status = true };
			var pepperoni = new Product { IdProduct = idPiPepperoni, Name = "Pizza Pepperoni", IdType = 1, Price = 120, Size = "Chica", Status = true };
			var mexicana = new Product { IdProduct = idMexicana, Name = "Pizza Mexicana", IdType = 1, Price = 140, Size = "Chica", Status = true };
			var vegetariana = new Product { IdProduct = Guid.NewGuid(), Name = "Pizza Vegetariana", IdType = 1, Price = 130, Size = "Chica", Status = false };
			var hawaiana = new Product { IdProduct = Guid.NewGuid(), Name = "Pizza Hawaiana", IdType = 1, Price = 150, Size = "Chica", Status = false };

			var expected = new List<Product>
			{
				margarita,
				pepperoni,
				mexicana
			};

			using (var db = new ItaliasPizzaDBEntities())
			{
				foreach (var product in expected)
				{
					ProductOperations.SaveProduct(product);
				}
			}

			var result = ProductOperations.GetActiveProducts().ToList();

			using (var db = new ItaliasPizzaDBEntities())
			{
				db.Product.Attach(margarita);
				db.Product.Attach(pepperoni);
				db.Product.Attach(mexicana);
				db.Product.Remove(margarita);
				db.Product.Remove(pepperoni);
				db.Product.Remove(mexicana);

				db.SaveChanges();
			}

			foreach (var product in expected)
			{
				Assert.IsTrue(result.Any(p => p.IdProduct == product.IdProduct));
			}
		}

		[TestMethod]
		public void GetPreparableProductQuantityTest()
		{
			var idMargarita = Guid.NewGuid();
			var idPiPepperoni = Guid.NewGuid();
			var idMexicana = Guid.NewGuid();
			var idVegetariana = Guid.NewGuid();
			var idHawaiana = Guid.NewGuid();
			var margarita = new Product { IdProduct = idMargarita, Name = "Pizza Margarita", IdType = 1, Price = 100, Size = "Chica", Status = true };
			var pepperoni = new Product { IdProduct = idPiPepperoni, Name = "Pizza Pepperoni", IdType = 1, Price = 120, Size = "Chica", Status = true };
			var mexicana = new Product { IdProduct = idMexicana, Name = "Pizza Mexicana", IdType = 1, Price = 140, Size = "Chica", Status = true };
			var vegetariana = new Product { IdProduct = idVegetariana, Name = "Pizza Vegetariana", IdType = 1, Price = 130, Size = "Chica", Status = false };
			var hawaiana = new Product { IdProduct = idHawaiana, Name = "Pizza Hawaiana", IdType = 1, Price = 150, Size = "Chica", Status = false };
			var pizzas = new List<Product>
			{               
				margarita,
				pepperoni,
				mexicana,
				vegetariana,
				hawaiana
			};
			foreach (var pizza in pizzas)
			{
				ProductOperations.SaveProduct(pizza);
			}

			var idHarina = Guid.NewGuid();
			var idQueso = Guid.NewGuid();
			var idTomate = Guid.NewGuid();
			var idPepperoni = Guid.NewGuid();
			var idPina = Guid.NewGuid();
			var idChile = Guid.NewGuid();
			var supHarina = new Supply { IdSupply = idHarina, Name = "Harina", Quantity = 100.00m, IdSupplyCategory = 1, IdMeasurementUnit = 1, Status = true };
			var supQueso = new Supply { IdSupply = idQueso, Name = "Queso", Quantity = 50.00m, IdSupplyCategory = 3, IdMeasurementUnit = 1, Status = true };
			var supTomate = new Supply { IdSupply = idTomate, Name = "Tomate", Quantity = 30.00m, IdSupplyCategory = 6, IdMeasurementUnit = 1, Status = true };
			var supPeppeorni = new Supply { IdSupply = idPepperoni, Name = "Pepperoni", Quantity = 20.00m, IdSupplyCategory = 4, IdMeasurementUnit = 1, Status = true };
			var supPina = new Supply { IdSupply = idPina, Name = "Piña", Quantity = 10.00m, IdSupplyCategory = 6, IdMeasurementUnit = 1, Status = true };
			var supChile = new Supply { IdSupply = idChile, Name = "Chile", Quantity = 10.00m, IdSupplyCategory = 6, IdMeasurementUnit = 1, Status = true };
			var supplies = new List<Supply>
			{
				supHarina,
				supQueso,
				supTomate,
				supPeppeorni,
				supPina,
				supChile
			};
			using (var db = new ItaliasPizzaDBEntities())
			{
				foreach (var supply in supplies)
				{
					SupplyOperations.SaveSupply(supply);
				}
			}

			var recMargarita = Guid.NewGuid();
			var recPiPepperoni = Guid.NewGuid();
			var recHawaiana = Guid.NewGuid();
			var recMexicana = Guid.NewGuid();
			var recVegetariana = Guid.NewGuid();
			var reciMargarita = new Recipe { IdRecipe = recMargarita, IdProduct = idMargarita, Instructions = "Mezclar la harina con el agua y la levadura, dejar reposar por 1 hora" };
			var reciPepperoni = new Recipe { IdRecipe = recPiPepperoni, IdProduct = idPiPepperoni, Instructions = "Mezclar la harina con el agua y la levadura, dejar reposar por 1 hora, agregar el pepperoni" };
			var reciHawaiana = new Recipe { IdRecipe = recHawaiana, IdProduct = idHawaiana, Instructions = "Mezclar la harina con el agua y la levadura, dejar reposar por 1 hora, agregar la piña" };
			var reciVegetariana = new Recipe { IdRecipe = recVegetariana, IdProduct = idVegetariana, Instructions = "Mezclar la harina con el agua y la levadura, dejar reposar por 1 hora, agregar los vegetales" };
			var reciMexicana = new Recipe { IdRecipe = recMexicana, IdProduct = idMexicana, Instructions = "Mezclar la harina con el agua y la levadura, dejar reposar por 1 hora, agregar el chile" };
			var recipes = new List<Recipe>
			{
				reciMargarita,
				reciPepperoni,
				reciHawaiana,
				reciVegetariana,
				reciMexicana
			};
			using (var db = new ItaliasPizzaDBEntities()) 
			{
				foreach (var recipe in recipes)
				{
					db.Recipe.Add(recipe);
					db.SaveChanges();
				}
			}

			var recSup1Margarita = new RecipeSupply { IdRecipeSupply = Guid.NewGuid(), IdRecipe = recMargarita, IdSupply = idHarina, SupplyAmount = 1.00m, IdMeasurementUnit = 1 };
			var recSup2Margarita = new RecipeSupply { IdRecipeSupply = Guid.NewGuid(), IdRecipe = recMargarita, IdSupply = idQueso, SupplyAmount = 0.50m, IdMeasurementUnit = 1 };
			var recSup1Pepperoni = new RecipeSupply { IdRecipeSupply = Guid.NewGuid(), IdRecipe = recPiPepperoni, IdSupply = idHarina, SupplyAmount = 1.00m, IdMeasurementUnit = 1 };
			var recSup2Pepperoni = new RecipeSupply { IdRecipeSupply = Guid.NewGuid(), IdRecipe = recPiPepperoni, IdSupply = idPepperoni, SupplyAmount = 0.20m, IdMeasurementUnit = 1 };
			var recSup1Hawaiana = new RecipeSupply { IdRecipeSupply = Guid.NewGuid(), IdRecipe = recHawaiana, IdSupply = idHarina, SupplyAmount = 1.00m, IdMeasurementUnit = 1 };
			var recSup2Hawaiana = new RecipeSupply { IdRecipeSupply = Guid.NewGuid(), IdRecipe = recHawaiana, IdSupply = idPina, SupplyAmount = 0.30m, IdMeasurementUnit = 1 };
			var recSup1Vegetariana = new RecipeSupply { IdRecipeSupply = Guid.NewGuid(), IdRecipe = recVegetariana, IdSupply = idHarina, SupplyAmount = 1.00m, IdMeasurementUnit = 1 };
			var recSup2Vegetariana = new RecipeSupply { IdRecipeSupply = Guid.NewGuid(), IdRecipe = recVegetariana, IdSupply = idTomate, SupplyAmount = 0.50m, IdMeasurementUnit = 1 };
			var recSup1Mexicana = new RecipeSupply { IdRecipeSupply = Guid.NewGuid(), IdRecipe = recMexicana, IdSupply = idHarina, SupplyAmount = 1.00m, IdMeasurementUnit = 1 };
			var recSup2Mexicana = new RecipeSupply { IdRecipeSupply = Guid.NewGuid(), IdRecipe = recMexicana, IdSupply = idChile, SupplyAmount = 0.20m, IdMeasurementUnit = 1 };
			var recipeSupplies = new List<RecipeSupply>
			{
				recSup1Margarita,
				recSup2Margarita,
				recSup1Pepperoni,
				recSup2Pepperoni,
				recSup1Hawaiana,
				recSup2Hawaiana,
				recSup1Vegetariana,
				recSup2Vegetariana,
				recSup1Mexicana,
				recSup2Mexicana
			};
			using (var db = new ItaliasPizzaDBEntities())
			{
				db.RecipeSupply.AddRange(recipeSupplies);
				db.SaveChanges();
			}

			var expected = 100;
			var result = ProductOperations.GetPreparableProductQuantity(pepperoni);

			using (var db = new ItaliasPizzaDBEntities())
			{
				db.Product.Attach(margarita);
				db.Product.Attach(pepperoni);
				db.Product.Attach(mexicana);
				db.Product.Attach(vegetariana);
				db.Product.Attach(hawaiana);
				db.Product.Remove(margarita);
				db.Product.Remove(pepperoni);
				db.Product.Remove(mexicana);
				db.Product.Remove(vegetariana);
				db.Product.Remove(hawaiana);

				db.Supply.Attach(supHarina);
				db.Supply.Attach(supQueso);
				db.Supply.Attach(supTomate);
				db.Supply.Attach(supPeppeorni);
				db.Supply.Attach(supPina);
				db.Supply.Attach(supChile);
				db.Supply.Remove(supHarina);
				db.Supply.Remove(supQueso);
				db.Supply.Remove(supTomate);
				db.Supply.Remove(supPeppeorni);
				db.Supply.Remove(supPina);
				db.Supply.Remove(supChile);
				

				db.Recipe.Attach(reciMargarita);
				db.Recipe.Attach(reciPepperoni);
				db.Recipe.Attach(reciHawaiana);
				db.Recipe.Attach(reciVegetariana);
				db.Recipe.Attach(reciMexicana);
				db.Recipe.Remove(reciMargarita);
				db.Recipe.Remove(reciPepperoni);
				db.Recipe.Remove(reciHawaiana);
				db.Recipe.Remove(reciVegetariana);
				db.Recipe.Remove(reciMexicana);

				db.RecipeSupply.Attach(recSup1Margarita);
				db.RecipeSupply.Attach(recSup2Margarita);
				db.RecipeSupply.Attach(recSup1Pepperoni);
				db.RecipeSupply.Attach(recSup2Pepperoni);
				db.RecipeSupply.Attach(recSup1Hawaiana);
				db.RecipeSupply.Attach(recSup2Hawaiana);
				db.RecipeSupply.Attach(recSup1Vegetariana);
				db.RecipeSupply.Attach(recSup2Vegetariana);
				db.RecipeSupply.Attach(recSup1Mexicana);
				db.RecipeSupply.Attach(recSup2Mexicana);
				db.RecipeSupply.Remove(recSup1Margarita);
				db.RecipeSupply.Remove(recSup2Margarita);
				db.RecipeSupply.Remove(recSup1Pepperoni);
				db.RecipeSupply.Remove(recSup2Pepperoni);
				db.RecipeSupply.Remove(recSup1Hawaiana);
				db.RecipeSupply.Remove(recSup2Hawaiana);
				db.RecipeSupply.Remove(recSup1Vegetariana);
				db.RecipeSupply.Remove(recSup2Vegetariana);
				db.RecipeSupply.Remove(recSup1Mexicana);
				db.RecipeSupply.Remove(recSup2Mexicana);

				db.SaveChanges();
			}

			Assert.AreEqual(expected, result);
		}

        [TestMethod]
		public void GetProductDetailsTest()
		{
            var idMargarita = Guid.NewGuid();
            var margarita = new Product { IdProduct = idMargarita, Name = "Pizza Margarita", IdType = 1, Price = 100, Size = "Chica", Status = true };
            using (var db = new ItaliasPizzaDBEntities())
            {
                ProductOperations.SaveProduct(margarita);
            }

            Assert.IsNotNull(ProductOperations.GetProductDetails());

            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Product.Attach(margarita);
                db.Product.Remove(margarita);
                db.SaveChanges();
            }

        }
    }
}
