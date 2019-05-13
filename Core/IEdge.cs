
namespace Core
{
    public interface IEdge<TVertex>
    {
        double Weight { get; set; }
        TVertex Start { get; }
        TVertex End { get; }
    }
}
