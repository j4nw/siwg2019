using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerlinNoise;
using Core;

namespace AlgorithmsLibrary
{
    public class AntColonyOptimization : ProblemVisualization
    {
        private double minPheromoneValue = 0;
        private int numberOfSteps = 50;
        private double alpha = 1; // influence of pheromone
        private double beta = 3; // influence of cost
        private double evaporation = 0.9f; // the speed at which evaporation of the pheromone
        private double Q = 100; // pheromone added to edge
        private int numberOfAnts = 10;
        private int cellSize = 10; // size of Perlin noise grid cell size
        private bool bfs = true; // use bfs?
        private int startPoint = 120, endPoint = 312; // startiong and end node id 

        private List<Ant> antsList = new List<Ant>(); // list of ants used in algorithm
        private List<Node> bestPath = new List<Node>(); 
        private double bestPathCost = double.MaxValue;

        private Node[] parent; // store information from bfs
        private Node endNode, startNode;
        private int red = 1, green = 0, blue = 0;
        private int height = 100, width = 100;

        private Random random = new Random();
        private int antCnt = 0; // used to add id to ant (for debug)
        private int cntSave = 0; // used to add name to saved file (for debug)
        private int listCnt = 0; // used to show all result of algorithm in visualisation
        private List<System.Drawing.Bitmap> bmpList = new List<System.Drawing.Bitmap>();
        private Graph graph = new Graph(4);

        public override System.Drawing.Bitmap Visualization
        {
            get
            {
                if(listCnt >= bmpList.Count - 1)
                {
                    bmpList = new List<System.Drawing.Bitmap>();
                    System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(50,50);
                    // initialization
                    Init(Settings.GetIntValue("Width"), 
                        Settings.GetIntValue("Height"), 
                        0, 
                        Settings.GetIntValue("Number of steps"),
                        Settings.GetIntValue("Ant number"), 
                        Settings.GetIntValue("Q"), 
                        Settings.GetIntValue("Evaporation [1-100]"), 
                        Settings.GetIntValue("Alpha"), 
                        Settings.GetIntValue("Beta"),
                        Settings.GetIntValue("StartPoint"), 
                        Settings.GetIntValue("EndPoint"), 
                        Settings.GetStringValue("Use BFS?[y/n]"),
                        Settings.GetIntValue("Perlin Cell Size")
                        );
                    bmp = Start();
                    listCnt = 0;
                }
                else
                {
                    listCnt++;
                }
                return bmpList[listCnt];
            }
        }

        public AntColonyOptimization()
        {
            Name = "AntColonyOptimization";
            Settings.Add("Width", "100");
            Settings.Add("Height", "100");
            Settings.Add("Ant number", "10");
            Settings.Add("Number of steps", "50");
            Settings.Add("Alpha", "1");
            Settings.Add("Beta", "3");
            Settings.Add("Q", "500");
            Settings.Add("Evaporation [1-100]", "90");
            Settings.Add("StartPoint", "120");
            Settings.Add("EndPoint", "312");
            Settings.Add("Use BFS?[y/n]", "y");
            Settings.Add("Perlin Cell Size", "100");
        }

        public void Init(int width, int height, int minPheromoneValue, int numberOfSteps, int numberOfAnts, double Q, double evaporation, double alpha, double beta, int startPoint, int endPoint, string bfs, int cell)
        {
            this.width = width;
            this.height = height;
            this.alpha = alpha;
            this.beta = beta;
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            this.numberOfAnts = numberOfAnts;
            this.numberOfSteps = numberOfSteps;
            this.minPheromoneValue = minPheromoneValue;
            this.Q = Q;
            this.evaporation = evaporation / 100;
            this.cellSize = cell;
            if(bfs == "y")
            {
                this.bfs = true;
            }
            else
            {
                this.bfs = false;
            }
        }

