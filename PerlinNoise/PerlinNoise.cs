using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PerlinNoise
{
    public class PerlinNoise : ProblemVisualization
    {
        private Random random = new Random();
        private (float x, float y)[,] grad;

        public PerlinNoise()
        {
            Name = "Perlin Noise";
            Settings.Add("Width", "640");
            Settings.Add("Height", "480");
            Settings.Add("Grid Cell Size", "200");
            Settings.Add("Red 0/1", "1");
            Settings.Add("Green 0/1", "0");
            Settings.Add("Blue 0/1", "0");
        }

        public override System.Drawing.Bitmap Visualization
        {
            get
            {
                int dim1 = Settings.GetIntValue("Width");
                int dim2 = Settings.GetIntValue("Height");
                int r = Settings.GetIntValue("Red 0/1");
                int g = Settings.GetIntValue("Green 0/1");
                int b = Settings.GetIntValue("Blue 0/1");

                DrawGradients();
                CreateNoise();

                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(dim1, dim2);

                for (int i = 0; i < dim1; i++)
                {
                    for (int j = 0; j < dim2; j++)
                    {
                        bmp.SetPixel(i, j, System.Drawing.Color.FromArgb(
                            (int)((NoiseTable[i, j] + 1) * 128 * r),
                            (int)((NoiseTable[i, j] + 1) * 128 * g),
                            (int)((NoiseTable[i, j] + 1) * 128 * b)));
                    }
                }

                return bmp;
            }
        }

        public float[,] NoiseTable { get; private set; }

        public void CreateNoise(int dim1 = 0, int dim2 = 0, float gridCellSize = 0)
        {
            dim1 = (dim1 == 0) ? Settings.GetIntValue("Width") : dim1;
            dim2 = (dim2 == 0) ? Settings.GetIntValue("Height") : dim2;
            gridCellSize = (gridCellSize == 0) ? Settings.GetFloatValue("Grid Cell Size") : gridCellSize; 

            NoiseTable = new float[dim1, dim2];

            for (int i = 0; i < dim1; i++)
            {
                for (int j = 0; j < dim2; j++)
                {
                    float x = i / gridCellSize;
                    float y = j / gridCellSize;

                    // Determine grid cell coordinates
                    int x0 = (int)x;
                    int x1 = x0 + 1;
                    int y0 = (int)y;
                    int y1 = y0 + 1;

                    // Determine interpolation weights
                    // Could also use higher order polynomial/s-curve here
                    float sx = smootherstep(0, 1, x - (float)x0);
                    float sy = smootherstep(0, 1, y - (float)y0);

                    // Interpolate between grid point gradients
                    float n0, n1, ix0, ix1;

                    n0 = dotGridGradient(x0, y0, x, y);
                    n1 = dotGridGradient(x1, y0, x, y);
                    ix0 = lerp(n0, n1, sx);

                    n0 = dotGridGradient(x0, y1, x, y);
                    n1 = dotGridGradient(x1, y1, x, y);
                    ix1 = lerp(n0, n1, sx);

                    NoiseTable[i, j] = lerp(ix0, ix1, sy);
                }
            }
        }

        private float lerp(float a0, float a1, float w)
        {
            return (1.0f - w) * a0 + w * a1;
        }

        // Computes the dot product of the distance and gradient vectors.
        private float dotGridGradient(int ix, int iy, float x, float y)
        {
            // Compute the distance vector
            float dx = x - ix;
            float dy = y - iy;

            // Compute the dot-product
            return (dx * grad[ix, iy].x + dy * grad[ix, iy].y);
        }

        private float smootherstep(float edge0, float edge1, float x)
        {
            // Scale, and clamp x to 0..1 range
            x = clamp((x - edge0) / (edge1 - edge0), 0.0f, 1.0f);
            // Evaluate polynomial
            return x * x * x * (x * (x * 6 - 15) + 10);
        }

        private float clamp(float x, float lowerlimit, float upperlimit)
        {
            if (x < lowerlimit)
                x = lowerlimit;
            if (x > upperlimit)
                x = upperlimit;
            return x;
        }

        public void DrawGradients(int dim1 = 0, int dim2 = 0, float gridCellSize = 0)
        {
            dim1 = (dim1 == 0) ? Settings.GetIntValue("Width") : dim1;
            dim2 = (dim2 == 0) ? Settings.GetIntValue("Height") : dim2;
            gridCellSize = (gridCellSize == 0) ? Settings.GetFloatValue("Grid Cell Size") : gridCellSize;

            var width = (int)Math.Ceiling(dim1 / gridCellSize) + 1;
            var height = (int)Math.Ceiling(dim2 / gridCellSize) + 1;

            grad = new (float x, float y)[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grad[x, y] = RandomUnitVector();
                }
            }
        }

        private (float x, float y) RandomUnitVector()
        {
            var angle = random.NextDouble() * 2 * Math.PI;
            var x = (float)Math.Cos(angle);
            var y = (float)Math.Sin(angle);
            return (x, y);
        }        
    }
}
