using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Realms;

namespace ImportTamilRecipesIntoRealmDb.RelmnDb
{
    public class RecipeConfig : RealmObject
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [PrimaryKey]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the created on.
        /// </summary>
        /// <value>The created on.</value>
        public String CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the updated on.
        /// </summary>
        /// <value>The updated on.</value>
        public String UpdatedOn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String NextSyncAt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String ClientId { get; set; }
    }
}
