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
            ImportHelper importHelper = new ImportHelper();
            Helper helper = new Helper();

            //// Briyani Recipes
            //importHelper.ImportIntoRealmDb("briyanirecipes.realm"
            //    , helper.GetCategoryList("samayal365_briyani_recipe_categories")
            //    , helper.GetRecipesList("samayal365_briyani_recipes"));

            //// Chettinad Recipes
            //importHelper.ImportIntoRealmDb("Chettinadrecipes.realm"
            //    , helper.GetCategoryList("samayal365_chettinadu_recipe_categories")
            //    , helper.GetRecipesList("samayal365_chettinadu_recipes"));

            // tiffin  Recipes
            importHelper.ImportIntoRealmDb("tiffinrecipes.realm"
                , helper.GetCategoryList("samayal365_tiffin_recipe_categories")
                , helper.GetRecipesList("samayal365_tiffin_recipes"));
        }
    }
}
