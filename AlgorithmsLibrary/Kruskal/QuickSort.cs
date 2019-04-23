using Core;

namespace Kruskal
{
    public static class QuickSort
    {
        public static void Sort<TVertex, TEdge>(TEdge[] S)
            where TEdge : IEdge<TVertex>
        {
            Sort<TVertex,TEdge>(S, 0, S.Length-1);
        }

        static void Sort<TVertex, TEdge>(TEdge[] S, int l, int r)   
            where TEdge : IEdge<TVertex>
        {
            if (l < r)
            {
                int i = Partition<TVertex, TEdge>(S, l, r);   //podziel i zapamiętaj punkt podziału 
                Sort<TVertex, TEdge>(S, l, i - 1);  //posortuj lewą część 
                Sort<TVertex, TEdge>(S, i + 1, r);   //posortuj prawą część 
            }
        }

       // wybiera element, który ma być użyty do podziału
       //i przenosi wszystkie elementy mniejsze na lewo od
       //tego elementu, a elementy większe lub równe, na prawo
       //od wybranego elementu
        static int Partition<TVertex, TEdge>(TEdge[] S, int l, int r)
            where TEdge : IEdge<TVertex>
        {
            int divIndex = l + (r - l) / 2;    //wybierz element, który posłuży do podziału tablicy
            double divideValue = S[divIndex].Weight;    //zapamiętaj wartość elementu
            Swap<TVertex, TEdge>(S, divIndex, r);   //przenieś element podziału na koniec tablicy, aby sam nie brał udziału w podziale

            int currIndex = l;
            //iteruj przez wszystkie elementy, jeśli element jest mniejszy niż wartość elementu podziału dodaj go do "lewej" strony
            for (int i = l; i < r; i++)
            {
                if (S[i].Weight < divideValue)
                {
                    Swap<TVertex, TEdge>(S, i, currIndex);
                    currIndex++;
                }
            }

            Swap<TVertex, TEdge>(S, currIndex, r);  //wstaw element podziału we właściwe miejsce
            return currIndex;
        }

        //zamienia miejscami elementy w komórkach i, j
        static void Swap<TVertex, TEdge>(TEdge[] S, int i, int j)
            where TEdge : IEdge<TVertex>
        {
            TEdge tmp = S[i];
            S[i] = S[j];
            S[j] = tmp;
        }
    }
}
