using System;
using Godot;

namespace Dijkstra
{
    public class PointRecord<T> : Reference, IComparable where T : IPoint
    {
        public T Point { get; }
        public Edge<T> Edge { get; set; }
        public int CostSoFar { get; set; }

        public PointRecord(T point, Edge<T> edge = null, int costSoFar = 0)
        {
            Point = point;
            Edge = edge;
            CostSoFar = costSoFar;
        }

        public int CompareTo(object obj)
        {
            switch (obj)
            {
                case null: return 1;
                case PointRecord<T> otherRecord: return CostSoFar.CompareTo(otherRecord.CostSoFar);
                default: throw new ArgumentException("Object is not a Record");
            }
        }
    }
}
