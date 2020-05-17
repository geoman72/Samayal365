using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Realms;

namespace ImportTamilRecipesIntoRealmDb
{
    public class Recipe : RealmObject
    {
        /// <summary>
        /// 
        /// </summary>
        [PrimaryKey]
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CreatedOn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UpdatedOn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RecipeDetail { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Ratings { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int IsFavorite { get; set; }
    }
}
