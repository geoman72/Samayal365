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
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [PrimaryKey]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the image path.
        /// </summary>
        /// <value>The image path.</value>
        public String ImagePath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Gets or sets the is favorite.
        /// </summary>
        /// <value>The is favorite.</value>
        public int IsFavorite { get; set; }

        /// <summary>
        /// Gets or sets the ratings.
        /// </summary>
        /// <value>The ratings.</value>
        public int Rating { get; set; }

        /// <summary>
        /// Gets or sets my ratings.
        /// </summary>
        /// <value>My ratings.</value>
        public int MyRating { get; set; }

        /// <summary>
        /// Gets or sets the created at.
        /// </summary>
        /// <value>The created at.</value>
        public String CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the updated at.
        /// </summary>
        /// <value>The updated at.</value>
        public String UpdatedOn { get; set; }

        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        /// <value>The category identifier.</value>
        public int CategoryId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String MyRatingUpdatedAt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SyncMyRating { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int RatingTotal { get; set; }
    }
}
