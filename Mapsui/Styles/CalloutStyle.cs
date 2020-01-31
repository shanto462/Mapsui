using Mapsui.Geometries;

namespace Mapsui.Styles
{
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
        public CalloutStyle()
        {
        }

        /// <summary>
        /// Offset position in pixels of Callout
        /// </summary>
        public Point Offset = new Point(0, 0);

        /// <summary>
        /// Gets or sets the rotation of the Callout in degrees (clockwise is positive)
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// When true a Callout will rotate along with the rotation of the map.
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
    }
}
