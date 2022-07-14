using Godot;

namespace Dijkstra
{
    public class Edge : Reference
    {
        public object From { get; }
        public object To { get; }
        public int Cost { get; }

        public Edge(object from, object to, int cost)
        {
            From = from;
            To = to;
            Cost = cost;
        }

        public void Print()
        {
            GD.Print($"{(From as Dots)?.Name} => {(To as Dots)?.Name}");
        }
    }
}
