using System.Drawing.Imaging;

namespace Labirynths.CLI
{
    class Program
    {
        static void Main()
        {
            var method = new RandomizedDepthFirstSearch();
            var labirynth = method.Generate(100, 100);

            var image = labirynth.Visualize(1, 5);
            image.Save("labirynth.png", ImageFormat.Png);
        }
    }
}
