using Core;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
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
        private ICommand hideAllCommand = null;
        private ICommand updateCommand = null;
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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedProblem"));
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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedRecent"));
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
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Visualization"));
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
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Visualization"));
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
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Visualization"));
                    });
                }
                return removeAllRecentCommand;
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
                        foreach (var item in RecentList)
                        {
                            item.Visible = false;
                        }
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Visualization"));
                    });
                }
                return hideAllCommand;
            }
        }

        public ICommand UpdateCommand
        {
            get
            {
                if (updateCommand == null)
                {
                    updateCommand = new RelayCommand(item =>
                    {
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Visualization"));
                    });
                }
                return updateCommand;
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
