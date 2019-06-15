using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Core;

namespace Labirynths
{
    public class RandomizedDepthFirstSearch : ProblemVisualization, ILabirynthGenerationMethod
    {
        private Random random;
        private Labirynth labirynth;
        private bool[,] visited;
        private IEnumerator<Labirynth> sequence;

        public RandomizedDepthFirstSearch()
        {
            Name = "Maze Generation: Randomized DFS";
            Settings.Add("Width", "10");
            Settings.Add("Height", "10");
            Settings.Add("Wall Size", "2");
            Settings.Add("Cell Size", "20");
            Settings.Add("Step By Step (y/n)", "y");
        }

        public Labirynth Generate(int width, int height)
        {
            random = new Random();
            labirynth = new Labirynth(width, height);
            visited = new bool[width, height];

            var x = random.Next(width);
            var y = random.Next(height);

            DFS(x, y).ToList(); // ToList guarantees IEnumerable is exhausted and the DFS is performed

            return labirynth;
        }

        IEnumerable<Labirynth> DFS(int x, int y)
        {
            if (visited[x, y]) yield return null;
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

                yield return labirynth;
                foreach (var m in DFS(adjacent.x, adjacent.y))
                {
                    yield return m;
                }
            }
        }

        public override Bitmap Visualization
        {
            get
            {
                var width = Settings.GetIntValue("Width");
                var height = Settings.GetIntValue("Height");
                var cellSize = Settings.GetIntValue("Cell Size");
                var wallSize = Settings.GetIntValue("Wall Size");
                var showStepByStep = Settings.GetStringValue("Step By Step (y/n)");
                if (showStepByStep == "n")
                {
                    var maze = Generate(width, height);
                    return new Bitmap(maze.Visualize(wallSize, cellSize));
                }

                if (sequence == null)
                {

                    random = new Random();
                    labirynth = new Labirynth(width, height);
                    visited = new bool[width, height];

                    var x = random.Next(width);
                    var y = random.Next(height);

                    sequence = DFS(x, y).GetEnumerator();
                }

                sequence.MoveNext();
                var currentMaze = sequence.Current;
                return new Bitmap(currentMaze.Visualize(wallSize, cellSize));

            }
        }
    }
}