using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Extensions;

namespace LearningCadLib
{
    internal class Program
    {
        private static void Main()
        {
            try
            {
                List<string> filenameList = new List<string>
                {
                    new DxfTriangleExample().Draw(),
                    new DxfSquareDashedLineTypeExample().Draw(),
                    new DxfLineTypeExample().Draw(),

                    // Visit https://sunglass.io/autocad-hatch-patterns-what-you-need-to-know/
                    // This patterns range from grass to bricks to any imaginable design and are stored as .pat files.
                    // CAD hatch patterns are a set of graphic patterns that are used to hatch a surface in a drawing.
                    new DxfHatchExample().Draw(),
                    new DxfTriangleExampleAssociatedHatch().Draw(),
                    new DxfHatchExampleWithSplineEdge().Draw(),
                    new DxfHatch3DMultiHatchingExample().Draw(),

                    // 3D drawing 
                    new DxfCubeExample().Draw(),

                    // Profile prototype seed
                    new DxfProfileExample().Draw(),
                };

                if (filenameList.Count == 1)
                    OpenFile(filenameList[0]);
                else
                    ShowResultingFiles(filenameList);

            }
            catch (WW.InternalException exception)
            {
                Console.WriteLine($"WW.CadLib - {exception.Message} - Press [ Enter ] to exit...");
                Console.ReadLine();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                Console.ReadLine();
            }
        }

        /// <summary>
        /// Shows the list of generated drawings
        /// </summary>
        /// <param name="filenameList"></param>
        private static void ShowResultingFiles(List<string> filenameList)
        {
            foreach (string filename in filenameList)
            {
                int index = filenameList.FindIndex(str => str.Contains(filename));
                Console.WriteLine($"{index + 1} - {Path.GetFileName(filename)}");
            }

            int filesCount = filenameList.Count;
            while (true)
            {
                // Clear line of previous writes/reads
                Console.SetCursorPosition(0, filesCount + 1);
                Console.Write(new string(' ', 80));
                Console.SetCursorPosition(0, filesCount + 1);
                Console.Write("Select a number to display that drawing or press [Enter] to finish: ");

                string userInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(userInput)) break;

                try
                {
                    int index = userInput.ToInt(throwExceptionIfFailed: true);
                    if (index > filenameList.Count) continue;

                    var filename = filenameList[--index];
                    
                    OpenFile(filename);
                }
                catch (FormatException)
                {
                    // Just continue
                }
                catch (Exception)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Opens the drawing file
        /// </summary>
        /// <param name="filename"></param>
        private static void OpenFile(string filename)
        {
            try
            {
                // Previously with ExpandEnvironmentVariables
                // string targetApp = Environment.ExpandEnvironmentVariables("%ProgramW6432%\\CAD Assistant\\CADAssistant.exe");
                
                string targetApp = Properties.Settings.Default.DefaultCADViewer;
                if (!string.IsNullOrWhiteSpace(targetApp))
                    Process.Start(targetApp, filename);

                // targetApp = Properties.Settings.Default.AlternativeCADViewer;
                // if (!string.IsNullOrWhiteSpace(targetApp))
                //    Process.Start(targetApp, filename);

                // Alternately open with de associate application
                // Process.Start(@"cmd.exe", "/C start " + filename);
            }
            catch (Win32Exception)
            {
                // Can not open the file, go to folder
                Process.Start("explorer.exe", Path.GetDirectoryName(filename));
            }
        }
    }
}
