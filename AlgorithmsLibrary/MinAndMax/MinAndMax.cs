using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsLibrary.MinAndMax
{
    // Example:
    //            var minMax = new MinAndMax<Vertex, Edge>(depth, graph);
    //              var resultValue = minMax.FindMinAndMax(vertex0, 0, 0, true);
    //              var resultVertex = minMax.ResultVertex;

    public class MinAndMax<TVertex, TEdge> where TEdge : IEdgeExtension<TVertex>
    {
        private IGraph<TVertex, TEdge> _iG;
        private Stack<TEdge> _edges;
        private int _depth;

        public TVertex ResultVertex { get; set; }

        public MinAndMax(int depth, IGraph<TVertex, TEdge> iG)
        {
            _iG = iG;
            _depth = depth;
            _edges = new Stack<TEdge>();
        }

        public double FindMinAndMax(TVertex vertex, double value, int depth, bool maximizingPlayer)
        {
            if (depth == _depth)
                return value;

            if (maximizingPlayer)
            {
                value = Double.MinValue;
                var edges = _iG.IncidentEdges(vertex);

                foreach (var item in edges)
                {
                    if (_edges.Contains(item))
                        continue;
                    
                    value = Math.Max(value, item.WeightExtension);

                    _edges.Push(item);
                    vertex = item.End;

                    var nextValue = FindMinAndMax(vertex, value, depth + 1, false);
                    value = Math.Max(value, nextValue);

                    _iG.Edges.Where(edge => Equals(edge.End, vertex)).First().WeightExtension = nextValue;

                }

                ResultVertex = _iG.Edges.Where(edge => Equals(edge.Start, _iG.Edges.First().Start)).OrderByDescending(item => item.WeightExtension).First().End;
                return value;
            }
            else
            {
                value = Double.MaxValue;
                var edges = _iG.IncidentEdges(vertex);

                foreach (var item in edges)
                {
                    if (_edges.Contains(item))
                        continue;

                    value = Math.Min(value, item.WeightExtension);
                    _edges.Push(item);
                    vertex = item.End;
                    var nextValue = FindMinAndMax(vertex, value, depth + 1, true);
                    value = Math.Min(value, nextValue);
                }

                return value;
            }
        }
    }
}
