using AlgorithmsLibrary.PrimiKruskal;
using Core;
using System.Collections.Generic;
using System.Linq;


namespace Prim
{
    public static class Prim
    {
        //znajduje minimalne drzewo rozpinające podanego grafu IG stosując algorytm Prima
        //graf IG musi być spójny i ważony
        public static Graph<TVertex, TEdge> FindMST<TVertex, TEdge>(IGraph<TVertex, TEdge> IG)
            where TEdge : IEdge<TVertex>
        {
            //lista wierzchołków nie bedących jeszcze w MDR
            List<TVertex> VerticesLeft = new List<TVertex>();

            //dodawanie każdego wierzchołka grafu IG do listy
            foreach (TVertex v in IG.Vertices)
            {
                VerticesLeft.Add(v);
            }

            //graf MST który będzie minimalnym drzewem rozpinającym
            Graph<TVertex, TEdge> MST = new Graph<TVertex, TEdge>();

            //dodanie 1-wszego wierzchołka do MST
            MST.AddVertex(VerticesLeft[0]);

            //utworzenie kolejki priorytetowej zawierającej krawędzie wychodzące z MST
            BinaryHeap<TVertex, TEdge> EdgeQueue = new BinaryHeap<TVertex, TEdge>(IG.Edges.Count());
            //w tym momencie MST zawiera 1 wierzchołek, więc dodane są tylko jego krawędzie
            foreach (TEdge e in IG.IncidentEdges(VerticesLeft[0]))
            {
                EdgeQueue.Insert(e);
            }

            //usunięcie wierzchołka dodanego do MST z listy brakujących wierzchołków
            VerticesLeft.RemoveAt(0);

            bool end = false;
            bool start = false;

            //powtarzajnie dopóki drzewo nie obejmuje wszystkich wierzchołków grafu
            while (VerticesLeft.Count > 0)
            {
                //wyciągnięcie z kolejki krawędzi o najmniejszym koszcie dojścia
                TEdge MinEdge = EdgeQueue.ExtractMin();

                //zmienne bool mówiące czy wierzchołki, które łączy wyciągnięta krawędź są na liście brakujących wierzchołków
                end = VerticesLeft.Contains(MinEdge.End);
                start = VerticesLeft.Contains(MinEdge.Start);

                //zmienna przechowująca wierzchołek do dodania do drzewa
                TVertex vertexToAdd;

                //jeśli wierzchołek end nie został jeszcze dodany do drzewa
                if (end)
                    vertexToAdd = MinEdge.End;
                //jeśli wierzchołek start nie został jeszcze dodany do drzewa
                else if (start)
                    vertexToAdd = MinEdge.Start;
                //jeśli krawędź łączyła wierzchołki będące już w drzewie
                else
                    continue;

                //dodanie do drzewa wybranych wierzchołka i krawędzi
                MST.AddVertex(vertexToAdd);
                MST.AddEdge(MinEdge);

                //usunięcie dodaengo wierzchołka z listy brakujących wierzchołków
                VerticesLeft.Remove(vertexToAdd);

                //dodanie krawędzi wychodzących z nowododanego wierzchołka do kolejki, ale tylko tych, które nie utworzą pętli w drzewie
                foreach (TEdge e in IG.IncidentEdges(vertexToAdd))
                {
                    if (VerticesLeft.Contains(e.End) || VerticesLeft.Contains(e.Start))
                        EdgeQueue.Insert(e);
                }
            }

            return MST;
        }
    }
}
