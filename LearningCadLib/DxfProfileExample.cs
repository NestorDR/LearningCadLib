using System;
using System.Reflection;
using WW.Cad.Model.Entities;
using WW.Math;

namespace LearningCadLib
{
    internal class DxfProfileExample : DxfBase
    {

        internal DxfProfileExample() : base(MethodBase.GetCurrentMethod()?.DeclaringType?.Name.Replace("Dxf", ""))
        {
        }

        internal override string Draw()
        {
            // Set the dimensions of the profile (container rectangle)
            double profileLength = 1000;
            double profileHeight = 200;

            // Try to keep some aspect ratio with the profile/container
            const int drawsToAdd = 4;

            // Draw a circle keeping aspect ratio with the profile/container
            double radius = 30;
            double x = profileLength / drawsToAdd / 2 - radius;         // It is centered on the area that is the first quarter of the abscissas (of the profile)
            double y = profileHeight / 2;                               // Centers in the middle of the ordinates (of the profile)
            DrawCircle(x, y, radius);

            // Draw a rectangle keeping aspect ratio with the profile/container and the circle
            double length = radius * 4;
            double height = radius * 2;
            x = x + radius + profileLength / drawsToAdd - length / 2;   // Centers on the area that is the second quarter of the abscissas (of the profile)
            y -= radius;                                                // Centers in the middle of the ordinates (of the profile)
            DrawRectangle(x, y, length, height);

            // Draw a slot keeping aspect ratio with the profile/container and the circle
            x += profileLength / drawsToAdd;                            // Centers on the third quarter of the abscissas (of the profile)
            radius /= 4;                                                // For rounded corners
            DrawRoundedRectangle(x, y, length, height, radius);

            // Draw a slot keeping aspect ratio with the profile/container and the circle
            x += profileLength / drawsToAdd;                            // Centers on the last quarter of the abscissas (of the profile)
            radius = height / 2;                                        // For rounded height
            DrawSlot(x, y, length, height, radius);

            // Set text dimensions
            const double charHeight = 6;
            const double estimatedTextWidth = 120;
            // Prepare label to be added
            const string cR = "\\P";                                    // Carriage return
            string message = $"I am just a proof of concept.{cR}Hello World Co.";
            x = profileLength - estimatedTextWidth;
            y = -3;
            // Add a multiline label
            WriteText(message, x, y, charHeight, estimatedTextWidth);

            // Draw the profile/container as a rectangle
            x = (double)0m;
            y = (double)0m;
            DrawRectangle(x, y, profileLength, profileHeight);

            return SaveFile();
        }

        /// <summary>
        /// Draws a circle on the CAD model.
        /// </summary>
        /// <param name="x">X coordinate for the circle center.</param>
        /// <param name="y">Y coordinate for the circle center.</param>
        /// <param name="radius">Circle radius.</param>
        /// <returns>A <see cref="DxfCircle"/> added on the CAD model.</returns>
        private DxfCircle DrawCircle(double x, double y, double radius)
        {
            DxfCircle circle = new DxfCircle(new Point2D(x, y), radius);
            Model.Entities.Add(circle);
         
            return circle;
        }

        /// <summary>
        /// Draws a rectangle on the CAD model.
        /// </summary>
        /// <param name="x">X coordinate for the bottom left vertex of the rectangle.</param>
        /// <param name="y">Y coordinate for the bottom left vertex of the rectangle.</param>
        /// <param name="length">Length of the base of the rectangle.</param>
        /// <param name="height">Length of the height of the rectangle.</param>
        /// <returns>A <see cref="DxfLwPolyline"/> added on the CAD model.</returns>
        private DxfLwPolyline DrawRectangle(double x, double y, double length, double height)
        {
            DxfLwPolyline rectangle = new DxfLwPolyline();
            DxfLwPolyline.Vertex[] vertices =
            {
                new DxfLwPolyline.Vertex(x, y),
                new DxfLwPolyline.Vertex(x + length, y),
                new DxfLwPolyline.Vertex(x + length, y + height),
                new DxfLwPolyline.Vertex(x, y + height)
            };
            rectangle.Vertices.AddRange(vertices);
            rectangle.Closed = true;
            Model.Entities.Add(rectangle);
            
            return rectangle;
        }

