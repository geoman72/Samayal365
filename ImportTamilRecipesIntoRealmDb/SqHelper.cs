using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Remotion.Linq.Clauses;

namespace ImportTamilRecipesIntoRealmDb
{
    public class SqHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="recipeCategoryTableName"></param>
        /// <returns></returns>
        private static List<Category> GetCategoriesFromSqlServer(String recipeCategoryTableName)
        {
            List<Category> retValue = new List<Category>();

            using (var context = new TamilRecipeEntities())
            {
                var query= from category in  context.samayal365_briyani_recipe_categories
                select new Category
                {
                    Id = category.id,
                    Count = category.count,
                    CreatedOn = category.created_at.Date.ToShortDateString(),
                    ImagePath = category.image_path,
                    Name = category.name,
                    UpdatedOn = category.updated_at.Date.ToShortDateString()
                };

                retValue.AddRange(query.ToList());
            }

            return retValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recipeTableName"></param>
        /// <returns></returns>
        private static List<Recipe> GetRecipesFromSqlServer(String recipeTableName)
        {
            List<Recipe> retValue = new List<Recipe>();

            using (var context = new TamilRecipeEntities())
            {
                var query = from recipe in context.samayal365_briyani_recipes
                    select new Recipe
                    {
                        CategoryId = recipe.category_id,
                        CreatedOn = recipe.created_at.Date.ToShortDateString(),
                        Id = recipe.id,
                        ImagePath = recipe.,
                        IsFavorite = recipe.is_favorite == true ? 1 : 0,
                        Name = recipe.name,
                        Ratings = recipe.ratings,
                        RecipeDetail = recipe.recipe_detail,
                        UpdatedOn = recipe.updated_at.Date.ToShortDateString()
                    };

                retValue.AddRange(query.ToList());
            }

            return retValue;
        }

    }
}
