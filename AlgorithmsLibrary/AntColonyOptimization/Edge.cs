using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using GraphToImage;

namespace AlgorithmsLibrary
{
    public class Edge : IEdge, IEdgeExtension<Node> 
    {
        public Node targetNode;
        Node IEdgeExtension<Node>.target { get => targetNode; set => targetNode = value; }
        float IEdge.cost { get => cost; set => cost = (int)value; }
        public double pheromone = 0;
        
        public int cost;
        
        public Edge(Node target, int cost)
        {
            this.targetNode = target;
            this.cost = cost;
            pheromone = 0;
        }

        public void PheromoneEvaporation(double factor, double minValue)
        {
            pheromone *= factor;

            if (pheromone < minValue)
                pheromone = minValue;
        }


        public void PrintCost()
        {
            Console.Write(cost + " ");
        }

    }
}