        public System.Drawing.Bitmap Start()
        {
            // reset to intial state
            cntSave = 0;
            antsList = new List<Ant>();
            bestPath = new List<Node>();
            bestPathCost = double.MaxValue;
            antCnt = 0;
            cntSave = 0;

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(width, height);
            // generate map
            PerlinNoise.PerlinNoise perlin = new PerlinNoise.PerlinNoise();
            perlin.DrawGradients(width, height, cellSize);
            perlin.CreateNoise(width, height, cellSize);
            // load map to graph
            graph = TabToGraph(perlin.NoiseTable, width, height);
            // set satrting and end position
            startNode = graph.nodeList[startPoint];
            endNode = graph.nodeList[endPoint];
            // start algorithm
            AntColonyAlgorithm(startNode, endNode);
            // save output to bitmap
            bmp = GraphToBMP(width, height, bestPath, true, false);
            bmpList.Add(bmp);
            bmp = GraphToBMP(width, height, bestPath, true, true);
            bmpList.Add(bmp);

            return bmp;
        }

        public Graph TabToGraph(float[,] tab, int dim1, int dim2)
        {
            Graph g = new Graph();
            // add nodes to graph
            for (int i = 0; i < dim1; i++)
            {
                for (int j = 0; j < dim2; j++)
                {
                    Node node = new Node((i * dim2 + j).ToString(), i, j, (tab[i,j]));
                    g.nodeList.Add(node);
                    g.edgeDict.Add(g.nodeList[i * dim2 + j], new List<Edge>());
                }
            }
            // add edges (directed graph)
            for (int i = 0; i < dim1; i++)
            {
                for (int j = 0; j < dim2 - 1; j++)
                {
                    float cost = Math.Abs(tab[i, j] * 100);// - tab[i, j + 1] * 100);
                    g.edgeDict[g.nodeList[i * dim2 + j]].Add(new Edge(g.nodeList[i * dim2 + j + 1], (int)cost));
                    g.edgeDict[g.nodeList[i * dim2 + j + 1]].Add(new Edge(g.nodeList[i * dim2 + j], (int)cost));
                }
            }
            for (int i = 0; i < dim1 - 1; i++)
            {
                for (int j = 0; j < dim2; j++)
                {
                    float cost = Math.Abs(tab[i, j] * 100);// - tab[i + 1, j] * 100);
                    g.edgeDict[g.nodeList[i * dim2 + j]].Add(new Edge(g.nodeList[i * dim2 + j + dim2], (int)cost));
                    g.edgeDict[g.nodeList[i * dim2 + j + dim2]].Add(new Edge(g.nodeList[i * dim2 + j], (int)cost));
                }
            }
            return g;
        }

        // function change graph to bitmap, additionally you can save to disk in jpg format, choose to show the pheromone distribution
        public System.Drawing.Bitmap GraphToBMP(int dim1, int dim2, List<Node> path, bool save, bool pheromone)
        {
            System.Drawing.Bitmap bmpOut = new System.Drawing.Bitmap(dim1, dim2);
            // setting the pheromone of a given point based on the average value of all point edges
            for (int i = 0; i < dim1; i++)
            {
                for (int j = 0; j < dim2; j++)
                {
                    double sum = 0;
                    int cnt = 0;
                    foreach (Edge edge in graph.edgeDict[graph.nodeList[i * dim2 + j]])
                    {
                        sum += edge.pheromone;
                        cnt++;
                    }
                    graph.nodeList[i * dim2 + j].pheromoneValue = (float)(sum / cnt); //(float)graph.edgeDict[graph.nodeList[i * dim2 + j]][0].pheromone; // (float)(sum / cnt);
                }
            }
            // finding the largest amount of pheromone
            float maxPhValue = 0;
            for (int i = 0; i < dim1; i++)
            {
                for (int j = 0; j < dim2; j++)
                {
                    if (graph.nodeList[i * dim2 + j].pheromoneValue > maxPhValue)
                    {
                        maxPhValue = graph.nodeList[i * dim2 + j].pheromoneValue;
                    }
                }
            }
            // scaling the value of the pheromone
            for (int i = 0; i < dim1; i++)
            {
                for (int j = 0; j < dim2; j++)
                {
                    graph.nodeList[i * dim2 + j].pheromoneValue /= (0.51f * maxPhValue);
                }
            }

            if (pheromone)
            {
                // drawing pheromone map
                for (int i = 0; i < dim1; i++)
                {
                    for (int j = 0; j < dim2; j++)
                    {
                        float tmp = 0;
                        tmp = (graph.nodeList[i * dim2 + j].pheromoneValue);
                        bmpOut.SetPixel(i, j, System.Drawing.Color.FromArgb(
                                (int)(tmp * 128 * 0),
                                (int)(tmp * 128 * 1),
                                (int)(tmp * 128 * 0)));
                    }
                }
            }
            else
            {
                // drwaing map
                for (int i = 0; i < dim1; i++)
                {
                    for (int j = 0; j < dim2; j++)
                    {
                        float tmp = 0;
                        tmp = (graph.nodeList[i * dim2 + j].value + 1);
                        bmpOut.SetPixel(i, j, System.Drawing.Color.FromArgb(
                                (int)(tmp * 128 * 1),
                                (int)(tmp * 128 * 0),
                                (int)(tmp * 128 * 0)));
                    }
                }
                if (cntSave != 0)
                {
                    // drawing road
                    for (int i = 0; i < dim1; i++)
                    {
                        for (int j = 0; j < dim2; j++)
                        {
                            if (path.Contains(graph.nodeList[i * dim2 + j]))
                            {
                                float tmp2 = 0;
                                tmp2 = (graph.nodeList[i * dim2 + j].pheromoneValue);
                                bmpOut.SetPixel(i, j, System.Drawing.Color.FromArgb(
                                        (int)(tmp2 * 128 * 0),
                                        (int)(tmp2 * 128 * 0),
                                        (int)(tmp2 * 128 * 1)));
                            }
                        }
                    }
                }
            }
            // save to file "number".jpg
            if (save)
            {
                bmpOut.Save(cntSave.ToString() + ".jpg");
                cntSave++;
            }
            return bmpOut;
        }


