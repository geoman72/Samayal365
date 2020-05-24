using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImportTamilRecipesIntoRealmDb.RelmnDb;
using NLog;
using Realms;

namespace ImportTamilRecipesIntoRealmDb
{
    public class ImportHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="realmDbName"></param>
        /// <param name="categoryList"></param>
        /// <param name="recipeList"></param>
        public void ImportIntoRealmDb(String realmDbName, List<Category> categoryList, List<Recipe> recipeList)
        {
            Helper helper = new Helper();

            String realmDbPath = System.String.Format(@"{0}\{1}",
                System.Configuration.ConfigurationManager.AppSettings["RelmnDbOutputPath"].ToString(), realmDbName);

            RealmConfiguration config = new RealmConfiguration(realmDbPath);
            Realm recipesRealm = Realm.GetInstance(config);

           // List<Category> categoryList = helper.GetBriyaniCategoriesFromSqlServer();
            foreach (Category category in categoryList)
            {
                Program.Logger.Info(category.Name);

                recipesRealm.Write(() => { recipesRealm.Add(category); });
            }

            System.Diagnostics.Trace.WriteLine("Successfully imported categories.");
            Program.Logger.Info("Successfully imported categories.");


           // List<Recipe> recipeList = helper.GetBriyaniRecipesFromSqlServer();
            foreach (Recipe recipe in recipeList)
            {
                Program.Logger.Info(recipe.Name);

                recipesRealm.Write(() => { recipesRealm.Add(recipe); });
            }

            System.Diagnostics.Trace.WriteLine("Successfully imported recipes.");
            Program.Logger.Info("Successfully imported recipes.");

            foreach (RecipeConfig recipeConfig in helper.GetRecipeConfigList())
            {
                recipesRealm.Write(() => { recipesRealm.Add(recipeConfig); });
            }

            System.Diagnostics.Trace.WriteLine("Successfully imported recipe config.");
            Program.Logger.Info("Successfully imported recipe config.");
        }

    }
}
