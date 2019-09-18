using Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bee
{
    public class Bee : ProblemVisualization
    {
        private const int C = 5;       
        private List<Individual> population;
        private Individual bestIndividual;
        public override System.Drawing.Bitmap Visualization
        {
            get
            {
                Bitmap image = new Bitmap(Settings.GetIntValue("Map width") * C, Settings.GetIntValue("Map height") * C);
                Graphics g = Graphics.FromImage(image);
                g.Clear(Color.BlueViolet);
                g.FillRectangle(new SolidBrush(Color.GreenYellow),
                    new Rectangle(
                        new Point((Settings.GetIntValue("Target X") - 3) * C, (Settings.GetIntValue("Target Y") - 3) * C),
                        new Size(6 * C, 6 * C)));

                NextGeneration();
                bestIndividual = Score().First().Value;
                PrintPopulation(g);

                return image;
            }
        }

        public Bee()
        {
            population = new List<Individual>();
            Name = "BeeAlgorithm";
            Settings.Add("Population count", "10");
            Settings.Add("Map width", "100");
            Settings.Add("Map height", "100");
            Settings.Add("Best areas count", "4");
            Settings.Add("Target X", "74");
            Settings.Add("Target Y", "36");
            population = GenerateRandomPopulation(Settings.GetIntValue("Best areas count"),
                    Settings.GetIntValue("Map width"), Settings.GetIntValue("Map height"));
        }

        private SortedList<int, Individual> Score()
        {
            SortedList<int, Individual> scores = new SortedList<int, Individual>(new DuplicateKeyComparer<int>());
            foreach (var item in population)
            {
                scores.Add(GetWorth(item), item);
            }
            return scores;
        }

        private int GetWorth(Individual individual)
        {
            int targetX = Settings.GetIntValue("Target X");
            int targetY = Settings.GetIntValue("Target Y");
            return (int)Math.Sqrt(Math.Pow(targetX - individual.X, 2) + Math.Pow(targetY - individual.Y, 2));
        }

        private void PrintPopulation(Graphics g)
        {
            foreach (var item in population)
            {
                g.FillRectangle(new SolidBrush(Color.DarkRed),
                    new Rectangle(
                        new Point(item.X * C, item.Y * C),
                        new Size(C, C)));
            }
        }

        private List<Individual> GenerateRandomPopulation(int count, int width, int height)
        {
            List<Individual> newPopulation = new List<Individual>();
            Random random = new Random();

            for (int i = 0; i < count; i++)
            {
                newPopulation.Add(new Individual(random.Next(0, width), random.Next(0, height)));
            }

            return newPopulation;
        }

        private void NextGeneration()
        {
            SortedList<int, Individual> scores = Score();
            int bestAreasCount = Settings.GetIntValue("Best areas count");
            int width = Settings.GetIntValue("Map width");
            int height = Settings.GetIntValue("Map height");
            List<Individual> nextGen = new List<Individual>();

            for (int i = 0; i < bestAreasCount; i++)
            {
                nextGen.Add(Neighborhood(scores.First().Value).First().Value);
                scores.RemoveAt(0);
            }

            nextGen.AddRange(GenerateRandomPopulation(bestAreasCount, width, height));

            population = nextGen;
        }

        private SortedList<int, Individual> Neighborhood(Individual individual)
        {
            SortedList<int, Individual> neighbors = new SortedList<int, Individual>(new DuplicateKeyComparer<int>());
            for (int i = individual.X - 1; i < individual.X + 2; i++)
            {
                for (int j = individual.Y - 1; j < individual.Y + 2; j++)
                {
                    Individual neighbor = new Individual(i, j);
                    neighbors.Add(GetWorth(neighbor), neighbor);
                }
            }
            return neighbors;
        }

        private class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
        {
            public int Compare(TKey x, TKey y)
            {
                int result = x.CompareTo(y);

                if (result == 0)
                    return 1;
                else
                    return result;
            }
        }
    }
}
