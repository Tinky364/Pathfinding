using Godot;
using System.Collections.Generic;

namespace Dijkstra
{
    public class Graph : Reference
    {
        private readonly List<Edge> _edges;

        public int Count => _edges.Count;

        public Graph() { }

        public Graph(params Edge[] edges)
        {
            _edges = new List<Edge>();
            foreach (Edge edge in edges) _edges.Add(edge);
        }

        public void AddEdge(Edge edge)
        {
            _edges.Add(edge);
        }

        public void RemoveEdge(Edge edge)
        {
            _edges.Remove(edge);
        }

        public IEnumerable<Edge> GetAllEdges()
        {
            return _edges;
        }

        public IEnumerable<Edge> GetEdges(PointRecord pointRecord)
        {
            List<Edge> edgesOfRecord = new List<Edge>();
            foreach (Edge edge in _edges)
            {
                if (edge.From == pointRecord.Point) edgesOfRecord.Add(edge);
            }
            return edgesOfRecord;
        }
    }
}
