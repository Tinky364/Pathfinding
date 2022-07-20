using System;
using Godot;

namespace Demo.Pathfinding
{
    public struct Coordinate : IEquatable<Coordinate>
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Vector2 ToVector => new Vector2(X, Y);

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Coordinate(Vector2 vector)
        {
            X = Mathf.FloorToInt(vector.x);
            Y = Mathf.FloorToInt(vector.y);
        }

        public static Coordinate operator +(Coordinate a, Coordinate b) =>
            new Coordinate(a.X + b.X, a.Y + b.Y);

        public static Coordinate operator -(Coordinate a, Coordinate b) =>
            new Coordinate(a.X - b.X, a.Y - b.Y);

        public static Coordinate operator /(Coordinate a, int b) => new Coordinate(a.X / b, a.Y / b);

        public static Coordinate operator *(Coordinate a, int b) => new Coordinate(a.X * b, a.Y * b);

        public static bool operator ==(Coordinate a, Coordinate b) => a.X == b.X && a.Y == b.Y;

        public static bool operator !=(Coordinate a, Coordinate b) => !(a == b);
        
        public bool Equals(Coordinate other) => X == other.X && Y == other.Y;

        public override bool Equals(object obj) => obj is Coordinate other && Equals(other);

        public override int GetHashCode()
        {
            unchecked { return (X * 397) ^ Y; }
        }

        public override string ToString() => $"Coor({X}, {Y})";
    }
}
