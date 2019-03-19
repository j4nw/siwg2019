using System.Collections.Generic;

namespace Core
{
    interface IGraph<TVertex, TEdge>
    {
        IEnumerable<TVertex> Vertices { get; }
        IEnumerable<TEdge> Edges { get; }
        IEnumerable<TEdge> IncidentVertices(TVertex vertex);
    }
}
