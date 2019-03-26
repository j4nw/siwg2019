using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsLibrary.TreeSearchWithQueue
{
    public interface IFringe<Element>
    {
        void Add(Element element);
        bool IsEmpty { get; }
        Func<Element, int> GetCost { get; set; }
        Element Pop();
    }
}
