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
    class GraphToImage
    {
        Graph graph;
        int width = 640;
        int height = 800;
        int nodeRadius = 30;

        public GraphToImage(Graph graph)
        {
            this.graph = graph;
            
        }

        public void Start(string filename)
        {
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                PositionProvider pp = new PositionProvider(graph.nodeList.Count, 200);

                using (Pen p = new Pen(Color.Red))
                {
                    DrawLine(p, pp, g);
                }
                using (Pen p2 = new Pen(Color.Green))
                {
                    DrawNodes(p2, pp, g);
                }
                using (Font f = new Font("Arial", 8))
                {
                    DrawString(pp, g, f);
                }
                bmp.Save(filename);//,System.Drawing.Imaging.ImageFormat.Jpeg);
                Console.WriteLine("To end  press key!");
                Console.ReadKey();
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

        private void DrawString(PositionProvider pp, Graphics g, Font f)
        {
            foreach (var node in graph.nodeList)
            {
                Position nodePos;
                nodePos = pp.ReturnNodePosition(Convert.ToInt32(node.nodeName), 0);
                Console.WriteLine(node.nodeName);
                float x = (float)(nodePos.posX + width / 2 - nodeRadius / 4), y = (float)(nodePos.posY + height / 2 - nodeRadius / 4);
                TextRenderer.DrawText(g, node.nodeName, f, new Point((int)x, (int)y), SystemColors.ControlText);
            }
        }
    }
}
