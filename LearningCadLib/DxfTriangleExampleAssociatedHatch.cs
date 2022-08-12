using System.Reflection;
using WW.Cad.Model.Entities;
using WW.Math;

namespace LearningCadLib
{
    /// <summary>
    /// This demonstrates a hatch that's associated with a 2D polyline entity (LWPOLYLINE).
    /// </summary>
    internal class DxfTriangleExampleAssociatedHatch : DxfBase
    {
        private readonly bool _filled;

        internal DxfTriangleExampleAssociatedHatch() : base(MethodBase.GetCurrentMethod()?.DeclaringType?.Name.Replace("Dxf", ""))
        {
            _filled = true;
        }

        internal DxfTriangleExampleAssociatedHatch(string drawingName, bool filled) : base(drawingName)
        {
            _filled = filled;
        }

        internal override string Draw()
        {
            Point2D[] points = new[] {
                new Point2D(0, 0),
                new Point2D(5, 5),
                new Point2D(3, -2)
            };

            DxfLwPolyline polygon = new DxfLwPolyline();
            polygon.Vertices.AddRange(points);
            polygon.Closed = true;

            DxfHatch.BoundaryPath boundaryPath = new DxfHatch.BoundaryPath
            {
                Type = BoundaryPathType.External | BoundaryPathType.Polyline | BoundaryPathType.Derived,
                PolylineData = new DxfHatch.BoundaryPath.Polyline(true, points),
                BoundaryObjects = {
                    polygon
                }
            };


            DxfHatch hatch;
            if (_filled)
            {
                hatch = new DxfHatch();
                hatch.Associative = true;
                hatch.BoundaryPaths.Add(boundaryPath);
                hatch.HatchStyle = HatchStyle.Outer;
                // Add random seed point, otherwise AutoCAD does not recognize the polygon association.
                hatch.SeedPoints.Add(new Point2D(-5, -4));

                // This is also part of the association.
                polygon.AddPersistentReactor(hatch);

                Model.Entities.Add(hatch);
            }


            Model.Entities.Add(polygon);

            return SaveFile();
        }
    }
}
