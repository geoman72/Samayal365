using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImportTamilRecipesIntoRealmDb.RelmnDb;
using Remotion.Linq.Clauses;

namespace ImportTamilRecipesIntoRealmDb
{
    public class SqHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Category> GetBriyaniCategoriesFromSqlServer()
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
        public static List<Recipe> GetBriyaniRecipesFromSqlServer()
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
                        Rating = recipe.ratings,
                        Description = recipe.recipe_detail,
                        UpdatedOn = recipe.updated_at.Date.ToString("MM-dd-yyyy"),
                        MyRating = 0,
                        MyRatingUpdatedAt = null,
                        RatingTotal = recipe.rating_total,
                        SyncMyRating = 0
                    });
                }
            }

            return retValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Category> GetChettinadCategoriesFromSqlServer()
        {
            List<Category> retValue = new List<Category>();

            using (var context = new TamilRecipeEntities())
            {
                var query = from category in context.samayal365_chettinadu_recipes
                            select category;

                foreach (var category in query.ToList())
                {
                    retValue.Add(new Category
                    {
                        Id = category.id,
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
        public static List<Recipe> GetChettinadRecipesFromSqlServer()
        {
            List<Recipe> retValue = new List<Recipe>();

            using (var context = new TamilRecipeEntities())
            {
                var query = from recipe in context.samayal365_chettinadu_recipes
                            select recipe;

                foreach (var recipe in query.ToList())
                {
                    retValue.Add(new Recipe
                    {
                        CategoryId = recipe.category_id,
                        CreatedOn = recipe.created_at.Date.ToString("MM-dd-yyyy"),
                        Id = recipe.id,
                        ImagePath = recipe.image_path.Split('\\').Last(),
                        IsFavorite = recipe.is_favorite == true ? 1 : 0,
                        Name = recipe.name.Replace("&gt;", "").Trim(),
                        Rating = recipe.ratings,
                        Description = recipe.recipe_detail,
                        UpdatedOn = recipe.updated_at.Date.ToString("MM-dd-yyyy"),
                        MyRating = 0,
                        MyRatingUpdatedAt = null,
                        RatingTotal = recipe.rating_total,
                        SyncMyRating = 0
                    });
                }
            }

            return retValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<RecipeConfig> GetRecipeConfigList()
        {
            List<RecipeConfig> retValue = new List<RecipeConfig>();

            retValue.Add(new RecipeConfig
            {
                Id = 1,
                CreatedOn = DateTime.Today.Date.ToString("dd-M-yyyy"),
                UpdatedOn = DateTime.Today.Date.ToString("dd-M-yyyy"),
                NextSyncAt = null,
                ClientId = null
            });

            return retValue;
        }
    }
}
