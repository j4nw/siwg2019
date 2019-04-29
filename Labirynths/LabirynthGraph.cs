﻿using System.Collections.Generic;
using Core;

namespace Labirynths
{
    public class LabirynthGraph : IGraph<(int x, int y), ((int x, int y) a, (int x, int y) b)>
    {
        // x = 0, y = 0 is the upper left (north west) corner

        protected (bool n, bool w, bool s, bool e)[,] matrix;

        public int Width { get; }
        public int Height { get; }

        public LabirynthGraph(int width, int height)
        {
            Width = width;
            Height = height;
            matrix = new (bool n, bool w, bool s, bool e)[width, height];
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
                        if (matrix[x, y].s)
                        {
                            yield return ((x, y), (x, y + 1));
                        }

                        if (matrix[x, y].e)
                        {
                            yield return ((x, y), (x + 1, y));
                        }
                    }
                }
            }
        }

        public IEnumerable<((int x, int y) a, (int x, int y) b)> IncidentEdges((int x, int y) vertex)
        {
            if (matrix[vertex.x, vertex.y].n) yield return (vertex, (vertex.x, vertex.y - 1));
            if (matrix[vertex.x, vertex.y].w) yield return (vertex, (vertex.x - 1, vertex.y));
            if (matrix[vertex.x, vertex.y].s) yield return (vertex, (vertex.x, vertex.y + 1));
            if (matrix[vertex.x, vertex.y].e) yield return (vertex, (vertex.x + 1, vertex.y));
        }
    }
}
