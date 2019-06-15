//using AlgorithmsLibrary.PrimiKruskal;

//namespace Prim
//{
//    public class BinaryHeap<TVertex, TEdge>
//        where TEdge : IEdge<TVertex>
//    {
//        TEdge[] heap;
//        int length;
//        int heapSize;


//        public BinaryHeap(int len)
//        {
//            length = len;
//            heap = new TEdge[length];
//            heapSize = 0;
//        }

//        public int Parent(int i) {
//            return ((i + 1) / 2) - 1;
//        }

//        public int ChildLeft(int i) {
//            return 2 * (i + 1) - 1;
//        }

//        public int ChildRight(int i) {
//            return 2 * (i + 1);
//        }


//        public void Insert(TEdge e)
//        {
//            heap[heapSize] = e;

//            if (heapSize != 0)
//            {
//                int i = heapSize;
//                int parenti = Parent(i);
//                while (heap[i].Weight < heap[parenti].Weight)
//                {
//                    TEdge tmp = heap[i];
//                    heap[i] = heap[parenti];
//                    heap[parenti] = tmp;

//                    i = parenti;
//                    parenti = Parent(i);
//                    if (i == 0)
//                        break;
//                }
//            }

//            heapSize++;
//            if (heapSize > length)
//                heapSize = length;
//        }

       
//        public TEdge ExtractMin()
//        {
//            if (heapSize == 0)
//                return default(TEdge);

//            if (heapSize == 1)
//            {
//                heapSize--;
//                return heap[0];
//            }

//            TEdge minEdge = heap[0];
//            if (heapSize > 1)
//            {
//                int parent = 0;
//                heap[0] = heap[heapSize-1];
//                heapSize--;

//                int min = parent;
//                int childL = ChildLeft(parent);
//                int childR = ChildRight(parent);

//                while (true)
//                {
//                    if (childL < heapSize && heap[min].Weight > heap[childL].Weight)
//                        min = childL;

//                    if (childR < heapSize && heap[min].Weight > heap[childR].Weight)
//                        min = childR;

//                    if (min == parent)
//                        break;

//                    TEdge tmp = heap[min];
//                    heap[min] = heap[parent];
//                    heap[parent] = tmp;

//                    parent = min;
//                    childL = ChildLeft(parent);
//                    childR = ChildRight(parent);
//                    min = parent;
//                }
//            }
//            return minEdge;
//        }  

//    }
//}
