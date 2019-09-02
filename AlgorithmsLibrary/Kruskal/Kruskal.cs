using AlgorithmsLibrary.PrimiKruskal;
using Core;
using System.Linq;

namespace Kruskal
{
    public class Kruskal
    {
        //znajduje minimalne drzewo rozpinające podanego grafu IG stosując algorytm Kruskala
        //graf IG musi być spójny i ważony
        public static Graph<TVertex, TEdge> FindMST<TVertex, TEdge>(IGraph<TVertex, TEdge> IG)
            where TEdge : IEdge<TVertex>
        {
            //las L z wierzchołków oryginalnego grafu IG – każdy wierzchołek jest na początku osobnym drzewem
            UnionFind<TVertex> L = new UnionFind<TVertex>(IG.Vertices);

            //graf MST który będzie minimalnym drzewem rozpinającym
            Graph<TVertex, TEdge> MST = new Graph<TVertex, TEdge>();

            //dodawanie wierzchołków do drzewa rozpinającego
            foreach (TVertex v in IG.Vertices)
            {
                MST.AddVertex(v);
            }

            //zbiór S zawierający wszystkie krawędzie oryginalnego grafu
            TEdge[] S = new TEdge[IG.Edges.Count()];
            int i = 0;
            foreach (TEdge e in IG.Edges)
            {
                S[i] = e;
                i++;
            }

            //posortowane rosnąco po wagach krawędzi
            QuickSort.Sort<TVertex, TEdge>(S);

            i = 0;
            //powtarzaj dopóki MST nie jest drzewem rozpinającym (wszystkie wierzchołki nie są połączone)
            while (MST.Edges.Count() != MST.Vertices.Count() - 1)
            {
                if (i >= S.Length)
                    break;
                //krawędź o najmniejszej wadze (pierwsza z posortowanej listy)
                TEdge e = S[i];
                i++;

                //korzenie wierzchołków, które łączy wybrana krawędź e
                TVertex startRoot = L.Find(e.Start);
                TVertex endRoot = L.Find(e.End);

                //jeśli krawędź ta łączyła dwa różne drzewa (id korzeni są różne)
                if (L.DictID(startRoot) != L.DictID(endRoot))
                {
                    //połącz dwa drzewa w jedno
                    L.Union(e.Start, e.End);
                    //dodaj krawędź do drzewa rozpinającego
                    MST.AddEdge(e);
                }
            }

            return MST;
        }
    }
}
