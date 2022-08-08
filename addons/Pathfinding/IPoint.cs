namespace Pathfinding
{
    public interface IPoint
    {
        string Key { get; }

        float EstimateTotalCost<T>(T goalPoint) where T : IPoint;
    }
}
