using Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace VisualizationWPFApp
{
    public class Model
    {
        public ObservableCollection<ProblemVisualization> ProblemList { get; private set; }
        public ObservableCollection<ProblemVisualization> ProblemHistory { get; set; }
        public ProblemVisualization SelectedOnList { get; set; }
        public ProblemVisualization SelectedOnHistory { get; set; }
        public Bitmap Visualization { get; set; }

        public void LoadProblemList()
        {
            ProblemList.Clear();

            // PROBLEM CLASSES LIST
            ProblemList.Add(new ExampleProblemVisualization());
            ProblemList.Add(new ExampleProblemVisualization());
            // ---
            SelectedOnList = ProblemList[0];
            
        }

        public Model()
        {
            ProblemList = new ObservableCollection<ProblemVisualization>();
            ProblemHistory = new ObservableCollection<ProblemVisualization>();
        }
    }
}
