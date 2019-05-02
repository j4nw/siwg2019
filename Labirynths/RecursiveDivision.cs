using System;

namespace Labirynths
{
    public class RecursiveDivision : ILabirynthGenerationMethod
    {
        private Labirynth labirynth;
        private Random random;

        public Labirynth Generate(int width, int height)
        {
            random = new Random();

            labirynth = new Labirynth(width, height);

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var node = labirynth[x, y];
                    node.right = x < width - 1;
                    node.down = y < height - 1;
                    labirynth[x, y] = node;
                }
            }

            Divide(0, 0, width, height);

            return labirynth;
        }

        private void Divide(int x, int y, int w, int h)
        {
            if (w < 2 || h < 2)
            {
                return;
            }

            if (w < h || w == h && random.NextDouble() > 0.5) // divide horizontally
            {
                var row = random.Next(y, y + h - 1);
                var col = random.Next(x, x + w);

                for (var i = x; i < x + w; i++)
                {
                    if (i == col) continue;
                    var node = labirynth[i, row];
                    node.down = false;
                    labirynth[i, row] = node;
                }

                var hNorth = row - y + 1;
                var hSouth = h + y - row - 1;

                Divide(x, y, w, hNorth);
                Divide(x, y + hNorth, w, hSouth);
            }
            else // divide vertically
            {
                var row = random.Next(y, y + h);
                var col = random.Next(x, x + w - 1);

                for (var i = y; i < y + h; i++)
                {
                    if (i == row) continue;
                    var node = labirynth[col, i];
                    node.right = false;
                    labirynth[col, i] = node;
                }

                var wWest = col - x + 1;
                var wEast = w + x - col - 1;

                Divide(x, y, wWest, h);
                Divide(x + wWest, y, wEast, h);
            }
        }
    }
}