/*
 * Last Update: 04.28.2021
 * Generating a template
*/

using System;
using System.IO;

namespace JokerGhost
{
    public static class TemplateGenerator
    {
        /// <summary>
        /// Deletes the file along the way if there is one 
        /// and creates a new one with the passed parameters
        /// </summary>
        /// <param name="pathName"> File path where the file will be created </param>
        /// <param name="nameClass"> The name of the class to be inserted in the file </param>
        /// <param name="pathTemplate"> The path that the template follows </param>
        public static string Create(string pathName, string nameClass, string pathTemplate)
        {
            // Check for the existence of the file
            if (File.Exists(pathName))
            {
                File.Delete(pathName);
            }

            return CreateTemplate(GetTemplateContent(pathTemplate), pathName, nameClass);
        }

        private static string CreateTemplate(string proto, string pathName, string name)
        {
            if (pathName.IsNullOrEmpty())
            {
                return "Invalid filename";
            }

            if (proto.IsNullOrEmpty())
            {
                return "Template is null";
            }

            var lastIndex = pathName.LastIndexOf("/", StringComparison.Ordinal);
            var ns = "DEFAULT";
            if (lastIndex - 7 > 0)
            {
                ns = pathName.Substring(7, lastIndex - 7).Replace("/", ".");
            }

            proto = proto.Replace("#NS#", ns).Replace("#NAME#", name);

            try
            {
                File.WriteAllText(pathName, proto);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return null;
        }

        private static string GetTemplateContent(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch
            {
                return null;
            }
        }
    }
}