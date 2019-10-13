using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeHtmlParser.Entity
{
    public class RecipeJsonEx
    {
        public string context { get; set; }
        public List<GraphEx> graph { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RecipeString
        {
            get
            {

                StringBuilder retValue = new StringBuilder();

                // Name
                StringBuilder name = new StringBuilder(string.Empty);
                name.AppendFormat("<p>{0}</p>", this.graph.FirstOrDefault().name.Trim());

                // Ingredients
                StringBuilder ingredients = new StringBuilder(string.Empty);
                foreach (string ingredient in this.graph.FirstOrDefault().recipeIngredient)
                {
                    ingredients.AppendFormat("<p>{0}</p>", ingredient.Trim());
                }

                // instructions
                StringBuilder instructions = new StringBuilder(string.Empty);

                foreach (string instruction in this.graph.FirstOrDefault().recipeInstructions)
                {
                    instructions.AppendFormat("<p>{0}</p>", instruction.Trim());
                }

                StringBuilder recipeBody = new StringBuilder();
                recipeBody.Append("<div class=\"recipebody\">");
                recipeBody.AppendFormat("<h3>தேவையான பொருட்கள்:</h3>{0}", ingredients.ToString());
                recipeBody.AppendFormat("<h3>செய்முறை:</h3>{0}", instructions.ToString());
                recipeBody.AppendFormat("<h3>குறிப்புகள்:</h3>{0}", "<p></p>");
                recipeBody.Append("</div>");

                retValue.AppendFormat(@"<html><body><h1>{0}</h1>{1}</body></html>", name.ToString(), recipeBody.ToString());

               // RecipeDetailValidation recipeValidation = new RecipeDetailValidation();
                //recipeValidation.IsValidRecipeDescription(recipeBody.ToString());

                return retValue.ToString();
                
            }
        }
    }
}
