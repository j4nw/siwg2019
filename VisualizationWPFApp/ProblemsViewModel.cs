using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualizationWPFApp
{
    public class ProblemsViewModel : INotifyPropertyChanged
    {
        private ProblemsModel model = new ProblemsModel();

        public List<string> Problems { get { return model.Problems; } }

        public string SelectedProblem
        {
            get
            {
                return model.SelectedProblem;
            }
            set
            {
                model.SelectedProblem = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedProblem"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
