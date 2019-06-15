using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphToImage;
using System;

namespace DijkstraAlgorithm
{
    public class Node : INodeExtension
    {
        public string nodeName;
        public int nodeId;
        public int id { get => nodeId; set => nodeId = value; }

        public Node(string name)
        {
            nodeName = name;
            id = Convert.ToInt32(name);
        }
        public Node() { }
    }
}
