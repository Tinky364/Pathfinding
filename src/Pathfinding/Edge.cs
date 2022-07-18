using Godot;

namespace Pathfinding
{
    public class Edge<T> : Reference where T : IPoint
    {
        public T From { get; }
        public T To { get; }
        public float Cost { get; }

        public Edge(T from, T to, float cost)
        {
            From = from;
            To = to;
            Cost = cost;
        }
    }
}
