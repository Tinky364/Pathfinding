using System.Collections.Generic;
using Dijkstra;

namespace TileMap
{
    public class Tile : IPoint
    {
        public int X { get; }
        public int Y { get; }
        public string Name { get; }
        public int Cost { get; }

        public Tile(int x, int y, int cost = 1)
        {
            X = x;
            Y = y;
            Name = ToName(x, y);
            Cost = cost;
        }

        public static string ToName(int x, int y) => $"{x},{y}";

        public static (int, int) ToCoordinate(string name)
        {
            string[] splitName = name.Split(',');
            return (int.Parse(splitName[0]), int.Parse(splitName[1]));
        }

        public static List<string> NeighborsNames(int x, int y, int xSize, int ySize)
        {
            List<string> list = new List<string>();
            if (x != xSize - 1) list.Add(ToName(x + 1, y));
            if (y != ySize - 1) list.Add(ToName(x, y + 1));
            if (x != 0) list.Add(ToName(x - 1, y));
            if (y != 0) list.Add(ToName(x, y - 1));
            return list;
        }

        public static List<string> NeighborsNames(Tile tile, int xSize, int ySize) =>
            NeighborsNames(tile.X, tile.Y, xSize, ySize);
    }
}
