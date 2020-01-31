using System;
using Mapsui.Geometries;
using Mapsui.Providers;
using Mapsui.Styles;
using SkiaSharp;

namespace Mapsui.Rendering.Skia
{
    static class PointRenderer
    {
        public static void Draw(SKCanvas canvas, IReadOnlyViewport viewport, IStyle style, IFeature feature, 
            IGeometry geometry, SymbolCache symbolCache, float opacity)
        {
            var point = geometry as Point;
            var destination = viewport.WorldToScreen(point);

            if (style is LabelStyle labelStyle)    // case 1) LabelStyle
            {
                LabelRenderer.Draw(canvas, labelStyle, feature, (float) destination.X, (float) destination.Y, 
                    opacity);
            }
            else if (style is SymbolStyle)
            {
                var symbolStyle = (SymbolStyle)style;

                if ( symbolStyle.BitmapId >= 0)   // case 2) Bitmap Style
                {
                    DrawPointWithBitmapStyle(canvas, symbolStyle, destination, symbolCache, opacity, (float)viewport.Rotation);
                }
                else                              // case 3) SymbolStyle without bitmap
                {
                    DrawPointWithSymbolStyle(canvas, symbolStyle, destination, opacity, symbolStyle.SymbolType, (float)viewport.Rotation);
                }
            }
            else if (style is CalloutStyle)
            {
                DrawPointWithCalloutStyle(canvas, (CalloutStyle)style, destination, symbolCache, opacity, (float)viewport.Rotation);
            }
            else if (style is VectorStyle)        // case 4) VectorStyle
            {
                DrawPointWithVectorStyle(canvas, (VectorStyle) style, destination, opacity);
            }
            else
            {
                throw new Exception($"Style of type '{style.GetType()}' is not supported for points");
            }
        }

        private static void DrawPointWithCalloutStyle(SKCanvas canvas, CalloutStyle style, Point destination, SymbolCache symbolCache,
            float opacity, float mapRotation)
        {
            var shadow = new SKPaint { IsAntialias = true, Style = SKPaintStyle.Stroke, StrokeWidth = 1.5f, Color = SKColors.Gray, MaskFilter = SKMaskFilter.CreateBlur(SKBlurStyle.Normal, style.ShadowWidth) };
            var fill = new SKPaint { IsAntialias = true, Style = SKPaintStyle.Fill, Color = style.BackgroundColor.ToSkia(opacity) };
            var stroke = new SKPaint { IsAntialias = true, Style = SKPaintStyle.Stroke, Color = style.Color.ToSkia(opacity), StrokeWidth = style.StrokeWidth };

            var (path, center) = CreateCalloutPath(style, symbolCache);

            var rotation = style.Rotation;
            if (style.RotateWithMap) rotation += mapRotation;

            canvas.Save();
            canvas.Translate((float)destination.X - center.X + (float)style.Offset.X, (float)destination.Y - center.Y - (float)style.Offset.Y);

            //canvas.Clear(SKColors.Transparent);
            canvas.DrawPath(path, shadow);
            canvas.DrawPath(path, fill);
            canvas.DrawPath(path, stroke);

            // Draw content
            if (style.Content >= 0)
            {
                var offsetX = symbolCache.GetOrCreate(style.Content).Width * 0.5f + style.ShadowWidth + style.Padding;
                var offsetY = -(symbolCache.GetOrCreate(style.Content).Height * 0.5f + style.ShadowWidth + style.Padding);
                DrawBitmap(canvas, symbolCache.GetOrCreate(style.Content), new Point(0, 0), new Point(offsetX, offsetY), symbolCache, opacity, rotation, 1.0);
            }

            // Draw close button
            //if (IsCloseVisible)
            //{
            //    var paint = new SKPaint { IsAntialias = true, Style = SKPaintStyle.Stroke, Color = SKColors.DarkGray, StrokeWidth = 2 };
            //    var pos = _close.Bounds.Offset(_grid.Bounds.Left, _grid.Bounds.Top).Inflate(-4, -4);
            //    canvas.DrawLine((float)pos.Left, (float)pos.Top, (float)pos.Right, (float)pos.Bottom, paint);
            //    canvas.DrawLine((float)pos.Left, (float)pos.Bottom, (float)pos.Right, (float)pos.Top, paint);
            //}

            canvas.Restore();
        }

