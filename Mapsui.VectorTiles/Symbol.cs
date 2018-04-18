using Mapsui.Styles;

namespace Mapsui.VectorTiles
{
    public class Symbol
    {
        public VectorTileFeature Feature;
        public IStyle IconStyle { get; }
        public IStyle LabelStyle { get; }
        public int ZIndex { get; }
        public int Rank { get => Feature?.Rank ?? int.MaxValue; }

        public Symbol(VectorTileFeature feature, IStyle iconStyle, IStyle labelStyle, int zIndex)
        {
            Feature = feature;
            IconStyle = iconStyle;
            LabelStyle = labelStyle;
            ZIndex = zIndex;
        }
    }
}
