using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsLibrary.MinAndMax
{
    public class MinAndMax<TVertex, TEdge> where TEdge : IEdge<TVertex>
    {
        private Stack<TEdge> _edges;
        private int _depth;

        public TVertex ResultVertex { get; set; }

        public MinAndMax(int depth)
        {
            _depth = depth;
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
                    _edges.Push(item);
                    var nextValue = FindMinAndMax(iG, item.End, value, depth + 1, false);
                    value = Math.Max(value, nextValue);
                }

                ResultVertex = vertex;
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
                    value = Math.Min(value, FindMinAndMax(iG, item.End, value, depth + 1, true));
                }

                ResultVertex = vertex;
                return value;
            }
        }
    }
}
