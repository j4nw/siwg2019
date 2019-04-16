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
        private ICommand addNewVisualizationCommand = null;
        private ICommand submitModalCommand = null;
        private ICommand removeRecentCommand = null;
        private ICommand removeAllRecentCommand = null;
        private ICommand updateVisualizationCommand = null;
        private int historyCount = 0;

        public ObservableCollection<ProblemVisualization> ProblemList { get { return model.ProblemList; } }
        public ObservableCollection<ProblemVisualization> RecentList { get { return model.RecentList; } }

        public ProblemVisualization SelectedProblem
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

        public ProblemVisualization SelectedRecent
        {
            get
            {
                return model.SelectedRecent;
            }
            set
            {
                model.SelectedRecent = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedRecent"));
            }
        }

        public Bitmap Visualization
        {
            get
            {
                return model.GetVisualization();
            }
        }

        public ICommand AddNewVisualizationCommand
        {
            get
            {
                if (addNewVisualizationCommand == null)
                {
                    addNewVisualizationCommand = new RelayCommand(m =>
                    {
                        model.LoadProblemList();
                        visualWindow = new AddVisualizationWindow();
                        visualWindow.DataContext = this;
                        visualWindow.Show();
                    });
                }
                return addNewVisualizationCommand;
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
                        SelectedProblem.Name += " " + historyCount;
                        historyCount++;
                        RecentList.Add(SelectedProblem);
                        visualWindow.Close();
                    });
                }
                return submitModalCommand;
            }
        }

        public ICommand RemoveRecentCommand
        {
            get
            {
                if (removeRecentCommand == null)
                {
                    removeRecentCommand = new RelayCommand(item =>
                    {
                        ProblemVisualization toRemove = item as ProblemVisualization;
                        if (model.RecentList.Contains(toRemove))
                        {
                            model.RecentList.Remove(toRemove);
                        }
                    });
                }
                return removeRecentCommand;
            }
        }

        public ICommand RemoveAllRecentCommand
        {
            get
            {
                if (removeAllRecentCommand == null)
                {
                    removeAllRecentCommand = new RelayCommand(m =>
                    {
                        model.RecentList.Clear();
                    });
                }
                return removeAllRecentCommand;
            }
        }

        public ICommand UpdateVisualizationCommand
        {
            get
            {
                if (updateVisualizationCommand == null)
                {
                    updateVisualizationCommand = new RelayCommand(item =>
                    {
                        (item as ProblemVisualization).Visible = !(item as ProblemVisualization).Visible;
                    });
                }
                return updateVisualizationCommand;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
