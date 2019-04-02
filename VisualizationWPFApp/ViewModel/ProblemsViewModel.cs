using Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace VisualizationWPFApp.ViewModel
{
    public class ProblemsViewModel : INotifyPropertyChanged
    {
        private Model.ProblemsModel model = new Model.ProblemsModel();

        public List<ProblemVisualization> ProblemList { get { return model.ProblemList; } }
        public List<ProblemVisualization> ProblemHistory { get { return model.ProblemHistory; } }
        public string SelectedOnListSettings { get { return SelectedOnList.Settings; } }
        public string SelectedOnHistorySettings { get { return SelectedOnHistory.Settings; } }

        public ProblemVisualization SelectedOnList
        {
            get
            {
                return model.SelectedOnList;
            }
            set
            {
                model.SelectedOnList = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedOnList"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedOnListSettings"));
            }
        }

        public ProblemVisualization SelectedOnHistory
        {
            get
            {
                return model.SelectedOnHistory;
            }
            set
            {
                model.SelectedOnHistory = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedOnHistory"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedOnHistorySettings"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
