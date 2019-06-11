using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsLibrary.MinAndMax
{
    public class Graph<TVertex, TEdge> : IGraph<TVertex, TEdge>
            where TEdge : IEdgeExtension<TVertex>
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
}
