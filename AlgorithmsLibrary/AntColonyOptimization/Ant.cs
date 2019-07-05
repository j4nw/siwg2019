using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsLibrary
{
    public class Ant
    {
        public int id; // use to debug
        public Node currentPosition; //current position in graph
        public List<Node> path = new List<Node>(); // used nodes
        public List<Edge> edgePath = new List<Edge>(); // used edges
        public int pathCost; // path cost
        public double pheromone; // the amount of pheromone left
        public Node target; // goal Node
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
            edgePath.Add(moveOn);
        }

        // reset the ant to the starting position to search the road again
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

        // update the pheromone if the target has been reached (if not failed)
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
