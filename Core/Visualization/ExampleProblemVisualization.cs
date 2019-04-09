using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    // ADD REFERENCE IN VisualizationWPFApp TO PROBLEM PROJECT
    // ADD PROBLEM CLASS TO LoadProblemList() METHOD IN VisualizationWPFApp.Model.ProblemsModel

    public class ExampleProblemVisualization : ProblemVisualization
    {
        public ExampleProblemVisualization()
        {
            // name visible on list
            Name = "ShowColorRectangle";

            // default settings
            // settings methods: Add, Remove, GetStringValue, GetIntValue (convertion from string), GetDoubleValue (convertion from string)
            settings.Add("Width", "30"); 
            settings.Add("Height", "30");
            settings.Add("Color", "Blue");
        } 

        public override Bitmap Visualization // get image for visualization
        {
            get
            {
                Bitmap image = new Bitmap(settings.GetIntValue("Width"), settings.GetIntValue("Height"));
                Graphics g = Graphics.FromImage(image);
                switch (settings.GetStringValue("Color"))
                {
                    case "Green":
                        g.Clear(Color.Green);
                        break;
                    case "Red":
                        g.Clear(Color.Red);
                        break;
                    default:
                        g.Clear(Color.Blue);
                        break;
                }
                return image;
            }
        }
    }
}
