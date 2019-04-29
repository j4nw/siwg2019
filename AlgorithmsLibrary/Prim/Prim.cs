using Core;
using System.Collections.Generic;
using System.Linq;


namespace Prim
{
    public class Graph<TVertex, TEdge> : IGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        List<TVertex> vertices;
        List<TEdge> edges;
        Dictionary<TVertex, List<TEdge>> incidentVertices;

        public Graph()
        {
            vertices = new List<TVertex>();
            edges = new List<TEdge>();
            incidentVertices = new Dictionary<TVertex, List<TEdge>>();
        }

        public IEnumerable<TVertex> Vertices { get { return vertices; } }
        public IEnumerable<TEdge> Edges { get { return edges; } }
        public IEnumerable<TEdge> IncidentEdges(TVertex v) { return incidentVertices[v]; }

        public void AddVertex(TVertex v)
        {
            vertices.Add(v);
            incidentVertices.Add(v, new List<TEdge>());
        }

        public void RemoveVertex(TVertex v)
        {
            vertices.Remove(v);
            incidentVertices.Remove(v);

            foreach (KeyValuePair<TVertex, List<TEdge>> entry in incidentVertices)
            {
                foreach (TEdge e in entry.Value)
                {
                    if (e.Start.Equals(v) || e.End.Equals(v))
                        entry.Value.Remove(e);
                }
            }
        }

        public void AddEdge(TEdge e)
        {
            edges.Add(e);
            incidentVertices[e.Start].Add(e);
            incidentVertices[e.End].Add(e);

        }

        public void RemoveEdge(TEdge e)
        {
            edges.Remove(e);

            foreach (KeyValuePair<TVertex, List<TEdge>> entry in incidentVertices)
            {
                foreach (TEdge E in entry.Value)
                {
                    if (E.Equals(e))
                        entry.Value.Remove(e);
                }
            }
        }
    }



    public static class Prim
    {
        public static Graph<TVertex,TEdge> FindMST<TVertex, TEdge>(IGraph<TVertex,TEdge> IG)
            where TEdge : IEdge<TVertex>
        {       
            //lista wierzcholkow nie bedacych jeszcze w drzewie MST
            List<TVertex> VerticesLeft = new List<TVertex>();
            foreach (TVertex v in IG.Vertices) {
                VerticesLeft.Add(v);
            }

            //Utwórz drzewo zawierające jeden wierzchołek, dowolnie wybrany z grafu.
            Graph<TVertex, TEdge> MST = new Graph<TVertex, TEdge>();
            MST.AddVertex(VerticesLeft[0]);

            //Utwórz kolejkę priorytetową, zawierającą wierzchołki osiągalne z MDR(w tym momencie zawiera jeden wierzchołek), 
            //o priorytecie najmniejszego kosztu dotarcia do danego wierzchołka z MDR.
            BinaryHeap<TVertex, TEdge> EdgeQueue = new BinaryHeap<TVertex, TEdge>(IG.Edges.Count());
            foreach (TEdge e in IG.IncidentEdges(VerticesLeft[0]) ) { 
                EdgeQueue.Insert(e);
            }
            VerticesLeft.RemoveAt(0);

            bool end = false;
            bool start = false;
            //Powtarzaj, dopóki drzewo nie obejmuje wszystkich wierzchołków grafu:
            while (VerticesLeft.Count > 0)
            {
                //wśród nieprzetworzonych wierzchołków(spoza obecnego MDR) wybierz ten, dla którego koszt dojścia z obecnego MDR jest najmniejszy.
                TEdge MinEdge = EdgeQueue.ExtractMin();

                end = VerticesLeft.Contains(MinEdge.End);
                start = VerticesLeft.Contains(MinEdge.Start);
                if (end || start)
                {
                    if (end)
                    {
                        //dodaj do obecnego MDR wierzchołek i krawędź realizującą najmniejszy koszt
                        MST.AddVertex(MinEdge.End);
                        MST.AddEdge(MinEdge);

                        //zaktualizuj kolejkę priorytetową, uwzględniając nowe krawędzie wychodzące z dodanego wierzchołka
                        VerticesLeft.Remove(MinEdge.End);
                        //dodaj krawedzie wychodzace z nowo-dodanego wierzcholka ale takie ktore nie lacza sie z wierzcholkami juz bedacymi w MST
                        foreach (TEdge e in IG.IncidentEdges(MinEdge.End))
                        {
                            if (VerticesLeft.Contains(e.End) || VerticesLeft.Contains(e.Start))
                                EdgeQueue.Insert(e);
                        }
                    }
                    else //if (start)
                    {
                        //dodaj do obecnego MDR wierzchołek i krawędź realizującą najmniejszy koszt
                        MST.AddVertex(MinEdge.Start);
                        MST.AddEdge(MinEdge);

                        //zaktualizuj kolejkę priorytetową, uwzględniając nowe krawędzie wychodzące z dodanego wierzchołka
                        VerticesLeft.Remove(MinEdge.Start);
                        //dodaj krawedzie wychodzace z nowo-dodanego wierzcholka ale takie ktore nie lacza sie z wierzcholkami juz bedacymi w MST
                        foreach (TEdge e in IG.IncidentEdges(MinEdge.Start))
                        {
                            if (VerticesLeft.Contains(e.End) || VerticesLeft.Contains(e.Start))
                                EdgeQueue.Insert(e);
                        }
                    }
                    
                }
            }

            return MST;
        }
    }
}
