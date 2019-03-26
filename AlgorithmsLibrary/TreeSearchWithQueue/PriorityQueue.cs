using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsLibrary.TreeSearchWithQueue
{
    public class PriorityQueue<Element> : IFringe<Element>
    {
        private List<Element> queue = new List<Element>();

        public Func<Element, int> GetCost { get; set; }

        public PriorityQueue()
        {
            GetCost = x => { return 0; };
        }

        public bool IsEmpty { get { return queue.Count == 0; } }

        public void Add(Element element)
        {
            queue.Add(element);
            buildHeap(queue, queue.Count);
        }

        public Element Pop()
        {
            Element ret = queue[0];
            queue.RemoveAt(0);
            return ret;
        }

        // ---HEAP---
        private void heapify(List<Element> elemArray, int i, int heapSize)
        {
            Element x;
            int left, right, smallest;
            left = 2 * i + 1;
            right = 2 * i + 2;
            if (left < heapSize && GetCost(elemArray[left]) < GetCost(elemArray[i])) smallest = left;
            else smallest = i;
            if (right < heapSize && GetCost(elemArray[right]) < GetCost(elemArray[smallest])) smallest = right;
            if (smallest != i)
            {
                x = elemArray[i];
                elemArray[i] = elemArray[smallest];
                elemArray[smallest] = x;
                heapify(elemArray, smallest, heapSize);
            }
        }

        private void buildHeap(List<Element> elemArray, int n)
        {
            for (int i = (n - 1) / 2; i >= 0; i--)
            {
                heapify(elemArray, i, n);
            }
        }
    }
}
