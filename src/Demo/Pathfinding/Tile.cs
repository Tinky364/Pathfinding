using System.Collections.Generic;
using Godot;

namespace Pathfinding
{
    public class Tile : Reference, IPoint
    {
        public string Name { get; }
        public int X { get; }
        public int Y { get; }
        public int Cost { get; }

        public float EstimateTotalCost<T>(T goalPoint) where T : IPoint
        {
            return goalPoint is Tile goalTile ? Distance(X, Y, goalTile.X, goalTile.Y) : 0;
        }

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

        public static IEnumerable<string> NeighborsNames(int x, int y, int xSize, int ySize)
        {
            if (x != xSize - 1)
            {
                yield return ToName(x + 1, y);
                if (y != ySize - 1) yield return ToName(x + 1, y + 1);
            }
            if (y != ySize - 1)
            {
                yield return ToName(x, y + 1);
                if (x != 0) yield return ToName(x - 1, y + 1);
            }
            if (x != 0)
            {
                yield return ToName(x - 1, y);
                if (y != 0) yield return ToName(x - 1, y - 1);
            }
            if (y != 0)
            {
                yield return ToName(x, y - 1);
                if (x != xSize - 1) yield return ToName(x + 1, y - 1);
            }
        }

        public static IEnumerable<string> NeighborsNames(Tile tile, int xSize, int ySize)
        {
            return NeighborsNames(tile.X, tile.Y, xSize, ySize);
        }

        public static float Distance(int x1, int y1, int x2, int y2)
        {
            return Mathf.Sqrt(Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2));
        }

        public static float Distance(Tile tile1, Tile tile2)
        {
            return Distance(tile1.X, tile1.Y, tile2.X, tile2.Y);
        }

        public static float CostOfMove(Tile from, Tile to)
        {
            float distance = Distance(from, to);
            float tileCost = to.Cost;
            return distance * tileCost;
        }
    }
}
