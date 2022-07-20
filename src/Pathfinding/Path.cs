using System.Collections.Generic;
using System.Text;
using Godot;

namespace Pathfinding
{
    public class Path<T> : Reference where T : IPoint
    {
        public float Cost { get; private set; }
        public int Count => _list.Count;
        public Edge<T> this[int i] => _list[i];

        private readonly List<Edge<T>> _list;

        public Path()
        {
            _list = new List<Edge<T>>();
            Cost = 0;
        }

        public void Add(Edge<T> edge)
        {
            _list.Add(edge);
            Cost += edge.Cost;
        }

        public void Reverse()
        {
            _list.Reverse();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(
                $"Path: Start = {_list[0].From.Name}, Goal = {_list[_list.Count - 1].To.Name} \n" +
                $"{_list[0].From.Name} -> {_list[0].To.Name}"
            );
            for (int i = 1; i < _list.Count; i++) sb.Append($" -> {_list[i].To.Name}");
            sb.Append("\n------------------------");
            return sb.ToString();
        }
    }
}
