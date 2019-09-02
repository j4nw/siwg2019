using Core;
using System.Collections.Generic;


namespace AlgorithmsLibrary.PrimiKruskal
{
    //klasa reprezentująca graf
    //każda krawędź grafu jest nieskierowana i ważona
    public class Graph<TVertex, TEdge> : IGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        //lista wierzchołków grafu
        protected List<TVertex> vertices;

        //lista krawędzi grafu
        protected List<TEdge> edges;

        //słownik przechowujący informacje o sąsiadujących wierzchołkach (krawędziach wychodzących z grafu)
        protected Dictionary<TVertex, List<TEdge>> incidentEdges;

        public Graph()
        {
            vertices = new List<TVertex>();
            edges = new List<TEdge>();
            incidentEdges = new Dictionary<TVertex, List<TEdge>>();
        }

        public IEnumerable<TVertex> Vertices { get { return vertices; } }
        public IEnumerable<TEdge> Edges { get { return edges; } }
        public IEnumerable<TEdge> IncidentEdges(TVertex v) { return incidentEdges[v]; }

        //dodaje wierzchołek do grafu  
        public void AddVertex(TVertex v)
        {
            vertices.Add(v);
            incidentEdges.Add(v, new List<TEdge>());
        }

        //usuwa wierzchołek z grafu
        public void RemoveVertex(TVertex v)
        {
            //usuwanie wierzchołka v z listy i jego klucza ze słownika
            vertices.Remove(v);
            incidentEdges.Remove(v);

            //usuwanie wierzchołka v z list sąsiadów innych wierzchołków
            foreach (KeyValuePair<TVertex, List<TEdge>> entry in incidentEdges)
            {
                foreach (TEdge e in entry.Value)
                {
                    if (e.Start.Equals(v) || e.End.Equals(v))
                        entry.Value.Remove(e);
                }
            }
        }

        //dodaje krawędź do grafu
        public void AddEdge(TEdge e)
        {
            edges.Add(e);
            incidentEdges[e.Start].Add(e);
            incidentEdges[e.End].Add(e);
        }

        //usuwa krawędź z grafu
        public void RemoveEdge(TEdge e)
        {
            //usuwanie krawędzi e z listy
            edges.Remove(e);

            //usuwanie krawędzi e ze słownika
            foreach (KeyValuePair<TVertex, List<TEdge>> entry in incidentEdges)
            {
                foreach (TEdge E in entry.Value)
                {
                    if (E.Equals(e))
                        entry.Value.Remove(e);
                }
            }
        }
    }
}
