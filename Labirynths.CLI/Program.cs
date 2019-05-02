using System;
using System.Drawing.Imaging;

namespace Labirynths.CLI
{
    class Program
    {
        static void Main()
        {
            var labirynth = new Labirynth(30, 30);
            var image = labirynth.Visualize();
            image.Save("labirynth.png", ImageFormat.Png);
        }
    }
}
