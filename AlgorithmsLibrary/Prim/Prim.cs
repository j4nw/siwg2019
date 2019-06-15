//using AlgorithmsLibrary.PrimiKruskal;
//using Core;
//using System.Collections.Generic;
//using System.Linq;


//namespace Prim
//{  
//    public static class Prim
//    {
//        public static Graph<TVertex,TEdge> FindMST<TVertex, TEdge>(IGraph<TVertex,TEdge> IG)
//            where TEdge : IEdge<TVertex>
//        {       
//            //lista wierzcholkow nie bedacych jeszcze w drzewie MST
//            List<TVertex> VerticesLeft = new List<TVertex>();
//            foreach (TVertex v in IG.Vertices) {
//                VerticesLeft.Add(v);
//            }

//            //Utwórz drzewo zawierające jeden wierzchołek, dowolnie wybrany z grafu.
//            Graph<TVertex, TEdge> MST = new Graph<TVertex, TEdge>();
//            MST.AddVertex(VerticesLeft[0]);

//            //Utwórz kolejkę priorytetową, zawierającą wierzchołki osiągalne z MDR(w tym momencie zawiera jeden wierzchołek), 
//            //o priorytecie najmniejszego kosztu dotarcia do danego wierzchołka z MDR.
//            BinaryHeap<TVertex, TEdge> EdgeQueue = new BinaryHeap<TVertex, TEdge>(IG.Edges.Count());
//            foreach (TEdge e in IG.IncidentEdges(VerticesLeft[0]) ) { 
//                EdgeQueue.Insert(e);
//            }
//            VerticesLeft.RemoveAt(0);

//            bool end = false;
//            bool start = false;
//            //Powtarzaj, dopóki drzewo nie obejmuje wszystkich wierzchołków grafu:
//            while (VerticesLeft.Count > 0)
//            {
//                //wśród nieprzetworzonych wierzchołków(spoza obecnego MDR) wybierz ten, dla którego koszt dojścia z obecnego MDR jest najmniejszy.
//                TEdge MinEdge = EdgeQueue.ExtractMin();

//                end = VerticesLeft.Contains(MinEdge.End);
//                start = VerticesLeft.Contains(MinEdge.Start);
//                if (end || start)
//                {
//                    if (end)
//                    {
//                        //dodaj do obecnego MDR wierzchołek i krawędź realizującą najmniejszy koszt
//                        MST.AddVertex(MinEdge.End);
//                        MST.AddEdge(MinEdge);

//                        //zaktualizuj kolejkę priorytetową, uwzględniając nowe krawędzie wychodzące z dodanego wierzchołka
//                        VerticesLeft.Remove(MinEdge.End);
//                        //dodaj krawedzie wychodzace z nowo-dodanego wierzcholka ale takie ktore nie lacza sie z wierzcholkami juz bedacymi w MST
//                        foreach (TEdge e in IG.IncidentEdges(MinEdge.End))
//                        {
//                            if (VerticesLeft.Contains(e.End) || VerticesLeft.Contains(e.Start))
//                                EdgeQueue.Insert(e);
//                        }
//                    }
//                    else //if (start)
//                    {
//                        //dodaj do obecnego MDR wierzchołek i krawędź realizującą najmniejszy koszt
//                        MST.AddVertex(MinEdge.Start);
//                        MST.AddEdge(MinEdge);

//                        //zaktualizuj kolejkę priorytetową, uwzględniając nowe krawędzie wychodzące z dodanego wierzchołka
//                        VerticesLeft.Remove(MinEdge.Start);
//                        //dodaj krawedzie wychodzace z nowo-dodanego wierzcholka ale takie ktore nie lacza sie z wierzcholkami juz bedacymi w MST
//                        foreach (TEdge e in IG.IncidentEdges(MinEdge.Start))
//                        {
//                            if (VerticesLeft.Contains(e.End) || VerticesLeft.Contains(e.Start))
//                                EdgeQueue.Insert(e);
//                        }
//                    }
                    
//                }
//            }

//            return MST;
//        }
//    }
//}
