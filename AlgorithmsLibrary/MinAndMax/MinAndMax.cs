using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsLibrary.MinAndMax
{
    // Example:
    //            var minMax = new MinAndMax<Vertex, Edge>(2);
    //              var resultValue = minMax.FindMinAndMax(graph, v0, 0, 0, true);
    //              var resultVertex = minMax.ResultVertex;

    public class MinAndMax<TVertex, TEdge> where TEdge : IEdge<TVertex>
    {
        private Stack<TEdge> _edges;
        private int _depth;
        private double _oldValue;

        public TVertex ResultVertex { get; set; }

        public MinAndMax(int depth)
        {
            _depth = depth;
            _oldValue = 0;
            _edges = new Stack<TEdge>();
        }

        public double FindMinAndMax(IGraph<TVertex, TEdge> iG, TVertex vertex, double value, int depth, bool maximizingPlayer)
        {
            if (depth == _depth)
                return value;

            if (maximizingPlayer)
            {
                value = Double.MinValue;

                var edges = iG.IncidentEdges(vertex);

                foreach (var item in edges)
                {
                    if (_edges.Contains(item))
                        continue;
                    
                    value = Math.Max(value, item.Weight);
                    if (value != _oldValue)
                        ResultVertex = vertex;

                    _oldValue = value;

                    _edges.Push(item);
                    vertex = item.End;

                    var nextValue = FindMinAndMax(iG, vertex, value, depth + 1, false);
                    value = Math.Max(value, nextValue);
                }

                return value;
            }
            else
            {
                value = Double.MaxValue;
                var edges = iG.IncidentEdges(vertex);

                foreach (var item in edges)
                {
                    if (_edges.Contains(item))
                        continue;

                    value = Math.Min(value, item.Weight);
                    _edges.Push(item);
                    vertex = item.End;
                    var nextValue = FindMinAndMax(iG, vertex, value, depth + 1, true);
                    value = Math.Min(value, nextValue);
                }

                return value;
            }
        }
    }
}
