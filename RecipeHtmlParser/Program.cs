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
using Newtonsoft.Json;
using NLog;
using RecipeHtmlParser.Entity;

namespace RecipeHtmlParser
{
    class Program
    {
        // http://getschema.org/index.php?title=Recipe
        static void Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();

            

            List<RecipeMetaData> recipeMetaDataList = new List<RecipeMetaData>
                {
                    //new RecipeMetaData {Name = "கேக்", PageCount = 17, FolderName = @"D:\2018\Source\Repos\Samayal365\RecipeHtmlParser\data\2019\cake"},
                    //new RecipeMetaData {Name = "ஐஸ்கிரீம்", PageCount = 3, FolderName = @"D:\2018\Source\Repos\Samayal365\RecipeHtmlParser\data\2019\icecream"},
                    //new RecipeMetaData {Name = "ஜாம்", PageCount = 2, FolderName = @"D:\2018\Source\Repos\Samayal365\RecipeHtmlParser\data\2019\jam"},
                   // new RecipeMetaData {Name = "பிஸ்கட்", PageCount = 6, FolderName = @"D:\2018\Source\Repos\Samayal365\RecipeHtmlParser\data\2019\biscuit"},
                   // new RecipeMetaData {Name = "பானம்", PageCount = 35, FolderName = @"D:\2018\Source\Repos\Samayal365\RecipeHtmlParser\data\2019\beverages"},
                    //new RecipeMetaData {Name = "பச்சடி", PageCount = 16, FolderName = @"D:\2018\Source\Repos\Samayal365\RecipeHtmlParser\data\2019\paachadi"},
                   // new RecipeMetaData {Name = "வற்றல்", PageCount = 4, FolderName = @"D:\2018\Source\Repos\Samayal365\RecipeHtmlParser\data\2019\vattal"},
                   // new RecipeMetaData {Name = "பொடி", PageCount = 14, FolderName = @"D:\2018\Source\Repos\Samayal365\RecipeHtmlParser\data\2019\podi"},
                   // new RecipeMetaData {Name = "கூட்டு", PageCount = 25, FolderName = @"D:\2018\Source\Repos\Samayal365\RecipeHtmlParser\data\2019\kootu"},
                   // new RecipeMetaData {Name = "சாலட்", PageCount = 12, FolderName = @"D:\2018\Source\Repos\Samayal365\RecipeHtmlParser\data\2019\salad"},
                   // new RecipeMetaData {Name = "ஊறுகாய்", PageCount = 12, FolderName = @"D:\2018\Source\Repos\Samayal365\RecipeHtmlParser\data\2019\pickla"},
                   // new RecipeMetaData {Name = "பொரியல்", PageCount = 37, FolderName = @"D:\2018\Source\Repos\Samayal365\RecipeHtmlParser\data\2019\porial"},
                   // new RecipeMetaData {Name = "வறுவல்", PageCount = 39, FolderName = @"D:\2018\Source\Repos\Samayal365\RecipeHtmlParser\data\2019\fry\veg"},
                   // new RecipeMetaData {Name = "சாதம்", PageCount = 58, FolderName = @"D:\2018\Source\Repos\Samayal365\RecipeHtmlParser\data\2019\rice"},
                   // new RecipeMetaData {Name = "கீரை", PageCount = 21, FolderName = @"D:\2018\Source\Repos\Samayal365\RecipeHtmlParser\data\2019\keerai"},
                   // new RecipeMetaData {Name = "அரேபியா", PageCount = 21, FolderName = @"D:\2018\Source\Repos\Samayal365\RecipeHtmlParser\data\2019\country\Arabia"},
                   // new RecipeMetaData {Name = "இத்தாலி", PageCount = 4, FolderName = @"D:\2018\Source\Repos\Samayal365\RecipeHtmlParser\data\2019\country\Italy"},
                   // new RecipeMetaData {Name = "இலங்கை", PageCount = 27, FolderName = @"D:\2018\Source\Repos\Samayal365\RecipeHtmlParser\data\2019\country\Srilanka"},
                   // new RecipeMetaData {Name = "ப்ரான்ஸ்", PageCount = 4, FolderName = @"D:\2018\Source\Repos\Samayal365\RecipeHtmlParser\data\2019\country\france"},
                   // new RecipeMetaData {Name = "சீனா", PageCount = 5, FolderName = @"D:\2018\Source\Repos\Samayal365\RecipeHtmlParser\data\2019\country\china"},
                   // new RecipeMetaData {Name = "தாய்லாந்து", PageCount = 2, FolderName = @"D:\2018\Source\Repos\Samayal365\RecipeHtmlParser\data\2019\country\thailand"},
                   // new RecipeMetaData {Name = "மெக்சிகோ", PageCount = 2, FolderName = @"D:\2018\Source\Repos\Samayal365\RecipeHtmlParser\data\2019\country\Mexico"},
                   // new RecipeMetaData {Name = "மற்றநாடுகள்", PageCount = 43, FolderName = @"D:\2018\Source\Repos\Samayal365\RecipeHtmlParser\data\2019\country\others"},
                };

