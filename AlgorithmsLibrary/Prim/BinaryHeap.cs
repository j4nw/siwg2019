using AlgorithmsLibrary.PrimiKruskal;

namespace Prim
{
    //kopiec binarny typu min
    public class BinaryHeap<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        //tablica reprezentująca kopiec, korzeń przechowywany jest w jej pierwszej komórce
        TEdge[] heap;
        //rozmiar tablicy
        int length;
        //rozmiar kopca (ilość elementów w kopcu)
        int heapSize;

        public BinaryHeap(int len)
        {
            length = len;
            heap = new TEdge[length];
            heapSize = 0;
        }

        //zwraca ndeks rodzica węzła i niebędącego korzeniem 
        //( floor(i/2)  w indeksacji od 1)
        public int Parent(int i)
        {
            return ((i + 1) / 2) - 1;
        }

        //zwraca indeks lewego syna węzła i 
        //( 2 * i  w indeksacji od 1)
        public int ChildLeft(int i)
        {
            return 2 * (i + 1) - 1;
        }

        //zwraca indeks prawego syna węzła i 
        //( 2 * i + 1  w indeksacji od 1)
        public int ChildRight(int i)
        {
            return 2 * (i + 1);
        }

        //dodanie wierzchołka do kopca
        public void Insert(TEdge e)
        {
            //dodaj wierzchołek na koniec kopca
            heap[heapSize] = e;

            if (heapSize != 0)
            {
                int i = heapSize;
                int parenti = Parent(i);

                //dopóki wartość wierzchołka jest mniejsza od jego rodzica, zamieniej ich miejscami
                while (heap[i].Weight < heap[parenti].Weight)
                {
                    TEdge tmp = heap[i];
                    heap[i] = heap[parenti];
                    heap[parenti] = tmp;

                    i = parenti;
                    if (i == 0)
                        break;
                    parenti = Parent(i);
                }
            }

            heapSize++;
            if (heapSize > length)
                heapSize = length;
        }

        //usunięcie korzenia z kopca (wierzchołka o minimalnej wertości)
        public TEdge ExtractMin()
        {
            if (heapSize == 0)
                return default(TEdge);

            if (heapSize == 1)
            {
                heapSize--;
                return heap[0];
            }

            TEdge minEdge = heap[0];

            int parent = 0;
            //przestaw ostatni wierzchołek z pozycji heapSize na szczyt kopca
            heap[0] = heap[heapSize - 1];
            heapSize--;

            int min = parent;
            int childL = ChildLeft(parent);
            int childR = ChildRight(parent);

            //spychaj przestawiony wierzchołek w dół, zamieniając go pozycjami z większym z dzieci, aż do naprawienia kopca 
            while (true)
            {
                if (childL < heapSize && heap[min].Weight > heap[childL].Weight)
                    min = childL;

                if (childR < heapSize && heap[min].Weight > heap[childR].Weight)
                    min = childR;

                if (min == parent)
                    break;

                TEdge tmp = heap[min];
                heap[min] = heap[parent];
                heap[parent] = tmp;

                parent = min;
                childL = ChildLeft(parent);
                childR = ChildRight(parent);
            }

            return minEdge;
        }

    }
}
