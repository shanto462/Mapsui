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
        private ArrowAlignment _arrowAlignment = ArrowAlignment.Bottom;
        private float _arrowWidth = 8f;
        private float _arrowHeight = 8f;
        private float _arrowPosition = 0.5f;
        private float _rectRadius = 4f;
        private float _padding = 3f;
        private float _shadowWidth = 2f;

        public CalloutStyle()
        {
        }

        /// <summary>
        /// Storage for a prerendered path
        /// </summary>
        public object Path;

        /// <summary>
        /// Storage for the arrow center of callout
        /// </summary>
        public object Center;

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
        public ArrowAlignment ArrowAlignment 
        { 
            get
            { 
                return _arrowAlignment; 
            }
            set
            {
                if (value != _arrowAlignment)
                {
                    _arrowAlignment = value;
                    Path = null;
                }
            }
        }

        /// <summary>
        /// Width of opening of anchor of Callout
        /// </summary>
        public float ArrowWidth
        {
            get
            {
                return _arrowWidth;
            }
            set
            {
                if (value != _arrowWidth)
                {
                    _arrowWidth = value;
                    Path = null;
                }
            }
        }

        /// <summary>
        /// Height of anchor of Callout
        /// </summary>
        public float ArrowHeight
        {
            get
            {
                return _arrowHeight;
            }
            set
            {
                if (value != _arrowHeight)
                {
                    _arrowHeight = value;
                    Path = null;
                }
            }
        }

        /// <summary>
        /// Relative position of anchor of Callout on the side given by AnchorType
        /// </summary>
        public float ArrowPosition
        {
            get
            {
                return _arrowPosition;
            }
            set
            {
                if (value != _arrowPosition)
                {
                    _arrowPosition = value;
                    Path = null;
                }
            }
        }

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
        public float RectRadius
        {
            get
            {
                return _rectRadius;
            }
            set
            {
                if (value != _rectRadius)
                {
                    _rectRadius = value;
                    Path = null;
                }
            }
        }

        /// <summary>
        /// Padding around content of Callout
        /// </summary>
        public float Padding
        {
            get
            {
                return _padding;
            }
            set
            {
                if (value != _padding)
                {
                    _padding = value;
                    Path = null;
                }
            }
        }

        /// <summary>
        /// Width of shadow around Callout
        /// </summary>
        public float ShadowWidth
        {
            get
            {
                return _shadowWidth;
            }
            set
            {
                if (value != _shadowWidth)
                {
                    _shadowWidth = value;
                    Path = null;
                }
            }
        }

        /// <summary>
        /// Content of Callout
        /// </summary>
        /// <remarks>
        /// Is a BitmapId of a save image
        /// </remarks>
        public int Content;
    }
}