            foreach (RecipeMetaData recipeMetaData in recipeMetaDataList)
            {
                Dictionary<string, string> recipeNameList = new Dictionary<string, string>();
                List<string> manualLinks = new List<string>();
                Dictionary<string, string> uniqueNames = new Dictionary<string, string>();
                Dictionary<string, string> uniqueLinks = new Dictionary<string, string>();
                Dictionary<string, RecipeJson> recipeJsonList = new Dictionary<string, RecipeJson>();
                Dictionary<string, RecipeJsonEx> recipeJsonExList = new Dictionary<string, RecipeJsonEx>();

                for (int i = 0; i <= recipeMetaData.PageCount; i++)
                {
                    logger.Info("-----------------------------------------------------------------------------------------------");
                     String recipesPageUrl = String.Format(@"http://www.arusuvai.com/tamil/recipes/{0}/?page={1}", recipeMetaData.Name, i);
                   
                    logger.Info(recipesPageUrl);
                    logger.Info("-----------------------------------------------------------------------------------------------");

                    HtmlWeb hw = new HtmlWeb();
                    HtmlDocument doc = hw.Load(recipesPageUrl);
                    foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[starts-with(@href, '/tamil/node/')]"))
                    {
                        if (String.IsNullOrWhiteSpace(link.InnerText) == false)
                        {
                            WebClient myRecipesWebClient = new WebClient();
                            logger.Info(System.String.Format("{0} =>  http://www.arusuvai.com{1}", link.InnerText, link.Attributes.FirstOrDefault().Value));
                            byte[] myRecipesDataBuffer = myRecipesWebClient.DownloadData(System.String.Format("http://www.arusuvai.com{0}", link.Attributes.FirstOrDefault().Value));
                            HtmlAgilityPack.HtmlDocument recipesdoc = new HtmlAgilityPack.HtmlDocument();
                            recipesdoc.LoadHtml(Encoding.UTF8.GetString(myRecipesDataBuffer));
                            if (recipesdoc.DocumentNode.SelectNodes(@"/html/head/script[@type='application/ld+json']") != null)
                            {
                                String JsonString = recipesdoc.DocumentNode.SelectNodes(@"/html/head/script[@type='application/ld+json']").First().InnerText;
                                JsonString = JsonString.Replace("@context", "context").Replace("@graph", "graph").Replace("@type", "type");
                                try
                                {
                                    RecipeJson recipeJson = JsonConvert.DeserializeObject<RecipeJson>(JsonString);
                                    if (recipeJson != null)
                                    {
                                        String name = recipeJson.graph.FirstOrDefault().name.Replace(@"/", "-").Trim();
                                        int index = 1;
                                        while (uniqueNames.ContainsKey(name) == true)
                                        {
                                            // Get Unique recipe name
                                            name = string.Format("{0} ({1})", name, index);
                                            index = index + 1;
                                        }

                                        recipeNameList.Add(name, name);
                                        recipeJson.graph.FirstOrDefault().name = name;
                                        recipeJsonList.Add(name, recipeJson);
                                        uniqueNames.Add(name, link.Attributes.FirstOrDefault().Value);
                                        uniqueLinks.Add(link.Attributes.FirstOrDefault().Value, name);
                                        String filePath = System.String.Format(@"{0}\{1}.html",
                                            recipeMetaData.FolderName, name);
                                        try
                                        {
                                            System.IO.File.WriteAllText(filePath, recipeJson.RecipeString,
                                                Encoding.UTF8);
                                        }
                                        catch (System.Xml.XmlException e)
                                        {
                                            Console.WriteLine(e);
                                        }
                                        catch (Newtonsoft.Json.JsonSerializationException ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine(e);
                                        }
                                    }
                                }
                                catch (Newtonsoft.Json.JsonReaderException ex)
                                {
                                    try
                                    {
                                        RecipeJsonEx recipeJsonEx =
                                            JsonConvert.DeserializeObject<RecipeJsonEx>(JsonString);
                                        if (recipeJsonEx != null)
                                        {
                                            String name = recipeJsonEx.graph.FirstOrDefault().name.Replace(@"/", "-")
                                                .Trim();
                                            int index = 1;
                                            while (uniqueNames.ContainsKey(name) == true)
                                            {
                                                // Get Unique recipe name
                                                name = string.Format("{0} ({1})", name, index);
                                                index = index + 1;
                                            }

                                            recipeNameList.Add(name, name);
                                            recipeJsonEx.graph.FirstOrDefault().name = name;
                                            recipeJsonExList.Add(name, recipeJsonEx);
                                            uniqueNames.Add(name, link.Attributes.FirstOrDefault().Value);
                                            uniqueLinks.Add(link.Attributes.FirstOrDefault().Value, name);
                                            String filePath1 = System.String.Format(@"{0}\{1}.html",
                                                recipeMetaData.FolderName, name);
                                            System.IO.File.WriteAllText(filePath1, recipeJsonEx.RecipeString,
                                                Encoding.UTF8);
                                        }
                                    }
                                    catch (System.Xml.XmlException e)
                                    {
                                        Console.WriteLine(e);

                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e);
                                    }
                                }
                                catch (Newtonsoft.Json.JsonSerializationException ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }

                            }
                            else
                            {
                                String name = String.Empty;
                                ;
                                try
                                {
                                  
                                    WebClient myRecipesWebClient1 = new WebClient();
                                    byte[] myRecipesDataBuffer1 = myRecipesWebClient1.DownloadData(System.String.Format("http://www.arusuvai.com{0}", link.Attributes.FirstOrDefault().Value));
                                    HtmlAgilityPack.HtmlDocument recipesdoc1 = new HtmlAgilityPack.HtmlDocument();
                                    recipesdoc1.LoadHtml(Encoding.UTF8.GetString(myRecipesDataBuffer1));
                                    String[] ingridientList = recipesdoc1.DocumentNode.SelectNodes(@"/html/body/div[3]/div/section/div[1]/section/article/div[3]/div/div").First().InnerText.Split(new char[] { '\n' });
                                    String[] instructionList = recipesdoc1.DocumentNode.SelectNodes(@"/html/body/div[3]/div/section/div[1]/section/article/div[4]/div/div").First().InnerText.Split(new char[] { '\n' });
                                    String[] noteList = recipesdoc1.DocumentNode.SelectNodes(@"/html/body/div[3]/div/section/div[1]/section/article/div[5]/div/div").First().InnerText.Split(new char[] { '\n' });

                                     name = recipesdoc1.DocumentNode.SelectNodes(@"/html/body/div[3]/div/section/h1").First().InnerText.Trim().Replace(@"/", "-").Trim();
                                    int index = 1;
                                    while (uniqueNames.ContainsKey(name) == true)
                                    {
                                        // Get Unique recipe name
                                        name = string.Format("{0} ({1})", name, index);
                                        index = index + 1;
                                    }
                                    uniqueNames.Add(name, link.Attributes.FirstOrDefault().Value);
                                    uniqueLinks.Add(link.Attributes.FirstOrDefault().Value, name);
                                    StringBuilder retValue = new StringBuilder();

                                    // Name
                                    StringBuilder sbname = new StringBuilder(string.Empty);
                                    sbname.AppendFormat("<p>{0}</p>", name);

                                    // Ingredients
                                    StringBuilder ingredients = new StringBuilder(string.Empty);
                                    foreach (string ingredient in ingridientList)
                                    {
                                        if (String.IsNullOrWhiteSpace(ingredient) == false)
                                        {
                                            ingredients.AppendFormat("<p>{0}</p>", ingredient.Trim());
                                        }

                                    }

                                    // instructions
                                    StringBuilder instructions = new StringBuilder(string.Empty);

                                    foreach (string instruction in instructionList)
                                    {
                                        if (String.IsNullOrWhiteSpace(instruction) == false)
                                        {
                                            instructions.AppendFormat("<p>{0}</p>", instruction.Trim());
                                        }
                                    }


                                    StringBuilder notes = new StringBuilder(string.Empty);

                                    foreach (string note in noteList)
                                    {
                                        if (String.IsNullOrWhiteSpace(note) == false)
                                        {
                                            notes.AppendFormat("<p>{0}</p>", note.Trim());
                                        }
                                    }

                                    StringBuilder recipeBody = new StringBuilder();
                                    recipeBody.Append("<div class=\"recipebody\">");
                                    recipeBody.AppendFormat("<h3>தேவையான பொருட்கள்:</h3>{0}", ingredients.ToString());
                                    recipeBody.AppendFormat("<h3>செய்முறை:</h3>{0}", instructions.ToString());
                                    recipeBody.AppendFormat("<h3>குறிப்புகள்:</h3>{0}", String.IsNullOrWhiteSpace(notes.ToString()) == true ? "<p></p>" : notes.ToString());
                                    recipeBody.Append("</div>");

                                    retValue.AppendFormat(@"<html><body><h1>{0}</h1>{1}</body></html>", sbname.ToString(), recipeBody.ToString());

                                    String filePath3 = System.String.Format(@"{0}\{1}.html", recipeMetaData.FolderName, name);
                                    System.IO.File.WriteAllText(filePath3, retValue.ToString(), Encoding.UTF8);
                                }
                                catch (Newtonsoft.Json.JsonSerializationException ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                catch (Exception e)
                                {
                                    manualLinks.Add(link.InnerText + " " + name);
                                }                                
                            }
                        }
                    }
                }

                String filePath2 = System.String.Format(@"{0}\{1}.txt", recipeMetaData.FolderName, "manual-links");
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath2))
                {
                    // store manual links
                    foreach (String link in manualLinks)
                    {
                        file.WriteLine(link);
                    }
                }

                System.String test = "";
            }
        }
    }
}
