using System.Collections.Generic;
using System.Linq;


namespace Kruskal
{
    public class UnionFind<TVertex>
    {
        IDictionary<TVertex, int> dictID;
        int N;
        int[] count;      
        TVertex[] parent;                                   

        public UnionFind(IEnumerable<TVertex> list) 
        {
            N = list.Count();
            count = new int[N];
            parent = new TVertex[N];

            dictID = new Dictionary<TVertex, int>();

            int i = 0;
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

        public TVertex Find(TVertex V)
        {
            int id = dictID[V];
            if (parent[id] == null)
                return V;

            parent[id] = Find(parent[id]);
            return parent[id];
        }

	    public void Union(TVertex X, TVertex Y)
        {
            TVertex XRoot = Find(X);
            TVertex YRoot = Find(Y);

            int idX = dictID[XRoot];
            int idY = dictID[YRoot];

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

        public int Count(TVertex V)
        {
            int rootId = dictID[Find(V)];
            return count[rootId];
        }
    }
}
