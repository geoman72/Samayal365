using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GernerateFeedXml
{
    class Program
    {
        static void Main(string[] args)
        {
            int index = 1000;
            string directoryPath        = System.Configuration.ConfigurationManager.AppSettings["RecipeFolder"].ToString();
            string inputFolderPath      = System.Configuration.ConfigurationManager.AppSettings["InputRootFolder"].ToString();
            StringBuilder stringBuilder = new StringBuilder("<?xml version='1.0' encoding='utf-8' ?><feeds>");
                        
            foreach (string folderPath in System.IO.Directory.GetDirectories(directoryPath))
            {
                String[] values = folderPath.Split('\\');
                stringBuilder.Append(
                    String.Format(
                        @"<feed><identity>{0}</identity><categoryname>{1}</categoryname><inputfolderpath>{2}</inputfolderpath></feed>",
                        index, values[values.Length - 1], inputFolderPath + values[values.Length - 1]));

                index = index + 1000;
            }
            stringBuilder.Append("</feeds>");
            System.Diagnostics.Trace.WriteLine(stringBuilder.ToString());
        }
    }
}
