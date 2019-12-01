using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeToSqlImporter
{
    public static class Helper
    {
        /// <summary>
        /// 
        /// </summary>
        public static String SqlConnectionString
        {
            get { return System.Configuration.ConfigurationManager.ConnectionStrings["samayal365Str"].ToString(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static String MetaDataFeedFilePath
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["metaDataFeedFilePath"].ToString(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static String SqliteDbPath
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["sqliteDbPath"].ToString(); }
        }
    }
}