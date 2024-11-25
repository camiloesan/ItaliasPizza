using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;
using ItaliasPizza.Pages;
using ItaliasPizza.Utils;

namespace ItaliasPizza.DataAccessLayer
{
	public class RecipeOperations
	{
		// UNIT TEST
		public static int SaveRecipeWithSupplies(Recipe recipe, List<RecipeSupply> recipeSupplies)
		{
			int result = 0;

			using (var db = new ItaliasPizzaDBEntities())
			{
				using (var transaction = db.Database.BeginTransaction())
				{
					try {
						db.Recipe.Add(recipe);

						foreach (var recipeSupply in recipeSupplies)
						{
							db.RecipeSupply.Add(recipeSupply);
						}

						db.SaveChanges();

						transaction.Commit();

						result = recipeSupplies.Count + 1;
					}
					catch (Exception e)
					{
						transaction.Rollback();
					}
				}
			}

			return result;
		}

        public static Recipe GetRecipeByProductId(Guid productId)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                return db.Recipe.FirstOrDefault(r => r.IdProduct == productId);
            }
        }

        public static int UpdateRecipeWithSupplies(Recipe recipe, List<RecipeSupply> recipeSupplies)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        // Check if the recipe exists
                        var recipeToUpdate = db.Recipe.Find(recipe.IdRecipe);
                        if (recipeToUpdate == null)
                        {
                            throw new Exception($"Recipe with ID {recipe.IdRecipe} not found.");
                        }

                        // Update recipe instructions
                        recipeToUpdate.Instructions = recipe.Instructions;

                        // Remove all previous recipe supplies
                        var previousRecipeSupplies = db.RecipeSupply
                            .Where(rs => rs.IdRecipe == recipe.IdRecipe)
                            .ToList();

                        db.RecipeSupply.RemoveRange(previousRecipeSupplies);

                        // Add new recipe supplies
                        foreach (var recipeSupply in recipeSupplies)
                        {
                            recipeSupply.IdRecipeSupply = Guid.NewGuid(); // Ensure unique ID for new supplies
                            recipeSupply.IdRecipe = recipe.IdRecipe; // Associate with the recipe
                            db.RecipeSupply.Add(recipeSupply);
                        }

                        // Save changes and commit transaction
                        int result = db.SaveChanges();
                        transaction.Commit();
                        return result;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw; // Re-throw the exception after rollback
                    }
                }
            }
        }


        public static List<RecipeSupplyDetails> GetRecipeSuppliesDetailsByIdProduct(Guid productGuid)
        {
            using (var context = new ItaliasPizzaDBEntities())
            {
                var query = from recipeSupply in context.RecipeSupply
                            join supply in context.Supply
                            on recipeSupply.IdSupply equals supply.IdSupply
                            join recipe in context.Recipe
                            on recipeSupply.IdRecipe equals recipe.IdRecipe
                            where recipe.IdProduct == productGuid
                            select new RecipeSupplyDetails
                            {
                                IdRecipe = recipeSupply.IdRecipe,
                                IdSupply = recipeSupply.IdSupply,
                                SupplyName = supply.Name,
                                SupplyAmount = recipeSupply.SupplyAmount,
                                MeasurementUnit = recipeSupply.MeasurementUnit.MeasurementUnit1,
                                IdMeasurementUnit = recipeSupply.IdMeasurementUnit
                            };

                return query.ToList();
            }
        }
    }
}
