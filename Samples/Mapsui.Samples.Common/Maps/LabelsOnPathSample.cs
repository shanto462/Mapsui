using Mapsui.Geometries;
using Mapsui.Layers;
using Mapsui.Providers;
using Mapsui.Styles;
using Mapsui.Utilities;
using System.Collections.Generic;

namespace Mapsui.Samples.Common.Maps
{
    public class LabelOnPathSample
    {
        public static Map CreateMap()
        {
            var map = new Map();
            map.Layers.Add(OpenStreetMap.CreateTileLayer());
            map.Layers.Add(CreateLayer());
            return map;
        }

        public static ILayer CreateLayer()
        {
            var features = CreateFeatureWithDefaultStyle();
            var memoryProvider = new MemoryProvider(features);

            return new MemoryLayer {Name = "Paths with labels", DataSource = memoryProvider};
        }

        private static Features CreateFeatureWithDefaultStyle()
        {
            var featureWithDefaultStyle = new Features
            {
                new Feature {
                Geometry = new LineString(new List<Point> { new Point(0, 0), new Point(500000, 800000), new Point(1000000, 1000000), new Point(1500000, 800000), new Point(2000000, 0), new Point(2500000, 800000), new Point(3000000, 1000000) }), },
                new Feature  {
                Geometry = new LineString(new List<Point> { new Point(0, -1000000), new Point(500000, -200000), new Point(1000000, 0), new Point(1500000, -200000), new Point(2000000, -1000000), new Point(2500000, -200000), new Point(3000000, 0) }), },
            };
            featureWithDefaultStyle[0].Styles.Add(new LabelStyle {Text = "Default Label", Spacing = 200});
            featureWithDefaultStyle[1].Styles.Add(new LabelStyle { Text = "Default\nLabel", Spacing = 20 });
            return featureWithDefaultStyle;
        }
    }
}