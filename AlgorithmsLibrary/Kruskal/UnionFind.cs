using System.Collections.Generic;
using System.Linq;


namespace Kruskal
{
    //struktura zbiorów rozłącznych
    public class UnionFind<TVertex>
    {
        //słownik tłumaczący wierzchołki na odpowiadające im indeksy
        IDictionary<TVertex, int> dictID;
        //ilość elementów (wierzchołków) w strukturze
        int N;
        //ilość elementów zbioru i (count[i]), poprawna wartość tylko jeśli i jest indeksem korzenia
        int[] count;
        //rodzic wierzchołka i (parent[i]), jeśli i jest korzeniem, to parent[id] = null
        TVertex[] parent;

        public UnionFind(IEnumerable<TVertex> list)
        {
            N = list.Count();
            count = new int[N];
            parent = new TVertex[N];

            dictID = new Dictionary<TVertex, int>();

            int i = 0;
            //ustaw każdy wierzchołek jako jednoelementowy zbiór
            foreach (TVertex v in list)
            {
                dictID.Add(v, i);
                parent[i] = default(TVertex);
                count[i] = 1;
                i++;
            }
        }

        public int DictID(TVertex V)
        {
            return dictID[V];
        }

        public int Count(TVertex V)
        {
            int rootId = dictID[Find(V)];
            return count[rootId];
        }


        //wyznacza, w którym zbiorze jest podany wierzchołek, zwracając korzeń tego zbioru
        //wykonuje komprescję ścieżki podczas szukania
        public TVertex Find(TVertex V)
        {
            //znajdź indeks podanego wierzchołka
            int id = dictID[V];
            //jeśli nie ma on rodzica, zwróć ten wierzchołek
            if (parent[id] == null)
                return V;

            //zapisz korzeń szukanego zbioru jako rodzic podanego wierzchołka
            parent[id] = Find(parent[id]);
            return parent[id];
        }

        //łączy dwa zbiory, w których są podane wierzchołki, w jeden.
        public void Union(TVertex X, TVertex Y)
        {
            //znajdź korzenie zbiorów podanych wierzchołków
            TVertex XRoot = Find(X);
            TVertex YRoot = Find(Y);

            //znajdź indeksy korzeni zbiorów
            int idX = dictID[XRoot];
            int idY = dictID[YRoot];

            //dołącz mniejszy zbiór do korzenia większego, jeśli zbiory są różne
            if (count[idX] > count[idY])
            {
                parent[idY] = XRoot;
                count[idX] += count[idY];
            }
            else if (count[idX] < count[idY])
            {
                parent[idX] = YRoot;
                count[idY] += count[idX];
            }
            else if (idX != idY)
            {
                parent[idY] = XRoot;
                count[idX] += count[idY];
            }
        }
    }
}
