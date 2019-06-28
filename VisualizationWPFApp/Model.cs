using Core;
using System.Collections.ObjectModel;
using System.Drawing;

namespace VisualizationWPFApp
{
    public class Model
    {
        public ObservableCollection<ProblemVisualization> ProblemList { get; private set; }
        public ObservableCollection<ProblemVisualization> RecentList { get; set; }
        public ProblemVisualization SelectedProblem { get; set; }
        public ProblemVisualization SelectedRecent { get; set; }

        public Bitmap GetVisualization()
        {
            Bitmap toShow = new Bitmap(550, 550);
            Graphics g = Graphics.FromImage(toShow);

            foreach (var item in RecentList)
            {
                if (item.Visible == true)
                    g.DrawImage(item.Visualization, new Point(0, 0));
            }

            g.Dispose();

            return toShow;
        }

        public void LoadProblemList()
        {
            ProblemList.Clear();

            // PROBLEM CLASSES LIST
            ProblemList.Add(new VisualizationTest.DrawRect());
            ProblemList.Add(new PerlinNoise.PerlinNoise());
            ProblemList.Add(new Labirynths.RandomizedDepthFirstSearch());
            ProblemList.Add(new Labirynths.RecursiveDivision());
            ProblemList.Add(new CellularAutomata.Elementary());
            ProblemList.Add(new DijkstraAlgorithm.DijkstraAlgorithm());
            ProblemList.Add(new ParticleSwarmOptimization.ParticleSwarmOptimization());
            // ---
            SelectedProblem = ProblemList[0];           
        }

        public Model()
        {
            ProblemList = new ObservableCollection<ProblemVisualization>();
            RecentList = new ObservableCollection<ProblemVisualization>();
        }
    }
}
