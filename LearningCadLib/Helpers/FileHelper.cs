using System.IO;

namespace LearningCadLib.Helpers
{
    internal class FileHelper
    {
        /// <summary>
        /// Clear lines with certain text in a file read as text.
        /// </summary>
        /// <param name="text">Text contained in the lines to delete.</param>
        /// <param name="fullFilename">File name to process.</param>
        internal static void ClearLinesWith(string text, string fullFilename)
        {
            // Remove certain text
            string[] readText = File.ReadAllLines(fullFilename);
            File.WriteAllText(fullFilename, string.Empty);
            using (StreamWriter writer = new StreamWriter(fullFilename))
            {
                foreach (string s in readText) writer.WriteLine(s.Contains(text) ? "" : s);
            }
        }

    }
}
