using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Realms;

namespace ImportTamilRecipesIntoRealmDb
{
    class Program
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            RealmConfiguration config = new RealmConfiguration(@"D:\2018\Source\Repos\Samayal365\ImportTamilRecipesIntoRealmDb\RelmnDb\briyanirecipes.realm");
            Realm recipesRealm = Realm.GetInstance(config);

            List<Category> categoryList = SqHelper.GetCategoriesFromSqlServer();
            foreach (Category category in categoryList)
            {
                Logger.Info(category.Name);

                recipesRealm.Write(() =>
                {
                    recipesRealm.Add(category);
                });

            }

            List<Recipe> recipeList = SqHelper.GetRecipesFromSqlServer();

            foreach (Recipe recipe in recipeList)
            {
                Logger.Info(recipe.Name);

                recipesRealm.Write(() =>
                {
                    recipesRealm.Add(recipe);
                }); 
               
            }

            string s = "gest";

        }
    }
}
