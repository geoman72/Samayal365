using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace RecipeHtmlParser.Entity
{
    public class RecipeDetailValidation
    {
        public RecipeDetailValidation()
        {
            ValidDescription = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public Boolean ValidDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="descriptionXml"></param>
        /// <returns></returns>
        private String GetRecipeDetailDocumentAsString(System.String descriptionXml)
        {
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.LoadXml(descriptionXml);

            System.Text.StringBuilder dtdString = new StringBuilder("<!DOCTYPE div [");
            dtdString.Append("<!ATTLIST div class CDATA #FIXED \"recipebody\">");
            //dtdString.Append("<!ELEMENT div (h3, p*)+>");   
            dtdString.Append("<!ELEMENT div ((h3, p+), (h3, p+), (h3, p*))>");
            dtdString.Append("<!ELEMENT h3 (#PCDATA)>");
            dtdString.Append("<!ELEMENT p (#PCDATA|strong)*>");
            dtdString.Append("<!ELEMENT strong (#PCDATA)>");
            dtdString.Append("]>");
            dtdString.Append(xmlDoc.DocumentElement.OuterXml);

            return dtdString.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void IsValidRecipeDescription(System.String descriptionXml)
        {
            this.ValidDescription = true;

            MemoryStream memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(this.GetRecipeDetailDocumentAsString(descriptionXml)));

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;
            settings.ValidationType = ValidationType.DTD;
            settings.ValidationEventHandler += new ValidationEventHandler(this.ValidationCallBack);

            // Create the XmlReader object.
            XmlReader reader = XmlReader.Create(memoryStream, settings);
            while (reader.Read())  // Parse the file.
            {
            }
            ;
        }

        /// <summary>
        /// Display any validation errors.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ValidationCallBack(object sender, ValidationEventArgs e)
        {
            throw new ApplicationException("Invalid detail " + e.Message);
        }
    }
}
