
namespace AlgorithmsLibrary.PrimiKruskal
{
    //interfejs krawędzi grafu
    //zapewnia, że krawędź ma wagę i informacje o wierzchołkach, które łączy
    public interface IEdge<TVertex>
    {
        double Weight { get; }
        TVertex Start { get; }
        TVertex End { get; }
    }
}

