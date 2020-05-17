using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Realms;

namespace ImportTamilRecipesIntoRealmDb
{
    public class Category : RealmObject
    {
        /// <summary>
        /// 
        /// </summary>
        [PrimaryKey]
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String CreatedOn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String UpdatedOn { get; set; }

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
        public int Count { get; set; }

    }
}
