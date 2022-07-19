using System.Collections.Generic;

namespace Pathfinding
{
    public interface IGraph<T> where T : IPoint
    {
        List<Edge<T>>.Enumerator GetEdges(PointRecord<T> pointRecord);
    }
}
