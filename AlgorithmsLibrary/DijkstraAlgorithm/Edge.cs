using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstraAlgorithm
{
    public class Edge
    {
        public Node target { get; set; }
        public int cost;

        public Edge(Node target, int cost)
        {
            this.target = target;
            this.cost = cost;
        }
        
        public void PrintCost()
        {
            Console.Write(cost + " ");
        }
    }
}
