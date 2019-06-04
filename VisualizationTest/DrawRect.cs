using Core;
using System.Drawing;

namespace VisualizationTest
{
    public class DrawRect : ProblemVisualization
    {
        private int l = 0;
        public DrawRect()
        {
            // name visible on list
            Name = "Draw Rectangle";

            // default settings as strings
            // Available methods in 'Settings' Class: Add, Remove, GetStringValue, GetIntValue (parse from string), GetDoubleValue (parse from string)
            Settings.Add("Width", "30");
            Settings.Add("Height", "30");
            Settings.Add("Color", "Dynamic");
        }

        public override Bitmap Visualization // get image for visualization
        {
            get
            {
                l++;
                Bitmap image = new Bitmap(Settings.GetIntValue("Width"), Settings.GetIntValue("Height"));
                Graphics g = Graphics.FromImage(image);
                switch (Settings.GetStringValue("Color"))
                {
                    case "Green":
                        g.Clear(Color.Green);
                        break;
                    case "Red":
                        g.Clear(Color.Red);
                        break;
                    case "Dynamic":
                        if (l % 3 == 0)
                        {
                            g.Clear(Color.Red);
                        }
                        else if (l % 3 == 1)
                        {
                            g.Clear(Color.Green);
                        }
                        else
                        {
                            g.Clear(Color.Blue);
                        }
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
