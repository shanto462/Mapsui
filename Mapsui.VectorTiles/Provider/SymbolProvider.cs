using Mapsui.Geometries;
using Mapsui.Providers;
using Mapsui.Styles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapsui.VectorTiles
{
    public class SymbolProvider : IProvider
    {
        private readonly Map _map;
        private long _lastTime = DateTime.Now.Ticks;

        public ObservableCollection<Symbol> Symbols { get; }

        public List<IFeature> Bucket { get; } = new List<IFeature>();

        public string CRS { get; set; }

        public BoundingBox Bounds { get; } = new BoundingBox(0, 0, 0, 0);

        public SymbolProvider(Map map)
        {
            _map = map;

            _map.Viewport.ViewportChanged += ViewportChanged;

            Symbols = new ObservableCollection<Symbol>();

            Symbols.CollectionChanged += SymbolsCollectionChanged;
        }

        private void SymbolsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //if (DateTime.Now.Ticks - _lastTime > 5000000)
            //    Task.Run(() => UpdateData());
        }

        private void UpdateData()
        {
            List<Symbol> allSymbols;

            lock (Symbols)
            {
                allSymbols = Symbols.OrderBy<Symbol, int>((s) => s.Rank).ToList();
            }

            Bucket.Clear();

            int maxSymbols = 0;

            foreach (var symbol in allSymbols)
            {
                var feature = symbol.Feature;

                feature.Styles.Clear();
                feature.Styles.Add(symbol.IconStyle);
                feature.Styles.Add(symbol.LabelStyle);

                Bucket.Add(feature);

                maxSymbols++;

                if (maxSymbols > 9)
                    break;
            }
        }

        public void Add(Symbol symbol)
        {
            lock (Symbols)
            {
                Symbols.Add(symbol);
            }
        }

        private void ViewportChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Viewport.Resolution))
            {
                Symbols.Clear();
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
