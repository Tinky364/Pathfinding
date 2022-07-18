using System;
using Godot;

namespace Pathfinding
{
    public class PointRecord<T> : Reference, IComparable where T : IPoint
    {
        public Pathfinder.Type CurType { get; }
        public T Point { get; }
        public Edge<T> Edge { get; set; }
        public float CostSoFar { get; set; }
        public float EstimatedTotalCost { get; set; }
        public PointRecord<T> Next { get; set; }

        public PointRecord(T point, Edge<T> edge = null, float costSoFar = 0)
        {
            CurType = Pathfinder.Type.Dijkstra;
            Point = point;
            Edge = edge;
            CostSoFar = costSoFar;
            EstimatedTotalCost = 0;
        }

        public PointRecord(
            T point, float estimatedTotalCost, Edge<T> edge = null, float costSoFar = 0
        )
        {
            CurType = Pathfinder.Type.AStar;
            Point = point;
            Edge = edge;
            CostSoFar = costSoFar;
            EstimatedTotalCost = estimatedTotalCost;
        }

        public int CompareTo(object obj)
        {
            switch (obj)
            {
                case null: return 1;
                case PointRecord<T> otherRecord:
                    return otherRecord.CurType == Pathfinder.Type.AStar
                        ? EstimatedTotalCost.CompareTo(otherRecord.EstimatedTotalCost)
                        : CostSoFar.CompareTo(otherRecord.CostSoFar);
                default: throw new ArgumentException("Object is not a Record");
            }
        }
    }
}
