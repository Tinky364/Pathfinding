using System.Collections.Generic;
using Dijkstra;
using Godot;

namespace TileMap
{
    public class PathFinder : Node
    {
        public override void _Ready()
        {
            base._Ready();

            DijkstraAlgorithm dijkstraAlgorithm = new DijkstraAlgorithm();
            Map map = new Map(10, 10);
            List<Edge<Tile>> path1 = dijkstraAlgorithm.FindPath(
                map, map.GetTile(0, 0), map.GetTile(3, 4)
            );
            PrintPath(path1);
        }

        private void PrintPath(List<Edge<Tile>> path)
        {
            foreach (var edge in path)
            {
                GD.Print($"{edge.From.Name} => {edge.To.Name}");
            }
        }
    }
}