        private static void DrawPointWithSymbolStyle(SKCanvas canvas, SymbolStyle style,
            Point destination, float opacity, SymbolType symbolType, float mapRotation)
        {
            canvas.Save();
            canvas.Translate((float)destination.X, (float)destination.Y);
            canvas.Scale((float)style.SymbolScale, (float)style.SymbolScale);
            if (style.SymbolOffset.IsRelative)
                canvas.Translate((float)(SymbolStyle.DefaultWidth * style.SymbolOffset.X), (float)(-SymbolStyle.DefaultWidth * style.SymbolOffset.Y));
            else
                canvas.Translate((float) style.SymbolOffset.X, (float) -style.SymbolOffset.Y);
            if (style.SymbolRotation != 0)
            {
                var rotation = (float)style.SymbolRotation;
                if (style.RotateWithMap) rotation += mapRotation;
                canvas.RotateDegrees(rotation);
            }

            DrawPointWithVectorStyle(canvas, style, opacity, symbolType);
            canvas.Restore();
        }

        private static void DrawPointWithVectorStyle(SKCanvas canvas, VectorStyle vectorStyle,
            Point destination, float opacity, SymbolType symbolType = SymbolType.Ellipse)
        {
            canvas.Save();
            canvas.Translate((float)destination.X, (float)destination.Y);
            DrawPointWithVectorStyle(canvas, vectorStyle, opacity, symbolType);
            canvas.Restore();
        }

