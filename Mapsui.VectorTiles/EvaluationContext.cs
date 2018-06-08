namespace Mapsui.VectorTiles
{
    /// <summary>
    /// Context for which the style should be evaluated
    /// </summary>
    public class EvaluationContext
    {
        public float? Zoom { get; }
        public VectorTileFeature Feature { get; }

        public EvaluationContext(VectorTileFeature feature) : this(null, feature)
        { }

        public EvaluationContext(float? zoom, VectorTileFeature feature = null)
        {
            Zoom = zoom;
            Feature = feature;
        }
    }
}
