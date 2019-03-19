namespace Core
{
    public interface IBoard<TLayer>
    {
        int Width { get; }
        int Height { get; }
        byte Layer(TLayer layer, int x, int y);
    }
}
