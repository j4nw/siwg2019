using System.Drawing.Imaging;

namespace Labirynths.CLI
{
    class Program
    {
        static void Main()
        {
            var method = new RecursiveDivision();
            var labirynth = method.Generate(30, 30);

            var image = labirynth.Visualize();
            image.Save("labirynth.png", ImageFormat.Png);
        }
    }
}
