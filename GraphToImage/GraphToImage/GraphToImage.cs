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
    public class GraphToImage<TVertex,TEdge>
        where TEdge : IEdge, IEdgeExtension<TVertex>
        where TVertex : INodeExtension
    {
        Graph<TVertex, TEdge> graph;
        int width = 640;
        int height = 800;
        int nodeRadius = 30;
        int nodeDistanceFromCenter = 200;
        string lineColour, nodeColour, labelColour; 
       

        public Bitmap GetBitmap(Graph<TVertex,TEdge> graph, int width, int height, int nodeRadius, int nodeDistanceFromCenter, string lineColour, string nodeColour, string labelColour)
        {
            this.graph = graph;
            this.width = width;
            this.height = height;
            this.nodeRadius = nodeRadius;
            this.nodeDistanceFromCenter = nodeDistanceFromCenter;
            this.lineColour = lineColour;
            this.nodeColour = nodeColour;
            this.labelColour = labelColour;

            return Start();
        } 

        private Bitmap Start()
        {
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                PositionProvider pp = new PositionProvider(graph.nodeList.Count, this.nodeDistanceFromCenter);
                switch (lineColour)
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
                        using (Pen p = new Pen(Color.Green))
                        {
                            DrawLine(p, pp, g);
                        }
                        break;
                }

                switch (nodeColour)
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
                    switch (labelColour)
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
                    nodeOnePosition = pp.ReturnNodePosition(edge.Key.id, nodeRadius);
                    nodeTwoPosition = pp.ReturnNodePosition(target.target.id, nodeRadius);
                    g.DrawLine(pen, (float)nodeOnePosition.posX + width / 2, (float)nodeOnePosition.posY + height / 2, (float)nodeTwoPosition.posX + width / 2, (float)nodeTwoPosition.posY + height / 2 );
                }
            }
        }

        private void DrawString(PositionProvider pp, Graphics g, Font f, Color c)
        {
            foreach (var node in graph.nodeList)
            {
                Position nodePos;
                int name = node.id;// Convert.ToInt32(node);
                nodePos = pp.ReturnNodePosition(node.id, 0);// Convert.ToInt32(node), 0);
                float x = (float)(nodePos.posX + width / 2 - nodeRadius / 4), y = (float)(nodePos.posY + height / 2 - nodeRadius / 4);
                TextRenderer.DrawText(g, node.id.ToString(), f, new Point((int)x, (int)y), c);
            }
        }
    }
}
