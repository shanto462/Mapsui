using Mapsui.Geometries;
using Mapsui.Styles;
using SkiaSharp;
using System.Runtime.CompilerServices;

namespace Mapsui.Rendering.Skia
{
    /// <summary>
    /// Determins, where the pointer is
    /// </summary>
    public enum ArrowAlignment
    {
        /// <summary>
        /// Callout arrow is at bottom side of bubble
        /// </summary>
        Bottom,
        /// <summary>
        /// Callout arrow is at left side of bubble
        /// </summary>
        Left,
        /// <summary>
        /// Callout arrow is at top side of bubble
        /// </summary>
        Top,
        /// <summary>
        /// Callout arrow is at right side of bubble
        /// </summary>
        Right,
    }

    public class CalloutStyle : SymbolStyle
    {
        private SKPath _path;
        private SKPoint _center;
        private ArrowAlignment _arrowAlignment = ArrowAlignment.Bottom;
        private float _arrowWidth = 8f;
        private float _arrowHeight = 8f;
        private float _arrowPosition = 0.5f;
        private float _rectRadius = 4f;
        private float _shadowWidth = 2f;
        private BoundingBox _padding = new BoundingBox(3f, 3f, 3f, 3f);
        private Color _color = Color.Black;
        private Color _backgroundColor = Color.White;
        private float _strokeWidth = 1f;
        private int _content = -1;
        private Point _offset = new Point(0, 0);
        private double _rotation = 0;

        public new static double DefaultWidth { get; set; } = 100;
        public new static double DefaultHeight { get; set; } = 30;

        public CalloutStyle()
        {
        }

        /// <summary>
        /// Offset position in pixels of Callout
        /// </summary>
        public Point Offset
        {
            get => _offset;
            set
            {
                if (!_offset.Equals(value))
                {
                    _offset = value;
                    SymbolOffset = new Offset(_offset.X, _offset.Y);
                }
            }
        }

        /// <summary>
        /// BoundingBox relative to offset point
        /// </summary>
        public BoundingBox BoundingBox = new BoundingBox();

        /// <summary>
        /// Gets or sets the rotation of the Callout in degrees (clockwise is positive)
        /// </summary>
        public double Rotation
        { 
            get => _rotation;
            set
            {
                if (_rotation != value)
                {
                    _rotation = value;
                    SymbolRotation = _rotation;
                }
            }
        }