        private static void DrawPointWithVectorStyle(SKCanvas canvas, VectorStyle vectorStyle,
            float opacity, SymbolType symbolType = SymbolType.Ellipse)
        {
            var width = (float)SymbolStyle.DefaultWidth;
            var halfWidth = width / 2;
            var halfHeight = (float)SymbolStyle.DefaultHeight / 2;

            var fillPaint = CreateFillPaint(vectorStyle.Fill, opacity);

            var linePaint = CreateLinePaint(vectorStyle.Outline, opacity);

            switch (symbolType)
            {
                case SymbolType.Ellipse:
                    DrawCircle(canvas, 0, 0, halfWidth, fillPaint, linePaint);
                    break;
                case SymbolType.Rectangle:
                    var rect = new SKRect(-halfWidth, -halfHeight, halfWidth, halfHeight);
                    DrawRect(canvas, rect, fillPaint, linePaint);
                    break;
                case SymbolType.Triangle:
                    DrawTriangle(canvas, 0, 0, width, fillPaint, linePaint);
                    break;
                default: // Invalid value
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static SKPaint CreateLinePaint(Pen outline, float opacity)
        {
            if (outline == null) return null;

            return new SKPaint
            {
                Color = outline.Color.ToSkia(opacity),
                StrokeWidth = (float) outline.Width,
                StrokeCap = outline.PenStrokeCap.ToSkia(),
                PathEffect = outline.PenStyle.ToSkia((float)outline.Width),
                Style = SKPaintStyle.Stroke,
                IsAntialias = true
            };
        }

        private static SKPaint CreateFillPaint(Brush fill, float opacity)
        {
            if (fill == null) return null;

            return new SKPaint
            {
                Color = fill.Color.ToSkia(opacity),
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            };
        }

        private static void DrawCircle(SKCanvas canvas, float x, float y, float radius, SKPaint fillColor,
            SKPaint lineColor)
        {
            if (fillColor != null && fillColor.Color.Alpha != 0) canvas.DrawCircle(x, y, radius, fillColor);
            if (lineColor != null && lineColor.Color.Alpha != 0) canvas.DrawCircle(x, y, radius, lineColor);
        }

        private static void DrawRect(SKCanvas canvas, SKRect rect, SKPaint fillColor, SKPaint lineColor)
        {
            if (fillColor != null && fillColor.Color.Alpha != 0) canvas.DrawRect(rect, fillColor);
            if (lineColor != null && lineColor.Color.Alpha != 0) canvas.DrawRect(rect, lineColor);
        }

        /// <summary>
        /// Equilateral triangle of side 'sideLength', centered on the same point as if a circle of diameter 'sideLength' was there
        /// </summary>
        private static void DrawTriangle(SKCanvas canvas, float x, float y, float sideLength, SKPaint fillColor, SKPaint lineColor)
        {
            var altitude = Math.Sqrt(3) / 2.0 * sideLength;
            var inradius = altitude / 3.0;
            var circumradius = 2.0 * inradius;

            var top = new Point(x, y - circumradius);
            var left = new Point(x + sideLength * -0.5, y + inradius);
            var right = new Point(x + sideLength * 0.5, y + inradius);

            var path = new SKPath();
            path.MoveTo((float)top.X, (float)top.Y);
            path.LineTo((float)left.X, (float)left.Y);
            path.LineTo((float)right.X, (float)right.Y);
            path.Close();

            if ((fillColor != null) && fillColor.Color.Alpha != 0) canvas.DrawPath(path, fillColor);
            if ((lineColor != null) && lineColor.Color.Alpha != 0) canvas.DrawPath(path, lineColor);
        }

        private static void DrawPointWithBitmapStyle(SKCanvas canvas, SymbolStyle symbolStyle, Point destination,
            SymbolCache symbolCache, float opacity, float mapRotation)
        {
            var bitmap = symbolCache.GetOrCreate(symbolStyle.BitmapId);

            // Calc offset (relative or absolute)
            var offsetX = symbolStyle.SymbolOffset.IsRelative ? bitmap.Width * symbolStyle.SymbolOffset.X : symbolStyle.SymbolOffset.X;
            var offsetY = symbolStyle.SymbolOffset.IsRelative ? bitmap.Height * symbolStyle.SymbolOffset.Y : symbolStyle.SymbolOffset.Y;

            var rotation = (float)symbolStyle.SymbolRotation;
            if (symbolStyle.RotateWithMap) rotation += mapRotation;

            DrawBitmap(canvas, bitmap, destination, new Point(offsetX, offsetY), symbolCache, opacity, rotation, symbolStyle.SymbolScale);
        }
        
        private static void DrawBitmap(SKCanvas canvas, BitmapInfo bitmap, Point destination, Point offset, SymbolCache symbolCache, float opacity, float rotation, double scale)
        { 
            switch (bitmap.Type)
            {
                case BitmapType.Bitmap:
                    BitmapHelper.RenderBitmap(canvas, bitmap.Bitmap,
                        (float) destination.X, (float) destination.Y,
                        rotation,
                        (float) offset.X, (float) offset.Y,
                        opacity: opacity, scale: (float) scale);
                    break;
                case BitmapType.Svg:
                    BitmapHelper.RenderSvg(canvas, bitmap.Svg,
                        (float)destination.X, (float)destination.Y,
                        rotation,
                        (float)offset.X, (float)offset.Y,
                        opacity: opacity, scale: (float)scale);
                    break;
                case BitmapType.Sprite:
                    var sprite = bitmap.Sprite;
                    if (sprite.Data == null)
                    {
                        var bitmapAtlas = symbolCache.GetOrCreate(sprite.Atlas);
                        sprite.Data = bitmapAtlas.Bitmap.Subset(new SKRectI(sprite.X, sprite.Y, sprite.X + sprite.Width,
                            sprite.Y + sprite.Height));
                    }
                    BitmapHelper.RenderBitmap(canvas, (SKImage)sprite.Data,
                        (float)destination.X, (float)destination.Y,
                        rotation,
                        (float)offset.X, (float)offset.Y,
                        opacity: opacity, scale: (float)scale);
                    break;
            }
        }

        /// <summary>
        /// Update path
        /// </summary>
        private static (SKPath, SKPoint) CreateCalloutPath(CalloutStyle style, SymbolCache symbolCache)
        {
            var width = (float)symbolCache.GetSize(style.Content).Width + style.Padding * 2;
            //width += style.ArrowAlignment == ArrowAlignment.Left || style.ArrowAlignment == ArrowAlignment.Right ? style.ArrowHeight : 0;
            var height = (float)symbolCache.GetSize(style.Content).Height + style.Padding * 2;
            //height += style.ArrowAlignment == ArrowAlignment.Top || style.ArrowAlignment == ArrowAlignment.Bottom ? style.ArrowHeight : 0;
            var halfWidth = width * style.ArrowPosition;
            var halfHeight = height * style.ArrowPosition;
            var bottom = (float)height + style.ShadowWidth;
            var left = style.ShadowWidth;
            var top = style.ShadowWidth;
            var right = (float)width + style.ShadowWidth;
            var start = new SKPoint();
            var center = new SKPoint();
            var end = new SKPoint();

            // Check, if we are to near of the corners
            if (halfWidth - style.ArrowWidth * 0.5f - left < style.RectRadius)
                halfWidth = style.ArrowWidth * 0.5f + left + style.RectRadius;
            else if (halfWidth + style.ArrowWidth * 0.5f  > width - style.RectRadius)
                halfWidth = width - style.ArrowWidth * 0.5f - style.RectRadius;
            if (halfHeight - style.ArrowWidth * 0.5f - top < style.RectRadius)
                halfHeight = style.ArrowWidth * 0.5f + top + style.RectRadius;
            else if (halfHeight + style.ArrowWidth * 0.5f > height - style.RectRadius)
                halfHeight = height - style.ArrowWidth * 0.5f - style.RectRadius;

            switch (style.ArrowAlignment)
            {
                case ArrowAlignment.Bottom:
                    start = new SKPoint(halfWidth + style.ArrowWidth * 0.5f, bottom);
                    center = new SKPoint(halfWidth, bottom + style.ArrowHeight);
                    end = new SKPoint(halfWidth - style.ArrowWidth * 0.5f, bottom);
                    break;
                case ArrowAlignment.Top:
                    start = new SKPoint(halfWidth - style.ArrowWidth * 0.5f, top);
                    center = new SKPoint(halfWidth, top - style.ArrowHeight);
                    end = new SKPoint(halfWidth + style.ArrowWidth * 0.5f, top);
                    break;
                case ArrowAlignment.Left:
                    start = new SKPoint(left, halfHeight + style.ArrowWidth * 0.5f);
                    center = new SKPoint(left - style.ArrowHeight, halfHeight);
                    end = new SKPoint(left, halfHeight - style.ArrowWidth * 0.5f);
                    break;
                case ArrowAlignment.Right:
                    start = new SKPoint(right, halfHeight - style.ArrowWidth * 0.5f);
                    center = new SKPoint(right + style.ArrowHeight, halfHeight);
                    end = new SKPoint(right, halfHeight + style.ArrowWidth * 0.5f);
                    break;
            }

            // Create path
            var path = new SKPath();

            // Move to start point at left/top
            path.MoveTo(left + style.RectRadius, top);

            // Top horizontal line
            if (style.ArrowAlignment == ArrowAlignment.Top)
                DrawArrow(path, start, center, end);

            // Top right arc
            path.ArcTo(new SKRect(right - style.RectRadius, top, right, top + style.RectRadius), 270, 90, false);

            // Right vertical line
            if (style.ArrowAlignment == ArrowAlignment.Right)
                DrawArrow(path, start, center, end);

            // Bottom right arc
            path.ArcTo(new SKRect(right - style.RectRadius, bottom - style.RectRadius, right, bottom), 0, 90, false);

            // Bottom horizontal line
            if (style.ArrowAlignment == ArrowAlignment.Bottom)
                DrawArrow(path, start, center, end);

            // Bottom left arc
            path.ArcTo(new SKRect(left, bottom - style.RectRadius, left + style.RectRadius, bottom), 90, 90, false);

            // Left vertical line
            if (style.ArrowAlignment == ArrowAlignment.Left)
                DrawArrow(path, start, center, end);

            // Top left arc
            path.ArcTo(new SKRect(left, top, left + style.RectRadius, top + style.RectRadius), 180, 90, false);

            path.Close();

            return (path, center);
        }

        /// <summary>
        /// Draw arrow to path
        /// </summary>
        /// <param name="start">Start of arrow at bubble</param>
        /// <param name="center">Center of arrow</param>
        /// <param name="end">End of arrow at bubble</param>
        private static void DrawArrow(SKPath path, SKPoint start, SKPoint center, SKPoint end)
        {
            path.LineTo(start);
            path.LineTo(center);
            path.LineTo(end);
        }
    }
}