using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace GraphToImage
{
    public class Graph<TVertex, TEdge> : IGraph<TVertex, TEdge>
        where TEdge : IEdge, IEdgeExtension<TVertex>
        where TVertex : INodeExtension
    {
        public List<TVertex> nodeList = new List<TVertex>();
        public Dictionary<TVertex,List<TEdge>> edgeDict = new Dictionary<TVertex, List<TEdge>>();
        private int size = 0;
        public Graph(int size)
        {
            this.size = size;
        }

        public Graph(IGraph<TVertex, TEdge> graphToCopy)
        {
            size = graphToCopy.Vertices.Count();
            foreach(TVertex node in graphToCopy.Vertices)
            {
                this.nodeList.Add(node);
                this.edgeDict.Add(node, graphToCopy.IncidentEdges(node).ToList());
            }
        }

        public IEnumerable<TVertex> Vertices
        {
            get
            {
                return nodeList;
            }
        }

        public IEnumerable<TEdge> Edges { get { return edgeDict.SelectMany(x => x.Value); } }
        
        public IEnumerable<TEdge> IncidentEdges(TVertex vertex)
        {
            return edgeDict[vertex];
        }
    }
}