        /// <summary>
        /// Storage for an own bubble path
        /// </summary>
        public SKPath Path
        {
            get => _path;
            set
            {
                if (_path != value)
                {
                    _path = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Anchor position of Callout
        /// </summary>
        public ArrowAlignment ArrowAlignment 
        { 
            get => _arrowAlignment; 
            set
            {
                if (value != _arrowAlignment)
                {
                    _arrowAlignment = value;
                    _path = null;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Width of opening of anchor of Callout
        /// </summary>
        public float ArrowWidth
        {
            get => _arrowWidth;
            set
            {
                if (value != _arrowWidth)
                {
                    _arrowWidth = value;
                    _path = null;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Height of anchor of Callout
        /// </summary>
        public float ArrowHeight
        {
            get => _arrowHeight;
            set
            {
                if (value != _arrowHeight)
                {
                    _arrowHeight = value;
                    _path = null;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Relative position of anchor of Callout on the side given by AnchorType
        /// </summary>
        public float ArrowPosition
        {
            get => _arrowPosition;
            set
            {
                if (value != _arrowPosition)
                {
                    _arrowPosition = value;
                    _path = null;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Color of stroke around Callout
        /// </summary>
        public Color Color
        {
            get => _color;
            set
            {
                if (value != _color)
                {
                    _color = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// BackgroundColor of Callout
        /// </summary>
        public Color BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                if (value != _backgroundColor)
                {
                    _backgroundColor = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Stroke width of frame around Callout
        /// </summary>
        public float StrokeWidth
        {
            get => _strokeWidth;
            set
            {
                if (value != _strokeWidth)
                {
                    _strokeWidth = value;
                    _path = null;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Radius of rounded corners of Callout
        /// </summary>
        public float RectRadius
        {
            get => _rectRadius;
            set
            {
                if (value != _rectRadius)
                {
                    _rectRadius = value;
                    _path = null;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Padding around content of Callout
        /// </summary>
        public BoundingBox Padding
        {
            get => _padding;
            set
            {
                if (value != _padding)
                {
                    _padding = value;
                    _path = null;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Width of shadow around Callout
        /// </summary>
        public float ShadowWidth
        {
            get => _shadowWidth;
            set
            {
                if (value != _shadowWidth)
                {
                    _shadowWidth = value;
                    _path = null;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Content of Callout
        /// </summary>
        /// <remarks>
        /// Is a BitmapId of a save image
        /// </remarks>
        public int Content
        {
            get => _content;
            set
            {
                if (_content != value)
                {
                    _content = value;
                    _path = null;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        ///             // Something changed, so create new image

        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (_content < 0)
                return;

            // Get size of content
            var bitmapInfo = BitmapHelper.LoadBitmap(BitmapRegistry.Instance.Get(_content));

            double contentWidth = bitmapInfo.Width;
            double contentHeight = bitmapInfo.Height;

            (var width, var height) = CalcSize(contentWidth, contentHeight);

            // Create a canvas for drawing
            var imageInfo = new SKImageInfo((int)width, (int)height);
            using (var surface = SKSurface.Create(imageInfo))
            {
                var canvas = surface.Canvas;

                // Is there a prerendered path?
                if (_path == null)
                {
                    // No, than create a new path
                    (_path, _center) = CreateCalloutPath(contentWidth, contentHeight);
                    // Now move SymbolOffset to the position of the arrow
                    SymbolOffset = new Offset(Offset.X + (width * 0.5 - _center.X), Offset.Y - (height * 0.5 - _center.Y));
                }

                // Draw path for bubble
                DrawCallout(canvas);

                // Draw content
                DrawContent(canvas, bitmapInfo);

                // Create image from canvas
                var image = surface.Snapshot();
                var data = image.Encode(SKEncodedImageFormat.Png, 100);

                // Register 
                if (BitmapId >= 0)
                    BitmapRegistry.Instance.Set(BitmapId, data.AsStream(true));
                else
                    BitmapId = BitmapRegistry.Instance.Register(data.AsStream(true));
            }
        }

        /// <summary>
        /// Calc the size which is neededfor the canvas
        /// </summary>
        /// <returns></returns>
        private (double, double) CalcSize(double contentWidth, double contentHeight)
        {
            // Add padding around the content
            var paddingLeft = _padding.Left < _rectRadius * 0.5 ? _rectRadius * 0.5 : _padding.Left;
            var paddingTop = _padding.Top < _rectRadius * 0.5 ? _rectRadius * 0.5 : _padding.Top;
            var paddingRight = _padding.Right < _rectRadius * 0.5 ? _rectRadius * 0.5 : _padding.Right;
            var paddingBottom = _padding.Bottom < _rectRadius * 0.5 ? _rectRadius * 0.5 : _padding.Bottom;
            var width = contentWidth + paddingLeft + paddingRight + 1;
            var height = contentHeight + paddingTop + paddingBottom + 1;

            // Add length of arrow
            switch (ArrowAlignment)
            {
                case ArrowAlignment.Bottom:
                case ArrowAlignment.Top:
                    height += ArrowHeight;
                    break;
                case ArrowAlignment.Left:
                case ArrowAlignment.Right:
                    width += ArrowHeight;
                    break;
            }

            // Add half of StrokeWidth to all sides
            width += _strokeWidth;
            height += _strokeWidth;

            // Add shadow to all sides
            width += _shadowWidth * 2;
            height += _shadowWidth * 2;

            return (width, height);
        }

        private void DrawCallout(SKCanvas canvas)
        {
            var shadow = new SKPaint { IsAntialias = true, Style = SKPaintStyle.Stroke, StrokeWidth = 1.5f, Color = SKColors.Gray, MaskFilter = SKMaskFilter.CreateBlur(SKBlurStyle.Normal, _shadowWidth) };
            var fill = new SKPaint { IsAntialias = true, Style = SKPaintStyle.Fill, Color = ToSkia(_backgroundColor) };
            var stroke = new SKPaint { IsAntialias = true, Style = SKPaintStyle.Stroke, Color = ToSkia(_color), StrokeWidth = _strokeWidth };

            canvas.Clear(SKColors.Transparent);
            canvas.DrawPath(_path, shadow);
            canvas.DrawPath(_path, fill);
            canvas.DrawPath(_path, stroke);
        }
         
        private void DrawContent(SKCanvas canvas, BitmapInfo bitmapInfo)
        { 
            // Draw content
            if (_content >= 0)
            {
                var offsetX = _shadowWidth + (_padding.Left < _rectRadius * 0.5 ? _rectRadius * 0.5f : (float)_padding.Left);
                var offsetY = _shadowWidth + (_padding.Top < _rectRadius * 0.5 ? _rectRadius * 0.5f : (float)_padding.Top);

                switch (ArrowAlignment)
                {
                    case ArrowAlignment.Left:
                        offsetX += ArrowHeight;
                        break;
                    case ArrowAlignment.Top:
                        offsetY += ArrowHeight;
                        break;
                }

                var offset = new SKPoint(offsetX, offsetY);

                switch (bitmapInfo.Type)
                {
                    case BitmapType.Bitmap:
                        canvas.DrawImage(bitmapInfo.Bitmap, offset);
                        break;
                    case BitmapType.Sprite:
                        throw new System.Exception();
                    case BitmapType.Svg:
                        canvas.DrawPicture(bitmapInfo.Svg.Picture, offset);
                        break;
                }
            }

            //canvas.Restore();
        }

        /// <summary>
        /// Update path
        /// </summary>
        private (SKPath, SKPoint) CreateCalloutPath(double contentWidth, double contentHeight)
        {
            var paddingLeft = _padding.Left < _rectRadius * 0.5 ? _rectRadius * 0.5 : _padding.Left;
            var paddingTop = _padding.Top < _rectRadius * 0.5 ? _rectRadius * 0.5 : _padding.Top;
            var paddingRight = _padding.Right < _rectRadius * 0.5 ? _rectRadius * 0.5 : _padding.Right;
            var paddingBottom = _padding.Bottom < _rectRadius * 0.5 ? _rectRadius * 0.5 : _padding.Bottom;
            var width = (float)contentWidth + (float)paddingLeft + (float)paddingRight;
            var height = (float)contentHeight + (float)paddingTop + (float)paddingBottom;
            var halfWidth = width * ArrowPosition;
            var halfHeight = height * ArrowPosition;
            var bottom = (float)height + ShadowWidth;
            var left = ShadowWidth;
            var top = ShadowWidth;
            var right = (float)width + ShadowWidth;
            var start = new SKPoint();
            var center = new SKPoint();
            var end = new SKPoint();

            // Check, if we are to near at corners
            if (halfWidth - ArrowWidth * 0.5f - left < RectRadius)
                halfWidth = ArrowWidth * 0.5f + left + RectRadius;
            else if (halfWidth + ArrowWidth * 0.5f > width - RectRadius)
                halfWidth = width - ArrowWidth * 0.5f - RectRadius;
            if (halfHeight - ArrowWidth * 0.5f - top < RectRadius)
                halfHeight = ArrowWidth * 0.5f + top + RectRadius;
            else if (halfHeight + ArrowWidth * 0.5f > height - RectRadius)
                halfHeight = height - ArrowWidth * 0.5f - RectRadius;

            switch (ArrowAlignment)
            {
                case ArrowAlignment.Bottom:
                    start = new SKPoint(halfWidth + ArrowWidth * 0.5f, bottom);
                    center = new SKPoint(halfWidth, bottom + ArrowHeight);
                    end = new SKPoint(halfWidth - ArrowWidth * 0.5f, bottom);
                    break;
                case ArrowAlignment.Top:
                    top += ArrowHeight;
                    bottom += ArrowHeight;
                    start = new SKPoint(halfWidth - ArrowWidth * 0.5f, top);
                    center = new SKPoint(halfWidth, top - ArrowHeight);
                    end = new SKPoint(halfWidth + ArrowWidth * 0.5f, top);
                    break;
                case ArrowAlignment.Left:
                    left += ArrowHeight;
                    right += ArrowHeight;
                    start = new SKPoint(left, halfHeight + ArrowWidth * 0.5f);
                    center = new SKPoint(left - ArrowHeight, halfHeight);
                    end = new SKPoint(left, halfHeight - ArrowWidth * 0.5f);
                    break;
                case ArrowAlignment.Right:
                    start = new SKPoint(right, halfHeight - ArrowWidth * 0.5f);
                    center = new SKPoint(right + ArrowHeight, halfHeight);
                    end = new SKPoint(right, halfHeight + ArrowWidth * 0.5f);
                    break;
            }

            // Create path
            var path = new SKPath();

            // Move to start point at left/top
            path.MoveTo(left + RectRadius, top);

            // Top horizontal line
            if (ArrowAlignment == ArrowAlignment.Top)
                DrawArrow(path, start, center, end);

            // Top right arc
            path.ArcTo(new SKRect(right - RectRadius, top, right, top + RectRadius), 270, 90, false);

            // Right vertical line
            if (ArrowAlignment == ArrowAlignment.Right)
                DrawArrow(path, start, center, end);

            // Bottom right arc
            path.ArcTo(new SKRect(right - RectRadius, bottom - RectRadius, right, bottom), 0, 90, false);

            // Bottom horizontal line
            if (ArrowAlignment == ArrowAlignment.Bottom)
                DrawArrow(path, start, center, end);

            // Bottom left arc
            path.ArcTo(new SKRect(left, bottom - RectRadius, left + RectRadius, bottom), 90, 90, false);

            // Left vertical line
            if (ArrowAlignment == ArrowAlignment.Left)
                DrawArrow(path, start, center, end);

            // Top left arc
            path.ArcTo(new SKRect(left, top, left + RectRadius, top + RectRadius), 180, 90, false);

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

        /// <summary>
        /// Convert Mapsui color to Skia color
        /// </summary>
        /// <param name="color">Color in Mapsui format</param>
        /// <returns>Color in Skia format</returns>
        public SKColor ToSkia(Color color)
        {
            if (color == null) return new SKColor(128, 128, 128, 0);
            return new SKColor((byte)color.R, (byte)color.G, (byte)color.B, (byte)color.A);
        }
    }
}
