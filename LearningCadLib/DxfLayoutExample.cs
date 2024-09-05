using System;
using System.Reflection;
using WW.Cad.Drawing;
using WW.Cad.Model.Entities;
using WW.Cad.Model.Objects;
using WW.Cad.Model.Tables;
using WW.Math;

namespace LearningCadLib
{
    internal class DxfLayoutExample : DxfBase
    {
        /// <summary>
        /// Visit https://www.woutware.com/doc/cadlib4.0/api/WW.Cad.Model.Entities.DxfViewport.html
        /// </summary>
        internal DxfLayoutExample() : base(
            MethodBase.GetCurrentMethod()?.DeclaringType?.Name.Replace("Dxf", ""))
        {
        }

        internal override string Draw()
        {
            const double UNPRINTABLE_MARGIN = 5;
            const double TOTAL_MARGIN = UNPRINTABLE_MARGIN * 2;
            
            Model.Entities.Add(new DxfCircle(EntityColors.Red, new Point3D(0d, 0d, 0d), 2d) { LineWeight = 100 });
            Model.Entities.Add(new DxfLine(EntityColors.GreenYellow, new Point3D(-3d, 4d, 0d),
                new Point3D(5d, 3d, 0d)));
            Model.Entities.Add(new DxfLine(EntityColors.Yellow, new Point3D(-4d, 0d, 0d), new Point3D(5d, 5d, 0d)));

            DxfLayer frozenLayer = new DxfLayer("Frozen");
            Model.Layers.Add(frozenLayer);

            // Layout number 1
            {
                DxfLayout layout1 = Model.Layouts["layout1"];
                layout1.Name = "Layout number 1";
                layout1.Entities.Add(new DxfText("Text on layout 1", new Point3D(2d, 0d, 0d), 5d));
                // Paper size and margins are defined before being rotated according to property PlotRotation.
                layout1.PlotPaperSize = new Size2D(100d, 100d);
                layout1.UnprintableMarginLeft = UNPRINTABLE_MARGIN;
                layout1.UnprintableMarginRight = UNPRINTABLE_MARGIN;
                layout1.UnprintableMarginTop = UNPRINTABLE_MARGIN;
                layout1.UnprintableMarginBottom = UNPRINTABLE_MARGIN;
                layout1.TabOrder = 1;
                layout1.PlotRotation = PlotRotation.None;

                {
                    // This viewport is mandatory, it describes paper space itself.
                    DxfViewport paperSpaceViewport = new DxfViewport
                    {
                        PaperSpace = true,
                        Size = new Size2D(100d, 100d),
                        ViewCenter = new Point2D(0d, 0d),
                        ViewHeight = 100d,
                        Visible = true,
                    };
                    paperSpaceViewport.FrozenLayers.Add(frozenLayer);
                    layout1.Viewports.Add(paperSpaceViewport);

                    // This viewport is a viewport showing a piece of model space in paper space.
                    // The total scale factor will be Size.Y / ViewHeight.
                    DxfViewport modelSpaceViewport = new DxfViewport
                    {
                        ViewHeight = 6d,
                        ViewCenter = new Point2D(0d, 0d),
                        Center = new Point3D(40d, 30d, 0d),
                        Target = new Point3D(0d, 0d, 0d),
                        Size = new Size2D(80d, 60d),
                        PaperSpace = true,
                        Visible = true
                    };
                    layout1.Viewports.Add(modelSpaceViewport);
                }
            }
    
            // Two model Viewports
            {
                // A landscape layout with 2 viewports.
                DxfLayout landScapeLayout = new DxfLayout("Two model Viewports");
                Model.Layouts.Add(landScapeLayout);
                landScapeLayout.Entities.Add(new DxfText("Text on layout 2", new Point3D(2d, 0d, 0d), 10d));
                // Paper size and margins are defined before being rotated according to property PlotRotation.
                landScapeLayout.PlotPaperSize = new Size2D(210, 297); // A4 size in mm.
                landScapeLayout.UnprintableMarginLeft = UNPRINTABLE_MARGIN;
                landScapeLayout.UnprintableMarginRight = UNPRINTABLE_MARGIN;
                landScapeLayout.UnprintableMarginTop = UNPRINTABLE_MARGIN;
                landScapeLayout.UnprintableMarginBottom = UNPRINTABLE_MARGIN;
                landScapeLayout.TabOrder = 2;
                landScapeLayout.PlotRotation = PlotRotation.QuarterCounterClockwise;

                {
                    // This viewport is mandatory, it describes paper space itself.
                    DxfViewport paperSpaceViewport = new DxfViewport
                    {
                        ViewCenter = new Point2D(0d, 0d),
                        Size = landScapeLayout.PlotPaperSize,
                        Visible = true
                    };
                    paperSpaceViewport.ViewHeight = paperSpaceViewport.Size.Y;
                    paperSpaceViewport.PaperSpace = true;
                    paperSpaceViewport.FrozenLayers.Add(frozenLayer);
                    landScapeLayout.Viewports.Add(paperSpaceViewport);

                    // This viewport is a viewport showing a piece of model space in paper space.
                    // The total scale factor will be Size.Y / ViewHeight.
                    DxfViewport modelSpaceViewport1 = new DxfViewport
                    {
                        Center = new Point3D(55d, 90d, 0d),
                        PaperSpace = true,
                        Size = new Size2D(100d, 180d),
                        ViewCenter = new Point2D(0d, 0d),
                        Target = new Point3D(0d, 0d, 0d),
                        ViewHeight = 6d,
                        Visible = true
                    };
                    landScapeLayout.Viewports.Add(modelSpaceViewport1);

                    // This viewport is a viewport showing a piece of model space in paper space.
                    // The total scale factor will be Size.Y / ViewHeight.
                    DxfViewport modelSpaceViewport2 = new DxfViewport
                    {
                        Center = new Point3D(185d, 90d, 0d),
                        PaperSpace = true,
                        Size = new Size2D(120d, 180d),
                        Target = new Point3D(0d, 0d, 0d),
                        ViewCenter = new Point2D(0d, 0d),
                        ViewHeight = 9d,
                        Visible = true
                    };
                    landScapeLayout.Viewports.Add(modelSpaceViewport2);
                }
            }

            // Calculate model size and displacement to center
            BoundsCalculator boundsCalculator = new BoundsCalculator();
            boundsCalculator.GetBounds(Model);
            Bounds3D bounds = boundsCalculator.Bounds;
            double modelWidth = bounds.Delta.X;
            double modelHeight = bounds.Delta.Y;
            double modelDisplacementOnX = (bounds.Max.X + bounds.Min.X) / 2;
            double modelDisplacementOnY = (bounds.Max.Y + bounds.Min.Y) / 2;
            
            // Portrait Layout
            {
                DxfLayout portraitLayout = new DxfLayout("Portrait Layout");
                Model.Layouts.Add(portraitLayout);
                portraitLayout.Entities.Add(new DxfText("A text on layout 3", new Point3D(0d, 4d, 0d), 10d));
                // Paper size and margins are defined before being rotated according to property PlotRotation.
                portraitLayout.PlotPaperSize = new Size2D(210, 297); // A4 size in mm.
                portraitLayout.UnprintableMarginLeft = UNPRINTABLE_MARGIN;
                portraitLayout.UnprintableMarginRight = UNPRINTABLE_MARGIN;
                portraitLayout.UnprintableMarginTop = UNPRINTABLE_MARGIN;
                portraitLayout.UnprintableMarginBottom = UNPRINTABLE_MARGIN;
                portraitLayout.TabOrder = 3;
                portraitLayout.PlotRotation = PlotRotation.None; // Portrait

                Size2D paperSize = portraitLayout.PlotPaperSize;
                {
                    // This viewport is mandatory, it describes paper space itself.
                    DxfViewport paperSpaceViewport = new DxfViewport
                    {
                        PaperSpace = true,
                        Size = paperSize,
                        ViewCenter = new Point2D(0d, 0d),
                        ViewHeight = paperSize.Y,
                        Visible = true,
                    };
                    paperSpaceViewport.FrozenLayers.Add(frozenLayer);
                    portraitLayout.Viewports.Add(paperSpaceViewport);

                    // This viewport is a viewport showing a piece of model space in paper space.
                    Size2D viewportSize = new Size2D(paperSize.X - TOTAL_MARGIN, paperSize.Y - TOTAL_MARGIN);
                    double scaleFactor = Math.Min(viewportSize.X / modelWidth, viewportSize.Y / modelHeight);
                    DxfViewport modelSpaceViewport = new DxfViewport
                    {
                        // Centering point on the paper
                        Center = new Point3D(                   // Originally in WW example new Point3D(90d, 120d, 0d)
                            viewportSize.X / 2,
                            viewportSize.Y / 2, 
                            0d), 
                        PaperSpace = true,
                        Size = viewportSize,                   // Originally in WW example new Size2D(180d, 240d)
                        Target = new Point3D(0d, 0d, 0d),
                        ViewCenter = new Point2D(modelDisplacementOnX, modelDisplacementOnY),
                        // ViewPort.ViewHeight visit: https://www.woutware.com/doc/cadlib4.0/api/WW.Cad.Model.Entities.DxfViewport.html & https://www.woutware.com/Forum/Topic/342
                        ViewHeight = viewportSize.Y / scaleFactor,
                        // Set false to hide borders of the model space viewport
                        Visible = true                     
                    };

                    portraitLayout.Viewports.Add(modelSpaceViewport);
                }
            }

            // Landscape Layout
            {
                DxfLayout landscape = new DxfLayout("Landscape Layout");
                Model.Layouts.Add(landscape);
                landscape.Entities.Add(new DxfText("A text on layout 4", new Point3D(0d, 4d, 0d), 10d));
                // Paper size and margins are defined before being rotated according to property PlotRotation.
                landscape.PlotPaperSize = new Size2D(210, 297);     // A4 size in mm.
                landscape.UnprintableMarginLeft = UNPRINTABLE_MARGIN;
                landscape.UnprintableMarginRight = UNPRINTABLE_MARGIN;
                landscape.UnprintableMarginTop = UNPRINTABLE_MARGIN;
                landscape.UnprintableMarginBottom = UNPRINTABLE_MARGIN;
                landscape.TabOrder = 4;
                landscape.PlotRotation = PlotRotation.QuarterCounterClockwise; // Landscape

                Size2D paperSize = landscape.PlotPaperSize;
                {
                    // This viewport is mandatory, it describes paper space itself.
                    DxfViewport paperSpaceViewport = new DxfViewport
                    {
                        Size = paperSize,
                        PaperSpace = true,
                        ViewCenter = new Point2D(0d, 0d),
                        ViewHeight = paperSize.Y,
                        Visible = true,
                    };
                    paperSpaceViewport.FrozenLayers.Add(frozenLayer);
                    landscape.Viewports.Add(paperSpaceViewport);

                    // This viewport is a viewport showing a piece of model space in paper space.
                    // ViewPort.ViewHeight = ViewPort.Size.Y / Scale factor (Visit: https://www.woutware.com/Forum/Topic/342)
                    Size2D rotatedViewportSize = new Size2D(paperSize.Y - TOTAL_MARGIN, paperSize.X - TOTAL_MARGIN);
                    double scaleFactor = Math.Min(rotatedViewportSize.X / modelWidth, rotatedViewportSize.Y / modelHeight) * 0.99;
                    DxfViewport modelSpaceViewport = new DxfViewport
                    {
                        // Centering point on the paper
                        Center = new Point3D(
                            rotatedViewportSize.X / 2,
                            rotatedViewportSize.Y / 2, 
                            0d), 
                        PaperSpace = true,
                        Size = rotatedViewportSize,
                        Target = new Point3D(0, 0, 0d),
                        ViewCenter = new Point2D(modelDisplacementOnX, modelDisplacementOnY),
                        // ViewPort.ViewHeight visit: https://www.woutware.com/doc/cadlib4.0/api/WW.Cad.Model.Entities.DxfViewport.html & https://www.woutware.com/Forum/Topic/342
                        ViewHeight = rotatedViewportSize.Y / scaleFactor,
                        // Set false to hide borders of the model space viewport
                        Visible = true                     
                    };

                    landscape.Viewports.Add(modelSpaceViewport);
                }
            }
            
            return SaveFile();
        }
    }
}
