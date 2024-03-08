using System.Diagnostics;
using System.IO;
using System.Reflection;
using WW.Cad.IO;
using WW.Cad.Model;
using Extensions;
using LearningCadLib.Helpers;

namespace LearningCadLib
{
    public enum FileType
    {
        Dxf, 
        Dwg
    }

    internal abstract class DxfBase
    {
        protected readonly DxfModel Model;
        protected string DrawingName;

        /// <summary>
        /// Base class for the different drawings.
        /// </summary>
        /// <param name="drawingName">Drawing name, used to save a file.</param>
        protected DxfBase(string drawingName)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(drawingName), $"The {nameof(drawingName).ToSentence()} must not be null or empty.");
            DrawingName = drawingName;
            Model = new DxfModel(DxfVersion.Dxf24);         // Dxf24: DXF revision 24.1.01 (AutoCAD 2010, AC1024)
                                                            // Visit: https://www.woutware.com/doc/cadlib4.0/api/WW.Cad.Model.DxfVersion.html
            Model.Entities.Clear();
        }

        /// <summary>
        /// Draw the required object(s)
        /// </summary>
        /// <returns>Full path of the file with the drawing.</returns>
        internal abstract string Draw();

        /// <summary>
        /// Saves a file base and return its full path.
        /// </summary>
        /// <returns>Full path of the file saved.</returns>
        protected string SaveFile(FileType fileType = FileType.Dxf)
        {
            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string dxfPath = Path.Combine((assemblyPath ?? string.Empty).Replace("\\bin\\Debug", ""), "DxfFiles");

            string fullFilename = Path.Combine(dxfPath, DrawingName);
            fullFilename = Path.ChangeExtension(fullFilename, fileType == FileType.Dxf ? ".dxf" : ".dwg");

            DirectoryInfo dxfDirectory = new DirectoryInfo(Path.GetDirectoryName(fullFilename) ?? string.Empty);
            if (!dxfDirectory.Exists) dxfDirectory.Create();

            if (File.Exists(fullFilename)) File.Delete(fullFilename);
            
            switch (fileType)
            {
                case FileType.Dxf:
                    DxfWriter.Write(fullFilename, Model);
                    FileHelper.ClearLinesWith(typeof(Program).Assembly.GetName().Name.Remove(0, 8), fullFilename);
                    break;
                case FileType.Dwg:
                    DwgWriter.Write(fullFilename, Model);
                    break;
            }

            return fullFilename;
        }

    }

}
