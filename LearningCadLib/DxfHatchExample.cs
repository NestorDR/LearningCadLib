using System.Reflection;
using WW.Cad.Model.Entities;
using WW.Math;

namespace LearningCadLib
{
    internal class DxfHatchExample : DxfBase
    {
        internal DxfHatchExample() : base(MethodBase.GetCurrentMethod()?.DeclaringType?.Name.Replace("Dxf", ""))
        {
        }

        internal override string Draw()
        {
            Model.Entities.Add(CreateHatch());

            return SaveFile();
        }

        internal DxfHatch CreateHatch()
        {
            DxfHatch hatch = new DxfHatch();
            hatch.Color = EntityColors.Green;
            hatch.ElevationPoint = new Point3D(0, 0, 2);
            hatch.ZAxis = new Vector3D(-0.707, 0, 0.707);

            // A boundary path bounded by lines.
            DxfHatch.BoundaryPath boundaryPath1 = new DxfHatch.BoundaryPath();
            boundaryPath1.Type = BoundaryPathType.None;
            hatch.BoundaryPaths.Add(boundaryPath1);
            boundaryPath1.Edges.Add(new DxfHatch.BoundaryPath.LineEdge(new Point2D(0, 0), new Point2D(1, 0)));
            boundaryPath1.Edges.Add(new DxfHatch.BoundaryPath.LineEdge(new Point2D(1, 0), new Point2D(1, 1)));
            boundaryPath1.Edges.Add(new DxfHatch.BoundaryPath.LineEdge(new Point2D(1, 1), new Point2D(0, 1)));
            boundaryPath1.Edges.Add(new DxfHatch.BoundaryPath.LineEdge(new Point2D(0, 1), new Point2D(0, 0)));

            // A boundary path bounded by an ellipse and a line.
            DxfHatch.BoundaryPath boundaryPath2 = new DxfHatch.BoundaryPath();
            boundaryPath2.Type = BoundaryPathType.None;
            hatch.BoundaryPaths.Add(boundaryPath2);
            DxfHatch.BoundaryPath.EllipseEdge edge = new DxfHatch.BoundaryPath.EllipseEdge();
            edge.CounterClockWise = true;
            edge.Center = new Point2D(1, 1);
            edge.MajorAxisEndPoint = new Vector2D(0.4d, -0.2d);
            edge.MinorToMajorRatio = 0.7;
            edge.StartAngle = 0d;
            edge.EndAngle = System.Math.PI * 2d / 3d;
            boundaryPath2.Edges.Add(edge);
            // Close the boundary path.
            boundaryPath2.Edges.Add(new DxfHatch.BoundaryPath.LineEdge(edge.EndPoint, edge.StartPoint));

            // A boundary path bounded by lines and an arc.
            DxfHatch.BoundaryPath boundaryPath3 = new DxfHatch.BoundaryPath();
            boundaryPath3.Type = BoundaryPathType.Outermost;
            hatch.BoundaryPaths.Add(boundaryPath3);
            DxfHatch.BoundaryPath.ArcEdge arcEdge = new DxfHatch.BoundaryPath.ArcEdge();
            arcEdge.Center = new Point2D(0, 1);
            arcEdge.Radius = 0.5d;
            arcEdge.StartAngle = 0;
            arcEdge.EndAngle = System.Math.PI / 2d;
            arcEdge.CounterClockWise = true;
            boundaryPath3.Edges.Add(arcEdge);
            boundaryPath3.Edges.Add(new DxfHatch.BoundaryPath.LineEdge(new Point2D(0, 1.5d), new Point2D(-0.5, 1d)));
            boundaryPath3.Edges.Add(new DxfHatch.BoundaryPath.LineEdge(new Point2D(-0.5, 1d), new Point2D(0d, 0.5d)));
            boundaryPath3.Edges.Add(new DxfHatch.BoundaryPath.LineEdge(new Point2D(0d, 0.5d), new Point2D(0.5d, 1d)));

            // A boundary path bounded by a polyline.
            DxfHatch.BoundaryPath boundaryPath6 = new DxfHatch.BoundaryPath();
            boundaryPath6.Type = BoundaryPathType.Polyline;
            hatch.BoundaryPaths.Add(boundaryPath6);
            boundaryPath6.PolylineData =
                new DxfHatch.BoundaryPath.Polyline(
                    new DxfHatch.BoundaryPath.Polyline.Vertex[] {
                    new DxfHatch.BoundaryPath.Polyline.Vertex(0.5, -0.5),
                    new DxfHatch.BoundaryPath.Polyline.Vertex(0.5, 0.5),
                    new DxfHatch.BoundaryPath.Polyline.Vertex(1.5, 0.5),
                    new DxfHatch.BoundaryPath.Polyline.Vertex(1.5, 0-.25),
                    new DxfHatch.BoundaryPath.Polyline.Vertex(0.75, -0.25),
                    new DxfHatch.BoundaryPath.Polyline.Vertex(0.75, 0.25),
                    new DxfHatch.BoundaryPath.Polyline.Vertex(1.25, 0.25),
                    new DxfHatch.BoundaryPath.Polyline.Vertex(1.25, -0.5)
                    }
                );
            boundaryPath6.PolylineData.Closed = true;

            return hatch;

            // Define the hatch fill pattern. 
            // Don't set a pattern for solid fill.
            hatch.Pattern = new DxfPattern();
            DxfPattern.Line patternLine = new DxfPattern.Line();
            hatch.Pattern.Lines.Add(patternLine);
            patternLine.Angle = System.Math.PI / 4d;
            patternLine.Offset = new Vector2D(0.02, -0.01d);
            patternLine.DashLengths.Add(0.02d);
            patternLine.DashLengths.Add(-0.01d);
            patternLine.DashLengths.Add(0d);
            patternLine.DashLengths.Add(-0.01d);

            return hatch;
        }
    }
}
