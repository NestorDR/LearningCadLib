using System.Reflection;
using WW.Cad.Model.Entities;
using WW.Cad.Model.Tables;

namespace LearningCadLib
{
    internal class DxfSquareDashedLineTypeExample : DxfBase
    {
        internal DxfSquareDashedLineTypeExample() : base(
            MethodBase.GetCurrentMethod()?.DeclaringType?.Name.Replace("Dxf", ""))
        {
        }

        internal override string Draw()
        {
            DxfLineType dashedLineType = new DxfLineType("Dashed", 0.5d, -0.3d);
            Model.LineTypes.Add(dashedLineType);

            int sideLength = 100;
            DxfLwPolyline rectangle = new DxfLwPolyline();
            DxfLwPolyline.Vertex[] vertices =
            {
                new DxfLwPolyline.Vertex(x:0, y:0),
                new DxfLwPolyline.Vertex(x:sideLength, y:0),
                new DxfLwPolyline.Vertex(x:sideLength, y:sideLength),
                new DxfLwPolyline.Vertex(x:0, y:sideLength)
            };
            rectangle.Vertices.AddRange(vertices);
            rectangle.Closed = true;
            rectangle.LineType = Model.LineTypes["Dashed"];
            
            Model.Entities.Add(rectangle);

            return SaveFile();
        }
    }
}