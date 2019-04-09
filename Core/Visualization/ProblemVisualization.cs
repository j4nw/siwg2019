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
        protected ProblemVisualizationSettings settings = new ProblemVisualizationSettings();
        
        public abstract Bitmap Visualization { get; }

        public string Name { get; set; }

        public string Settings
        {
            get
            {
                return settings.ToString();
            }
            set
            {
                settings.MakeSettingsFromString(value);
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
