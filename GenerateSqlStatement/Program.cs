using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace GenerateSqlStatement
{
    class Program
    {
        static void Main(string[] args)
        {
            HtmlAgilityPack.HtmlDocument recipesdoc = new HtmlAgilityPack.HtmlDocument();
            recipesdoc.Load(System.Configuration.ConfigurationManager.AppSettings["FeedFileName"].ToString());
            System.Diagnostics.Trace.WriteLine("DELETE FROM recipes");
		    System.Diagnostics.Trace.WriteLine("DELETE FROM recipe_categories");		
		    System.Diagnostics.Trace.WriteLine("DBCC CHECKIDENT ('[recipe_categories]', RESEED, 0);");
            System.Diagnostics.Trace.WriteLine("DBCC CHECKIDENT ('[recipes]', RESEED, 0);");
            System.Diagnostics.Trace.WriteLine("SET IDENTITY_INSERT dbo.recipe_categories ON");
            foreach (HtmlNode htmlNode in recipesdoc.DocumentNode.SelectNodes(@"//feed"))
            {
                System.Diagnostics.Trace.WriteLine(System.String.Format(@"INSERT INTO [dbo].[recipe_categories] ([id], [name] ,[image_path]) VALUES ({0},N'{1}', '{2}.png')",
                htmlNode.SelectSingleNode(@"identity").InnerText,
                htmlNode.SelectSingleNode(@"categoryname").InnerText,
                htmlNode.SelectSingleNode(@"identity").InnerText));
            }
        }
    }
}
