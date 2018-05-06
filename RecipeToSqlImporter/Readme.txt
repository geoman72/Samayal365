How to generate the sqlite database for devices

1. In sql server create or copy existing database.  if you copy then delete all the values in tables and reset the identity to 0 using command 
		DELETE FROM recipes
		DELETE FROM recipe_categories		
		DBCC CHECKIDENT ('[recipe_categories]', RESEED, 0);
		DBCC CHECKIDENT ('[recipes]', RESEED, 0);
2. Run the GenerateSqlStatement exe with the right FeedFileName path is set in app.config to generate the recipe categories sql statment. 
   run the generated sql command in database to populate the categories.
3. Make a copy of sqlite\master.sqlite and rename to your sql database name.
4. Run the RecipeToSqlImporter application with 
      1. Change the Entity framework database connection string point to newly created sql datbase.
	  2. Now configure the app.config point to right feed and sqlite datbase.
	  3. Run the application so the sqlite datbase is populated.