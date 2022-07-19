namespace Pathfinding
{
    public interface IPoint
    {
        string Name { get; }

        float EstimateTotalCost<T>(T goalPoint) where T : IPoint;
    }
}