        /// <summary>
        /// Draws a rectangle with round corners on the CAD model.
        /// </summary>
        /// <param name="x">X coordinate for the bottom left vertex of the rectangle.</param>
        /// <param name="y">Y coordinate for the bottom left vertex of the rectangle.</param>
        /// <param name="length">Length of the base of the rectangle.</param>
        /// <param name="height">Length of the height of the rectangle.</param>
        /// <param name="radius">Radius for round corners.</param>
        /// <returns>A <see cref="DxfLwPolyline"/> added on the CAD model.</returns>
        private DxfLwPolyline DrawRoundedRectangle(double x, double y, double length, double height, double radius)
        {
            double bulge = Math.Tan(Math.PI / 8); // bulge for CCW quarter arc, tan of one fourth of 90°

            DxfLwPolyline slot = new DxfLwPolyline();
            DxfLwPolyline.Vertex[] vertices =
            {
                new DxfLwPolyline.Vertex(new Point2D(x + radius, y)),
                new DxfLwPolyline.Vertex(new Point2D(x + length - radius, y), bulge),
                new DxfLwPolyline.Vertex(new Point2D(x + length, y + radius)),
                new DxfLwPolyline.Vertex(new Point2D(x + length, y + height - radius), bulge),
                new DxfLwPolyline.Vertex(new Point2D(x + length - radius, y + height)),
                new DxfLwPolyline.Vertex(new Point2D(x + radius, y + height), bulge),
                new DxfLwPolyline.Vertex(new Point2D(x, y + height - radius)),
                new DxfLwPolyline.Vertex(new Point2D(x, y + radius), bulge)
            };
            slot.Vertices.AddRange(vertices);
            slot.Closed = true;
            Model.Entities.Add(slot);

            return slot;
        }

        /// <summary>
        /// Draw a rectangle with round height side on the CAD model.
        /// </summary>
        /// <param name="x">X coordinate for the bottom left vertex of the slot.</param>
        /// <param name="y">Y coordinate for the bottom left vertex of the slot.</param>
        /// <param name="length">Length of the base of the rectangle, or slot width.</param>
        /// <param name="height">Length of the height of the rectangle.</param>
        /// <param name="radius">Radius for round side (the height side).</param>
        /// <returns>A <see cref="DxfLwPolyline"/> added on the CAD model.</returns>
        private DxfLwPolyline DrawSlot(double x, double y, double length, double height, double radius)
        {
            double bulge = Math.Tan(Math.PI / 4); // bulge for CCW quarter arc, tan of one fourth of 90°

            DxfLwPolyline slot = new DxfLwPolyline();
            DxfLwPolyline.Vertex[] vertices =
            {
                new DxfLwPolyline.Vertex(new Point2D(x + radius, y)),
                new DxfLwPolyline.Vertex(new Point2D(x + length - radius, y), bulge),
                new DxfLwPolyline.Vertex(new Point2D(x + length - radius, y + height)),
                new DxfLwPolyline.Vertex(new Point2D(x + radius, y + height), bulge),
            };
            slot.Vertices.AddRange(vertices);
            slot.Closed = true;
            Model.Entities.Add(slot);

            return slot;
        }

        /// <summary>
        /// Writes a multi-line text on the CAD model.
        /// </summary>
        /// <param name="message">Multi-line text to be added on the CAD model.</param>
        /// <param name="x">X coordinate for the upper left vertex of the text.</param>
        /// <param name="y">Y coordinate for the upper left vertex of the text</param>
        /// <param name="charHeight">Desired height for each character.</param>
        /// <param name="estimatedTextWidth">Estimated width that the text will have in the model.</param>
        private DxfMText WriteText(string message, double x, double y, double charHeight, double estimatedTextWidth)
        {
            // Represents a multiline text entity
            DxfMText dxfMultilineText = new DxfMText(message, new Point3D(x, y, 0))
            {
                Height = charHeight,
                ReferenceRectangleWidth = estimatedTextWidth
            };
            Model.Entities.Add(dxfMultilineText);
            
            return dxfMultilineText;
        }
    }
}
