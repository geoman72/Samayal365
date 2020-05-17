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
        /// <returns></returns>
        public static List<Category> GetCategoriesFromSqlServer()
        {
            List<Category> retValue = new List<Category>();

            using (var context = new TamilRecipeEntities())
            {
                var query = from category in context.samayal365_briyani_recipe_categories
                    select category;

                foreach (var category in query.ToList())
                {
                    retValue.Add(new Category
                    {
                        Id = category.id,
                        Count = category.count,
                        CreatedOn = category.created_at.Date.ToString("MM-dd-yyyy"),
                        ImagePath = category.image_path.Split('\\').Last(),
                        Name = category.name.Trim(),
                        UpdatedOn = category.updated_at.Date.ToString("MM-dd-yyyy")
                    });   
                }
            }

            return retValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Recipe> GetRecipesFromSqlServer()
        {
            List<Recipe> retValue = new List<Recipe>();

            using (var context = new TamilRecipeEntities())
            {
                var query = from recipe in context.samayal365_briyani_recipes
                    select recipe;

                foreach (var recipe in query.ToList())
                {
                    retValue.Add( new Recipe
                    {
                        CategoryId = recipe.category_id,
                        CreatedOn = recipe.created_at.Date.ToString("MM-dd-yyyy"),
                        Id = recipe.id,
                        ImagePath = recipe.image_path.Split('\\').Last(),
                        IsFavorite = recipe.is_favorite == true ? 1 : 0,
                        Name = recipe.name.Replace("&gt;","").Trim(),
                        Ratings = recipe.ratings,
                        RecipeDetail = recipe.recipe_detail,
                        UpdatedOn = recipe.updated_at.Date.ToString("MM-dd-yyyy")
                    });
                }
            }

            return retValue;
        }

    }
}