        public void AntColonyAlgorithm(Node start, Node end)
        {
            // ise bfs for first pheromone road?
            if (bfs)
            {
                Bfs bfs = new Bfs();
                parent = bfs.StartBfs(graph, start, end);
                Node node = end;
                bestPathCost = 0;
                while (node.id != start.id)
                {
                    Node oldNode = node;
                    node = parent[node.id];
                    foreach (Edge edge in graph.edgeDict[node])
                    {
                        if (edge.targetNode.id == oldNode.id)
                        {
                            bestPathCost += edge.cost;
                            edge.pheromone += Q;
                            bestPath.Add(oldNode);
                        }
                    }
                }
                CreateAnts();
            }
            else
            {
                // if no bfs then use random movement while path not found or try 1000 times
                int cntLos = 0;
                CreateAnts();
                while (bestPath.Count == 0 || cntLos >= 1000)
                {
                    OneStep();
                    cntLos++;
                }
            }
            int cnt2 = 0;
            while (cnt2 != numberOfSteps)
            {
                OneStep();
                cnt2++;
            }
            Console.WriteLine("Best path cost: " + bestPathCost);
            foreach (var node2 in bestPath)
            {
                Console.Write("\t" + node2.id);
            }
            Console.WriteLine();
        }

        public void CreateAnts()
        {
            for (int i = 0; i < numberOfAnts; i++)
            {
                Ant newAnt = new Ant(Q, startNode, endNode, antCnt);
                antCnt++;
                antsList.Add(newAnt);
            }
        }

