//using AlgorithmsLibrary.PrimiKruskal;
//using Core;
//using System.Linq;

//namespace Kruskal
//{
//    public class Kruskal
//    {
//        public static Graph<TVertex, TEdge> FindMST<TVertex, TEdge>(IGraph<TVertex, TEdge> IG)
//            where TEdge : IEdge<TVertex>
//        {
//            //Utwórz las L z wierzchołków oryginalnego grafu – każdy wierzchołek jest na początku osobnym drzewem.
//            UnionFind<TVertex> L = new UnionFind<TVertex>(IG.Vertices);

//            Graph<TVertex, TEdge> MST = new Graph<TVertex, TEdge>();
//            foreach (TVertex v in IG.Vertices)
//            {
//                MST.AddVertex(v);
//            }

//            //Utwórz zbiór S zawierający wszystkie krawędzie oryginalnego grafu.
//            TEdge[] S = new TEdge[IG.Edges.Count()];
//            int i = 0;
//            foreach (TEdge e in IG.Edges)
//            {
//                S[i] = e;
//                i++;
//            }
//            //posortowane rosnąco po wadze krawędzi
//            QuickSort.Sort<TVertex, TEdge>(S);

//            i = 0;
//            //Dopóki MST nie jest drzewem rozpinającym:
//            while (MST.Edges.Count() != MST.Vertices.Count() - 1)
//            {
//                //Wybierz i usuń z S jedną z krawędzi o minimalnej wadze.
//                if (i >= S.Length)
//                    break;
//                TEdge e = S[i];
//                i++;

//                TVertex startRoot = L.Find(e.Start);
//                TVertex endRoot = L.Find(e.End);

//                //Jeśli krawędź ta łączyła dwa różne drzewa, to dodaj ją do lasu L, tak aby połączyła dwa odpowiadające drzewa w jedno.
//                if (L.DictID(startRoot) != L.DictID(endRoot))
//                {
//                    L.Union(e.Start, e.End);
//                    MST.AddEdge(e);
//                }
//            }

//            return MST;
//        }
//    }
//}
