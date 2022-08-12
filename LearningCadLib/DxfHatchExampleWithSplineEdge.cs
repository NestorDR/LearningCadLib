using System.Reflection;
using WW.Cad.Base;
using WW.Cad.Model.Entities;
using WW.Math;

namespace LearningCadLib
{
    internal class DxfHatchExampleWithSplineEdge : DxfBase
    {
        public DxfHatchExampleWithSplineEdge() : base(MethodBase.GetCurrentMethod()?.DeclaringType?.Name.Replace("Dxf", ""))
        {
        }

        internal override string Draw()
        {
            DxfHatch hatch = new DxfHatch();

            DxfHatch.BoundaryPath boundaryPath = new DxfHatch.BoundaryPath();
            DxfHatch.BoundaryPath.SplineEdge edge1 = new DxfHatch.BoundaryPath.SplineEdge
            {
                Degree = 3
            };
            edge1.ControlPoints.Add(new Point2D(-1, 0));
            edge1.ControlPoints.Add(new Point2D(-1, 2));
            edge1.ControlPoints.Add(new Point2D(1, 2));
            edge1.ControlPoints.Add(new Point2D(1, 0));
            edge1.Knots.AddRange(BSplineD.CreateDefaultKnotValues(edge1.Degree, 4, false));
            boundaryPath.Edges.Add(edge1);

            DxfHatch.BoundaryPath.SplineEdge edge2 = new DxfHatch.BoundaryPath.SplineEdge
            {
                Degree = 3
            };
            edge2.ControlPoints.Add(new Point2D(1, 0));
            edge2.ControlPoints.Add(new Point2D(1, -2));
            edge2.ControlPoints.Add(new Point2D(-1, -2));
            edge2.ControlPoints.Add(new Point2D(-1, 0));
            edge2.Knots.AddRange(BSplineD.CreateDefaultKnotValues(edge2.Degree, 4, false));
            boundaryPath.Edges.Add(edge2);

            hatch.BoundaryPaths.Add(boundaryPath);
            Model.Entities.Add(hatch);

            return SaveFile();
        }
    }
}
