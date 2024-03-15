using System.IO;
using System.Reflection;
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

            DxfCircle circle = new DxfCircle(Point2D.Zero, 5d)
            {
                LineType = lineType
            };
            sourceModel.Entities.Add(circle);

            DxfLine line = new DxfLine(Point2D.Zero, new Point2D(5d, 0d))
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
                dxfBlock.Entities.Add(new DxfCircle(new Point2D(10d, 0), 10d));
                
                if (File.Exists(filename))
                {
                    sourceModel = CadReader.Read(filename);

                    cloneContext = new CloneContext(sourceModel, Model, ReferenceResolutionType.CloneMissing);

                    foreach (DxfEntity entity in sourceModel.Entities)
                    {
                        DxfEntity clonedEntity = (DxfEntity)entity.Clone(cloneContext);
                        dxfBlock.Entities.Add(clonedEntity);
                    }

                    cloneContext.ResolveReferences();
                }

                Model.Blocks.Add(dxfBlock);
            }

            // Add drawing as a block insert
            DxfInsert insert = new DxfInsert(dxfBlock, Point3D.Zero);
            Model.Entities.Add(insert);

            #endregion

            return SaveFile();
        }
    }
}
