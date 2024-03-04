using System.Reflection;
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
            Model.Entities.Add(new DxfCircle(EntityColors.Red, new Point3D(0d, 0d, 0d), 2d) { LineWeight = 100 });
            Model.Entities.Add(new DxfLine(EntityColors.GreenYellow, new Point3D(-3d, 4d, 0d),
                new Point3D(5d, 3d, 0d)));
            Model.Entities.Add(new DxfLine(EntityColors.Yellow, new Point3D(-4d, 0d, 0d), new Point3D(5d, 5d, 0d)));

            DxfLayer frozenLayer = new DxfLayer("Frozen");
            Model.Layers.Add(frozenLayer);

            {
                DxfLayout layout1 = Model.Layouts["layout1"];
                layout1.Name = "Layout number 1";
                layout1.Entities.Add(new DxfText("Text on layout 1", new Point3D(2d, 0d, 0d), 5d));
                // Paper size and margins are defined before being rotated according to property PlotRotation.
                layout1.PlotPaperSize = new Size2D(100d, 100d);
                layout1.UnprintableMarginLeft = 5d;
                layout1.UnprintableMarginRight = 10d;
                layout1.UnprintableMarginTop = 5d;
                layout1.UnprintableMarginBottom = 5d;
                layout1.TabOrder = 1;
                layout1.PlotRotation = PlotRotation.None;

                {
                    // This viewport is mandatory, it describes paper space itself.
                    DxfViewport paperSpaceViewport = new DxfViewport();
                    paperSpaceViewport.ViewCenter = new Point2D(0d, 0d);
                    paperSpaceViewport.Size = new Size2D(100d, 100d);
                    paperSpaceViewport.Visible = true;
                    paperSpaceViewport.ViewHeight = 100d;
                    paperSpaceViewport.PaperSpace = true;
                    paperSpaceViewport.FrozenLayers.Add(frozenLayer);
                    layout1.Viewports.Add(paperSpaceViewport);

                    // This viewport is a viewport showing a piece of model space in paper space.
                    // The total scale factor will be Size.Y / ViewHeight.
                    DxfViewport modelSpaceViewport = new DxfViewport();
                    modelSpaceViewport.ViewHeight = 6d;
                    modelSpaceViewport.ViewCenter = new Point2D(0d, 0d);
                    modelSpaceViewport.Center = new Point3D(40d, 30d, 0d);
                    modelSpaceViewport.Target = new Point3D(0d, 0d, 0d);
                    modelSpaceViewport.Size = new Size2D(80d, 60d);
                    modelSpaceViewport.PaperSpace = true;
                    modelSpaceViewport.Visible = true;
                    layout1.Viewports.Add(modelSpaceViewport);
                }
            }

            {
                // A landscape layout with 2 viewports.
                DxfLayout landScapeLayout = new DxfLayout("Landscape Layout");
                Model.Layouts.Add(landScapeLayout);
                landScapeLayout.Entities.Add(new DxfText("Text on layout 2", new Point3D(2d, 0d, 0d), 10d));
                // Paper size and margins are defined before being rotated according to property PlotRotation.
                landScapeLayout.PlotPaperSize = new Size2D(210, 297); // A4 size in mm.
                landScapeLayout.UnprintableMarginLeft = 5d;
                landScapeLayout.UnprintableMarginRight = 10d;
                landScapeLayout.UnprintableMarginTop = 5d;
                landScapeLayout.UnprintableMarginBottom = 5d;
                landScapeLayout.TabOrder = 2;
                landScapeLayout.PlotRotation = PlotRotation.QuarterCounterClockwise;

                {
                    // This viewport is mandatory, it describes paper space itself.
                    DxfViewport paperSpaceViewport = new DxfViewport();
                    paperSpaceViewport.ViewCenter = new Point2D(0d, 0d);
                    paperSpaceViewport.Size = landScapeLayout.PlotPaperSize;
                    paperSpaceViewport.Visible = true;
                    paperSpaceViewport.ViewHeight = paperSpaceViewport.Size.Y;
                    paperSpaceViewport.PaperSpace = true;
                    paperSpaceViewport.FrozenLayers.Add(frozenLayer);
                    landScapeLayout.Viewports.Add(paperSpaceViewport);

                    // This viewport is a viewport showing a piece of model space in paper space.
                    // The total scale factor will be Size.Y / ViewHeight.
                    DxfViewport modelSpaceViewport1 = new DxfViewport();
                    modelSpaceViewport1.ViewHeight = 6d;
                    modelSpaceViewport1.ViewCenter = new Point2D(0d, 0d);
                    modelSpaceViewport1.Center = new Point3D(55d, 90d, 0d);
                    modelSpaceViewport1.Target = new Point3D(0d, 0d, 0d);
                    modelSpaceViewport1.Size = new Size2D(100d, 180d);
                    modelSpaceViewport1.PaperSpace = true;
                    modelSpaceViewport1.Visible = true;
                    landScapeLayout.Viewports.Add(modelSpaceViewport1);

                    // This viewport is a viewport showing a piece of model space in paper space.
                    // The total scale factor will be Size.Y / ViewHeight.
                    DxfViewport modelSpaceViewport2 = new DxfViewport();
                    modelSpaceViewport2.ViewHeight = 9d;
                    modelSpaceViewport2.ViewCenter = new Point2D(0d, 0d);
                    modelSpaceViewport2.Center = new Point3D(185d, 90d, 0d);
                    modelSpaceViewport2.Target = new Point3D(0d, 0d, 0d);
                    modelSpaceViewport2.Size = new Size2D(120d, 180d);
                    modelSpaceViewport2.PaperSpace = true;
                    modelSpaceViewport2.Visible = true;
                    landScapeLayout.Viewports.Add(modelSpaceViewport2);
                }
            }

            {

                DxfLayout portraitLayout = new DxfLayout("Portrait Layout");
                Model.Layouts.Add(portraitLayout);
                portraitLayout.Entities.Add(new DxfText("Text on layout 3", new Point3D(2d, 0d, 0d), 10d));
                // Paper size and margins are defined before being rotated according to property PlotRotation.
                portraitLayout.PlotPaperSize = new Size2D(210, 297); // A4 size in mm.
                portraitLayout.UnprintableMarginLeft = 5d;
                portraitLayout.UnprintableMarginRight = 10d;
                portraitLayout.UnprintableMarginTop = 5d;
                portraitLayout.UnprintableMarginBottom = 5d;
                portraitLayout.TabOrder = 3;
                portraitLayout.PlotRotation = PlotRotation.None; // Portrait

                {
                    // This viewport is mandatory, it describes paper space itself.
                    DxfViewport paperSpaceViewport = new DxfViewport();
                    paperSpaceViewport.ViewCenter = new Point2D(0d, 0d);
                    paperSpaceViewport.Size = portraitLayout.PlotPaperSize;
                    paperSpaceViewport.Visible = true;
                    paperSpaceViewport.ViewHeight = paperSpaceViewport.Size.Y;
                    paperSpaceViewport.PaperSpace = true;
                    paperSpaceViewport.FrozenLayers.Add(frozenLayer);
                    portraitLayout.Viewports.Add(paperSpaceViewport);

                    // This viewport is a viewport showing a piece of model space in paper space.
                    // The total scale factor will be Size.Y / ViewHeight.
                    DxfViewport modelSpaceViewport = new DxfViewport();
                    modelSpaceViewport.ViewHeight = 6d;
                    modelSpaceViewport.ViewCenter = new Point2D(0d, 0d);
                    modelSpaceViewport.Center = new Point3D(90d, 120d, 0d);
                    modelSpaceViewport.Target = new Point3D(0d, 0d, 0d);
                    modelSpaceViewport.Size = new Size2D(180d, 240d);
                    modelSpaceViewport.PaperSpace = true;
                    modelSpaceViewport.Visible = true;
                    portraitLayout.Viewports.Add(modelSpaceViewport);
                }
            }

            return SaveFile();
        }
    }
}
