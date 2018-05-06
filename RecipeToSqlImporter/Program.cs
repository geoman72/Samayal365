using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.ApplicationBlocks.Data;

namespace RecipeToSqlImporter
{
    class Program
    {
        static void Main(string[] args)
        {

            string sqliteDbPath = System.Configuration.ConfigurationManager.AppSettings["sqliteDbPath"].ToString();
            string metaDataFeedFilePath = System.Configuration.ConfigurationManager.AppSettings["metaDataFeedFilePath"].ToString();

            Program.DeleteRecipes();
            Program.DeleteSqliteRecipes(sqliteDbPath);

            recipe_categories recipeCategory = null;
            System.Xml.XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(metaDataFeedFilePath);
            foreach (XmlNode feedsXmlNode in xmlDocument.SelectNodes(@"//feeds/feed"))
            {
                int seedIdentiy = int.Parse(feedsXmlNode.SelectSingleNode(@"identity").InnerText);
                string categoryName = feedsXmlNode.SelectSingleNode(@"categoryname").InnerText;
                string inputfolderpath = feedsXmlNode.SelectSingleNode(@"inputfolderpath").InnerText;

                recipeCategory = Program.GetRecipeCategory(categoryName);
                Program.AddOrUpdateRecipe(inputfolderpath, recipeCategory, seedIdentiy - 1);
            }

            Program.PublishRecipeCategoriesToSqlite(sqliteDbPath);
            Program.PublishRecipesToSqlite(sqliteDbPath);
            Program.TraceRecipeDetailInSqlite(sqliteDbPath);
        }

