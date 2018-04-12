using System;
using System.Collections.Generic;
using Mapsui.Geometries;
using Mapsui.Providers;
using Mapsui.Styles;
using SkiaSharp;

namespace Mapsui.Rendering.Skia
{
    public static class LineStringRenderer
    {
        private static readonly SKPaint PaintStroke = new SKPaint { IsAntialias = true, IsStroke = true };

        public static void Draw(SKCanvas canvas, IViewport viewport, IStyle style, IFeature feature, IGeometry geometry,
            float opacity)
        {
            if (style is LabelStyle labelStyle)
            {
                var worldCenter = geometry.GetBoundingBox().GetCentroid();
                var center = viewport.WorldToScreen(worldCenter);
                LabelRenderer.Draw(canvas, labelStyle, feature, (float) center.X, (float) center.Y, opacity);
            }
            else
            {
                var lineString = ((LineString) geometry).Vertices;

                float lineWidth = 1;
                var lineColor = new Color();

                var vectorStyle = style as VectorStyle;
                var strokeCap = PenStrokeCap.Butt;
                var strokeJoin = StrokeJoin.Miter;
                var strokeMiterLimit = 4f;
                var strokeStyle = PenStyle.Solid;
                float[] dashArray = null;

                if (vectorStyle != null)
                {
                    lineWidth = (float) vectorStyle.Line.Width;
                    lineColor = vectorStyle.Line.Color;
                    strokeCap = vectorStyle.Line.PenStrokeCap;
                    strokeJoin = vectorStyle.Line.StrokeJoin;
                    strokeMiterLimit = vectorStyle.Line.StrokeMiterLimit;
                    strokeStyle = vectorStyle.Line.PenStyle;
                    dashArray = vectorStyle.Line.DashArray;
                }

                var path = ToSkia(viewport, lineString);
                //var path1 = ToSkia(viewport, lineString);
                //var path2 = ToSkiaAlternative(viewport, lineString);
                //var path3 = ToSkia(viewport, lineString);
                //var path4 = ToSkiaAlternative(viewport, lineString);
                //var path5 = ToSkia(viewport, lineString);

                PaintStroke.StrokeWidth = lineWidth;
                PaintStroke.Color = lineColor.ToSkia(opacity);
                PaintStroke.StrokeCap = strokeCap.ToSkia();
                PaintStroke.StrokeJoin = strokeJoin.ToSkia();
                PaintStroke.StrokeMiter = strokeMiterLimit;
                if (strokeStyle != PenStyle.Solid)
                    PaintStroke.PathEffect = strokeStyle.ToSkia(lineWidth, dashArray);

                canvas.DrawPath(path, PaintStroke);
            }
        }

        private static SKPath ToSkia(IViewport viewport, IList<Point> vertices)
        {
            if (vertices.Count == 0)
                return new SKPath();

            var points = new SKPoint[vertices.Count];
            var pointPos = 0;

            var screenCenterX = viewport.Width * 0.5;
            var screenCenterY = viewport.Height * 0.5;
            var centerX = viewport.Center.X;
            var centerY = viewport.Center.Y;
            var resolution = 1.0 / viewport.Resolution;

            double sin = 0;
            double cos = 1;

            var vertice = vertices[0];
            var screenX = (vertice.X - centerX) * resolution;
            var screenY = (centerY - vertice.Y) * resolution;

            if (viewport.IsRotated)
            {
                var rotation = viewport.Rotation / 180f * Math.PI;
                sin = Math.Sin(rotation);
                cos = Math.Cos(rotation);
                var newX = screenX * cos - screenY * sin;
                var newY = screenX * sin + screenY * cos;
                screenX = newX;
                screenY = newY;
            }

            screenX += screenCenterX;
            screenY += screenCenterY;

            points[pointPos++] = new SKPoint((float)screenX, (float)screenY);

            for (var i = 1; i < vertices.Count; i++)
            {
                vertice = vertices[i];
                screenX = (vertice.X - centerX) * resolution;
                screenY = (centerY - vertice.Y) * resolution;

                if (viewport.IsRotated)
                {
                    var newX = screenX * cos - screenY * sin;
                    var newY = screenX * sin + screenY * cos;
                    screenX = newX;
                    screenY = newY;
                }

                screenX += screenCenterX;
                screenY += screenCenterY;

                points[pointPos++] = new SKPoint((float)screenX, (float)screenY);
            }

            var path = new SKPath();

            path.AddPoly(points, false);

            return path;
        }

        private static SKPath ToSkiaAlternative(IViewport viewport, IList<Point> vertices)
        {
            if (vertices.Count == 0)
                return new SKPath();

            var points = new SKPoint[vertices.Count];
            var pointPos = 0;

            var screenCenterX = (float)(viewport.Width * 0.5);
            var screenCenterY = (float)(viewport.Height * 0.5);
            var centerX = (float)viewport.Center.X;
            var centerY = (float)viewport.Center.Y;
            var resolution = 1f / (float)viewport.Resolution;

            points[pointPos++] = new SKPoint((float)vertices[0].X, -(float)vertices[0].Y);

            for (var i = 1; i < vertices.Count; i++)
            {
                points[pointPos++] = new SKPoint((float)vertices[i].X, -(float)vertices[i].Y);
            }

            var matrix = SKMatrix.MakeTranslation(-centerX, centerY);
            SKMatrix.PostConcat(ref matrix, SKMatrix.MakeScale(resolution, resolution));
            SKMatrix.PostConcat(ref matrix, SKMatrix.MakeRotation((float)(viewport.Rotation/180f*Math.PI)));
            SKMatrix.PostConcat(ref matrix, SKMatrix.MakeTranslation(screenCenterX, screenCenterY));

            var path = new SKPath();

            path.AddPoly(points, false);
            path.Transform(matrix);

            return path;
        }

        private static List<Point> WorldToScreen(IViewport viewport, IEnumerable<Point> points)
        {
            var result = new List<Point>();
            foreach (var point in points)
            {
                result.Add(viewport.WorldToScreen(point.X, point.Y));
            }
            return result;
        }
    }
}