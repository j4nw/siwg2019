using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Core;

namespace WorldGeneration.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var world = new World(1024, 512);
            world.GeneratePerlinTerrain();
            world.DrawRectangle(100, 100, 100, 100, WorldLayer.Wall);

            var colorsByLayer = new Dictionary<WorldLayer, (Color low, Color high)>
            {
                {WorldLayer.Wall, (Color.Black, Color.Black)},
                {WorldLayer.Water, (Color.DodgerBlue, Color.DodgerBlue)},
                {WorldLayer.Land, (Color.DarkOliveGreen, Color.GreenYellow)}
            };

            var orderedLayers = new List<WorldLayer>
            {
                WorldLayer.Wall,
                WorldLayer.Water,
                WorldLayer.Land
            };

            var image = Visualizer<WorldLayer>.BoardToImage(world, colorsByLayer, orderedLayers);
            image.Save("heightmap.png", ImageFormat.Png);
        }
    }
}