        /// <summary>
        ///  delete all the recipes imported to the sql database from file
        /// </summary>
        private static void DeleteRecipes()
        {
            using (var db = new RecipeModelContext())
            {
                var query = from c in db.recipes
                            select c;
                foreach (var row in query)
                {
                    db.recipes.Remove(row);
                }

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static void DeleteSqliteRecipes(string filePath)
        {
            SQLiteHelper.ExecuteNonQuery(@"Data Source=" + filePath + ";Version=3;Pooling=False;Max Pool Size=100;"
                                         , CommandType.Text
                                        , @"delete from recipe_categories"
                                        );

            SQLiteHelper.ExecuteNonQuery(@"Data Source=" + filePath + ";Version=3;Pooling=False;Max Pool Size=100;"
                                         , CommandType.Text
                                        , @"delete from recipes"
                                        );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        private static void TraceRecipeDetailInSqlite(string filePath)
        {
            SQLiteDataReader sqlDataReader =
                    SQLiteHelper.ExecuteReader(
                        @"Data Source=" + filePath + ";Version=3;Pooling=False;Max Pool Size=100;"
                        , CommandType.Text, "SELECT recipe_detail FROM recipes");
            while (sqlDataReader.Read())
            {
                System.Diagnostics.Trace.WriteLine(sqlDataReader["recipe_detail"].ToString());
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        private static void PublishRecipeCategoriesToSqlite(string filePath)
        {
            using (var context = new RecipeModelContext())
            {               
                var categories = from recipeCategory in context.recipe_categories
                                 select recipeCategory;

                foreach (var category in categories)
                {
                    Program.AddSqliteCategory(filePath, category);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        private static void PublishRecipesToSqlite(string filePath)
        {
            using (var context = new RecipeModelContext())
            {
                var recipes = from recipe in context.recipes
                              select recipe;

                foreach (var recipe in recipes)
                {
                    Program.AddSqliteRecipe(filePath, recipe);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="recipeCategory"></param>
        private static void AddSqliteCategory(string filePath, recipe_categories recipeCategory)
        {
            SQLiteParameter[] commandParameters = new SQLiteParameter[6];
            commandParameters[0] = new SQLiteParameter("@id", recipeCategory.id);
            commandParameters[0].DbType = System.Data.DbType.Int64;

            commandParameters[1] = new SQLiteParameter("@name", recipeCategory.name);
            commandParameters[1].DbType = System.Data.DbType.String;

            commandParameters[2] = new SQLiteParameter("@updated_at", recipeCategory.updated_at);
            commandParameters[2].DbType = System.Data.DbType.DateTime;

            commandParameters[3] = new SQLiteParameter("@created_at", recipeCategory.created_at);
            commandParameters[3].DbType = System.Data.DbType.DateTime;

            commandParameters[4] = new SQLiteParameter("@count", recipeCategory.count);
            commandParameters[4].DbType = System.Data.DbType.Int64;

            commandParameters[5] = new SQLiteParameter("@image_path", recipeCategory.image_path);
            commandParameters[5].DbType = System.Data.DbType.String;

            SQLiteHelper.ExecuteNonQuery(@"Data Source=" + filePath + ";Version=3;Pooling=False;Max Pool Size=100;"
                                     , CommandType.Text
                                    , @"INSERT INTO recipe_categories (id, name, updated_at, created_at, count, image_path) VALUES (@id, @name, @updated_at, @created_at, @count, @image_path );"
                                    , commandParameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="recipe"></param>
        private static void AddSqliteRecipe(string filePath, recipe recipe)
        {
            SQLiteParameter[] commandParameters = new SQLiteParameter[9];

            commandParameters[0] = new SQLiteParameter("@id", recipe.id);
            commandParameters[0].DbType = System.Data.DbType.Int64;

            commandParameters[1] = new SQLiteParameter("@name", recipe.name);
            commandParameters[1].DbType = System.Data.DbType.String;

            commandParameters[2] = new SQLiteParameter("@updated_at", recipe.updated_at);
            commandParameters[2].DbType = System.Data.DbType.DateTime;

            commandParameters[3] = new SQLiteParameter("@created_at", recipe.created_at);
            commandParameters[3].DbType = System.Data.DbType.DateTime;

            commandParameters[4] = new SQLiteParameter("@category_id", recipe.category_id);
            commandParameters[4].DbType = System.Data.DbType.Int64;

            commandParameters[5] = new SQLiteParameter("@recipe_detail", recipe.recipe_detail);
            commandParameters[5].DbType = System.Data.DbType.String;

            commandParameters[6] = new SQLiteParameter("@image_path", recipe.image_path);
            commandParameters[6].DbType = System.Data.DbType.String;

            commandParameters[7] = new SQLiteParameter("@is_favorite", recipe.is_favorite);
            commandParameters[7].DbType = System.Data.DbType.Int64;

            commandParameters[8] = new SQLiteParameter("@ratings", recipe.ratings);
            commandParameters[8].DbType = System.Data.DbType.Int64;

            SQLiteHelper.ExecuteNonQuery(@"Data Source=" + filePath + ";Version=3;Pooling=False;Max Pool Size=100;"
                                    , CommandType.Text
                                   , @"INSERT INTO recipes (id, name, updated_at, created_at, category_id, recipe_detail, image_path,is_favorite, ratings) VALUES (@id, @name, @updated_at, @created_at, @category_id, @recipe_detail, @image_path, @is_favorite, @ratings);"
                                   , commandParameters);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputFilePath"></param>
        /// <param name="recipeCategory"></param>
        /// <param name="reseedIdentity"></param>
        private static void AddOrUpdateRecipe(string inputFilePath, recipe_categories recipeCategory, int reseedIdentity)
        {
            using (var db = new RecipeModelContext())
            {
                db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ('[recipes]', RESEED, {0});", reseedIdentity));
                foreach (string fileName in System.IO.Directory.GetFiles(inputFilePath).OrderBy(x => x))
                {
                   // System.Diagnostics.Trace.WriteLine(fileName);

                    System.Xml.XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(fileName);

                    string recipeName = xmlDocument.SelectNodes(@"//body/h1")[0].InnerText;

                    Boolean paresRating =
                        Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings["parseRating"].ToString());
                    string[] values = fileName.Split('\\');

                    string iosRecipe = xmlDocument.SelectNodes(@"//body/div[@class='recipebody']")[0].OuterXml;
                    string androidRecipe = iosRecipe.Replace("<li>", "<p>").Replace("</li>", "</p>").Replace("<ol>", "").Replace("</ol>", "");
                    iosRecipe = iosRecipe.Replace("<li>", "<p>").Replace("</li>", "</p>").Replace("<ol>", "").Replace("</ol>", "");

                    //System.Diagnostics.Trace.WriteLine(iosRecipe);
                    //System.Diagnostics.Trace.WriteLine(iosRecipe.Replace("<li>", "<p>").Replace("</li>", "</p>").Replace("<ol>","").Replace("</ol>",""));
                    try
                    {
                        var updateRecipe = db.recipes.FirstOrDefault(b => b.name == recipeName);
                        if (updateRecipe == null)
                        {
                            var recipe = new recipe
                            {
                                name = recipeName,
                                recipe_detail = iosRecipe,
                                category_id = recipeCategory.id,
                                image_path = recipeCategory.image_path,
                                created_at = DateTime.UtcNow,
                                updated_at = DateTime.UtcNow,
                                ratings = (paresRating == true) ? int.Parse(values[values.Length-1].Substring(0,1)): 0
                            };
                            // add new recipe
                            db.recipes.Add(recipe);
                        }
                        else
                        {
                            updateRecipe.recipe_detail = iosRecipe;
                            updateRecipe.updated_at = DateTime.UtcNow;
                        }
                        // update existing
                        db.SaveChanges();
                    }
                    catch (SystemException ex)
                    {
                        System.Diagnostics.Trace.WriteLine(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        private static recipe_categories GetRecipeCategory(string categoryName)
        {
            recipe_categories recipeCategory;
            // check to see if recipe category exist and get it.
            using (var db = new RecipeModelContext())
            {
                var query = from c in db.recipe_categories
                            where c.name == categoryName
                            select c;
                recipeCategory = query.FirstOrDefault();
            }
            return recipeCategory;
        }
    }
}
