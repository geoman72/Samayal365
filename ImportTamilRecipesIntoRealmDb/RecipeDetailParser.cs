using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using MetaWeblogApiWebApp.Models;

namespace ImportTamilRecipesIntoRealmDb
{
    public class RecipeDetailParser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strDescription"></param>
        /// <returns></returns>
        public String ParseDetail(String strDescription)
        {
            XmlDocument xmlDocument = this.ParsedXmlDetail(this.ParseDetailDictionary(strDescription));
            TamilRecipeValidationUtility recipeValidation = new TamilRecipeValidationUtility();
            recipeValidation.IsValidRecipeDescription(xmlDocument.OuterXml);
            return xmlDocument.OuterXml;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strDescription"></param>
        /// <returns></returns>
        private Dictionary<string, List<string>> ParseDetailDictionary(String strDescription)
        {
            Dictionary<string, List<string>> retValue = new Dictionary<string, List<string>>();

            if (String.IsNullOrWhiteSpace(strDescription) == false)
            {
                XmlDocument xmlDocument = new XmlDocument();
                Regex regex = new Regex(@"\s+");
                xmlDocument.LoadXml(strDescription.Replace(@"<strong>", "").Replace(@"</strong>", ""));

                String currentHeading = "";
                foreach (XmlNode xmlNode in xmlDocument.DocumentElement.ChildNodes)
                {
                    if (xmlNode.Name.CompareTo("h3") == 0)
                    {
                        currentHeading = xmlNode.InnerText.Trim();
                        retValue.Add(currentHeading, new List<string>());
                    }
                    else
                    {
                        if (xmlNode.Name.CompareTo("p") == 0)
                        {
                            if (xmlNode.LastChild != null && xmlNode.LastChild.Name.CompareTo("p") == 0)
                            {
                                retValue[currentHeading].Add("------------------------------");
                                retValue[currentHeading].Add(xmlNode.FirstChild.InnerText.Trim());
                                retValue[currentHeading].Add("------------------------------");
                                foreach (XmlNode xmlpNode in xmlNode.ChildNodes)
                                {
                                    if (xmlpNode.Name.CompareTo("p") == 0)
                                    {
                                        retValue[currentHeading].Add(xmlpNode.InnerText.Trim());
                                    }
                                }

                                retValue[currentHeading].Add("------------------------------");
                            }
                            else
                            {
                                retValue[currentHeading].Add(xmlNode.InnerText.Trim());
                            }
                        }
                    }
                }

                //for (int section = 1; section <= 3; section++)
                //{
                //    foreach (XmlNode xmlNode in xmlDocument.DocumentElement.SelectSingleNode(System.String.Format(@"/div/p[{0}]", section)))
                //    {
                //        retValue.Add(xmlNode.InnerText.Trim(), new List<string>());

                    //        // /html/body/div/p[1]/p
                    //        foreach (XmlNode xmlOlNode in xmlDocument.DocumentElement.SelectNodes(System.String.Format(@"/div/p[{0}]/p", section)))
                    //        {
                    //            if (xmlOlNode.FirstChild != null)
                    //            {
                    //                if (xmlOlNode.LastChild.Name == "p")
                    //                {
                    //                    retValue[xmlNode.InnerText.Trim()].Add("----------------------------------------------------");
                    //                    retValue[xmlNode.InnerText.Trim()].Add(regex.Replace(xmlOlNode.FirstChild.InnerText.Trim().Replace("\"", "'"), " "));
                    //                    retValue[xmlNode.InnerText.Trim()].Add("----------------------------------------------------");
                    //                    foreach (XmlNode xmlOl1Node in xmlOlNode.LastChild.ChildNodes)
                    //                    {
                    //                        retValue[xmlNode.InnerText.Trim()].Add(regex.Replace(xmlOl1Node.InnerText.Trim().Replace("\"", "'"), " "));
                    //                    }
                    //                    retValue[xmlNode.InnerText.Trim()].Add("----------------------------------------------------");
                    //                }
                    //                else
                    //                {
                    //                    retValue[xmlNode.InnerText.Trim()].Add(regex.Replace(xmlOlNode.InnerText.Trim().Replace("\"", "'"), " "));
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
            }

            return retValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parsedRecipeList"></param>
        /// <returns></returns>
        private void TraceRecipeDetail(Dictionary<string, List<string>> parsedRecipeList)
        {
            if (parsedRecipeList != null)
            {
                foreach (String key in parsedRecipeList.Keys)
                {
                    System.Diagnostics.Debug.WriteLine("-------------------------------------");
                    System.Diagnostics.Debug.WriteLine(key);
                    System.Diagnostics.Debug.WriteLine("-------------------------------------");

                    Program.Logger.Info("-------------------------------------");
                    Program.Logger.Info(key);
                    Program.Logger.Info("-------------------------------------");

                    foreach (String item in parsedRecipeList[key])
                    {
                        System.Diagnostics.Debug.WriteLine(item);
                        Program.Logger.Info(item);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parsedRecipeList"></param>
        /// <returns></returns>
        private XmlDocument ParsedXmlDetail(Dictionary<string, List<string>> parsedRecipeList)
        {
            XmlDocument retValue = new XmlDocument();

            Dictionary<string, string> formattedRecipeItemList = new Dictionary<string, string>();

            if (parsedRecipeList != null)
            {
                foreach (String key in parsedRecipeList.Keys)
                {
                    StringBuilder sb = new StringBuilder(String.Empty);
                    foreach (String item in parsedRecipeList[key])
                    {
                        sb.AppendFormat("<p>{0}</p>", item.Replace("&", "&amp;"));
                    }

                    formattedRecipeItemList.Add(key, sb.ToString());
                }

                String[] keyArray = formattedRecipeItemList.Keys.ToArray();

                retValue.LoadXml(System.String.Format(
                    @"<div><h3>தேவையான பொருட்கள்:</h3>{0}<h3>செய்முறை:</h3>{1}<h3>குறிப்புகள்:</h3>{2}</div>"
                    , formattedRecipeItemList[keyArray[0]]
                    , formattedRecipeItemList[keyArray[1]]
                    , formattedRecipeItemList[keyArray[2]].Trim()));
            }

            return retValue;
        }
    }
}
