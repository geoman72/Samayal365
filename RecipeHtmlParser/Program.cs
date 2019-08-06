using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace RecipeHtmlParser
{
    class Program
    {
        // http://getschema.org/index.php?title=Recipe
        static void Main(string[] args)
        {
            WebClient myRecipesWebClient = new WebClient();

            Dictionary<string, int> categories = new  Dictionary<string, int>
            {
                {"கேக்", 17}
            };
           
            foreach (string name in categories.Keys)
            {
                for (int i = 0; i <= categories[name]; i++)
                {
                    String recipesUrl = String.Format(@"http://www.arusuvai.com/tamil/recipes/{0}/?page={1}", name, i);
                    System.Diagnostics.Debug.WriteLine(recipesUrl);
                    System.Console.WriteLine(recipesUrl);
                }
            }
            

            //string recipesPageUrl = recipesUrl;
            //String searchTerm     = "பிரியாணி";
            //string downloadFolder = @"briyani\delta";
            //bool ignoreSearchTerm = true;
            //int pages = 411;

            //Dictionary<string,string> recipes  = new Dictionary<string, string>();
            //Dictionary<string, int> hitRate    = new Dictionary<string, int>();

            //for (int i = 0; i < pages; i++)
            //{
            //    //if (i > 0)
            //   // {
            //    //    recipesPageUrl = System.String.Format("{0}?page={1}", recipesUrl, i);
            //    //}

            //    recipesPageUrl = System.String.Format(recipesUrl, i);

            //    System.Diagnostics.Trace.WriteLine(recipesPageUrl);
            //    byte[] myRecipesDataBuffer = myRecipesWebClient.DownloadData(recipesPageUrl);
            //    HtmlAgilityPack.HtmlDocument recipesdoc = new HtmlAgilityPack.HtmlDocument();

            //    recipesdoc.LoadHtml(Encoding.UTF8.GetString(myRecipesDataBuffer));

            //    foreach (HtmlNode htmlNode in recipesdoc.DocumentNode.SelectNodes(@"//div[@class='view-content']/table/tbody/tr/td[@class='views-field views-field-title']/a"))
            //    {
            //        if (ignoreSearchTerm == true)
            //        {
            //            if (htmlNode.ChildNodes[0].InnerHtml.Contains(searchTerm) == true)
            //            {
            //                if (hitRate.ContainsKey(htmlNode.ChildNodes[0].InnerHtml) == false)
            //                {
            //                    hitRate.Add(htmlNode.ChildNodes[0].InnerHtml, 0);
            //                }
            //                else
            //                {
            //                    hitRate[htmlNode.ChildNodes[0].InnerHtml] = hitRate[htmlNode.ChildNodes[0].InnerHtml] + 1;
            //                }

            //                String recipeName;
            //                if (hitRate[htmlNode.ChildNodes[0].InnerHtml] == 0)
            //                {
            //                    recipeName = htmlNode.ChildNodes[0].InnerHtml;
            //                }
            //                else
            //                {
            //                    recipeName = System.String.Format(htmlNode.ChildNodes[0].InnerHtml + " ({0})", hitRate[htmlNode.ChildNodes[0].InnerHtml]);
            //                }

            //                recipes.Add(recipeName, "http://www.arusuvai.com/" + htmlNode.Attributes["href"].Value);

            //                //System.Diagnostics.Trace.WriteLine(recipeName + "    http://http://www.arusuvai.com/" + htmlNode.Attributes["href"].Value + " " + i.ToString());
            //            }
            //        }
            //        else
            //        {
            //            if (hitRate.ContainsKey(htmlNode.ChildNodes[0].InnerHtml) == false)
            //            {
            //                hitRate.Add(htmlNode.ChildNodes[0].InnerHtml, 0);
            //            }
            //            else
            //            {
            //                hitRate[htmlNode.ChildNodes[0].InnerHtml] = hitRate[htmlNode.ChildNodes[0].InnerHtml] + 1;
            //            }

            //            String recipeName;
            //            if (hitRate[htmlNode.ChildNodes[0].InnerHtml] == 0)
            //            {
            //                recipeName = htmlNode.ChildNodes[0].InnerHtml;
            //            }
            //            else
            //            {
            //                recipeName = System.String.Format(htmlNode.ChildNodes[0].InnerHtml + " ({0})", hitRate[htmlNode.ChildNodes[0].InnerHtml]);
            //            }
                      
            //            //if (recipeName == "காய்கறி சாம்பார் (2)") 
            //                //recipeName = recipeName.Replace("(2)", Guid.NewGuid().ToString());

            //            try
            //            {
            //                recipes.Add(recipeName, "http://www.arusuvai.com/" + htmlNode.Attributes["href"].Value);
            //            }
            //            catch (System.ArgumentException ex)
            //            {
            //                if (ex.Message == "An item with the same key has already been added.")
            //                {
            //                    // Increase the count
            //                    hitRate[htmlNode.ChildNodes[0].InnerHtml] = hitRate[htmlNode.ChildNodes[0].InnerHtml] + 1;
            //                    recipeName = System.String.Format(htmlNode.ChildNodes[0].InnerHtml + " ({0})", hitRate[htmlNode.ChildNodes[0].InnerHtml]);
            //                    // Now add the recipe
            //                    recipes.Add(recipeName, "http://www.arusuvai.com/" + htmlNode.Attributes["href"].Value);
            //                }
            //            }
                        

            //            //System.Diagnostics.Trace.WriteLine(recipeName + "    http://http://www.arusuvai.com/" + htmlNode.Attributes["href"].Value + " " + i.ToString());
            //        }                    
            //    }
            //}

            //List<RecipeInfo> allRecipes = new List<RecipeInfo>();
            //foreach (string reciepName in recipes.Keys.OrderBy(x => x))
            //{
                

            //    // System.Diagnostics.Trace.WriteLine(reciepName + "  " + recipes[reciepName]);
            //     //// Create a new WebClient instance.
            //    WebClient myWebClient = new WebClient();
            //    //myWebClient.Proxy = new WebProxy("webproxy.corp.symetra.com", 8080);
            //    //myWebClient.Proxy.Credentials = CredentialCache.DefaultCredentials;
            //    // Download the Web resource and save it into a data buffer. 
            //    byte[] myDataBuffer = myWebClient.DownloadData(recipes[reciepName]);

            //    // Display the downloaded data. 
            //    string download = Encoding.UTF8.GetString(myDataBuffer);
            //    //Console.WriteLine(download);

            //    string template = "<body><h1>{0}</h1><h2>{1}</h2><div class=\"recipebody\"><h3>தேவையான பொருட்கள்:</h3><ol>{2}</ol><h3>செய்முறை:</h3><ol>{3}</ol><h3>குறிப்புகள்:</h3><ol>{4}</ol></div></body>";

            //    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            //    HtmlAgilityPack.HtmlDocument OutDoc = new HtmlAgilityPack.HtmlDocument();

            //    // doc.Load("arusuvai.html");
            //    doc.LoadHtml(download);

            //    string recipeName = doc.DocumentNode.SelectSingleNode(@"//h1").InnerHtml;
            //    string authorName = doc.DocumentNode.SelectSingleNode(@"//div[@class='auth']/a") == null? "": doc.DocumentNode.SelectSingleNode(@"//div[@class='auth']/a").InnerHtml;
            //    string rating = doc.DocumentNode.SelectSingleNode(@"//div[@class='star star-1 star-odd star-first']/span") == null ? "0" : ((int)Decimal.Parse(doc.DocumentNode.SelectSingleNode(@"//div[@class='star star-1 star-odd star-first']/span").InnerHtml)).ToString();

            //    string createdDateStr;

            //    try
            //    {
            //        createdDateStr = doc.DocumentNode
            //            .SelectSingleNode(@"//*[@id='content']/div[2]/div[1]/div[1]/div[2]").InnerHtml;
            //    }
            //    catch (Exception e)
            //    {
            //        createdDateStr = doc.DocumentNode
            //            .SelectSingleNode(@" //*[@id='rtopright']/div[2]").InnerHtml;
            //    }
               

            //    Regex pattern = new Regex(@"\d{2}\/\d{2}\/\d{4}");
            //    Match match = pattern.Match(createdDateStr);
               

            //    DateTime createdDateTime = System.DateTime.ParseExact(match.Value, "dd/MM/yyyy", null);

            //    StringBuilder ingredients = new StringBuilder();
            //    StringBuilder preparations = new StringBuilder();
            //    StringBuilder notes = new StringBuilder();

            //    //if (createdDateTime > DateTime.Parse(@"2014-06-21"))
            //    //{
            //        System.Diagnostics.Trace.WriteLine(reciepName);
            //        System.Diagnostics.Trace.WriteLine(authorName);
            //        System.Diagnostics.Trace.WriteLine(rating);
            //        System.Diagnostics.Trace.WriteLine(createdDateTime.Date.ToString("MM-dd-yyyy"));
            //    //}

            //    foreach (HtmlNode htmlNode in doc.DocumentNode.SelectNodes(@"//div[@id='ingbox']/ul/div[@class='ingre']/li"))
            //    {
            //        ingredients.AppendFormat("<li>{0}</li>", htmlNode.ChildNodes[0].InnerHtml);
            //        //System.Diagnostics.Trace.WriteLine(htmlNode.ChildNodes[0].InnerHtml);
            //    }

            //    if (doc.DocumentNode.SelectNodes(@"//div[@id='right_col']/p") != null)
            //    {
            //        foreach (HtmlNode htmlNode in doc.DocumentNode.SelectNodes(@"//div[@id='right_col']/p"))
            //        {
            //            preparations.AppendFormat("<li>{0}</li>", htmlNode.ChildNodes[0].InnerHtml);
            //            //System.Diagnostics.Trace.WriteLine(htmlNode.ChildNodes[0].InnerHtml);
            //        }

            //        if (doc.DocumentNode.SelectNodes(@"//div[@class='recinote']/p") != null)
            //        {
            //            foreach (HtmlNode htmlNode in doc.DocumentNode.SelectNodes(@"//div[@class='recinote']/p"))
            //            {
            //                notes.AppendFormat("<li>{0}</li>", htmlNode.ChildNodes[0].InnerHtml);
            //                System.Diagnostics.Trace.WriteLine(htmlNode.InnerHtml);
            //            }
            //        }

            //        if (doc.DocumentNode.SelectNodes(@"//div[@class='recinote']/div") != null)
            //        {
            //            foreach (HtmlNode htmlNode in doc.DocumentNode.SelectNodes(@"//div[@class='recinote']/div"))
            //            {
            //                notes.AppendFormat("<li>{0}</li>", htmlNode.ChildNodes[0].InnerHtml);
            //                System.Diagnostics.Trace.WriteLine(htmlNode.InnerHtml);
            //            }
            //        }

            //    }
            //    else
            //    {
            //        foreach (HtmlNode htmlNode in doc.DocumentNode.SelectNodes(@"//div[@id='procebox']/ul/div[@class='direc']/li"))
            //        {
            //            preparations.AppendFormat("<li>{0}</li>", htmlNode.ChildNodes[0].InnerHtml);
            //            //System.Diagnostics.Trace.WriteLine(htmlNode.ChildNodes[0].InnerHtml);
            //        }

            //        if (doc.DocumentNode.SelectNodes(@"//div[@class='recinote']/p") != null)
            //        {
            //            foreach (HtmlNode htmlNode in doc.DocumentNode.SelectNodes(@"//div[@class='recinote']/p"))
            //            {
            //                notes.AppendFormat("<li>{0}</li>", htmlNode.ChildNodes[0].InnerHtml);
            //                System.Diagnostics.Trace.WriteLine(htmlNode.InnerHtml);
            //            }
            //        }

            //        if (doc.DocumentNode.SelectNodes(@"//div[@class='recinote']/div") != null)
            //        {
            //            foreach (HtmlNode htmlNode in doc.DocumentNode.SelectNodes(@"//div[@class='recinote']/div"))
            //            {
            //                notes.AppendFormat("<li>{0}</li>", htmlNode.ChildNodes[0].InnerHtml);
            //                System.Diagnostics.Trace.WriteLine(htmlNode.InnerHtml);
            //            }
            //        }                        
            //    }

            //    // using name in metadata xml for unique names
            //    String recipeStr = System.String.Format(template, reciepName, authorName, ingredients.ToString()
            //        , preparations.ToString(), notes.ToString());

            //    //recipeStr = recipeStr.Replace("<li>", "<p>").Replace("</li>", "</p>").Replace("<ol>", "").Replace("</ol>", "");

            //    OutDoc.LoadHtml(recipeStr);
            //    //if (createdDateTime > DateTime.Parse(@"2014-06-21"))
            //    //{

            //        OutDoc.Save(ConfigurationManager.AppSettings["folderBasePath"].ToString() + downloadFolder + @"\"
            //                    + createdDateTime.Date.ToString("MM-dd-yyyy") + "-"
            //                    + rating + "-" + reciepName.Replace("/", " (அ) ").Replace(":", "").Replace("?", "") + @".html", Encoding.UTF8);

            //        allRecipes.Add(new RecipeInfo
            //        {
            //            Name = reciepName,
            //            Url = recipes[reciepName],
            //            Rating = rating,
            //            CreatedDate = createdDateTime
            //        });
            //    //}

            //}

            //// csv fiel
            //using (
            //    System.IO.StreamWriter file =
            //        new System.IO.StreamWriter(ConfigurationManager.AppSettings["folderBasePath"].ToString() +
            //                                   downloadFolder + @"\all.csv"))
            //{
            //    file.WriteLine("Name,URL,Rating");
            //    foreach (RecipeInfo recipeInfo in allRecipes.OrderByDescending(x => x.Rating))
            //    {
            //       file.WriteLine(String.Format("{0},{1},{2}", recipeInfo.Name, recipeInfo.Url, recipeInfo.Rating));
            //    }
            //}

            //foreach (string fileName in Directory.GetFiles(ConfigurationManager.AppSettings["folderBasePath"].ToString() + @"metadata"))
            //{
            //    HtmlAgilityPack.HtmlDocument metaDoc = new HtmlAgilityPack.HtmlDocument();
            //    metaDoc.Load(fileName);
            //    foreach (HtmlNode htmlRecipeNode in metaDoc.DocumentNode.SelectNodes(@"//recipes/recipe"))
            //    {
            //        string url = htmlRecipeNode.SelectSingleNode(@"url").InnerHtml;
            //        string downloadFolder = htmlRecipeNode.SelectSingleNode(@"downfolder").InnerHtml;
            //        string name = htmlRecipeNode.SelectSingleNode(@"name").InnerHtml;

            //        // Create a new WebClient instance.
            //        WebClient myWebClient = new WebClient();
            //        //myWebClient.Proxy = new WebProxy("webproxy.corp.symetra.com", 8080);
            //        //myWebClient.Proxy.Credentials = CredentialCache.DefaultCredentials;
            //        // Download the Web resource and save it into a data buffer. 
            //        byte[] myDataBuffer = myWebClient.DownloadData(url);

            //        // Display the downloaded data. 
            //        string download = Encoding.UTF8.GetString(myDataBuffer);
            //        //Console.WriteLine(download);

            //        string template = "<body><h1>{0}</h1><h2>{1}</h2><div class=\"recipebody\"><h3>தேவையான பொருட்கள்:</h3><ol>{2}</ol><h3>செய்முறை:</h3><ol>{3}</ol><h3>குறிப்புகள்:</h3><ol>{4}</ol></div></body>";

            //        HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            //        HtmlAgilityPack.HtmlDocument OutDoc = new HtmlAgilityPack.HtmlDocument();

            //        // doc.Load("arusuvai.html");
            //        doc.LoadHtml(download);

            //        string recipeName = doc.DocumentNode.SelectSingleNode(@"//h1").InnerHtml;
            //        string authorName = doc.DocumentNode.SelectSingleNode(@"//div[@class='auth']/a") == null? "": doc.DocumentNode.SelectSingleNode(@"//div[@class='auth']/a").InnerHtml;
            //        StringBuilder ingredients = new StringBuilder();
            //        StringBuilder preparations = new StringBuilder();
            //        StringBuilder notes = new StringBuilder();

            //        System.Diagnostics.Trace.WriteLine(name);
            //        System.Diagnostics.Trace.WriteLine(authorName);

            //        foreach (HtmlNode htmlNode in doc.DocumentNode.SelectNodes(@"//div[@id='ingbox']/ul/div[@class='ingre']/li"))
            //        {
            //            ingredients.AppendFormat("<li>{0}</li>", htmlNode.ChildNodes[0].InnerHtml);
            //            //System.Diagnostics.Trace.WriteLine(htmlNode.ChildNodes[0].InnerHtml);
            //        }

            //        if (doc.DocumentNode.SelectNodes(@"//div[@id='right_col']/p") != null)
            //        {
            //            foreach (HtmlNode htmlNode in doc.DocumentNode.SelectNodes(@"//div[@id='right_col']/p"))
            //            {
            //                preparations.AppendFormat("<li>{0}</li>", htmlNode.ChildNodes[0].InnerHtml);
            //                //System.Diagnostics.Trace.WriteLine(htmlNode.ChildNodes[0].InnerHtml);
            //            }

            //            if (doc.DocumentNode.SelectNodes(@"//div[@class='recinote']/p") != null)
            //            {
            //                foreach (HtmlNode htmlNode in doc.DocumentNode.SelectNodes(@"//div[@class='recinote']/p"))
            //                {
            //                    notes.AppendFormat("<li>{0}</li>", htmlNode.ChildNodes[0].InnerHtml);
            //                    System.Diagnostics.Trace.WriteLine(htmlNode.InnerHtml);
            //                }
            //            }

            //            if (doc.DocumentNode.SelectNodes(@"//div[@class='recinote']/div") != null)
            //            {
            //                foreach (HtmlNode htmlNode in doc.DocumentNode.SelectNodes(@"//div[@class='recinote']/div"))
            //                {
            //                    notes.AppendFormat("<li>{0}</li>", htmlNode.ChildNodes[0].InnerHtml);
            //                    System.Diagnostics.Trace.WriteLine(htmlNode.InnerHtml);
            //                }
            //            }

            //        }
            //        else
            //        {
            //            foreach (HtmlNode htmlNode in doc.DocumentNode.SelectNodes(@"//div[@id='procebox']/ul/div[@class='direc']/li"))
            //            {
            //                preparations.AppendFormat("<li>{0}</li>", htmlNode.ChildNodes[0].InnerHtml);
            //                //System.Diagnostics.Trace.WriteLine(htmlNode.ChildNodes[0].InnerHtml);
            //            }

            //            if (doc.DocumentNode.SelectNodes(@"//div[@class='recinote']/p") != null)
            //            {
            //                foreach (HtmlNode htmlNode in doc.DocumentNode.SelectNodes(@"//div[@class='recinote']/p"))
            //                {
            //                    notes.AppendFormat("<li>{0}</li>", htmlNode.ChildNodes[0].InnerHtml);
            //                    System.Diagnostics.Trace.WriteLine(htmlNode.InnerHtml);
            //                }
            //            }

            //            if (doc.DocumentNode.SelectNodes(@"//div[@class='recinote']/div") != null)
            //            {
            //                foreach (HtmlNode htmlNode in doc.DocumentNode.SelectNodes(@"//div[@class='recinote']/div"))
            //                {
            //                    notes.AppendFormat("<li>{0}</li>", htmlNode.ChildNodes[0].InnerHtml);
            //                    System.Diagnostics.Trace.WriteLine(htmlNode.InnerHtml);
            //                }
            //            }                        
            //        }

            //        // using name in metadata xml for unique names
            //        OutDoc.LoadHtml(System.String.Format(template, name, authorName, ingredients.ToString(), preparations.ToString(), notes.ToString()));
            //        OutDoc.Save(ConfigurationManager.AppSettings["folderBasePath"].ToString() + downloadFolder + @"\" + name + @".html", Encoding.UTF8);
            //    }                
            // }              
        }
    }
}
