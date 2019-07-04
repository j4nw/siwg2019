using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphToImage;
using System;

namespace AlgorithmsLibrary
{
    public class Node : INodeExtension
    {
        public string nodeName;
        public int nodeId;
        public int id { get => nodeId; set => nodeId = value; }
        public bool visited = false;
        public int posX, posY;
        public float value;
        public float pheromoneValue = 0;

        public Node(string name)
        {
            nodeName = name;
            id = Convert.ToInt32(name);
            visited = false;
        }

        public Node(string name, int x, int y)
        {
            nodeName = name;
            id = Convert.ToInt32(name);
            visited = false;
            posX = x;
            posY = y;
            value = 0;
        }

        public Node(string name, int x, int y, float noise)
        {
            nodeName = name;
            id = Convert.ToInt32(name);
            visited = false;
            posX = x;
            posY = y;
            value = noise;
        }

        public Node()
        {
            visited = false;
        }
    }
}
