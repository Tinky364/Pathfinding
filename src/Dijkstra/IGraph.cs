using System.Collections.Generic;

namespace Dijkstra
{
    public interface IGraph<T> where T : IPoint
    {
        IEnumerable<Edge<T>> GetEdges(PointRecord<T> pointRecord);
    }
}
