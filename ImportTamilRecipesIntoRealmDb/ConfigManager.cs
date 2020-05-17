using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportTamilRecipesIntoRealmDb
{
    public class ConfigManager
    {
        /// <summary>
        /// 
        /// </summary>
        public static String RelmnDbOutputPath
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["RelmnDbOutputPath"].ToString(); }
        }
    }
}
