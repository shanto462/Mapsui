using Mapsui.Geometries;
using Mapsui.Providers;
using Mapsui.Styles;
using Mapsui.UI.Forms;
using Mapsui.UI.Forms.Extensions;
using SkiaSharp;
using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Topten.RichTextKit;
using Xamarin.Forms;

namespace Mapsui.UI.Objects
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
        /// Content is custom, the bitmap given in Content is shown
        /// </summary>
        Custom,
    }

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


    public class Callout : BindableObject, IFeatureProvider
    {
        private Pin _pin;
        private TextBlock _textBlock;
        private bool _updating = false;

        public event EventHandler<EventArgs> CalloutClosed;
        public event EventHandler<EventArgs> CalloutClicked;

        public static string DefaultTitleFontName = "Arial";
        public static double DefaultTitleFontSize = Device.GetNamedSize(NamedSize.Title, typeof(Label));
        public static FontAttributes DefaultTitleFontAttributes = FontAttributes.Bold;
        public static Xamarin.Forms.Color DefaultTitleFontColor = Xamarin.Forms.Color.Black;
        public static Xamarin.Forms.TextAlignment DefaultTitleTextAlignment = Xamarin.Forms.TextAlignment.Center;
        public static string DefaultSubtitleFontName = "Arial";
        public static double DefaultSubtitleFontSize = Device.GetNamedSize(NamedSize.Subtitle, typeof(Label));
        public static FontAttributes DefaultSubtitleFontAttributes = FontAttributes.None;
        public static Xamarin.Forms.Color DefaultSubtitleFontColor = Xamarin.Forms.Color.Black;
        public static Xamarin.Forms.TextAlignment DefaultSubtitleTextAlignment = Xamarin.Forms.TextAlignment.Start; // Center;

        public static readonly BindableProperty TypeProperty = BindableProperty.Create(nameof(Type), typeof(CalloutType), typeof(MapView), default(CalloutType));
        public static readonly BindableProperty AnchorProperty = BindableProperty.Create(nameof(Anchor), typeof(Xamarin.Forms.Point), typeof(MapView), default(Xamarin.Forms.Point));
        public static readonly BindableProperty ArrowAlignmentProperty = BindableProperty.Create(nameof(ArrowAlignment), typeof(ArrowAlignment), typeof(MapView), default(ArrowAlignment), defaultBindingMode: BindingMode.TwoWay);
        public static readonly BindableProperty ArrowWidthProperty = BindableProperty.Create(nameof(ArrowWidth), typeof(float), typeof(MapView), 12f);
        public static readonly BindableProperty ArrowHeightProperty = BindableProperty.Create(nameof(ArrowHeight), typeof(float), typeof(MapView), 16f);
        public static readonly BindableProperty ArrowPositionProperty = BindableProperty.Create(nameof(ArrowPosition), typeof(float), typeof(MapView), 0.5f);
        public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Xamarin.Forms.Color), typeof(MapView), Xamarin.Forms.Color.White);
        public static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Xamarin.Forms.Color), typeof(MapView), Xamarin.Forms.Color.White);
        public static readonly BindableProperty ShadowWidthProperty = BindableProperty.Create(nameof(ShadowWidth), typeof(float), typeof(MapView), default(float));
        public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(nameof(StrokeWidth), typeof(float), typeof(MapView), default(float));
        public static readonly BindableProperty RotationProperty = BindableProperty.Create(nameof(Rotation), typeof(float), typeof(MapView), default(float));
        public static readonly BindableProperty RotateWithMapProperty = BindableProperty.Create(nameof(RotateWithMap), typeof(bool), typeof(MapView), false);
        public static readonly BindableProperty RectRadiusProperty = BindableProperty.Create(nameof(RectRadius), typeof(float), typeof(MapView), default(float));
        public static readonly BindableProperty PaddingProperty = BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(MapView), new Thickness(6));
        // public static readonly BindableProperty IsCloseVisibleProperty = BindableProperty.Create(nameof(IsCloseVisible), typeof(bool), typeof(MapView), true);
        public static readonly BindableProperty IsClosableByClickProperty = BindableProperty.Create(nameof(IsClosableByClick), typeof(bool), typeof(MapView), true);
        public static readonly BindableProperty ContentProperty = BindableProperty.Create(nameof(Content), typeof(int), typeof(MapView), -1);
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(MapView), default(string));
        public static readonly BindableProperty TitleFontNameProperty = BindableProperty.Create(nameof(TitleFontName), typeof(string), typeof(MapView), DefaultTitleFontName);
        public static readonly BindableProperty TitleFontSizeProperty = BindableProperty.Create(nameof(TitleFontSize), typeof(double), typeof(MapView), DefaultTitleFontSize);
        public static readonly BindableProperty TitleFontAttributesProperty = BindableProperty.Create(nameof(TitleFontAttributes), typeof(FontAttributes), typeof(MapView), DefaultTitleFontAttributes);
        public static readonly BindableProperty TitleFontColorProperty = BindableProperty.Create(nameof(TitleFontColor), typeof(Xamarin.Forms.Color), typeof(MapView), DefaultTitleFontColor);
        public static readonly BindableProperty TitleTextAlignmentProperty = BindableProperty.Create(nameof(TitleTextAlignment), typeof(Xamarin.Forms.TextAlignment), typeof(MapView), DefaultTitleTextAlignment);
        public static readonly BindableProperty SubtitleProperty = BindableProperty.Create(nameof(Subtitle), typeof(string), typeof(MapView), default(string));
        public static readonly BindableProperty SubtitleFontNameProperty = BindableProperty.Create(nameof(SubtitleFontName), typeof(string), typeof(MapView), DefaultSubtitleFontName);
        public static readonly BindableProperty SubtitleFontSizeProperty = BindableProperty.Create(nameof(SubtitleFontSize), typeof(double), typeof(MapView), DefaultSubtitleFontSize);
        public static readonly BindableProperty SubtitleFontAttributesProperty = BindableProperty.Create(nameof(SubtitleFontAttributes), typeof(FontAttributes), typeof(MapView), DefaultSubtitleFontAttributes);
        public static readonly BindableProperty SubtitleFontColorProperty = BindableProperty.Create(nameof(SubtitleFontColor), typeof(Xamarin.Forms.Color), typeof(MapView), DefaultSubtitleFontColor);
        public static readonly BindableProperty SubtitleTextAlignmentProperty = BindableProperty.Create(nameof(SubtitleTextAlignment), typeof(Xamarin.Forms.TextAlignment), typeof(MapView), DefaultSubtitleTextAlignment);

        public Callout(Pin pin)
        {
            if (pin == null)
            {
                throw new ArgumentNullException("Pin shouldn't be null");
            }

            _pin = pin;
            if (_pin.Feature != null)
                Feature = (Feature)_pin.Feature.Copy();
            else
                Feature = new Feature();
            Feature.Styles.Clear();
        }

        /// <summary>
        /// Type of Callout
        /// </summary>
        public CalloutType Type
        {
            get { return (CalloutType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        /// <summary>
        /// Anchor position of Callout
        /// </summary>
        public Xamarin.Forms.Point Anchor
        {
            get { return (Xamarin.Forms.Point)GetValue(AnchorProperty); }
            set { SetValue(AnchorProperty, value); }
        }

        /// <summary>
        /// Anchor position of Callout
        /// </summary>
        public ArrowAlignment ArrowAlignment
        {
            get { return (ArrowAlignment)GetValue(ArrowAlignmentProperty); }
            set { SetValue(ArrowAlignmentProperty, value); }
        }

        /// <summary>
        /// Width of opening of anchor of Callout
        /// </summary>
        public float ArrowWidth
        {
            get { return (float)GetValue(ArrowWidthProperty); }
            set { SetValue(ArrowWidthProperty, value); }
        }

        /// <summary>
        /// Height of anchor of Callout
        /// </summary>
        public float ArrowHeight
        {
            get { return (float)GetValue(ArrowHeightProperty); }
            set { SetValue(ArrowHeightProperty, value); }
        }

        /// <summary>
        /// Relative position of anchor of Callout on the side given by AnchorType
        /// </summary>
        public float ArrowPosition
        {
            get { return (float)GetValue(ArrowPositionProperty); }
            set { SetValue(ArrowPositionProperty, value); }
        }

        /// <summary>
        /// Color of stroke around Callout
        /// </summary>
        public Xamarin.Forms.Color Color
        {
            get { return (Xamarin.Forms.Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        /// <summary>
        /// BackgroundColor of Callout
        /// </summary>
        public Xamarin.Forms.Color BackgroundColor
        {
            get { return (Xamarin.Forms.Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        /// <summary>
        /// Shadow width around Callout
        /// </summary>
        public float ShadowWidth
        {
            get { return (float)GetValue(ShadowWidthProperty); }
            set { SetValue(ShadowWidthProperty, value); }
        }

        /// <summary>
        /// Stroke width of frame around Callout
        /// </summary>
        public float StrokeWidth
        {
            get { return (float)GetValue(StrokeWidthProperty); }
            set { SetValue(StrokeWidthProperty, value); }
        }

        /// <summary>
        /// Rotation of Callout around the anchor
        /// </summary>
        public float Rotation
        {
            get { return (float)GetValue(RotationProperty); }
            set { SetValue(RotationProperty, value); }
        }

        /// <summary>
        /// Rotate of Callout with map
        /// </summary>
        public bool RotateWithMap
        {
            get { return (bool)GetValue(RotateWithMapProperty); }
            set { SetValue(RotateWithMapProperty, value); }
        }

        /// <summary>
        /// Radius of rounded corners of Callout
        /// </summary>
        public float RectRadius
        {
            get { return (float)GetValue(RectRadiusProperty); }
            set { SetValue(RectRadiusProperty, value); }
        }

        /// <summary>
        /// Padding around content of Callout
        /// </summary>
        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

        /// <summary>
        /// Is Callout visible on map
        /// </summary>
        public bool IsVisible
        {
            get { return _pin.IsCalloutVisible(); }
        }

        /// <summary>
        /// Is Callout closable by a click on the callout
        /// </summary>
        public bool IsClosableByClick
        {
            get { return (bool)GetValue(IsClosableByClickProperty); }
            private set { SetValue(IsClosableByClickProperty, value); }
        }

        /// <summary>
        /// Content of Callout
        /// </summary>
        public int Content
        {
            get { return (int)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        /// <summary>
        /// Content of Callout header
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// Font name to use rendering title
        /// </summary>
        public string TitleFontName
        {
            get { return (string)GetValue(TitleFontNameProperty); }
            set { SetValue(TitleFontNameProperty, value); }
        }

        /// <summary>
        /// Font size to rendering title
        /// </summary>
        public double TitleFontSize
        {
            get { return (double)GetValue(TitleFontSizeProperty); }
            set { SetValue(TitleFontSizeProperty, value); }
        }

        /// <summary>
        /// Font attributes to render title
        /// </summary>
        public FontAttributes TitleFontAttributes
        {
            get { return (FontAttributes)GetValue(TitleFontAttributesProperty); }
            set { SetValue(TitleFontAttributesProperty, value); }
        }

        /// <summary>
        /// Font color to render title
        /// </summary>
        public Xamarin.Forms.Color TitleFontColor
        {
            get { return (Xamarin.Forms.Color)GetValue(TitleFontColorProperty); }
            set { SetValue(TitleFontColorProperty, value); }
        }

        /// <summary>
        /// Text alignment to render title
        /// </summary>
        public Xamarin.Forms.TextAlignment TitleTextAlignment
        {
            get { return (Xamarin.Forms.TextAlignment)GetValue(TitleTextAlignmentProperty); }
            set { SetValue(TitleTextAlignmentProperty, value); }
        }

        /// <summary>
        /// Content of Callout detail label
        /// </summary>
        public string Subtitle
        {
            get { return (string)GetValue(SubtitleProperty); }
            set { SetValue(SubtitleProperty, value); }
        }

        /// <summary>
        /// Font name to use rendering subtitle
        /// </summary>
        public string SubtitleFontName
        {
            get { return (string)GetValue(SubtitleFontNameProperty); }
            set { SetValue(SubtitleFontNameProperty, value); }
        }

        /// <summary>
        /// Font size to rendering subtitle
        /// </summary>
        public double SubtitleFontSize
        {
            get { return (double)GetValue(SubtitleFontSizeProperty); }
            set { SetValue(SubtitleFontSizeProperty, value); }
        }

        /// <summary>
        /// Font attributes to render subtitle
        /// </summary>
        public FontAttributes SubtitleFontAttributes
        {
            get { return (FontAttributes)GetValue(SubtitleFontAttributesProperty); }
            set { SetValue(SubtitleFontAttributesProperty, value); }
        }

        /// <summary>
        /// Font color to render subtitle
        /// </summary>
        public Xamarin.Forms.Color SubtitleFontColor
        {
            get { return (Xamarin.Forms.Color)GetValue(SubtitleFontColorProperty); }
            set { SetValue(SubtitleFontColorProperty, value); }
        }

        /// <summary>
        /// Text alignment to render title
        /// </summary>
        public Xamarin.Forms.TextAlignment SubtitleTextAlignment
        {
            get { return (Xamarin.Forms.TextAlignment)GetValue(SubtitleTextAlignmentProperty); }
            set { SetValue(SubtitleTextAlignmentProperty, value); }
        }

        /// <summary>
        /// Feature, which belongs to callout. Should be the same as for the pin.
        /// </summary>
        public Feature Feature { get; }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);

            if (!_updating && Type != CalloutType.Custom && propertyName.Equals(nameof(Content)))
            {
                Type = CalloutType.Custom;
            }

            if (!_updating && IsVisible && (propertyName.Equals(nameof(Title))
                || propertyName.Equals(nameof(Subtitle))
                || propertyName.Equals(nameof(Content))
                || propertyName.Equals(nameof(Type))
                || propertyName.Equals(nameof(TitleFontName))
                || propertyName.Equals(nameof(TitleFontSize))
                || propertyName.Equals(nameof(TitleFontAttributes))
                || propertyName.Equals(nameof(TitleFontColor))
                || propertyName.Equals(nameof(TitleTextAlignment))
                || propertyName.Equals(nameof(SubtitleFontName))
                || propertyName.Equals(nameof(SubtitleFontSize))
                || propertyName.Equals(nameof(SubtitleFontAttributes))
                || propertyName.Equals(nameof(SubtitleFontColor))
                || propertyName.Equals(nameof(SubtitleTextAlignment)))
                )
            {
                UpdateContent();
                UpdateCalloutStyle();
            }
            else if (IsVisible && propertyName.Equals(nameof(ArrowAlignment))
                || propertyName.Equals(nameof(ArrowWidth))
                || propertyName.Equals(nameof(ArrowHeight))
                || propertyName.Equals(nameof(ArrowPosition))
                || propertyName.Equals(nameof(Anchor))
                || propertyName.Equals(nameof(IsVisible))
                || propertyName.Equals(nameof(Padding))
                || propertyName.Equals(nameof(Color))
                || propertyName.Equals(nameof(BackgroundColor))
                || propertyName.Equals(nameof(RectRadius)))
            {
                UpdateCalloutStyle();
            }
            else if (!_updating && IsVisible)
            {
                throw new Exception("Unknown property name");
            }
        }

        /// <summary>
        /// Callout is touched
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">SKTouchEventArgs</param>
        private void HandleCalloutClicked(object sender, EventArgs e)
        {
            CalloutClicked?.Invoke(this, e);
        }

        /// <summary>
        /// Checks type of Callout and activates correct content
        /// </summary>
        private void UpdateContent()
        {
            if (Type == CalloutType.Custom)
                return;

            _updating = true;

            if (_textBlock == null)
            {
                _textBlock = new TextBlock();
            }
            else
            {
                _textBlock.Clear();
            }

            var styleTitle = new Topten.RichTextKit.Style()
            {
                FontFamily = TitleFontName,
                FontSize = (float)TitleFontSize,
                FontItalic = (TitleFontAttributes & FontAttributes.Italic) == FontAttributes.Italic,
                FontWeight = TitleFontAttributes == FontAttributes.Bold ? 700 : 400,
                TextColor = new SkiaSharp.SKColor((byte)(TitleFontColor.R * 256), (byte)(TitleFontColor.G * 256), (byte)(TitleFontColor.B * 256)),
            };

            var styleSubtitle = new Topten.RichTextKit.Style()
            {
                FontFamily = SubtitleFontName,
                FontSize = (float)SubtitleFontSize,
                FontItalic = (SubtitleFontAttributes & FontAttributes.Italic) == FontAttributes.Italic,
                FontWeight = SubtitleFontAttributes == FontAttributes.Bold ? 700 : 400,
                TextColor = new SkiaSharp.SKColor((byte)(SubtitleFontColor.R * 256), (byte)(SubtitleFontColor.G * 256), (byte)(SubtitleFontColor.B * 256)),
            };

            switch (Type)
            {
                case CalloutType.Single:
                    _textBlock.Alignment = (Topten.RichTextKit.TextAlignment)(TitleTextAlignment + 1);
                    _textBlock.AddText(Title, styleTitle);
                    CreateContent();
                    break;
                case CalloutType.Detail:
                    _textBlock.Alignment = (Topten.RichTextKit.TextAlignment)(TitleTextAlignment + 1);
                    _textBlock.AddText(Title, styleTitle);
                    _textBlock.AddText("\n", styleTitle);
                    _textBlock.AddText(Subtitle, styleSubtitle);
                    CreateContent();
                    break;
                case CalloutType.Custom:
                    break;
            }

            _updating = false;
        }

        /// <summary>
        /// Update CalloutStyle of Feature
        /// </summary>
        private void UpdateCalloutStyle()
        {
            CalloutStyle style = (CalloutStyle)Feature.Styles.Where((s) => s is CalloutStyle).FirstOrDefault();

            if (style == null)
            {
                style = new CalloutStyle();
                Feature.Styles.Add(style);
            }

            style.ArrowAlignment = (Mapsui.Styles.ArrowAlignment)ArrowAlignment;
            style.ArrowHeight = ArrowHeight;
            style.ArrowPosition = ArrowPosition;
            style.BackgroundColor = BackgroundColor.ToMapsui(); ;
            style.Color = Color.ToMapsui();
            style.Offset = new Geometries.Point(Anchor.X, Anchor.Y);
            style.Padding = new BoundingBox(Padding.Left, Padding.Top, Padding.Right, Padding.Bottom);
            style.RectRadius = RectRadius;
            style.RotateWithMap = RotateWithMap;
            style.Rotation = Rotation;
            style.ShadowWidth = ShadowWidth;
            style.StrokeWidth = StrokeWidth;
            style.Content = Content;

            var margin = Padding;

            switch (ArrowAlignment)
            {
                case ArrowAlignment.Bottom:
                    margin.Bottom += ArrowHeight;
                    break;
                case ArrowAlignment.Top:
                    margin.Top += ArrowHeight;
                    break;
                case ArrowAlignment.Left:
                    margin.Left += ArrowHeight;
                    break;
                case ArrowAlignment.Right:
                    margin.Right += ArrowHeight;
                    break;
            }

            _pin?.MapView?.Refresh();
        }

        /// <summary>
        /// Update content and style of callout before display it the first time
        /// </summary>
        internal void Update()
        {
            UpdateContent();
            UpdateCalloutStyle();
        }

        /// <summary>
        /// Create content BitmapId from given TextBlock
        /// </summary>
        private void CreateContent()
        {
            // Layout TextBlock
            _textBlock.Layout();
            // Create bitmap from TextBlock
            var info = new SKImageInfo((int)(_textBlock.MeasuredWidth+1), (int)(_textBlock.MeasuredHeight+1), SKImageInfo.PlatformColorType, SKAlphaType.Premul);
            var memStream = new MemoryStream();
            using (var bitmap = new SKBitmap(info))
            using (var canvas = new SKCanvas(bitmap))
            using (SKManagedWStream wstream = new SKManagedWStream(memStream))
            {
                canvas.Clear(SKColors.Transparent);
                // surface.Canvas.Scale(DeviceDpi / 96.0f);
                _textBlock.Paint(canvas);
                SKPixmap.Encode(wstream, bitmap, SKEncodedImageFormat.Png, 100);
                Content = BitmapRegistry.Instance.Register(memStream);
            }
        }

        /// <summary>
        /// Called, when Callout close button is pressed
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event arguments</param>
        private void CloseCalloutClicked(object sender, EventArgs e)
        {
            CalloutClosed?.Invoke(this, new EventArgs());
        }
    }
}
