using Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VisualizationWPFApp
{
    public class ViewModel : INotifyPropertyChanged
    {
        private AddVisualizationWindow visualWindow = null;
        private Model model = new Model();
        private ICommand showModalCommand = null;
        private ICommand submitModalCommand = null;
        private ICommand showHideImageCommand = null;
        private ICommand hideAllCommand = null;
        private int historyCount = 0;

        public ObservableCollection<ProblemVisualization> ProblemList { get { return model.ProblemList; } }
        public ObservableCollection<ProblemVisualization> ProblemHistory { get { return model.ProblemHistory; } }

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
            }
        }

        public Bitmap Visualization
        {
            get
            {
                return model.Visualization;
            }
            set
            {
                model.Visualization = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Vizualisation"));
            }
        }

        public ICommand ShowModalCommand
        {
            get
            {
                if (showModalCommand == null)
                {
                    showModalCommand = new RelayCommand(m =>
                    {
                        model.LoadProblemList();
                        visualWindow = new AddVisualizationWindow();
                        visualWindow.DataContext = this;
                        visualWindow.Show();
                    });
                }
                return showModalCommand;
            }
        }

        public ICommand SubmitModalCommand
        {
            get
            {
                if (submitModalCommand == null)
                {
                    submitModalCommand = new RelayCommand(m =>
                    {
                        SelectedOnList.Name += " " + historyCount;
                        historyCount++;
                        ProblemHistory.Add(SelectedOnList);
                        visualWindow.Close();
                        PropertyChanged(this, new PropertyChangedEventArgs("ProblemHistory"));
                    });
                }
                return submitModalCommand;
            }
        }

        public ICommand ShowHideImageCommand
        {
            get
            {
                if (showHideImageCommand == null)
                {
                    showHideImageCommand = new RelayCommand(m =>
                    {
                        if (SelectedOnHistory != null)
                        {
                            Visualization = SelectedOnHistory.Visualization;
                            PropertyChanged(this, new PropertyChangedEventArgs("Visualization"));
                        }
                    });
                }
                return showHideImageCommand;
            }
        }

        public ICommand HideAllCommand
        {
            get
            {
                if (hideAllCommand == null)
                {
                    hideAllCommand = new RelayCommand(m =>
                    {
                        Visualization = null;
                        PropertyChanged(this, new PropertyChangedEventArgs("Visualization"));
                    });
                }
                return hideAllCommand;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
