using Mapsui.Geometries;
using Mapsui.Providers;
using Mapsui.Styles;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mapsui.VectorTiles
{
    public class SymbolProvider : IProvider
    {
        private readonly Map _map;
        private long _lastTime = DateTime.Now.Ticks;
        private ConcurrentBag<Symbol> _symbols;

        public List<IFeature> Bucket { get; } = new List<IFeature>();

        public string CRS { get; set; }

        public BoundingBox Bounds { get; } = new BoundingBox(0, 0, 0, 0);

        public SymbolProvider(Map map)
        {
            _map = map;

            _map.Viewport.ViewportChanged += ViewportChanged;

            _symbols = new ConcurrentBag<Symbol>();
        }

        private void SymbolsChanged()
        {
            if (DateTime.Now.Ticks - _lastTime > 50000)
                Task.Run(() => UpdateData());
        }

        private void UpdateData()
        {
            List<Symbol> allSymbols;

            allSymbols = _symbols.ToArray().OrderBy<Symbol, int>((s) => s.Rank).ToList();

            lock (Bucket)
            {
                Bucket.Clear();

                int maxSymbols = 0;

                foreach (var symbol in allSymbols)
                {
                    var feature = symbol.Feature;

                    feature.Styles.Clear();
                    if (symbol.IconStyle is SymbolStyle && ((SymbolStyle)symbol.IconStyle).BitmapId >= 0)
                        feature.Styles.Add(symbol.IconStyle);
                    feature.Styles.Add(symbol.LabelStyle);

                    Bucket.Add(feature);

                    maxSymbols++;

                    if (maxSymbols > 9)
                        break;
                }
            }

            _lastTime = DateTime.Now.Ticks;
        }

        public void Add(Symbol symbol)
        {
            _symbols.Add(symbol);

            SymbolsChanged();
        }

        public void Clear()
        {
            var newSymbols = new ConcurrentBag<Symbol>();
            Interlocked.Exchange<ConcurrentBag<Symbol>>(ref _symbols, newSymbols);

            SymbolsChanged();
        }

        private void ViewportChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Viewport.Resolution))
            {
                Clear();
            }
        }

        public BoundingBox GetExtents()
        {
            return Bounds;
        }

        public IEnumerable<IFeature> GetFeaturesInView(BoundingBox box, double resolution)
        {
            return Bucket;
        }
    }
}
