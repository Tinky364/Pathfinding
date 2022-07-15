using Godot;

namespace Dijkstra
{
    public class Edge<T> : Reference where T : IPoint
    {
        public T From { get; }
        public T To { get; }
        public int Cost { get; }

        public Edge(T from, T to, int cost)
        {
            From = from;
            To = to;
            Cost = cost;
        }
        
        public void Print()
        {
            GD.Print($"{From.Name} => {To.Name}");
        }
    }
}
