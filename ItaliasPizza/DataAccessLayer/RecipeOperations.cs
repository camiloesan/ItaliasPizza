using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;

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

						return recipeSupplies.Count + 1;
					}
					catch (Exception e)
					{
						transaction.Rollback();
					}
				}
			}

			return result;
		}
	}
}
