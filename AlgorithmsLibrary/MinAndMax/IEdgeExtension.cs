using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsLibrary.MinAndMax
{
    public interface IEdgeExtension<TVertex>
    {
        TVertex Start { get; }
        TVertex End { get; }
        double WeightExtension { get; set; }
    }
}
