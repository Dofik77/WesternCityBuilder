/*
 * Last Update: 04.28.2021
 * Class for generate a list of enumerations
*/

using System;
using System.Collections.Generic;
using System.IO;

namespace JokerGhost
{
    public static class GenerateEnum
    {
        /// <summary>
        /// Deletes the file along the way if there is one 
        /// and creates a new one with the passed parameters
        /// </summary>
        /// <param name="pathEnum"> File path where the file will be created </param>
        /// <param name="listNamesEnum"> List of names that will be in enum </param>
        /// <param name="nameEnum"> The name that will be used to refer to enum </param>
        public static string Create(string pathEnum, List<string> listNamesEnum, string nameEnum)
        {
            // Check for the existence of the file
            if (File.Exists(pathEnum))
            {
                File.Delete(pathEnum);
            }

            var contents = "public enum " + nameEnum + " { ";

            listNamesEnum.ForEach(name =>
            {
                contents += name + ", ";
            });

            contents += '}';

            try
            {
                File.WriteAllText(pathEnum, contents);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return null;
        }
    }
}
