using System.Reflection;
using WW.Cad.Model;
using WW.Cad.Model.Entities;
using WW.Cad.Model.Tables;
using WW.Math;

namespace LearningCadLib
{
    internal class DxfDimensionAlignedExample : DxfBase
    {
        private readonly DxfBlock _block;
        
        /// <summary>
        /// Visit https://www.woutware.com/doc/cadlib4.0/api/WW.Cad.Model.Entities.DxfDimension.html
        /// </summary>
        internal DxfDimensionAlignedExample(bool useBlock) : base(
            MethodBase.GetCurrentMethod()?.DeclaringType?.Name.Replace("Dxf", ""))
        {
            _block = useBlock ? new DxfBlock("ALIGNED_DIMENSIONS") : null;
        }

        internal override string Draw()
        {
            DxfLayer layer;
            
            if (_block == null)
            {
                layer = null;
            }
            else
            {
                layer = new DxfLayer("DIMENSIONS");
                Model.Layers.Add(layer);

                Model.Blocks.Add(_block);

                DxfInsert insert = new DxfInsert(_block, new Point3D(0, -35, 0))
                {
                    Layer = layer
                };
                Model.Entities.Add(insert);
            }
            
            {
                if (_block == null)
                {
                    layer = new DxfLayer("DIMENSIONS_WITH_NO_TEXT");
                    Model.Layers.Add(layer);
                }

                // Dimension with no text.
                DxfDimension.Aligned dimension = new DxfDimension.Aligned(Model.CurrentDimensionStyle)
                {
                    Layer = layer,
                    Text = " ",      // This means no text.
                    DimensionLineLocation = new Point3D(2d, 2d, 0d),
                    ExtensionLine1StartPoint = new Point3D(3d, 1d, 0d),
                    ExtensionLine2StartPoint = new Point3D(0d, 0d, 0d),
                };
                dimension.DimensionStyleOverrides.TickSize = 0.5;
                dimension.DimensionStyleOverrides.DimensionLineExtension = 0.3;
                dimension.DimensionStyleOverrides.ExtensionLineOffset = 0.3;
                dimension.DimensionStyleOverrides.DimensionLineColor = Colors.Red;
                dimension.DimensionStyleOverrides.ExtensionLineColor = Colors.Green;
                
                AddDimension(dimension);
            }

            {
                if (_block == null)
                {
                    layer = new DxfLayer("DIMENSIONS_WITH_TEXT");
                    Model.Layers.Add(layer);
                }

                // Dimension with text that doesn't fit between extension lines.
                DxfDimension.Aligned dimension = new DxfDimension.Aligned(Model.CurrentDimensionStyle)
                {
                    Layer = layer,
                    Text = @"This is a long dimension text\PMulti line...",
                    DimensionLineLocation = new Point3D(2d, 4d, 0d),
                    ExtensionLine1StartPoint = new Point3D(3d, 3d, 0d),
                    ExtensionLine2StartPoint = new Point3D(0d, 2d, 0d),
                };
                dimension.DimensionStyleOverrides.ArrowSize = 1d;
                dimension.DimensionStyleOverrides.ExtensionLineExtension = 0.5;
                
                AddDimension(dimension);
            }

            {
                if (_block == null)
                {
                    layer = new DxfLayer("DIMENSIONS_WITH_BIG_ARROWS");
                    Model.Layers.Add(layer);
                }

                // Dimension with big arrows.
                DxfDimension.Aligned dimension = new DxfDimension.Aligned(Model.CurrentDimensionStyle)
                {
                    Layer = layer,
                    DimensionLineLocation = new Point3D(2d, 6d, 0d),
                    ExtensionLine1StartPoint = new Point3D(3d, 5d, 0d),
                    ExtensionLine2StartPoint = new Point3D(0d, 4d, 0d)
                };
                dimension.DimensionStyleOverrides.ArrowSize = 2d;                           // Increase arrows size
                dimension.DimensionStyleOverrides.TextVerticalAlignment = DimensionTextVerticalAlignment.Above;
                dimension.DimensionStyleOverrides.SuppressFirstExtensionLine = true;        // Hide one extension line

                AddDimension(dimension);
            }

            {
                if (_block == null)
                {
                    layer = new DxfLayer("DIMENSIONS_WITH_TEXT_ALIGNED_WITH_DIM_LINE");
                    Model.Layers.Add(layer);
                }

                // Dimension with text aligned with dimension line.
                DxfDimension.Aligned dimension = new DxfDimension.Aligned(Model.CurrentDimensionStyle)
                {
                    Layer = layer,
                    DimensionLineLocation = new Point3D(2d, 8d, 0d),
                    ExtensionLine1StartPoint = new Point3D(3d, 7d, 0d),
                    ExtensionLine2StartPoint = new Point3D(0d, 6d, 0d),
                        UseTextMiddlePoint = true,
                        TextMiddlePoint = new Point3D(3d, 8.5d, 0d)
                };
                dimension.DimensionStyleOverrides.ArrowSize = 1d;
                dimension.DimensionStyleOverrides.TextInsideHorizontal = false;
                dimension.DimensionStyleOverrides.TextVerticalAlignment = DimensionTextVerticalAlignment.Above;
                dimension.DimensionStyleOverrides.SuppressSecondExtensionLine = true;       // Hide one extension line
                dimension.DimensionStyleOverrides.DecimalPlaces = 2;                        // Change the number of decimal places
                
                AddDimension(dimension);
            }
            
            return SaveFile();
        }

        private void AddDimension(DxfDimension.Aligned dimension)
        {
            if (_block == null)
                Model.Entities.Add(dimension);
            else
                _block.Entities.Add(dimension);
        }
    }
}
