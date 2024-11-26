using Microsoft.VisualStudio.TestTools.UnitTesting;
using ItaliasPizza.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;
using ItaliasPizza.Pages;

namespace ItaliasPizza.DataAccessLayer.Tests
{
    [TestClass()]
    public class RecipeOperationsTests
    {
        [TestMethod()]
        public void UpdateRecipeWithSuppliesTest()
        {
            // create product
            Product product = new Product
            {
                IdProduct = Guid.NewGuid(),
                Name = "TestProductXXXX",
                IdType = 1,
                Price = 10,
                Size = "TestSizeXXX",
                Status = true
            };

            // recipe
            var recipeGuid = Guid.Parse("bb2e8def-6933-4fbb-aec4-9a2b8cfce549");
            Recipe recipe = new Recipe
            {
                IdRecipe = recipeGuid,
                IdProduct = product.IdProduct,
                Instructions = "TestInstructions"
            };

            // create a new list with supplies
            List<RecipeSupply> newRecipeSupplies = new List<RecipeSupply>
            {
                new RecipeSupply
                {
                    IdRecipeSupply = Guid.NewGuid(),
                    IdSupply = Guid.Parse("c8c0cf0e-a19e-4461-97f3-64cddb923849"),
                    SupplyAmount = 20,
                    IdMeasurementUnit = 1
                }
            };

            var result = RecipeOperations.UpdateRecipeWithSupplies(recipe, newRecipeSupplies);
            Assert.AreEqual(true, result>0);
        }
    }
}