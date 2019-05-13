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
        public IGraph<TVertex, IEdge<TVertex>> IG { get; set; }
        public TVertex ResultVertex { get; set; }

        public MinAndMax(IGraph<TVertex, IEdge<TVertex>> ig)
        {
            IG = ig;
        }

        public double FindMinAndMax(TVertex vertex, double value, int depth, bool maximizingPlayer)
        {
            if (maximizingPlayer)
            {
                value = Double.MinValue;

                foreach (var item in IG.IncidentEdges(vertex))
                {
                    value = item.Weight;
                    value = Math.Max(value, FindMinAndMax(item.End, 0, depth - 1, false));
                }

                ResultVertex = vertex;
                return value;
            }
            else
            {
                value = Double.MaxValue;
                foreach (var item in IG.IncidentEdges(vertex))
                {
                    value = item.Weight;
                    value = Math.Min(value, FindMinAndMax(item.End, 0, depth - 1, true));
                }

                ResultVertex = vertex;
                return value;
            }
        }
    }
}
