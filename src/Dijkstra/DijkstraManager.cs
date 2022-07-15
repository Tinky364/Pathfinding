using System.Collections.Generic;
using Godot;
using TileMap;

namespace Dijkstra
{
    public class DijkstraManager : Node
    {
        public override void _Ready()
        {
            base._Ready();

            Map map = new Map(10, 10);
            List<Edge<Tile>> path = FindPath(map, map.GetTile(0,0), map.GetTile(3,6));
            
            foreach (Edge<Tile> edge in path) edge.Print();
        }

        public List<Edge<T>> FindPath<T>(IGraph<T> graph, T startPoint, T goalPoint) where T : IPoint
        {
            PointRecord<T> startPointRecord = new PointRecord<T>(startPoint);

            PathFindingList<T> openList = new PathFindingList<T>();
            openList.Add(startPointRecord);
            PathFindingList<T> closedList = new PathFindingList<T>();

            PointRecord<T> currentPointRecord = openList.SmallestPointRecord();
            while (openList.Count > 0)
            {
                currentPointRecord = openList.SmallestPointRecord();

                if (currentPointRecord.Point.Equals(goalPoint)) break;

                IEnumerable<Edge<T>> edges = graph.GetEdges(currentPointRecord);

                foreach (Edge<T> edge in edges)
                {
                    T endPoint = edge.To;
                    int endPointCost = currentPointRecord.CostSoFar + edge.Cost;

                    PointRecord<T> endPointRecord;
                    if (closedList.Contains(endPoint)) // closed point
                    {
                        continue;
                    }
                    else if (openList.Contains(endPoint)) // open point
                    {
                        endPointRecord = openList.Find(endPoint);
                        
                        if (endPointRecord.CostSoFar <= endPointCost) continue;
                    }
                    else // unvisited point
                    {
                        endPointRecord = new PointRecord<T>(endPoint); 
                    }

                    endPointRecord.CostSoFar = endPointCost;
                    endPointRecord.Edge = edge;
                    
                    if (!openList.Contains(endPoint)) openList.Add(endPointRecord);
                }
                
                openList.Remove(currentPointRecord);
                closedList.Add(currentPointRecord);
            }
            
            if (!currentPointRecord.Point.Equals(goalPoint)) return null;
            
            List<Edge<T>> path = new List<Edge<T>>();
            while (!currentPointRecord.Point.Equals(startPoint))
            {
                path.Add(currentPointRecord.Edge);
                currentPointRecord = closedList.Find(currentPointRecord.Edge.From);
            }
            path.Reverse();
            return path;
        }
    }
}
