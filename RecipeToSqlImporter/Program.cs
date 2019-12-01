using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using HtmlAgilityPack;
using Microsoft.ApplicationBlocks.Data;
using NLog;
using Exception = System.Exception;

namespace RecipeToSqlImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

            String recipeTableName = "samayal365_rice_recipes";
            String recipeCategoryTableName = "samayal365_rice_recipe_categories";

            //Program.DeleteSqlRecipes(logger, recipeTableName, recipeCategoryTableName);
            //Dictionary<string, CategoryMetadata> categories = Program.GetCategoryMetaData(logger);
            //Program.AddSqlServerCategory(logger, recipCategoryTableName, categories);    
            //Program.AddSqlServerRecipeByCategory(logger, recipeTableName, Program.GetRecipes(logger, categories));

            //Program.DeleteSqliteRecipes();

            //List<Category> categoryList = Program.GetCategoriesFromSqlServer(recipeCategoryTableName);
            //List<Recipe> recipeList = Program.GetRecipesFromSqlServer(recipeTableName);

            //Program.PublishRecipeCategoriesToSqlite(categoryList);
            //Program.PublishRecipesToSqlite(recipeList);
            //Program.TraceRecipeDetailInSqlite();

            System.Xml.XmlDocument xmlDocument = new XmlDocument();                        
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recipeCategoryTableName"></param>
        /// <returns></returns>
        private static List<Category> GetCategoriesFromSqlServer(String recipeCategoryTableName)
        {
            List<Category> retValue = new List<Category>();

            SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(Helper.SqlConnectionString
                , CommandType.Text,
                "select * from " + recipeCategoryTableName);

            while (sqlDataReader.Read())
            {
                retValue.Add(new Category
                {
                    Count = int.Parse(sqlDataReader["count"].ToString()),
                    CreatedOn = DateTime.Parse(sqlDataReader["created_at"].ToString()),
                    Id = int.Parse(sqlDataReader["id"].ToString()),
                    ImagePath = sqlDataReader["image_path"].ToString(),
                    Name = sqlDataReader["name"].ToString(),
                    UpdatedOn = DateTime.Parse(sqlDataReader["updated_at"].ToString())
                });
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

            SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(Helper.SqlConnectionString
                , CommandType.Text,
                "select * from " + recipeTableName);

            while (sqlDataReader.Read())
            {
                retValue.Add(new Recipe
                {
                    CategoryId = int.Parse(sqlDataReader["category_id"].ToString()),
                    CreatedOn = DateTime.Parse(sqlDataReader["created_at"].ToString()),
                    Id = int.Parse(sqlDataReader["id"].ToString()),
                    ImagePath = sqlDataReader["image_path"].ToString(),
                    IsFavorite = Boolean.Parse(sqlDataReader["is_favorite"].ToString()) == true? 1 : 0,
                    Name = sqlDataReader["name"].ToString(),
                    Ratings = int.Parse(sqlDataReader["ratings"].ToString()),
                    RecipeDetail = sqlDataReader["recipe_detail"].ToString(),
                    UpdatedOn = DateTime.Parse(sqlDataReader["updated_at"].ToString())
                });
            }

            return retValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="recipesByCategoryList"></param>
        private static void AddSqlServerRecipeByCategory(Logger logger,  String recipeTableName, Dictionary<string, Dictionary<string, Recipe>> recipesByCategoryList)
        {
            SqlHelper.ExecuteReader(Helper.SqlConnectionString
                , CommandType.Text
                , "DBCC CHECKIDENT (" + recipeTableName + ", RESEED, 0);");

            foreach (string categoryName  in recipesByCategoryList.Keys)
            {
                logger.Info("Adding recipes for category => " + categoryName);
                Program.AddSqlServerRecipe(logger, recipeTableName, recipesByCategoryList[categoryName]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="recipeTableName"></param>
        /// <param name="recipes"></param>
        private static void AddSqlServerRecipe(Logger logger, String recipeTableName, Dictionary<string, Recipe> recipes)
        {
            foreach (Recipe recipe in recipes.Values)
            {
                SqlParameter[] commandParameters = new SqlParameter[4];
                commandParameters[0] = new SqlParameter("@name", SqlDbType.NVarChar);
                commandParameters[0].Value = recipe.Name;

                commandParameters[1] = new SqlParameter("@category_id", SqlDbType.Int);
                commandParameters[1].Value = recipe.CategoryId;

                commandParameters[2] = new SqlParameter("@image_path", SqlDbType.NVarChar);
                commandParameters[2].Value = recipe.ImagePath;

                commandParameters[3] = new SqlParameter("@recipe_detail", SqlDbType.NVarChar);
                commandParameters[3].Value = recipe.RecipeDetail;

                SqlHelper.ExecuteNonQuery(Helper.SqlConnectionString
                    , CommandType.Text
                    , "INSERT INTO " + recipeTableName + " (category_id, name, image_path, recipe_detail, ratings) VALUES (@category_id, @name, @image_path, @recipe_detail, 0);"
                    , commandParameters);

                logger.Info(System.String.Format(@"INSERT INTO " + recipeTableName + " (category_id, name, image_path) VALUES ({0}, {1}, {2})",
                    recipe.CategoryId, recipe.Name, recipe.ImagePath));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="recipCategoryName"></param>
        /// <param name="categoryMetadatas"></param>
        private static void AddSqlServerCategory(NLog.Logger logger, String recipCategoryName, Dictionary<string, CategoryMetadata> categoryMetadatas)
        {
            foreach (CategoryMetadata categoryMetadata in categoryMetadatas.Values)
            {
                SqlParameter[] commandParameters = new SqlParameter[3];
                commandParameters[0] = new SqlParameter("@id", SqlDbType.Int);
                commandParameters[0].Value = categoryMetadata.SeedIdentity;

                commandParameters[1] = new SqlParameter("@name", SqlDbType.NVarChar);
                commandParameters[1].Value = categoryMetadata.Name;

                commandParameters[2] = new SqlParameter("@image_path", SqlDbType.NVarChar);
                commandParameters[2].Value = categoryMetadata.SeedIdentity.ToString() + ".png";

                SqlHelper.ExecuteNonQuery(Helper.SqlConnectionString
                    , CommandType.Text
                    , "SET IDENTITY_INSERT samayal365_rice_recipe_categories ON;INSERT INTO " + recipCategoryName + " (id, name, image_path) VALUES (@id, @name, @image_path); SET IDENTITY_INSERT samayal365_rice_recipe_categories OFF;"
                    , commandParameters);

                logger.Info(System.String.Format(@"INSERT INTO " + recipCategoryName + " (id, name, image_path) VALUES ({0}, {1}, {2})",
                    categoryMetadata.SeedIdentity, categoryMetadata.Name, categoryMetadata.SeedIdentity.ToString() + ".png"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        private static Dictionary<string, CategoryMetadata> GetCategoryMetaData(NLog.Logger logger)
        {
            Dictionary<string, CategoryMetadata> retValue = new Dictionary<string, CategoryMetadata>();

            System.Xml.XmlDocument xmlDocument = new XmlDocument();
            
            xmlDocument.Load(Helper.MetaDataFeedFilePath);
            logger.Info("Categroy Meta data file path => " + Helper.MetaDataFeedFilePath);
            foreach (XmlNode feedsXmlNode in xmlDocument.SelectNodes(@"//feeds/feed"))
            {
                int seedIdentiy = int.Parse(feedsXmlNode.SelectSingleNode(@"identity").InnerText);
                string categoryName = feedsXmlNode.SelectSingleNode(@"categoryname").InnerText;
                string inputfolderpath = feedsXmlNode.SelectSingleNode(@"inputfolderpath").InnerText;

                retValue.Add(categoryName.Trim(), new CategoryMetadata
                {
                    SeedIdentity = seedIdentiy,
                    Name = categoryName.ToString(),
                    InputFolderPath = inputfolderpath
                });

                logger.Info("SeedIdentity => " + seedIdentiy + " CategoryName => " + categoryName + " InputFolderPath => " + inputfolderpath);
            }

            return retValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="categoryMetadatas"></param>
        /// <returns></returns>
        private static Dictionary<string, Dictionary<string, Recipe>> GetRecipes(NLog.Logger logger,
            Dictionary<string, CategoryMetadata> categoryMetadatas)
        {
            Dictionary<string, Dictionary<string, Recipe>> retValue = new Dictionary<string, Dictionary<string, Recipe>>();

            foreach (CategoryMetadata categoryMetadata in categoryMetadatas.Values)
            {
                retValue.Add(categoryMetadata.Name, Program.GetRecipes(logger, categoryMetadata));               
            }

            return retValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="categoryMetadata"></param>
        /// <returns></returns>
        private static Dictionary<string, Recipe> GetRecipes(NLog.Logger logger, CategoryMetadata categoryMetadata)
        {
            Dictionary<string, Recipe> retValue = new Dictionary<string, Recipe>();

            System.Xml.XmlDocument xmlDocument = new XmlDocument();

            string[] entries = Directory.GetFileSystemEntries(categoryMetadata.InputFolderPath
                , "*.html"
                , SearchOption.AllDirectories);

            RecipeInTamilValidator recipeInTamilValidator = new RecipeInTamilValidator();

            int count = 0;
            foreach (string fileNmae in entries)
            {
                xmlDocument.Load(fileNmae);
                // name => /html/body/h1
                string name = xmlDocument.DocumentElement.SelectSingleNode("/html/body/h1").InnerText;
                // /html/body/div
                string recipe = xmlDocument.DocumentElement.SelectSingleNode("/html/body/div").OuterXml
                    .Replace(System.Environment.NewLine, "");
              
                try
                {
                    xmlDocument.LoadXml(recipe);
                    recipeInTamilValidator.IsValidRecipeDescription(recipe);
                    logger.Info(name + " => " + recipe);
                    System.Diagnostics.Trace.WriteLine(name + " => " + recipe);
                    retValue.Add(name, new Recipe
                    {
                        Name = name,
                        ImagePath = categoryMetadata.SeedIdentity.ToString() + ".png",
                        RecipeDetail = recipe,
                        CategoryId = categoryMetadata.SeedIdentity
                    });
                    count = count + 1;
                }
                catch (System.Xml.XmlException e)
                {
                    // Console.WriteLine(fileNmae + "=>" + e.Message);
                    logger.Info(fileNmae + "=>" + e.Message);
                    System.Diagnostics.Trace.WriteLine(fileNmae + "=>" + e.Message);
                }
            }

            return retValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="reicpeTableName"></param>
        /// <param name="categoryTableName"></param>
        private static void DeleteSqlRecipes(NLog.Logger logger, String reicpeTableName, string categoryTableName)
    {
        SqlHelper.ExecuteNonQuery(Helper.SqlConnectionString, CommandType.Text, "delete from " + reicpeTableName);
        SqlHelper.ExecuteNonQuery(Helper.SqlConnectionString, CommandType.Text, "delete from " + categoryTableName);
        logger.Info("Succesfully deleted tables = > " + reicpeTableName + " and " + categoryTableName);
    }

        /// <summary>
        /// 
        /// </summary>
        private static void DeleteSqliteRecipes()
        {
            SQLiteHelper.ExecuteNonQuery(@"Data Source=" + Helper.SqliteDbPath + ";Version=3;Pooling=False;Max Pool Size=100;"
                                         , CommandType.Text
                                        , @"delete from recipes"
                                        );

            SQLiteHelper.ExecuteNonQuery(@"Data Source=" + Helper.SqliteDbPath + ";Version=3;Pooling=False;Max Pool Size=100;"
                                         , CommandType.Text
                                        , @"delete from recipe_categories"
                                        );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryList"></param>
        private static void PublishRecipeCategoriesToSqlite(List<Category> categoryList)
        {          
            foreach (var category in categoryList)
            {
                Program.AddSqliteCategory(category);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recipeList"></param>
        private static void PublishRecipesToSqlite(List<Recipe> recipeList)
        {
            foreach (var recipe in recipeList)
            {
                Program.AddSqliteRecipe(recipe);
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recipeCategory"></param>
        private static void AddSqliteCategory(Category recipeCategory)
        {
            SQLiteParameter[] commandParameters = new SQLiteParameter[5];
            commandParameters[0] = new SQLiteParameter("@id", recipeCategory.Id);
            commandParameters[0].DbType = System.Data.DbType.Int64;

            commandParameters[1] = new SQLiteParameter("@name", recipeCategory.Name);
            commandParameters[1].DbType = System.Data.DbType.String;

            commandParameters[2] = new SQLiteParameter("@updated_at", recipeCategory.UpdatedOn);
            commandParameters[2].DbType = System.Data.DbType.DateTime;

            commandParameters[3] = new SQLiteParameter("@created_at", recipeCategory.CreatedOn);
            commandParameters[3].DbType = System.Data.DbType.DateTime;

            commandParameters[4] = new SQLiteParameter("@image_path", recipeCategory.ImagePath);
            commandParameters[4].DbType = System.Data.DbType.String;

            SQLiteHelper.ExecuteNonQuery(@"Data Source=" + Helper.SqliteDbPath + ";Version=3;Pooling=False;Max Pool Size=100;"
                                     , CommandType.Text
                                    , @"INSERT INTO recipe_categories (_id, name, updated_at, created_at, image_path) VALUES (@id, @name, @updated_at, @created_at, @image_path );"
                                    , commandParameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recipe"></param>
        private static void AddSqliteRecipe(Recipe recipe)
        {
            SQLiteParameter[] commandParameters = new SQLiteParameter[9];

            commandParameters[0] = new SQLiteParameter("@id", recipe.Id);
            commandParameters[0].DbType = System.Data.DbType.Int64;

            commandParameters[1] = new SQLiteParameter("@name", recipe.Name);
            commandParameters[1].DbType = System.Data.DbType.String;

            commandParameters[2] = new SQLiteParameter("@updated_at", recipe.UpdatedOn);
            commandParameters[2].DbType = System.Data.DbType.DateTime;

            commandParameters[3] = new SQLiteParameter("@created_at", recipe.CreatedOn);
            commandParameters[3].DbType = System.Data.DbType.DateTime;

            commandParameters[4] = new SQLiteParameter("@category_id", recipe.CategoryId);
            commandParameters[4].DbType = System.Data.DbType.Int64;

            commandParameters[5] = new SQLiteParameter("@recipe_detail", recipe.RecipeDetail);
            commandParameters[5].DbType = System.Data.DbType.String;

            commandParameters[6] = new SQLiteParameter("@image_path", recipe.ImagePath);
            commandParameters[6].DbType = System.Data.DbType.String;

            commandParameters[7] = new SQLiteParameter("@is_favorite", recipe.IsFavorite);
            commandParameters[7].DbType = System.Data.DbType.Int64;

            commandParameters[8] = new SQLiteParameter("@ratings", recipe.Ratings);
            commandParameters[8].DbType = System.Data.DbType.Int64;

            SQLiteHelper.ExecuteNonQuery(@"Data Source=" + Helper.SqliteDbPath + ";Version=3;Pooling=False;Max Pool Size=100;"
                                    , CommandType.Text
                                   , @"INSERT INTO recipes (_id, name, updated_at, created_at, category_id, recipe_detail, image_path,is_favorite, ratings) VALUES (@id, @name, @updated_at, @created_at, @category_id, @recipe_detail, @image_path, @is_favorite, @ratings);"
                                   , commandParameters);

        }

        /// <summary>
        /// /
        /// </summary>
        private static void TraceRecipeDetailInSqlite()
        {
            SQLiteDataReader sqlDataReader =
                SQLiteHelper.ExecuteReader(
                    @"Data Source=" + Helper.SqliteDbPath + ";Version=3;Pooling=False;Max Pool Size=100;"
                    , CommandType.Text, "SELECT recipe_detail FROM recipes");
            while (sqlDataReader.Read())
            {
                System.Diagnostics.Trace.WriteLine(sqlDataReader["recipe_detail"].ToString());
            }
        }
    }
}
