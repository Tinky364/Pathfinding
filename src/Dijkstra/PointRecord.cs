using System;
using Godot;

namespace Dijkstra
{
    public class PointRecord : Reference, IComparable
    {
        public object Point { get; }
        public Edge Edge { get; set; }
        public int CostSoFar { get; set; }

        public PointRecord(object point, Edge edge = null, int costSoFar = 0)
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
                case PointRecord otherRecord: return CostSoFar.CompareTo(otherRecord.CostSoFar);
                default: throw new ArgumentException("Object is not a Record");
            }
        }
    }
}
