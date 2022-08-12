using System.Collections.Generic;
using System.Reflection;
using WW.Cad.Model.Entities;

namespace LearningCadLib
{
    internal class DxfCubeExample : DxfBase
    {
        /// <summary>
        /// Creates and writes a cube based on a <see cref="DxfPolyfaceMesh"/> object.
        /// </summary>
        public DxfCubeExample() : base(MethodBase.GetCurrentMethod()?.DeclaringType?.Name.Replace("Dxf", ""))
        {
        }

        internal override string Draw()
        {
            // Create a cube by a poly face mesh.
            DxfPolyfaceMesh polyline4 = new DxfPolyfaceMesh();
            // Cube corner vertices.
            polyline4.Vertices.AddRange(
                new[] {
                    new DxfVertex3D(2, 0, 0),
                    new DxfVertex3D(2, 1, 0),
                    new DxfVertex3D(3, 1, 0),
                    new DxfVertex3D(3, 0, 0),
                    new DxfVertex3D(2, 0, 1),
                    new DxfVertex3D(2, 1, 1),
                    new DxfVertex3D(3, 1, 1),
                    new DxfVertex3D(3, 0, 1)
                }
            );
            IList<DxfVertex3D> v = polyline4.Vertices;
            polyline4.Faces.AddRange(
                new[] {
                    new DxfMeshFace(v[0], v[1], v[2], v[3]),
                    new DxfMeshFace(v[4], v[5], v[6], v[7]),
                    new DxfMeshFace(v[0], v[1], v[5], v[4]),
                    new DxfMeshFace(v[1], v[2], v[6], v[5]),
                    new DxfMeshFace(v[2], v[3], v[7], v[6]),
                    new DxfMeshFace(v[3], v[0], v[4], v[7])
                }
            );
            polyline4.Faces[0].Color = EntityColors.Blue;
            polyline4.Faces[1].Color = EntityColors.Blue;
            polyline4.Faces[2].Color = EntityColors.Red;
            polyline4.Faces[3].Color = EntityColors.Red;
            polyline4.Faces[4].Color = EntityColors.Green;
            polyline4.Faces[5].Color = EntityColors.Green;
            
            Model.Entities.Add(polyline4);

            return SaveFile();

        }
    }
}
