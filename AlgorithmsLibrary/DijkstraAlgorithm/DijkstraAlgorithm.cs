using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using GraphToImage;
using System.IO;

namespace DijkstraAlgorithm
{
    public class DijkstraAlgorithm : ProblemVisualization
    {
        private int cnt = 0;
        Graph graphIn = new Graph(1);
        Graph graph = new Graph(1);

        public override System.Drawing.Bitmap Visualization
        {
            get
            {
                if (cnt == 0)
                {
                    graphIn = new Graph(Settings.GetIntValue("N"));
                    graphIn.CreateRandomGraph();
                    GraphToImage.Graph<Node, Edge> newGraph = new GraphToImage.Graph<Node, Edge>(graphIn);
                    GraphToImage.GraphToImage<Node, Edge> gti = new GraphToImage.GraphToImage<Node, Edge>();
                    cnt++;
                    return gti.GetBitmap(newGraph,
                        Settings.GetIntValue("Width"),
                        Settings.GetIntValue("Height"),
                        Settings.GetIntValue("NodeRadius"),
                        Settings.GetIntValue("NodeDistanceFromCenter"),
                        Settings.GetStringValue("LineColour"),
                        Settings.GetStringValue("NodeColour"),
                        Settings.GetStringValue("LabelColour"));
                }
                else
                {
                    graph = StartAlgorithm(graphIn, graphIn.nodeList[0]);
                    GraphToImage.Graph<Node, Edge> newGraph = new GraphToImage.Graph<Node, Edge>(graph);
                    GraphToImage.GraphToImage<Node, Edge> gti = new GraphToImage.GraphToImage<Node, Edge>();
                    cnt = 0;
                    return gti.GetBitmap(newGraph,
                        Settings.GetIntValue("Width"),
                        Settings.GetIntValue("Height"),
                        Settings.GetIntValue("NodeRadius"),
                        Settings.GetIntValue("NodeDistanceFromCenter"),
                        Settings.GetStringValue("LineColour"),
                        Settings.GetStringValue("NodeColour"),
                        Settings.GetStringValue("LabelColour"));
                }
                //System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(50,50);
                //return bmp;
            }
        }

        public DijkstraAlgorithm()
        {
            Name = "DijkstraAlgorithm";
            Settings.Add("Width", "640");
            Settings.Add("Height", "480");
            Settings.Add("NodeRadius", "30");
            Settings.Add("NodeDistanceFromCenter", "200");
            Settings.Add("NodeColour", "Red");
            Settings.Add("LineColour", "Blue");
            Settings.Add("LabelColour", "White");
            Settings.Add("N", "5");
        }

        public Graph StartAlgorithm(Graph graph, Node source)
        {
            Graph shortestWayGraph = new Graph(graph);

            // GraphToImage.GraphToImage gti = new GraphToImage.GraphToImage();
            using (var sw = new StreamWriter(@"d:\test2.txt"))
            {
                int v = graph.nodeList.Count;
                int[] dist = new int[v];
                int[] nodeFrom = new int[v]; 
                bool[] sptSet = new bool[v];
                int oldNode = 0;
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
                    nodeFrom[i] = int.MaxValue;
                    sptSet[i] = false;
                }

                for (int count = 0; count < v; count++)
                {
                    // pobieramy nr wierzchołka o najmniejszym koszcie przejścia do wierzchołka, wierzchołek ten musi być "nieodwiedzony"
                    int u = MinDistance(dist, sptSet, v);
                    if (source.id != u && nodeFrom[u] != int.MaxValue)
                    {
                        shortestWayGraph.edgeDict[graph.nodeList[u]].Add(new Edge(graph.nodeList[nodeFrom[u]], dist[u]));
                        sw.WriteLine("Add to edge [" + graph.nodeList[u].id + "] node: " + graph.nodeList[nodeFrom[u]].id + " dist " + dist[oldNode] + " " + nodeFrom[u]);
                    }

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
                            int kkk = u;
                            // sprawdzenie czy z wierzchołeka 1 można przejśc do wierzchołka 2 
                            for (int k = 0; k < graph.edgeDict[nodeOne].Count; k++)
                            {
                                // jeśli można
                                if (graph.edgeDict[nodeOne][k].targetNode == nodeTwo)
                                {
                                    int distance = graph.edgeDict[nodeOne][k].cost;
                                    // czy odległość jest większa od 0 <-- to jest do zmiany jeśli założymy, że mogą być zerowe odległości
                                    // oraz czy suma doległości do wierzchołka 1 i przejścia z tego wierzchołka do wierzchołka 2 jest mniejsza niż dotychczasowa najmniejsza odległość do wierzcholka 2
                                    if (distance > 0 && (distance + dist[u] < dist[j]))
                                    {
                                        dist[j] = dist[u] + distance;
                                        nodeFrom[j] = u;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            shortestWayGraph.PrintGraph("tf");
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
