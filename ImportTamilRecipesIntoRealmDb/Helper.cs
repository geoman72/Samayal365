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
using NLog.Fluent;
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
        public List<Category> GetCategoryList(string tableName)
        {
            List<Category> retValue = new List<Category>();

            SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(this.GetGodaddyConnectionString()
                , CommandType.Text
                , "select * from " + tableName + " order by id"
            );

            Program.Logger.Info("------------------------- Categories  ---------------");
            while (sqlDataReader.Read())
            {
                Program.Logger.Info(String.Format("Id => {0}, Name => {1}", sqlDataReader["id"].ToString(), sqlDataReader["name"].ToString()));
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

            Program.Logger.Info("------------------------- Recipes ---------------");
            while (sqlDataReader.Read())
            {
                Recipe recipe = new Recipe
                {
                    CategoryId = int.Parse(sqlDataReader["category_id"].ToString()),
                    CreatedOn = System.DateTime.Parse(sqlDataReader["created_at"].ToString()).Date
                        .ToString("MM-dd-yyyy"),
                    Id = int.Parse(sqlDataReader["id"].ToString()),
                    ImagePath = sqlDataReader["image_path"].ToString().Split('\\').Last(),
                    IsFavorite = Boolean.Parse(sqlDataReader["is_favorite"].ToString()) == true ? 1 : 0,
                    Name = sqlDataReader["name"].ToString().Replace("&gt;", "").Trim(),
                    Rating = int.Parse(sqlDataReader["ratings"].ToString()),
                    Description = new RecipeDetailParser().ParseDetail((sqlDataReader["recipe_detail"].ToString())),
                    UpdatedOn = System.DateTime.Parse(sqlDataReader["updated_at"].ToString()).Date
                        .ToString("MM-dd-yyyy"),
                    MyRating = 0,
                    MyRatingUpdatedAt = null,
                    RatingTotal = int.Parse(sqlDataReader["rating_total"].ToString()),
                    SyncMyRating = 0
                };

                retValue.Add(recipe);
               // Program.Logger.Info(String.Format("Id => {0}, Name => {1} \n{2}", recipe.Id, recipe.Name, recipe.Description));
            }

            return retValue;
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private String GetGodaddyConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["goDaddyConStr"].ToString();
        }

    }
}
