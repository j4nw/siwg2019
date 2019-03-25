using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualizationWPFApp
{
    public class ProblemsModel
    {
        public List<string> Problems { get; private set; }
        public string SelectedProblem { get; set; }

        public ProblemsModel()
        {
            Problems = new List<string>();
            Problems.Add("Plansza 2D");
            Problems.Add("Warcaby");
            Problems.Add("Kółko i Krzyżyk");
            Problems.Add("Labirynt");
            SelectedProblem = Problems[0];
        }
    }
}
