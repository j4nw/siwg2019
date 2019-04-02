using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualizationWPFApp.Model
{
    public class ProblemsModel
    {
        public List<ProblemVisualization> ProblemList { get; private set; }
        public List<ProblemVisualization> ProblemHistory { get; set; }
        public ProblemVisualization SelectedOnList { get; set; }
        public ProblemVisualization SelectedOnHistory { get; set; }

        public void LoadProblemList()
        {
            ProblemList.Clear();

            // PROBLEM CLASSES LIST
            ProblemList.Add(new ExampleProblemVisualization());
        }

        public ProblemsModel()
        {
            ProblemList = new List<ProblemVisualization>();
            ProblemHistory = new List<ProblemVisualization>();
        }
    }
}