        public void OneStep()
        {
            foreach(Ant ant in antsList)
            {
                while (!ant.done)
                {
                    //Console.WriteLine("Ant nr " + ant.id);
                    Edge nextMove = new Edge(graph.nodeList[0], 0);
                    //Console.WriteLine("Wybieranie drogi!");
                    foreach (var dict in graph.edgeDict)
                    {
                        if (dict.Key.id == ant.currentPosition.id)
                        {
                            //Console.WriteLine("Obliczanie prawdopodobieństwa:");
                            double[] probability = CalculateProbability(dict, ant);
                            //Console.WriteLine("Obliczono");
                            double probabilitySum = 0;
                            foreach (double value in probability)
                            {
                                probabilitySum += value;
                            }
                            //Console.WriteLine("Losowanie liczby z zakresu 0 - 1 (double)");
                            double r = probabilitySum * (double)random.Next(100000000) / 100000000;
                            //Console.WriteLine(r);
                            //Console.WriteLine("Wybieranie drogi: ");
                            for (int i = 0; i < dict.Value.Count; i++)
                            {
                                r -= probability[i];
                                //Console.WriteLine("r " + r);
                                if (r <= 0)
                                {
                                    nextMove = dict.Value[i];
                                    break;
                                }
                            }
                        }
                    }
                    //Console.WriteLine("\n\nNatępny ruch to " + nextMove.targetNode.id + " o koszcie " + nextMove.cost + ", feromonie " + nextMove.pheromone + "\n\n");
                    if(!ant.cantMove)
                        ant.MoveAnt(nextMove);
                    if (ant.CheckGoal())
                    {
//                        Console.WriteLine("\t\t\t\t DONE!");
                        ant.done = true;
                        if (ant.pathCost < bestPathCost)
                        {
                            System.Drawing.Bitmap bmp = GraphToBMP(width, height, ant.path, true, false);
                            bmpList.Add(bmp);
                            Console.WriteLine("New best path!\n");
                            bestPathCost = ant.pathCost;
                            Console.WriteLine("New path cost: " + bestPathCost);
                            bestPath = new List<Node>();
                            foreach (var node in ant.path)
                            {
                                Console.Write("\t" + node.id);
                                bestPath.Add(node);
                            }
                            Console.WriteLine();
                        }
                    }
                }
            }
//            Console.WriteLine("Evaporation");
            // update pheromone evaporation
            foreach (var node in graph.edgeDict)
            {
                foreach (var edge in node.Value)
                {
                    edge.PheromoneEvaporation(evaporation, minPheromoneValue);
                }
            }
//            Console.WriteLine("Ant add pheromone");
            // update pheromone
            foreach (var ant in antsList)
            {
                ant.UpdatePheromone();
            }
//            Console.WriteLine("Reset ant to start position!");
            // reset ant to starting state
            foreach (var ant in antsList)
            {
                ant.Reset(startNode);
            }

        }

        // Calculate the probability of useing edge
        public double[] CalculateProbability(KeyValuePair<Node,List<Edge>> pair, Ant ant)
        {
            double sum = 0;
            double newValue = 0;
            double[] probability = new double[pair.Value.Count];
            int cnt = 0;
            // calculate sum
            foreach (var edge in pair.Value)
            {
                if (!ant.edgePath.Contains(edge))
                {
                    //Console.Write("From Node " + edge.targetNode.id + " [" + edge.targetNode.posX + "," + edge.targetNode.posY + "] to Node " + endNode.id + " [" + endNode.posX + "," + endNode.posY + "]");
                    double ph = 0, d = 0; // ph - wpływ feromonu // d - wpływ drogi
                    double dist = graph.TwoNodeDistance(edge.targetNode, endNode);
                    //Console.Write(" Dystans: " + dist + " Pheromone: " + edge.pheromone);
                    d = (1f / (dist != 0 ? dist : 0.0000001));
                    ph = edge.pheromone;
                    for (int i = 1; i < alpha; i++)
                    {
                        ph = ph * ph;
                    }
                    for (int i = 1; i < beta; i++)
                    {
                        d = d * d;
                    }
                    sum += (d * ph);
                }
                else
                {
                    cnt++;
                    //Console.WriteLine("\t\t\tYou cant use the same edge!");
                }
            }
            // if ant cant move - use all edges from node (only possible when back to starting node) and it is starting node (to make sure)
            if (cnt == pair.Value.Count && ant.currentPosition.id == startNode.id)
            {
                ant.Reset(startNode);
                ant.done = true;
                ant.cantMove = true;
                return probability;
            }
            // set probability
            for (int i = 0; i < pair.Value.Count; i++)
            {
                if (!ant.edgePath.Contains(pair.Value[i]))
                {
                    double ph = 0, d = 0;
                    double dist = graph.TwoNodeDistance(pair.Value[i].targetNode, endNode);
                    d = (1f / (dist != 0 ? dist : 0.0000001));
                    ph = pair.Value[i].pheromone;
                    for (int j = 1; j < alpha; j++)
                    {
                        ph = ph * ph;
                    }
                    for (int j = 1; j < beta; j++)
                    {
                        d = d * d;
                    }
                    if (ph != 0)
                        newValue = d * ph;
                    else
                        newValue = d;
                    if (sum != 0)
                        probability[i] = newValue / sum;
                    else
                    {
                        probability[i] = 1f / (double)pair.Value.Count;
                    }
                    //Console.WriteLine("P[" + i + "] = " + probability[i] + " node " + pair.Value[i].targetNode.id + " cost " + pair.Value[i].cost + " pheromone " + pair.Value[i].pheromone);
                }
                else
                {
                    //Console.WriteLine("\t\t\tYou cant use the same edge!");
                }
            }
            return probability;
        }
    }
}
