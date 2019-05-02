using System.Collections.Generic;
using System.Drawing;
using Core;

namespace Labirynths
{
    public class Labirynth : IGraph<(int x, int y), ((int x, int y) a, (int x, int y) b)>
    {
        // x = 0, y = 0 is the upper left (north west) corner

        private (bool down, bool right)[,] matrix;

        public int Width { get; }
        public int Height { get; }

        public Labirynth(int width, int height)
        {
            Width = width;
            Height = height;
            matrix = new (bool down, bool right)[width, height];
        }

        protected internal (bool down, bool right) this[int x, int y]
        {
            get => matrix[x, y];
            set => matrix[x, y] = value;
        }

        public IEnumerable<(int x, int y)> Vertices
        {
            get
            {
                for (var x = 0; x < Width; x++)
                {
                    for (var y = 0; y < Height; y++)
                    {
                        yield return (x, y);
                    }
                }
            }
        }

        public IEnumerable<((int x, int y) a, (int x, int y) b)> Edges
        {
            get
            {
                for (var x = 0; x < Width; x++)
                {
                    for (var y = x; y < Height; y++)
                    {
                        if (matrix[x, y].down)
                        {
                            yield return ((x, y), (x, y + 1));
                        }

                        if (matrix[x, y].right)
                        {
                            yield return ((x, y), (x + 1, y));
                        }
                    }
                }
            }
        }

        public IEnumerable<((int x, int y) a, (int x, int y) b)> IncidentEdges((int x, int y) vertex)
        {
            if (matrix[vertex.x, vertex.y].down) yield return (vertex, (vertex.x, vertex.y + 1));
            if (matrix[vertex.x, vertex.y].right) yield return (vertex, (vertex.x + 1, vertex.y));
            if (vertex.x > 0 && matrix[vertex.x - 1, vertex.y].right) yield return (vertex, (vertex.x - 1, vertex.y));
            if (vertex.y > 0 && matrix[vertex.x, vertex.y - 1].down) yield return (vertex, (vertex.x, vertex.y - 1));
        }

        public Image Visualize(int wallSize = 2, int cellSize = 20)
        {
            var rects = new List<Rectangle>();
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    var px = wallSize + x * (wallSize + cellSize);
                    var py = wallSize + y * (wallSize + cellSize);
                    rects.Add(new Rectangle(px, py, cellSize, cellSize));
                    if (matrix[x, y].down)
                    {
                        rects.Add(new Rectangle(px, py + cellSize, cellSize, wallSize));
                    }

                    if (matrix[x, y].right)
                    {
                        rects.Add(new Rectangle(px + cellSize, py, wallSize, cellSize));
                    }
                }
            }

            var imageWidth = wallSize + Width * (wallSize + cellSize);
            var imageHeight = wallSize + Height * (wallSize + cellSize);
            var image = new Bitmap(imageWidth, imageHeight);

            using (var graphics = Graphics.FromImage(image))
            {
                graphics.Clear(Color.Black);
                graphics.FillRectangles(Brushes.White, rects.ToArray());
            }

            return image;
        }
    }
}
