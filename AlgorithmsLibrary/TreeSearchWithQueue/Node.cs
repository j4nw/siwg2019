using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsLibrary.TreeSearchWithQueue
{
    public class Node<State>
    {
        private void SetPathToRoot(Node<State> node, List<Node<State>> list)
        {
            if (node == null) return;
            list.Add(node);
            Steps++;
            SetPathToRoot(node.node, list);
        }

        public State state { get; private set; }
        public Node<State> node { get; private set; }
        public int CurrentCost { get; private set; }
        public int Steps { get; private set; }

        public IList<Node<State>> PathToRoot
        {
            get
            {
                List<Node<State>> ret = new List<Node<State>>();
                ret.Add(this);
                Steps++;
                SetPathToRoot(node, ret);
                return ret;
            }
        }

        public Node(State state, Node<State> node, int currentCost = 0)
        {
            this.state = state;
            this.node = node;
            this.CurrentCost = currentCost;
            Steps = 0;
        }

        public bool OnPathToRoot(State state, Func<State, State, bool> stateCompare)
        {
            if (stateCompare(this.state, state)) return true;
            if (node == null) return false;
            return node.OnPathToRoot(state, stateCompare);
        }
    }
}
