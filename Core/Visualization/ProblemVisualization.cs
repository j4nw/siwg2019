using System.ComponentModel;
using System.Drawing;

namespace Core
{
    public abstract class ProblemVisualization : INotifyPropertyChanged
    {
        protected bool visible;
        public ProblemVisualizationSettings Settings { get; set; }        
        
        public abstract Bitmap Visualization { get; }

        public string Name { get; set; }

        public static int Count { get; set; }
        public int Number { get; private set; }

        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Visible"));
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public ProblemVisualization()
        {
            Settings = new ProblemVisualizationSettings();
            Name = "Default Problem";
            Number = Count;
            Visible = true;
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
