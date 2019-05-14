using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsLibrary.MinAndMax
{
    public interface IEdgeExtension<TVertex> : IEdge<TVertex>
    {
        double WeightExtension { get; set; }
    }
}
