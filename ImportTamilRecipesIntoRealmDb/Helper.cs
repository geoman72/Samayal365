using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using ImportTamilRecipesIntoRealmDb.RelmnDb;
using MetaWeblogApiWebApp.Models;
using Microsoft.ApplicationBlocks.Data;
using Remotion.Linq.Clauses;

namespace ImportTamilRecipesIntoRealmDb
{
    public class Helper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private List<Category> GetCategoryList(string tableName)
        {
            List<Category> retValue = new List<Category>();

            SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(this.GetGodaddyConnectionString()
                , CommandType.Text
                , "select * from " + tableName + " order by id"
            );

            while (sqlDataReader.Read())
            {
                retValue.Add(new Category
                {
                    Id = int.Parse(sqlDataReader["id"].ToString()),
                    CreatedOn = System.DateTime.Parse(sqlDataReader["created_at"].ToString()).Date.ToString("MM-dd-yyyy"),
                    ImagePath = sqlDataReader["image_path"].ToString().Split('\\').Last(),
                    Name = sqlDataReader["name"].ToString().Trim(),
                    UpdatedOn = System.DateTime.Parse(sqlDataReader["updated_at"].ToString()).Date.ToString("MM-dd-yyyy"),
                });
            }

            return retValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<Recipe> GetRecipesList(System.String tableName)
        {
            List<Recipe> retValue = new List<Recipe>();

            SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(this.GetGodaddyConnectionString()
                , CommandType.Text
                , "select * from " + tableName + " order by id"
            );

            while (sqlDataReader.Read())
            {
                retValue.Add(new Recipe
                {
                    CategoryId = int.Parse(sqlDataReader["category_id"].ToString()),
                    CreatedOn = System.DateTime.Parse(sqlDataReader["created_at"].ToString()).Date.ToString("MM-dd-yyyy"),
                    Id = int.Parse(sqlDataReader["id"].ToString()),
                    ImagePath = sqlDataReader["image_path"].ToString().Split('\\').Last(),
                    IsFavorite = Boolean.Parse(sqlDataReader["is_favorite"].ToString()) == true ? 1 : 0,
                    Name = sqlDataReader["name"].ToString().Replace("&gt;", "").Trim(),
                    Rating = int.Parse(sqlDataReader["ratings"].ToString()),
                    Description = SteralizeAndValidateRecipeDetail(sqlDataReader["recipe_detail"].ToString()),
                    UpdatedOn = System.DateTime.Parse(sqlDataReader["updated_at"].ToString()).Date.ToString("MM-dd-yyyy"),
                    MyRating = 0,
                    MyRatingUpdatedAt = null,
                    RatingTotal = int.Parse(sqlDataReader["rating_total"].ToString()),
                    SyncMyRating = 0
                });
            }

            return retValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private String GetGodaddyConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["goDaddyConStr"].ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Category> GetBriyaniCategoriesFromSqlServer()
        {
            return this.GetCategoryList("samayal365_briyani_recipe_categories");

            //List<Category> retValue = new List<Category>();

            //using (var context = new TamilRecipeEntities())
            //{
            //    var query = from category in context.samayal365_briyani_recipe_categories
            //        select category;

            //    foreach (var category in query.ToList())
            //    {
            //        retValue.Add(new Category
            //        {
            //            Id = category.id,
            //            CreatedOn = category.created_at.Date.ToString("MM-dd-yyyy"),
            //            ImagePath = category.image_path.Split('\\').Last(),
            //            Name = category.name.Trim(),
            //            UpdatedOn = category.updated_at.Date.ToString("MM-dd-yyyy")
            //        });   
            //    }
            //}

            //return retValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Recipe> GetBriyaniRecipesFromSqlServer()
        {
            return this.GetRecipesList("samayal365_briyani_recipes");
            //List<Recipe> retValue = new List<Recipe>();

            //using (var context = new TamilRecipeEntities())
            //{
            //    var query = from recipe in context.samayal365_briyani_recipes
            //        select recipe;

            //    foreach (var recipe in query.ToList())
            //    {
            //        retValue.Add( new Recipe
            //        {
            //            CategoryId = recipe.category_id,
            //            CreatedOn = recipe.created_at.Date.ToString("MM-dd-yyyy"),
            //            Id = recipe.id,
            //            ImagePath = recipe.image_path.Split('\\').Last(),
            //            IsFavorite = recipe.is_favorite == true ? 1 : 0,
            //            Name = recipe.name.Replace("&gt;","").Trim(),
            //            Rating = recipe.ratings,
            //            Description = SteralizeAndValidateRecipeDetail(recipe.recipe_detail),
            //            UpdatedOn = recipe.updated_at.Date.ToString("MM-dd-yyyy"),
            //            MyRating = 0,
            //            MyRatingUpdatedAt = null,
            //            RatingTotal = recipe.rating_total,
            //            SyncMyRating = 0
            //        });
            //    }
            //}

            //return retValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<Category> GetChettinadCategoriesFromSqlServer()
        {
            List<Category> retValue = new List<Category>();

            using (var context = new TamilRecipeEntities())
            {
                var query = from category in context.samayal365_chettinadu_recipe_categories
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

        /// <summary
        /// 
        /// </summary>
        /// <returns></returns>
        private  String SteralizeAndValidateRecipeDetail(String recipeDetail)
        {
            String retValue = Regex.Replace(recipeDetail, @"\s+", " ");
            retValue = retValue.Replace(System.Environment.NewLine, System.String.Empty);
            retValue = retValue.Replace("<strong>", System.String.Empty);
            retValue = retValue.Replace(@"</strong>", System.String.Empty);
            TamilRecipeValidationUtility recipeValidation = new TamilRecipeValidationUtility();
            recipeValidation.IsValidRecipeDescription(retValue);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(retValue);
            retValue = xmlDoc.OuterXml;
            return retValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<Recipe> GetChettinadRecipesFromSqlServer()
        {
            return this.GetRecipesList("samayal365_chettinadu_recipes");
            //List<Recipe> retValue = new List<Recipe>();

            //using (var context = new TamilRecipeEntities())
            //{
            //    var query = from recipe in context.samayal365_chettinadu_recipes
            //            select recipe
            //        ;

            //    foreach (var recipe in query.ToList())
            //    {
            //        //Program.Logger.Info("validating recipe => " + recipe.id.ToString());

            //        retValue.Add(new Recipe
            //        {
            //            CategoryId = recipe.category_id,
            //            CreatedOn = recipe.created_at.Date.ToString("MM-dd-yyyy"),
            //            Id = recipe.id,
            //            ImagePath = recipe.image_path.Split('\\').Last(),
            //            IsFavorite = recipe.is_favorite == true ? 1 : 0,
            //            Name = recipe.name.Replace("&gt;", "").Trim(),
            //            Rating = recipe.ratings,
            //            Description = SteralizeAndValidateRecipeDetail(recipe.recipe_detail),
            //            UpdatedOn = recipe.updated_at.Date.ToString("MM-dd-yyyy"),
            //            MyRating = 0,
            //            MyRatingUpdatedAt = null,
            //            RatingTotal = recipe.rating_total,
            //            SyncMyRating = 0
            //        });
            //    }
            //}

            //return retValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<RecipeConfig> GetRecipeConfigList()
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
