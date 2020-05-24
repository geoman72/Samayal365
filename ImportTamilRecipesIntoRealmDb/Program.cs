using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ImportTamilRecipesIntoRealmDb.RelmnDb;
using Realms;

namespace ImportTamilRecipesIntoRealmDb
{
    public class Program
    {
        public static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            ImportBriyaniRecipes();
            ImportChettinadRecipes();
        }

        /// <summary>
        /// 
        /// </summary>
        private static void ImportBriyaniRecipes()
        {
            Helper helper = new Helper();

            RealmConfiguration config =
                new RealmConfiguration(
                    @"D:\2018\Source\Repos\Samayal365\ImportTamilRecipesIntoRealmDb\RelmnDb\briyanirecipes.realm");
            Realm recipesRealm = Realm.GetInstance(config);

            List<Category> categoryList = helper.GetBriyaniCategoriesFromSqlServer();
            foreach (Category category in categoryList)
            {
                Logger.Info(category.Name);

                recipesRealm.Write(() => { recipesRealm.Add(category); });
            }

            System.Diagnostics.Trace.WriteLine("Successfully imported categories.");
            Logger.Info("Successfully imported categories.");


            List<Recipe> recipeList = helper.GetBriyaniRecipesFromSqlServer();
            foreach (Recipe recipe in recipeList)
            {
                Logger.Info(recipe.Name);

                recipesRealm.Write(() => { recipesRealm.Add(recipe); });
            }

            System.Diagnostics.Trace.WriteLine("Successfully imported recipes.");
            Logger.Info("Successfully imported recipes.");

            foreach (RecipeConfig recipeConfig in helper.GetRecipeConfigList())
            {
                recipesRealm.Write(() => { recipesRealm.Add(recipeConfig); });
            }

            System.Diagnostics.Trace.WriteLine("Successfully imported recipe config.");
            Logger.Info("Successfully imported recipe config.");
        }

        /// <summary>
        /// 
        /// </summary>
        private static void ImportChettinadRecipes()
        {
            Helper helper = new Helper();

            RealmConfiguration config =
                new RealmConfiguration(
                    @"D:\2018\Source\Repos\Samayal365\ImportTamilRecipesIntoRealmDb\RelmnDb\Chettinadrecipes.realm");
            Realm recipesRealm = Realm.GetInstance(config);

            List<Category> categoryList = helper.GetChettinadCategoriesFromSqlServer();
            foreach (Category category in categoryList)
            {
                Logger.Info(category.Name);

                recipesRealm.Write(() => { recipesRealm.Add(category); });
            }

            System.Diagnostics.Trace.WriteLine("Successfully imported categories.");
            Logger.Info("Successfully imported categories.");


            List<Recipe> recipeList = helper.GetChettinadRecipesFromSqlServer();
            foreach (Recipe recipe in recipeList)
            {
                Logger.Info(recipe.Name);

                recipesRealm.Write(() => { recipesRealm.Add(recipe); });
            }

            System.Diagnostics.Trace.WriteLine("Successfully imported recipes.");
            Logger.Info("Successfully imported recipes.");

            foreach (RecipeConfig recipeConfig in helper.GetRecipeConfigList())
            {
                recipesRealm.Write(() => { recipesRealm.Add(recipeConfig); });
            }

            System.Diagnostics.Trace.WriteLine("Successfully imported recipe config.");
            Logger.Info("Successfully imported recipe config.");
        }
    }
}
