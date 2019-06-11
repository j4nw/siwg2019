using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using System.Drawing;
using System.Windows.Forms;

namespace GraphToImage
{
    public class GraphToImage : ProblemVisualization
    {
        Graph graph;
        int width = 640;
        int height = 800;
        int nodeRadius = 30;

        public override Bitmap Visualization
        {
            get
            {
                return Start(Settings.GetStringValue("Name"));
            }
        }

        public GraphToImage()//Graph graph)
        {
            this.graph = new Graph(5);
            this.graph.CreateRandomGraph();
            Name = "GraphVisualisation";
            Settings.Add("Width", "640");
            Settings.Add("Height", "480");
            Settings.Add("NodeRadius", "30");
            Settings.Add("NodeDistanceFromCenter", "200");
            Settings.Add("NodeColour", "Red");
            Settings.Add("LineColour", "Blue");
            Settings.Add("LabelColour", "White");
        }

        public Bitmap Start(string filename)
        {
            width = Settings.GetIntValue("Width");
            height = Settings.GetIntValue("Height");
            nodeRadius = Settings.GetIntValue("NodeRadius");
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                PositionProvider pp = new PositionProvider(graph.nodeList.Count, Settings.GetIntValue("NodeDistanceFromCenter"));
                switch (Settings.GetStringValue("LineColour"))
                {
                    case "Red":
                        using (Pen p = new Pen(Color.Red))
                        {
                            DrawLine(p, pp, g);
                        }
                        break;
                    case "Green":
                        using (Pen p = new Pen(Color.Green))
                        {
                            DrawLine(p, pp, g);
                        }
                        break;
                    case "Blue":
                        using (Pen p = new Pen(Color.Blue))
                        {
                            DrawLine(p, pp, g);
                        }
                        break;
                    case "White":
                        using (Pen p = new Pen(Color.White))
                        {
                            DrawLine(p, pp, g);
                        }
                        break;
                    default:
                        using (Pen p = new Pen(Color.Black))
                        {
                            DrawLine(p, pp, g);
                        }
                        break;
                }

                switch (Settings.GetStringValue("NodeColour"))
                {
                    case "Red":
                        using (Pen p2 = new Pen(Color.Red))
                        {
                            DrawNodes(p2, pp, g);
                        }
                        break;
                    case "Green":
                        using (Pen p2 = new Pen(Color.Green))
                        {
                            DrawNodes(p2, pp, g);
                        }
                        break;
                    case "Blue":
                        using (Pen p2 = new Pen(Color.Blue))
                        {
                            DrawNodes(p2, pp, g);
                        }
                        break;
                    case "White":
                        using (Pen p2 = new Pen(Color.White))
                        {
                            DrawNodes(p2, pp, g);
                        }
                        break;
                    default:
                        using (Pen p2 = new Pen(Color.Black))
                        {
                            DrawNodes(p2, pp, g);
                        }
                        break;
                }
                using (Font f = new Font("Arial", 8))
                {
                    Color color;
                    switch (Settings.GetStringValue("LabelColour"))
                    {
                        case "Red":
                            color = Color.Red;
                            break;
                        case "Green":
                            color = Color.Green;
                            break;
                        case "Blue":
                            color = Color.Blue;
                            break;
                        case "White":
                            color = Color.White;
                            break;
                        default:
                            color = Color.Black;
                            break;
                    }
                    DrawString(pp, g, f, color);
                }
                //bmp.Save(filename);
                return bmp;
            }
        }

        private void DrawNodes(Pen pen, PositionProvider pp, Graphics g)
        {
            foreach(var node in graph.nodeList)
            {
                Position position;
                position = pp.ReturnPosition();
                if(position!=null)
                    g.DrawEllipse(pen, (float)position.posX + width / 2 - nodeRadius/2, (float)position.posY + height / 2 - nodeRadius /2, nodeRadius, nodeRadius);
            }
        }
        private void DrawLine(Pen pen, PositionProvider pp, Graphics g)
        {
            foreach (var edge in graph.edgeDict)
            {
                foreach (var target in edge.Value)
                {
                    Position nodeOnePosition;
                    Position nodeTwoPosition;
                    nodeOnePosition = pp.ReturnNodePosition(Convert.ToInt32(edge.Key.nodeName), nodeRadius);
                    nodeTwoPosition = pp.ReturnNodePosition(Convert.ToInt32(target.target.nodeName), nodeRadius);
                    g.DrawLine(pen, (float)nodeOnePosition.posX + width / 2, (float)nodeOnePosition.posY + height / 2, (float)nodeTwoPosition.posX + width / 2, (float)nodeTwoPosition.posY + height / 2 );
                }
            }
        }

        private void DrawString(PositionProvider pp, Graphics g, Font f, Color c)
        {
            foreach (var node in graph.nodeList)
            {
                Position nodePos;
                nodePos = pp.ReturnNodePosition(Convert.ToInt32(node.nodeName), 0);
                Console.WriteLine(node.nodeName);
                float x = (float)(nodePos.posX + width / 2 - nodeRadius / 4), y = (float)(nodePos.posY + height / 2 - nodeRadius / 4);
                TextRenderer.DrawText(g, node.nodeName, f, new Point((int)x, (int)y), c);
            }
        }
    }
}
