using Mapsui.Geometries;
using System;
using System.Runtime.CompilerServices;

namespace Mapsui.Styles
{
    /// <summary>
    /// Type of Callout
    /// </summary>
    public enum CalloutType
    {
        /// <summary>
        /// Only one line is shown
        /// </summary>
        Single,
        /// <summary>
        /// Header and detail is shown
        /// </summary>
        Detail,
        /// <summary>
        /// Content is custom, ContentView in Content is shown
        /// </summary>
        Custom,
    }

    /// <summary>
    /// Determins, where the pointer is
    /// </summary>
    public enum ArrowAlignment
    {
        Bottom,
        Left,
        Top,
        Right,
    }

    public class CalloutStyle : VectorStyle
    {
        private Point _offset;

        public CalloutStyle()
        {
        }

        /// <summary>
        /// Type of Callout
        /// </summary>
        public CalloutType Type = CalloutType.Single;

        /// <summary>
        /// Offset position in pixels of Callout
        /// </summary>
        public Point Offset = new Point(0, 0);

        /// <summary>
        /// Gets or sets the rotation of the symbol in degrees (clockwise is positive)
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// When true a symbol will rotate along with the rotation of the map.
        /// The is useful if you need to symbolize the direction in which a vehicle
        /// is moving. When the symbol is false it will retain it's position to the
        /// screen. This is useful for pins like symbols. The default is false.
        /// </summary>
        public bool RotateWithMap { get; set; }

        /// <summary>
        /// Anchor position of Callout
        /// </summary>
        public ArrowAlignment ArrowAlignment = ArrowAlignment.Bottom;

        /// <summary>
        /// Width of opening of anchor of Callout
        /// </summary>
        public float ArrowWidth = 8f;

        /// <summary>
        /// Height of anchor of Callout
        /// </summary>
        public float ArrowHeight = 8f;

        /// <summary>
        /// Relative position of anchor of Callout on the side given by AnchorType
        /// </summary>
        public float ArrowPosition = 0.5f;

        /// <summary>
        /// Color of stroke around Callout
        /// </summary>
        public Color Color = Color.Black;

        /// <summary>
        /// BackgroundColor of Callout
        /// </summary>
        public Color BackgroundColor = Color.White;

        /// <summary>
        /// Stroke width of frame around Callout
        /// </summary>
        public float StrokeWidth = 1f;

        /// <summary>
        /// Radius of rounded corners of Callout
        /// </summary>
        public float RectRadius = 4f;

        /// <summary>
        /// Padding around content of Callout
        /// </summary>
        public float Padding = 3f;

        /// <summary>
        /// Width of shadow around Callout
        /// </summary>
        public float ShadowWidth = 2f;

        /// <summary>
        /// Content of Callout
        /// </summary>
        /// <remarks>
        /// Is a BitmapId of a save image
        /// </remarks>
        public int Content;

        /// <summary>
        /// Content of Callout header
        /// </summary>
        public string Title;

        /// <summary>
        /// Content of Callout detail label
        /// </summary>
        public string Subtitle;

        /// <summary>
        /// Get width of the Callout
        /// </summary>
        public float Width
        {
            get
            {
                return 100;
            }
        }

        /// <summary>
        /// Get height of the Callout
        /// </summary>
        public float Height
        {
            get
            {
                return 100;
            }
        }

        /// <summary>
        /// Resize the Callout, that grid is complete visible
        /// </summary>
        //private void UpdateGridSize()
        //{
        //    SizeRequest size;

        //    if (Type == CalloutType.Custom)
        //        size = BitmapRegistry.Instance. _content.Measure(double.PositiveInfinity, double.PositiveInfinity, MeasureFlags.None);
        //    else
        //        size = _grid.Measure(double.PositiveInfinity, double.PositiveInfinity, MeasureFlags.None);

        //    // Calc new size of info window to hold the complete content. 
        //    // Add some extra amount to be sure, that it is big enough.
        //    var width = size.Request.Width + Padding.Left + Padding.Right + ((ArrowAlignment == ArrowAlignment.Left || ArrowAlignment == ArrowAlignment.Right) ? ArrowHeight : 0) + _shadowWidth * 2 + 4;
        //    var height = size.Request.Height + Padding.Top + Padding.Bottom + ((ArrowAlignment == ArrowAlignment.Top || ArrowAlignment == ArrowAlignment.Bottom) ? ArrowHeight : 0) + _shadowWidth * 2 + 4;

        //    AbsoluteLayout.SetLayoutBounds(this, new Rectangle(X, Y, width, height));

        //    // Now, when we have updated info windows size, then we should update path
        //    UpdatePath();
        //}

        /// <summary>
        /// Update margins of grid
        /// </summary>
        //private void UpdateMargin()
        //{
        //    var margin = new BoundingBox();

        //    switch (ArrowAlignment)
        //    {
        //        case ArrowAlignment.Bottom:
        //            margin.Bottom += ArrowHeight;
        //            break;
        //        case ArrowAlignment.Top:
        //            margin.Top += ArrowHeight;
        //            break;
        //        case ArrowAlignment.Left:
        //            margin.Left += ArrowHeight;
        //            break;
        //        case ArrowAlignment.Right:
        //            margin.Right += ArrowHeight;
        //            break;
        //    }

        //    margin.Left += ShadowWidth;
        //    margin.Top += ShadowWidth;
        //    margin.Right += ShadowWidth;
        //    margin.Bottom += ShadowWidth;

        //    _grid.Margin = margin;
        //    _content.Margin = margin;

        //    UpdatePath();
        //}
    }
}
