using AlgorithmsLibrary.PrimiKruskal;

namespace Kruskal
{
    public static class QuickSort
    {
        public static void Sort<TVertex, TEdge>(TEdge[] S)
            where TEdge : IEdge<TVertex>
        {
            Sort<TVertex, TEdge>(S, 0, S.Length - 1);
        }

        static void Sort<TVertex, TEdge>(TEdge[] S, int l, int r)
            where TEdge : IEdge<TVertex>
        {
            if (l < r)
            {
                //dzielenie tablicy na 2 części
                //i - indeks elementu, na którym tablica została podzielona
                int i = Partition<TVertex, TEdge>(S, l, r);
                //posortuj lewą część 
                Sort<TVertex, TEdge>(S, l, i - 1);
                //posortuj prawą część 
                Sort<TVertex, TEdge>(S, i + 1, r);
            }
        }

        //dzieli tablicę na 2 częsci i zwraca indeks elementu podziału
        //przenosi wszystkie elementy mniejsze od niego na lewo, a elementy większe lub równe, na prawo
        static int Partition<TVertex, TEdge>(TEdge[] S, int l, int r)
            where TEdge : IEdge<TVertex>
        {
            //wybierz element, który posłuży do podziału tablicy
            int divIndex = l + (r - l) / 2;
            //wartość wybranego elementu
            double divideValue = S[divIndex].Weight;
            //zamień element podziału z ostatnim elementem, aby sam nie brał udziału w podziale
            Swap<TVertex, TEdge>(S, divIndex, r);

            //currIndex - po zakończeniu przenoszenia będzie indeksem miejsca podziału
            int currIndex = l;
            //iteruj przez wszystkie elementy
            for (int i = l; i < r; i++)
            {
                //jeśli element jest mniejszy niż wartość elementu podziału dodaj go do "lewej" strony
                if (S[i].Weight < divideValue)
                {
                    Swap<TVertex, TEdge>(S, i, currIndex);
                    currIndex++;
                }
            }

            //wstaw element podziału we właściwe miejsce
            Swap<TVertex, TEdge>(S, currIndex, r);
            return currIndex;
        }

        //zamienia miejscami elementy w komórce i z elementem j
        static void Swap<TVertex, TEdge>(TEdge[] S, int i, int j)
            where TEdge : IEdge<TVertex>
        {
            TEdge tmp = S[i];
            S[i] = S[j];
            S[j] = tmp;
        }
    }
}
