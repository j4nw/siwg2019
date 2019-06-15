using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphToImage
{
    public interface IEdgeExtension<TVertex>
    {
        TVertex target { get; set; }
    }
}
