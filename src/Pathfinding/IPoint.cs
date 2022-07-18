namespace Pathfinding
{
    public interface IPoint
    {
        string Name { get; }

        float EstimateHeuristic<T>(T goalPoint) where T : IPoint;
    }
}
