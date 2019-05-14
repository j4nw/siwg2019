using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsLibrary.TreeSearchWithQueue
{
    public static class TreeSearchWithQueue
    {
        public static Node<State> BFS<State>(IProblem<State> problem)
        {
            PriorityQueue<Node<State>> fringe = new PriorityQueue<Node<State>>();

            fringe.GetCost = (Node<State> node) =>
            {
                return problem.EstimatedCostToGoal(node.state);
            };

            return Search(problem, fringe);
        }

        public static Node<State> AStar<State>(IProblem<State> problem)
        {
            PriorityQueue<Node<State>> fringe = new PriorityQueue<Node<State>>();

            fringe.GetCost = (Node<State> node) =>
            {
                return problem.EstimatedCostToGoal(node.state) + problem.GetCurrentCost(node.state, node.node.state, node.CurrentCost);
            };

            return Search(problem, fringe);
        }

        private static Node<State> Search<State>(IProblem<State> problem, PriorityQueue<Node<State>> fringe)
        {
            fringe.Add(new Node<State>(problem.InitialState, null));

            int a = 1;
            while (!fringe.IsEmpty)
            {
                Node<State> node = fringe.Pop();

                if (problem.IsGoal(node.state)) return node;

                foreach (State state in problem.Expand(node.state))
                {
                    if (!node.OnPathToRoot(state, problem.StateCompare))
                    {
                        fringe.Add(new Node<State>(state, node, problem.GetCurrentCost(state, node.state, node.CurrentCost)));
                        //if (rownyRzad(a)) problem.Print(state);
                        a++;
                    }
                }
            }
            return null;
        }

        /*private static bool rownyRzad(int a)
        {
            if (a == 1) return true;
            if (a % 10 != 0) return false;
            return rownyRzad(a / 10);
        }*/
    }
}
