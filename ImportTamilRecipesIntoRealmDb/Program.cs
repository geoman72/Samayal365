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

            //RealmConfiguration config = new RealmConfiguration(@"D:\2018\Source\Repos\Prayers\SqliteDb\josephprayers.realm");
            //Realm prayerRecipesRealm = Realm.GetInstance(config);
            //prayerRecipesRealm.Write(() =>
            //{
            //    prayerRecipesRealm.Add(prayer);
            //});

            List<Category> categoryList = SqHelper.GetCategoriesFromSqlServer();

            List<Recipe> recipeList = SqHelper.GetRecipesFromSqlServer();

            string s;

        }
    }
}
