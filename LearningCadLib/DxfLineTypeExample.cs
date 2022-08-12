using System.Reflection;
using WW.Cad.Model.Entities;
using WW.Cad.Model.Tables;
using WW.Math;

namespace LearningCadLib
{
    internal class DxfLineTypeExample : DxfBase
    {
        internal DxfLineTypeExample() : base(
            MethodBase.GetCurrentMethod()?.DeclaringType?.Name.Replace("Dxf", ""))
        {
        }

        internal override string Draw()
        {
            // A dash length of 0.5, followed by a space of 0.3.
            DxfLineType dashedLineType = new DxfLineType("Dashed", 0.5d, -0.3d);
            Model.LineTypes.Add(dashedLineType);

            // A dash length of 0.5, followed by a space of 0.3, 
            // followed by a dot, followed by a space of 0.2.
            DxfLineType dashDotLineType = new DxfLineType("Dash dot", 0.5d, -0.3d, 0d, -0.2d);
            Model.LineTypes.Add(dashDotLineType);

            // A complex line type.
            DxfLineType complexLineType = new DxfLineType("-- D - S");
            {
                DxfLineType.Element element = new DxfLineType.Element
                {
                    Length = 2d
                };
                complexLineType.Elements.Add(element);
            }
            {
                DxfLineType.Element element = new DxfLineType.Element
                {
                    Text = "D",
                    IsText = true,
                    TextStyle = Model.DefaultTextStyle
                };
                complexLineType.Elements.Add(element);
            }
            {
                DxfLineType.Element element = new DxfLineType.Element
                {
                    Length = 1d
                };
                complexLineType.Elements.Add(element);
            }
            {
                DxfLineType.Element element = new DxfLineType.Element
                {
                    Text = "S",
                    IsText = true,
                    TextStyle = Model.DefaultTextStyle
                };
                complexLineType.Elements.Add(element);
            }
            Model.LineTypes.Add(complexLineType);

            DxfCircle circle1 = new DxfCircle(new Point2D(0d, 0d), 10d);
            circle1.LineType = dashedLineType;
            Model.Entities.Add(circle1);

            DxfCircle circle2 = new DxfCircle(new Point2D(15d, 0d), 10d);
            circle2.LineType = dashDotLineType;
            Model.Entities.Add(circle2);

            DxfLine line = new DxfLine();
            line.Start = new Point3D(-10d, -2d, 0d);
            line.End = new Point3D(30d, -2d, 0d);
            line.LineType = complexLineType;
            Model.Entities.Add(line);

            return SaveFile();
        }
    }
}
