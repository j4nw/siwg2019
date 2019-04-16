using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public abstract class ProblemVisualization
    {
        public ProblemVisualizationSettings Settings { get; set; }        
        
        public abstract Bitmap Visualization { get; }

        public string Name { get; set; }

        public bool Visible { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public ProblemVisualization()
        {
            Settings = new ProblemVisualizationSettings();
            Name = "Default Problem";
            Visible = false;
        }
    }
}
