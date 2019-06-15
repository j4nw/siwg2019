using Core;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Input;
using System.Windows.Threading;

namespace VisualizationWPFApp
{
    public class ViewModel : INotifyPropertyChanged
    {
        private DispatcherTimer timer = null;
        private string playStop;
        private int interval;
        private AddVisualizationWindow visualWindow = null;
        private Model model = new Model();
        private ICommand addNewVisualizationCommand = null;
        private ICommand submitModalCommand = null;
        private ICommand removeRecentCommand = null;
        private ICommand removeAllRecentCommand = null;
        private ICommand hideAllCommand = null;
        private ICommand updateCommand = null;
        private ICommand playStopCommand = null;

        public ObservableCollection<ProblemVisualization> ProblemList { get { return model.ProblemList; } }
        public ObservableCollection<ProblemVisualization> RecentList { get { return model.RecentList; } }

        public int Interval
        {
            get
            {
                return interval;
            }
            set
            {
                interval = value;
                timer.Interval = TimeSpan.FromMilliseconds(interval);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Interval"));
            }
        }

        public string PlayStop
        {
            get
            {
                return playStop;
            }
            set
            {
                playStop = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PlayStop"));
            }
        }

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

        public ViewModel()
        {
            timer = new DispatcherTimer();
            Interval = 30;
            timer.Tick += tick;
            PlayStop = "Play";
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
                        ProblemVisualization.Count++;
                        SelectedProblem.Name = SelectedProblem.Number + ") " + SelectedProblem.Name;
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
                        ProblemVisualization.Count = 0;
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
                        hideAll();
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

        public ICommand PlayStopCommand
        {
            get
            {
                if (playStopCommand == null)
                {
                    playStopCommand = new RelayCommand(item =>
                    {
                        if (!timer.IsEnabled)
                        {
                            PlayStop = "Stop";
                            timer.Start();
                        }
                        else
                        {
                            PlayStop = "Play";
                            timer.Stop();
                        }
                    });
                }
                return playStopCommand;
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void tick(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Visualization"));
        }

        private void hideAll()
        {
            foreach (var item in RecentList)
            {
                item.Visible = false;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Visualization"));
        }
    }
}
