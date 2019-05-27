using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace GraphToImage
{
    public class Graph : IGraph<Node, Edge>
    {
        public List<Node> nodeList = new List<Node>();
        public Dictionary<Node,List<Edge>> edgeDict = new Dictionary<Node, List<Edge>>();
        private int size = 0;
        public Graph(int size)
        {
            this.size = size;
        }

        public Graph(Graph graphToCopy)
        {
            this.size = graphToCopy.size;
            foreach(Node node in graphToCopy.nodeList)
            {
                this.nodeList.Add(node);
                this.edgeDict.Add(node, new List<Edge>());
            }
        }

        public IEnumerable<Node> Vertices
        {
            get
            {
                return nodeList;
            }
        }

        public IEnumerable<Edge> Edges { get { return edgeDict.SelectMany(x => x.Value); } }

        public void CreateRandomGraph()
        {
            int n = size;
            Random rng = new Random();
            for(int i = 0; i < n * n; i++)
            {
                Node newNode = new Node(i.ToString());
                nodeList.Add(newNode);
                edgeDict.Add(nodeList[i], new List<Edge>());
            }
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - 1; j++)
                {
                    int k = rng.Next(0, 10);
                    int cost = rng.Next(2, 5);
                    edgeDict[nodeList[i * n + j]].Add(new Edge(nodeList[i * n + j + 1], cost));
                    edgeDict[nodeList[i * n + j + 1]].Add(new Edge(nodeList[i * n + j], cost));
                    edgeDict[nodeList[i * n + j]].Add(new Edge(nodeList[i * n + j + n], cost));
                    edgeDict[nodeList[i * n + j + n]].Add(new Edge(nodeList[i * n + j], cost));
                }
            }
        }

        public IEnumerable<Edge> IncidentEdges(Node vertex)
        {
            return edgeDict[vertex];
        }

        public void PrintGraph()
        {
            Console.WriteLine("Graph: ");
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write(nodeList[i * size + j].nodeName + " ");
                }
                Console.WriteLine();
            }
        }
        public void PrintGraphCost()
        {
            Console.WriteLine("Graph: ");
            for (int i = 0; i < size * size; i++)
            {
                Console.WriteLine("Node " + nodeList[i].nodeName);
                foreach(Edge edge in edgeDict[nodeList[i]])
                {
                    Console.Write("to node " + edge.target.nodeName + " cost: ");
                    edge.PrintCost();
                }
                Console.WriteLine();
            }
        }
    }
}
