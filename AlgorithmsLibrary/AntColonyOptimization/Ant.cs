using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsLibrary
{
    public class Ant
    {
        public int id;
        public Node currentPosition;
        public List<Node> path = new List<Node>();
        public List<Edge> edgePath = new List<Edge>();
        public int pathCost;
        public double pheromone;
        public Node target;
        public Edge lastEdge;
        public bool done = false;
        public bool cantMove = false;

        public Ant(double pheromone, Node startPosition, Node target, int id)
        {
            this.pheromone = pheromone;
            currentPosition = startPosition;
            path.Add(startPosition);
            pathCost = 0;
            this.target = target;
            this.id = id;
        }

        public void MoveAnt(Edge moveOn)
        {
            // move / update node
            currentPosition = moveOn.targetNode;
            path.Add(moveOn.targetNode);
            pathCost += moveOn.cost;

            //update edge to update pheromone after all ants end move
            lastEdge = moveOn;
            edgePath.Add(moveOn);
        }

        public void Reset(Node startNode)
        {
            path = new List<Node>();
            edgePath = new List<Edge>();
            currentPosition = startNode;
            path.Add(startNode);
            pathCost = 0;
            cantMove = false;
            done = false;
        }

        public bool Visited(Node target)
        {
            if (path.Contains(target))
            {
                return true;
            }
            return false;
        }

        public bool CheckGoal()
        {
            if(currentPosition == target)
            {
                return true;
            }
            return false;
        }

        public void UpdatePheromone()
        {
            if(!cantMove)
                foreach(Edge edge in edgePath)
                {
                    edge.pheromone += pheromone;
                }
        }
    }
}
