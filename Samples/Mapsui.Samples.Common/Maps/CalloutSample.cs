using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.Providers;
using Mapsui.Styles;
using Mapsui.UI;
using Mapsui.Utilities;
using Newtonsoft.Json;
using SkiaSharp;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Mapsui.Samples.Common.Maps
{
    public class CalloutSample : ISample
    {
        private static Random Random = new Random();

        public string Name => "1 Callout";
        public string Category => "Geometries";

        public void Setup(IMapControl mapControl)
        {
            mapControl.Map = CreateMap();
        }

        public static Map CreateMap()
        {
            var map = new Map();

            map.Layers.Add(OpenStreetMap.CreateTileLayer());
            map.Layers.Add(CreatePointLayer());
            map.Home = n => n.NavigateTo(map.Layers[1].Envelope.Centroid, map.Resolutions[5]);
            return map;
        }

        private static MemoryLayer CreatePointLayer()
        {
            return new MemoryLayer
            {
                Name = "Points",
                IsMapInfoLayer = true,
                DataSource = new MemoryProvider(GetCitiesFromEmbeddedResource()),
                Style = new VectorStyle() {  } // CreateBitmapStyle()
            };
        }

        private static IEnumerable<IFeature> GetCitiesFromEmbeddedResource()
        {
            var path = "Mapsui.Samples.Common.EmbeddedResources.congo.json";
            var assembly = typeof(PointsSample).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream(path);
            var cities = DeserializeFromStream<City>(stream);
            
            return cities.Select(c =>
            {
                var feature = new Feature();
                var point = SphericalMercator.FromLonLat(c.Lng, c.Lat);
                feature.Geometry = point;
                feature["name"] = c.Name;
                feature["country"] = c.Country;
                // Create a bitmap of name with Skia
                var bitmapId = -1;
                // Get text size
                SKRect bounds;
                using (SKPaint paint = new SKPaint())
                {
                    paint.Color = new SKColor((byte)Random.Next(0, 256), (byte)Random.Next(0, 256), (byte)Random.Next(0, 256));
                    paint.Typeface = SKTypeface.FromFamilyName(null, SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);
                    paint.TextSize = 20;

                    using (SKPath textPath = paint.GetTextPath(c.Name, 0, 0))
                    {
                        // Set transform to center and enlarge clip path to window height
                        textPath.GetTightBounds(out bounds);
                    }
                    var bitmap = new SKBitmap((int)(bounds.Width + 1), (int)(bounds.Height + 1));
                    var canvas = new SKCanvas(bitmap);
                    canvas.Clear();
                    canvas.DrawText(c.Name, 0, bounds.Height, paint);
                    MemoryStream memStream = new MemoryStream();
                    SKManagedWStream wstream = new SKManagedWStream(memStream);
                    SKPixmap.Encode(wstream, bitmap, SKEncodedImageFormat.Png, 100);
                    bitmapId = BitmapRegistry.Instance.Register(memStream);
                }
                var calloutStyle = new CalloutStyle() { Content = bitmapId, ArrowPosition = Random.Next(1, 9) * 0.1f, RotateWithMap = true };
                switch ((int)Random.Next(0,4))
                {
                    case 0:
                        calloutStyle.ArrowAlignment = ArrowAlignment.Bottom;
                        calloutStyle.Offset = new Geometries.Point(0, SymbolStyle.DefaultHeight * 0.5f);
                        break;
                    case 1:
                        calloutStyle.ArrowAlignment = ArrowAlignment.Left;
                        calloutStyle.Offset = new Geometries.Point(SymbolStyle.DefaultHeight * 0.5f, 0);
                        break;
                    case 2:
                        calloutStyle.ArrowAlignment = ArrowAlignment.Top;
                        calloutStyle.Offset = new Geometries.Point(0, -SymbolStyle.DefaultHeight * 0.5f);
                        break;
                    case 3:
                        calloutStyle.ArrowAlignment = ArrowAlignment.Right;
                        calloutStyle.Offset = new Geometries.Point(-SymbolStyle.DefaultHeight * 0.5f, 0);
                        break;
                }
                calloutStyle.RectRadius = Random.Next(0, 9);
                calloutStyle.ShadowWidth = Random.Next(0, 9);
                feature.Styles.Add(calloutStyle);
                return feature;
            });
        }

        private class City
        {
            public string Country { get; set; }
            public string Name { get; set; }
            public double Lat { get; set; }
            public double Lng { get; set; }
        }

        public static IEnumerable<T> DeserializeFromStream<T>(Stream stream)
        {
            var serializer = new JsonSerializer();

            using (var sr = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(sr))
            {
                return serializer.Deserialize<List<T>>(jsonTextReader);
            }
        }

        private static SymbolStyle CreateBitmapStyle()
        {
            // For this sample we get the bitmap from an embedded resouce
            // but you could get the data stream from the web or anywhere
            // else.
            var path = "Mapsui.Samples.Common.Images.home.png"; // Designed by Freepik http://www.freepik.com
            var bitmapId = GetBitmapIdForEmbeddedResource(path);
            var bitmapHeight = 176; // To set the offset correct we need to know the bitmap height
            return new SymbolStyle { BitmapId = bitmapId, SymbolScale = 0.20, SymbolOffset = new Offset(0, bitmapHeight) };
        }

        private static int GetBitmapIdForEmbeddedResource(string imagePath)
        {
            var assembly = typeof(PointsSample).GetTypeInfo().Assembly;
            var image = assembly.GetManifestResourceStream(imagePath);
            return BitmapRegistry.Instance.Register(image);
        }
    }
}