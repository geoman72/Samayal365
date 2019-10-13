using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeHtmlParser.Entity
{
    public class Graph
    {
        public string type { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public List<Image> image { get; set; }
        public Author author { get; set; }
        public string cookTime { get; set; }
        public string prepTime { get; set; }
        public string totalTime { get; set; }
        public string datePublished { get; set; }
        public string recipeIngredient { get; set; }
        public string recipeCuisine { get; set; }
        public List<string> recipeInstructions { get; set; }
        public string recipeYield { get; set; }
    }
}
