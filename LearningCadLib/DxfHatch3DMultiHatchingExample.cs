using System.Reflection;
using WW.Cad.Model.Entities;
using WW.Math;

namespace LearningCadLib
{
    internal class DxfHatch3DMultiHatchingExample : DxfBase
    {
        public DxfHatch3DMultiHatchingExample() : base(MethodBase.GetCurrentMethod()?.DeclaringType?.Name
            .Replace("Dxf", ""))
        {
        }

        internal override string Draw()
        {
            // A triangle in the plane x = 3.
            AddHatch(EntityColors.Red, 
                new Point3D(3, 0, -3), new Point3D(3, -3, 3), new Point3D(3, 3, 3));
            // A triangle in the plane y = 3.
            AddHatch(EntityColors.Green,
                new Point3D(-3, 3, 3), new Point3D(0, 3, -3), new Point3D(3, 3, 3));
            // A rectangle in the plane z = 3.
            AddHatch(EntityColors.Blue,
                new Point3D(-3, -3, 3), new Point3D(3, -3, 3), new Point3D(3, 3, 3),
                new Point3D(-3, 3, 3));

            return SaveFile();
        }

        private void AddHatch(EntityColor color, params Point3D[] points)
        {
            // Find the z-axis:
            Vector3D zAxis = Vector3D.CrossProduct(points[1] - points[0], points[2] - points[0]).GetUnit();
            // The elevation is the projection of any of the points on the z-axis (as long as they are in the same plane).
            double elevation = Vector3D.DotProduct(zAxis, points[0] - Point3D.Zero);

            DxfHatch hatch = new DxfHatch { ZAxis = zAxis, ElevationPoint = new Point3D(0, 0, elevation), Color = color };
            // Get the object coordinate system to world coordinate system transform
            Matrix4D ocsToWcs = hatch.Transform;
            Matrix4D wcsToOcs = ocsToWcs.GetInverse();
            DxfHatch.BoundaryPath boundaryPath = new DxfHatch.BoundaryPath(BoundaryPathType.Polyline)
            {
                PolylineData = new DxfHatch.BoundaryPath.Polyline { Closed = true }
            };
            foreach (Point3D point in points)
            {
                boundaryPath.PolylineData.Vertices.Add(
                    new DxfHatch.BoundaryPath.Polyline.Vertex(wcsToOcs.TransformTo2D(point)));
            }
            hatch.BoundaryPaths.Add(boundaryPath);
            
            Model.Entities.Add(hatch);
        }
    }
}
