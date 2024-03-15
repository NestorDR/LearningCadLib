using System.IO;
using System.Reflection;
using WW.Cad.Drawing;
using WW.Cad.IO;
using WW.Cad.Model;
using WW.Cad.Model.Entities;
using WW.Cad.Model.Tables;
using WW.Math;

namespace LearningCadLib
{
    internal class CloneContextExample : DxfBase
    {
        public CloneContextExample() : base(
            MethodBase.GetCurrentMethod()?.DeclaringType?.Name.Replace("Dxf", ""))
        {
        }
        
        internal override string Draw() {
            #region 1º - Create source model
             
            DxfModel sourceModel = new DxfModel();
            DxfLineType lineType = new DxfLineType("DASH_DOT", 1d, -0.2d, 0d, -0.2d);
            sourceModel.LineTypes.Add(lineType);

            Point2D circleCenter = new Point2D(10d, 10d);
            DxfCircle circle = new DxfCircle(circleCenter, 5d)
            {
                LineType = lineType
            };
            sourceModel.Entities.Add(circle);

            DxfLine line = new DxfLine(circleCenter, new Point2D(circleCenter.X + circle.Radius, circleCenter.Y ))
            {
                LineType = lineType
            };
            sourceModel.Entities.Add(line);

            #endregion
            
            #region 2º - Clone to a new model

            DxfModel targetModel = new DxfModel();

            // The ReferenceResolutionType.CloneMissing will result in the DASH_DOT line type created
            // above to also be cloned indirectly as a result of cloning the entities.
            CloneContext cloneContext = new CloneContext(sourceModel, targetModel, ReferenceResolutionType.CloneMissing);

            foreach (DxfEntity entity in sourceModel.Entities) {
                DxfEntity clonedEntity = (DxfEntity)entity.Clone(cloneContext);
                targetModel.Entities.Add(clonedEntity);
            }

            cloneContext.ResolveReferences();

            string filename = $"{DrawingName}Target";
            SaveFile(filename, targetModel);
            
            #endregion
            
            #region 3º - Clone to a block in a new model
            
            filename = $"{DrawingName}Source";
            filename = SaveFile(filename, sourceModel);
            
            // Clone as a block
            const string BLOCK_NAME = "MyBlock";
            if (!Model.Blocks.TryGetValue(BLOCK_NAME, out var dxfBlock))
            {
                
                dxfBlock = new DxfBlock(BLOCK_NAME); 
                
                if (File.Exists(filename))
                {
                    sourceModel = CadReader.Read(filename);

                    // Get bounds
                    BoundsCalculator boundsCalculator = new BoundsCalculator();
                    boundsCalculator.GetBounds(sourceModel);
                    Bounds3D bounds = boundsCalculator.Bounds;
                    
                    Matrix4D centerTransform = Transformation4D.Translation(Point3D.Zero - bounds.Center);
                    TransformConfig transformConfig = new TransformConfig();
                    
                    cloneContext = new CloneContext(sourceModel, Model, ReferenceResolutionType.CloneMissing);

                    foreach (DxfEntity entity in sourceModel.Entities)
                    {
                        entity.TransformMe(transformConfig, centerTransform);
                        DxfEntity clonedEntity = (DxfEntity)entity.Clone(cloneContext);
                        dxfBlock.Entities.Add(clonedEntity);
                    }

                    cloneContext.ResolveReferences();
                }

                Model.Blocks.Add(dxfBlock);
            }

            // Add drawing as a block insert
            DxfInsert insert = new DxfInsert(dxfBlock, new Point3D(5d, 0d, 0d));
            Model.Entities.Add(insert);
            
            // Add circle at coordinate origin
            Model.Entities.Add(new DxfCircle(Point2D.Zero, 10d));

            #endregion

            return SaveFile();
        }
    }
}
