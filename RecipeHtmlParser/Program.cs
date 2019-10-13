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

            

            List<RecipeMetaData> recipeMetaDataList = new  List<RecipeMetaData>
            {
                new RecipeMetaData {Name = "பானம்", PageCount = 35, FolderName = @"D:\2018\Source\Repos\Samayal365\RecipeHtmlParser\data\2019\beverages"}
            };

            foreach (RecipeMetaData recipeMetaData in recipeMetaDataList)
            {
                Dictionary<string, string> recipeNameList = new Dictionary<string, string>();
                List<string> manualLinks = new List<string>();
                Dictionary<string,string> uniqueLinks = new Dictionary<string, string>();
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
                                        String name = recipeJson.graph.FirstOrDefault().name.Trim();
                                        int index = 1;
                                        while (recipeNameList.ContainsKey(name) == true)
                                        {
                                            // Get Unique recipe name
                                            name = string.Format("{0} ({1})", name, index);
                                            index = index + 1;
                                        }

                                        recipeNameList.Add(name, name);
                                        recipeJson.graph.FirstOrDefault().name = name;
                                        recipeJsonList.Add(name, recipeJson);
                                        uniqueLinks.Add(link.Attributes.FirstOrDefault().Value, link.InnerText);
                                        String filePath = System.String.Format(@"{0}\{1}.html", recipeMetaData.FolderName, link.InnerText.Trim().Replace(@"/", "-"));
                                        try
                                        {
                                            System.IO.File.WriteAllText(filePath, recipeJson.RecipeString, Encoding.UTF8);
                                        }
                                        catch (System.Xml.XmlException e)
                                        {
                                            Console.WriteLine(e);
                                        }
                                    }
                                }
                                catch (Newtonsoft.Json.JsonReaderException ex)
                                {
                                    try
                                    {
                                        RecipeJsonEx recipeJsonEx = JsonConvert.DeserializeObject<RecipeJsonEx>(JsonString);
                                        if (recipeJsonEx != null)
                                        {
                                            String name = recipeJsonEx.graph.FirstOrDefault().name.Trim();
                                            int index = 1;
                                            while (recipeNameList.ContainsKey(name) == true)
                                            {
                                                // Get Unique recipe name
                                                name = string.Format("{0} ({1})", name, index);
                                                index = index + 1;
                                            }
                                            recipeNameList.Add(name, name);
                                            recipeJsonEx.graph.FirstOrDefault().name = name;
                                            recipeJsonExList.Add(name, recipeJsonEx);
                                            uniqueLinks.Add(link.Attributes.FirstOrDefault().Value, link.InnerText);
                                            String filePath1 = System.String.Format(@"{0}\{1}.html", recipeMetaData.FolderName, link.InnerText.Trim().Replace(@"/", "-"));
                                            System.IO.File.WriteAllText(filePath1, recipeJsonEx.RecipeString, Encoding.UTF8);
                                        }
                                    }
                                    catch (System.Xml.XmlException e)
                                    {
                                        Console.WriteLine(e);
                                      
                                    }
                                    
                                }
                              
                            }
                            else
                            {
                                manualLinks.Add(link.InnerText + " " + link.Attributes.FirstOrDefault().Value);
                                uniqueLinks.Add(link.Attributes.FirstOrDefault().Value, link.InnerText);
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
