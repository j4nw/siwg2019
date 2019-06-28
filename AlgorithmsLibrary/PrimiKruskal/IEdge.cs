
namespace AlgorithmsLibrary.PrimiKruskal
{
    public interface IEdge<TVertex>
    {
        double Weight { get; }
        TVertex Start { get; }
        TVertex End { get; }
    }
}

