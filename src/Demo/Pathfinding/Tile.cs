using System.Collections.Generic;
using Godot;
using Pathfinding;

namespace Demo.Pathfinding
{
    public class Tile : Reference, IPoint
    {
        public string Key { get; }
        public int Cost { get; }
        public Coordinate Coor { get; }

        public Tile(Coordinate coor, int cost = 1)
        {
            Coor = coor;
            Key = ToName(coor);
            Cost = cost;
        }

        public float EstimateTotalCost<T>(T goalPoint) where T : IPoint =>
            goalPoint is Tile goalTile ? Distance(Coor, goalTile.Coor) : 0;

        public override string ToString() => $"Tile({Key})";

        public static string ToName(Coordinate coor) => $"{coor.X},{coor.Y}";

        public static Coordinate ToCoordinate(string name)
        {
            string[] splitName = name.Split(',');
            return new Coordinate(int.Parse(splitName[0]), int.Parse(splitName[1]));
        }

        public static IEnumerable<Coordinate> Possible8Neighbors(Coordinate coor)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0 ) continue;
                    yield return new Coordinate(coor.X + x, coor.Y + y);
                }
            }
        }
        
        public static IEnumerable<Coordinate> Possible8Neighbors(Tile tile) =>
            Possible8Neighbors(tile.Coor);

        public static IEnumerable<Coordinate> Possible4Neighbors(Coordinate coor)
        {
            yield return new Coordinate(coor.X + 1, coor.Y);
            yield return new Coordinate(coor.X, coor.Y + 1);
            yield return new Coordinate(coor.X - 1, coor.Y);
            yield return new Coordinate(coor.X, coor.Y - 1);
        }
        
        public static IEnumerable<Coordinate> Possible4Neighbors(Tile tile) =>
            Possible4Neighbors(tile.Coor);

        public static float Distance(Coordinate a, Coordinate b) =>
            Mathf.Sqrt(Mathf.Pow(a.X - b.X, 2) + Mathf.Pow(a.Y - b.Y, 2));

        public static float Distance(Tile tile1, Tile tile2) => Distance(tile1.Coor, tile2.Coor);

        public static float CostOfMove(Tile from, Tile to)
        {
            float distanceFactor = 0f;
            if (Distance(from, to) > 1.1f) distanceFactor = 0.5f;
            return distanceFactor + to.Cost;
        }
    }
}
