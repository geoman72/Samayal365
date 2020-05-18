﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ImportTamilRecipesIntoRealmDb.RelmnDb;
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

            System.Diagnostics.Trace.WriteLine("Successfully imported categories.");
            Logger.Info("Successfully imported categories.");


            List<Recipe> recipeList = SqHelper.GetRecipesFromSqlServer();
            foreach (Recipe recipe in recipeList)
            {
                Logger.Info(recipe.Name);

                recipesRealm.Write(() =>
                {
                    recipesRealm.Add(recipe);
                }); 
               
            }

            System.Diagnostics.Trace.WriteLine("Successfully imported recipes.");
            Logger.Info("Successfully imported recipes.");

            foreach (RecipeConfig recipeConfig in SqHelper.GetRecipeConfigList())
            {
                recipesRealm.Write(() =>
                {
                    recipesRealm.Add(recipeConfig);
                });
            }

            System.Diagnostics.Trace.WriteLine("Successfully imported recipe config.");
            Logger.Info("Successfully imported recipe config.");


        }
    }
}
