using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstraAlgorithm
{
    public class DijkstraAlgorithm
    {
        public Graph StartAlgorithm(Graph graph, Node source)
        {
            Graph shortestWayGraph = new Graph(graph);
            int v = graph.nodeList.Count;
            int[] dist = new int[v];
            bool[] sptSet = new bool[v];
            // ustawienie początkowej odległości dla wszystkich wierzchołków poza startowym na nieskończoność, a dla startowego na 0
            for (int i = 0; i < v; i++)
            {
                if (graph.nodeList[i] == source)
                {
                    dist[i] = 0;
                }
                else
                {
                    dist[i] = int.MaxValue;
                }
                sptSet[i] = false;
            }

            for (int count = 0; count < v; count++)
            {
                // pobieramy nr wierzchołka o najmniejszym koszcie przejścia do wierzchołka, wierzchołek ten musi być "nieodwiedzony"
                int u = MinDistance(dist, sptSet, v);
                // odwiedziliśmy wierzchołek o nr "u"
                sptSet[u] = true;
                for (int j = 0; j < v; j++)
                {
                    // jeśli nie odwiedzony i jest przejście
                    if (!sptSet[j] && dist[u] != int.MaxValue)
                    {
                        Node nodeOne = new Node();
                        Node nodeTwo = new Node();
                        nodeOne = graph.nodeList[u];
                        nodeTwo = graph.nodeList[j];
                        // sprawdzenie czy z wierzchołeka 1 można przejśc do wierzchołka 2 
                        for (int k = 0; k < graph.edgeDict[nodeOne].Count; k++)
                        {
                            // jeśli można
                            if (graph.edgeDict[nodeOne][k].target == nodeTwo)
                            {
                                int distance = graph.edgeDict[nodeOne][k].cost;
                                // czy odległość jest większa od 0 <-- to jest do zmiany jeśli założymy, że mogą być zerowe odległości
                                // oraz czy suma doległości do wierzchołka 1 i przejścia z tego wierzchołka do wierzchołka 2 jest mniejsza niż dotychczasowa najmniejsza odległość do wierzcholka 2
                                if (distance > 0 && (distance + dist[u] < dist[j]))
                                {
                                    dist[j] = dist[u] + distance;
                                    shortestWayGraph.edgeDict[nodeTwo].Add(new Edge(nodeOne, distance));
                                }
                            }
                        }
                    }
                }
            }
            //PrintSolution(dist, v);
            shortestWayGraph.PrintGraph();
            shortestWayGraph.PrintGraphCost();
            return shortestWayGraph;
        }

        private void PrintSolution(int[] dist, int v)
        {
            Console.Write("Vertex | Distance from Source\n");
            for (int i = 0; i < v; i++)
                Console.Write(i + " | " + dist[i] + "\n");
        }

        private int MinDistance(int[] dist, bool[] sptSet, int v)
        {
            int min = int.MaxValue, min_index = -1;
            for (int i = 0; i < v; i++)
            {
                if (sptSet[i] == false && dist[i] <= min)
                {
                    min = dist[i];
                    min_index = i;
                }
            }
            return min_index;
        }
    }
}
