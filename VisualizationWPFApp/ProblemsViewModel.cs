using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
                PropertyChanged(this, new PropertyChangedEventArgs("ProblemSettings"));
            }
        }

        public UserControl ProblemSettings
        {
            get
            {
                switch (model.SelectedProblem)
                {
                    case "Warcaby":
                        return new SettingsMenus.WarcabySettings();
                    case "Plansza 2D":
                        return new SettingsMenus.Plansza2DSettings();
                    case "Kółko i Krzyżyk":
                        return new SettingsMenus.KółkoIKrzyżykSettings();
                    case "Labirynt":
                        return new SettingsMenus.LabiryntSettings();
                    default:
                        return null;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
