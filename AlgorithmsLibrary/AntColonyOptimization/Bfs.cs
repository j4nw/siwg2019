using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsLibrary
{
    class Bfs
    {
        private bool[] visited;
        private Node[] parent;
        private List<Node> q = new List<Node>();
        private List<Node> result = new List<Node>();
        private Graph graph;

        public Node[] StartBfs(Graph g, Node start)
        {
            graph = g;
            visited = new bool[g.nodeList.Count];
            parent = new Node[g.nodeList.Count];

            for(int i = 0; i < g.nodeList.Count; i++)
            {
                visited[i] = false;
                parent[i] = g.nodeList[i];
            }
            q.Add(start);
            visited[start.id] = true;

            while(q.Count != 0)
            {
                int nodeId = q[0].id;
                Node node = q[0];
                visited[node.id] = true;
                q.Remove(node);
                foreach(var edge in g.edgeDict[node])
                {
                    if (!visited[edge.targetNode.id] && !q.Contains(edge.targetNode))
                    {
                        q.Add(edge.targetNode);
                        parent[edge.targetNode.id] = node;
                    }
                }
            }
            return parent;
        }

        public Node[] StartBfs(Graph g, Node start, Node end)
        {
            graph = g;
            visited = new bool[g.nodeList.Count];
            parent = new Node[g.nodeList.Count];

            for (int i = 0; i < g.nodeList.Count; i++)
            {
                visited[i] = false;
                parent[i] = g.nodeList[i];
            }
            q.Add(start);
            visited[start.id] = true;

            while (q.Count != 0)
            {
                int nodeId = q[0].id;
                Node node = q[0];
                visited[node.id] = true;
                q.Remove(node);
                foreach (var edge in g.edgeDict[node])
                {
                    if (!visited[edge.targetNode.id] && !q.Contains(edge.targetNode))
                    {
                        q.Add(edge.targetNode);
                        parent[edge.targetNode.id] = node;
                    }
                }

                if(node.id == end.id)
                {
                    break;
                }
            }
            return parent;
        }
    }
}
