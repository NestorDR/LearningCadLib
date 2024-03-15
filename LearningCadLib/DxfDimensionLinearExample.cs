using System;
using System.Reflection;
using WW.Cad.Model;
using WW.Cad.Model.Entities;
using WW.Cad.Model.Tables;
using WW.Math;

namespace LearningCadLib
{
    internal class DxfDimensionLinearExample: DxfBase
    {
        private readonly DxfBlock _block;
        
        /// <summary>
        /// Visit https://www.woutware.com/doc/cadlib4.0/api/WW.Cad.Model.Entities.DxfDimension.html
        /// </summary>
        internal DxfDimensionLinearExample(bool useBlock) : base(
            MethodBase.GetCurrentMethod()?.DeclaringType?.Name.Replace("Dxf", ""))
        {
            _block = useBlock ? new DxfBlock("LINEAR_DIMENSIONS") : null;
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

                DxfInsert insert = new DxfInsert(_block, new Point3D(5, -30, 0))
                {
                    Layer = layer
                };
                Model.Entities.Add(insert);
            }

            {
                if (_block == null)
                {
                    layer = new DxfLayer("DIMENSIONS_HORIZONTAL");
                    Model.Layers.Add(layer);
                }
                
                // Horizontal.
                DxfDimension.Linear dimension = new DxfDimension.Linear(Model.CurrentDimensionStyle)
                {
                    Layer = layer,
                    ExtensionLine1StartPoint = new Point3D(0, 0, 0),
                    ExtensionLine2StartPoint = new Point3D(2, 0, 0),
                    DimensionLineLocation = new Point3D(2, 1, 0)
                };
                AddDimension(dimension);
            }

            {
                if (_block == null)
                {
                    layer = new DxfLayer("DIMENSIONS_ROTATED_WITH_<>");
                    Model.Layers.Add(layer);
                }
                
                // Rotated and usage of "<>" text.
                DxfDimension.Linear dimension = new DxfDimension.Linear(Model.CurrentDimensionStyle)
                {
                    Layer = layer,
                    Rotation = Math.PI / 6d,
                    Text = "<> cm", // <> is replaced by measurement.
                    ExtensionLine1StartPoint = new Point3D(0, 2, 0),
                    ExtensionLine2StartPoint = new Point3D(2, 2, 0),
                    DimensionLineLocation = new Point3D(2, 4, 0)
                };
                
                AddDimension(dimension);
            }

            {
                if (_block == null)
                {
                    layer = new DxfLayer("DIMENSIONS_ROTATED_OTHER_SIDE");
                    Model.Layers.Add(layer);
                }

                // Rotated to the other side with rounded measurement and aligned text.
                DxfDimension.Linear dimension = new DxfDimension.Linear(Model.CurrentDimensionStyle)
                {
                    Layer = layer,
                    Rotation = -Math.PI / 6d,
                    ExtensionLine1StartPoint = new Point3D(3, 4, 0),
                    ExtensionLine2StartPoint = new Point3D(0, 4, 0),
                    DimensionLineLocation = new Point3D(0, 6, 0)
                };
                dimension.DimensionStyleOverrides.TextInsideHorizontal = false;
                dimension.DimensionStyleOverrides.Rounding = 0.25;
                dimension.DimensionStyleOverrides.DimensionLineColor = Colors.Red;
                dimension.DimensionStyleOverrides.ExtensionLineColor = Colors.Green;
                
                AddDimension(dimension);
            }

            {
                if (_block == null)
                {
                    layer = new DxfLayer("DIMENSIONS_ROTATED");
                    Model.Layers.Add(layer);
                }
                
                // Rotated.
                DxfDimension.Linear dimension = new DxfDimension.Linear(Model.CurrentDimensionStyle)
                {
                    Layer = layer,
                    TextRotation = 0,
                    Rotation = Math.PI / 6d,
                    ExtensionLine1StartPoint = new Point3D(2, 6, 0),
                    ExtensionLine2StartPoint = new Point3D(0, 6, 0),
                    DimensionLineLocation = new Point3D(2, 8, 0)
                };
                
                AddDimension(dimension);
            }

            {
                if (_block == null)
                {
                    layer = new DxfLayer("DIMENSIONS_VERTICAL_ROTATION");
                    Model.Layers.Add(layer);
                }
                
                // Vertical.
                DxfDimension.Linear dimension = new DxfDimension.Linear(Model.CurrentDimensionStyle)
                {
                    Layer = layer,
                    TextRotation = Math.PI / 8d,
                    Rotation = Math.PI / 2d,
                    ExtensionLine1StartPoint = new Point3D(0, 10, 0),
                    ExtensionLine2StartPoint = new Point3D(0, 8, 0),
                    DimensionLineLocation = new Point3D(2, 8, 0)
                };
                dimension.DimensionStyleOverrides.TextInsideHorizontal = false;
                
                AddDimension(dimension);
            }

            {
                if (_block == null)
                {
                    layer = new DxfLayer("DIMENSIONS_VERTICAL");
                    Model.Layers.Add(layer);
                }
                
                // Vertical.
                DxfDimension.Linear dimension = new DxfDimension.Linear(Model.CurrentDimensionStyle)
                {
                    Layer = layer,
                    Rotation = Math.PI / 2d,
                    ExtensionLine1StartPoint = new Point3D(0, 12, 0),
                    ExtensionLine2StartPoint = new Point3D(0, 10, 0),
                    DimensionLineLocation = new Point3D(2, 10, 0)
                };
                dimension.DimensionStyleOverrides.TextInsideHorizontal = false;
                
                AddDimension(dimension);
            }

            return SaveFile();
        }

        private void AddDimension(DxfDimension.Linear dimension)
        {
            if (_block == null)
                Model.Entities.Add(dimension);
            else
                _block.Entities.Add(dimension);
        }
    }
}
