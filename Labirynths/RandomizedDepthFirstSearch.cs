﻿using System;
using System.Linq;

namespace Labirynths
{
    public class RandomizedDepthFirstSearch : ILabirynthGenerationMethod
    {
        private Random random;
        private Labirynth labirynth;
        private bool[,] visited;

        public Labirynth Generate(int width, int height)
        {
            random = new Random();
            labirynth = new Labirynth(width, height);
            visited = new bool[width, height];

            var x = random.Next(width);
            var y = random.Next(height);

            DFS(x, y);

            return labirynth;
        }

        void DFS(int x, int y)
        {
            if (visited[x, y]) return;
            visited[x, y] = true;

            var adjacents = labirynth
                .Adjacents((x, y))
                .Where(a => !visited[a.x, a.y])
                .ToArray();

            for (var i = 0; i < adjacents.Length - 1; i++)
            {
                var j = random.Next(i, adjacents.Length);
                var tmp = adjacents[i];
                adjacents[i] = adjacents[j];
                adjacents[j] = tmp;
            }

            foreach (var adjacent in adjacents)
            {
                if (visited[adjacent.x, adjacent.y]) continue;

                if (x != adjacent.x) // carve horizontally
                {
                    if (x < adjacent.x) // carve from node to adjacent
                    {
                        var node = labirynth[x, y];
                        node.right = true;
                        labirynth[x, y] = node;
                    }
                    else // carve from adjacent to node
                    {
                        var node = labirynth[adjacent.x, adjacent.y];
                        node.right = true;
                        labirynth[adjacent.x, adjacent.y] = node;
                    }
                }
                else // carve vertically
                {
                    if (y < adjacent.y) // carve from node to adjacent
                    {
                        var node = labirynth[x, y];
                        node.down = true;
                        labirynth[x, y] = node;
                    }
                    else // carve from adjacent to node
                    {
                        var node = labirynth[adjacent.x, adjacent.y];
                        node.down = true;
                        labirynth[adjacent.x, adjacent.y] = node;
                    }
                }

                DFS(adjacent.x, adjacent.y);
            }
        }
    }
}