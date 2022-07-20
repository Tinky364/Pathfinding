using System.Collections.Generic;
using Godot;

namespace Pathfinding
{
    public class Pathfinder : Node
    {
        public enum Type { AStar, Dijkstra }

        public List<T> AvailableTs<T>(IGraph<T> graph, T startPoint, float maxCost)
            where T : IPoint
        {
            List<T> ts = new List<T>();
            PointRecord<T> startPointRecord = new PointRecord<T>(startPoint, 0);

            PathFindingList<T> openList = new PathFindingList<T>(startPointRecord);
            PathFindingList<T> closedList = new PathFindingList<T>();

            while (openList.Count > 0)
            {
                PointRecord<T> currentPointRecord = openList.SmallestPointRecord;

                float endPointEstimatedTotalCost = 0;
                List<Edge<T>>.Enumerator edgesIterator = graph.EdgeIterator(currentPointRecord);
                while (edgesIterator.MoveNext())
                {
                    Edge<T> edge = edgesIterator.Current;
                    if (edge == null) continue;
                    T endPoint = edge.To;
                    float endPointCostSoFar = currentPointRecord.CostSoFar + edge.Cost;

                    if (closedList.Find(endPoint, out PointRecord<T> endPointRecord))
                    {
                        if (endPointRecord.CostSoFar <= endPointCostSoFar) continue;
                        
                        closedList.Remove(endPointRecord);

                        endPointEstimatedTotalCost =
                            endPointRecord.EstimatedTotalCost - endPointRecord.CostSoFar;
                    }
                    else if (openList.Find(endPoint, out endPointRecord))
                    {
                        if (endPointRecord.CostSoFar <= endPointCostSoFar) continue;

                        endPointEstimatedTotalCost =
                            endPointRecord.EstimatedTotalCost - endPointRecord.CostSoFar;
                    }
                    else
                    {
                        endPointRecord = new PointRecord<T>(
                            endPoint, startPoint.EstimateTotalCost(endPoint)
                        );
                    }

                    endPointRecord.CostSoFar = endPointCostSoFar;
                    endPointRecord.Edge = edge;
                    endPointRecord.EstimatedTotalCost = endPointCostSoFar + endPointEstimatedTotalCost;

                    if (endPointRecord.EstimatedTotalCost <= maxCost && !openList.Contains(endPoint))
                        openList.Push(endPointRecord);
                }

                if (currentPointRecord != startPointRecord && 
                    currentPointRecord.EstimatedTotalCost <= maxCost)
                {
                    ts.Add(currentPointRecord.Point);
                }
                
                openList.Remove(currentPointRecord);
                closedList.Push(currentPointRecord);
            }

            openList.Free();
            closedList.Free();
            
            return ts;
        }
        
        public bool AStar<T>(out Path<T> path, IGraph<T> graph, T startPoint, T goalPoint)
            where T : IPoint
        {
            path = AStar(graph, startPoint, goalPoint);
            return path != null;
        }

        public Path<T> AStar<T>(IGraph<T> graph, T startPoint, T goalPoint) where T : IPoint
        {
            PointRecord<T> startPointRecord = new PointRecord<T>(
                startPoint, startPoint.EstimateTotalCost(goalPoint)
            );

            PathFindingList<T> openList = new PathFindingList<T>(startPointRecord);
            PathFindingList<T> closedList = new PathFindingList<T>();

            PointRecord<T> currentPointRecord = startPointRecord;
            while (openList.Count > 0)
            {
                currentPointRecord = openList.SmallestPointRecord;

                if (currentPointRecord.Point.Equals(goalPoint)) break;

                float endPointEstimatedTotalCost = 0;
                List<Edge<T>>.Enumerator edgesIterator = graph.EdgeIterator(currentPointRecord);
                while (edgesIterator.MoveNext())
                {
                    Edge<T> edge = edgesIterator.Current;
                    if (edge == null) continue;
                    T endPoint = edge.To;
                    float endPointCostSoFar = currentPointRecord.CostSoFar + edge.Cost;

                    if (closedList.Find(endPoint, out PointRecord<T> endPointRecord))
                    {
                        if (endPointRecord.CostSoFar <= endPointCostSoFar) continue;
                        
                        closedList.Remove(endPointRecord);

                        endPointEstimatedTotalCost =
                            endPointRecord.EstimatedTotalCost - endPointRecord.CostSoFar;
                    }
                    else if (openList.Find(endPoint, out endPointRecord))
                    {
                        if (endPointRecord.CostSoFar <= endPointCostSoFar) continue;

                        endPointEstimatedTotalCost =
                            endPointRecord.EstimatedTotalCost - endPointRecord.CostSoFar;
                    }
                    else
                    {
                        endPointRecord = new PointRecord<T>(
                            endPoint, endPoint.EstimateTotalCost(goalPoint)
                        );
                    }

                    endPointRecord.CostSoFar = endPointCostSoFar;
                    endPointRecord.Edge = edge;
                    endPointRecord.EstimatedTotalCost = endPointCostSoFar + endPointEstimatedTotalCost;
                    
                    if (!openList.Contains(endPoint)) openList.Push(endPointRecord);
                }
                
                openList.Remove(currentPointRecord);
                closedList.Push(currentPointRecord);
            }

            if (!currentPointRecord.Point.Equals(goalPoint)) return null;

            Path<T> path = new Path<T>();
            while (!currentPointRecord.Point.Equals(startPoint))
            {
                path.Add(currentPointRecord.Edge);
                currentPointRecord = closedList.Find(currentPointRecord.Edge.From);
            }
            path.Reverse();

            openList.Free();
            closedList.Free();
            
            return path;
        }
        
        public bool Dijkstra<T>(out Path<T> path, IGraph<T> graph, T startPoint, T goalPoint)
            where T : IPoint
        {
            path = Dijkstra(graph, startPoint, goalPoint);
            return path != null;
        }

        public Path<T> Dijkstra<T>(IGraph<T> graph, T startPoint, T goalPoint) where T : IPoint
        {
            PointRecord<T> startPointRecord = new PointRecord<T>(startPoint);

            PathFindingList<T> openList = new PathFindingList<T>(startPointRecord);
            PathFindingList<T> closedList = new PathFindingList<T>();

            PointRecord<T> currentPointRecord = startPointRecord;
            while (openList.Count > 0)
            {
                currentPointRecord = openList.SmallestPointRecord;

                if (currentPointRecord.Point.Equals(goalPoint)) break;

                List<Edge<T>>.Enumerator edges = graph.EdgeIterator(currentPointRecord);

                while (edges.MoveNext())
                {
                    Edge<T> edge = edges.Current;
                    if (edge == null) continue;
                    T endPoint = edge.To;
                    float endPointCostSoFar = currentPointRecord.CostSoFar + edge.Cost;

                    PointRecord<T> endPointRecord;
                    if (closedList.Contains(endPoint)) // closed point
                    {
                        continue;
                    }
                    else if (openList.Find(endPoint, out endPointRecord)) // open point
                    {
                        if (endPointRecord.CostSoFar <= endPointCostSoFar) continue;
                    }
                    else // unvisited point
                    {
                        endPointRecord = new PointRecord<T>(endPoint); 
                    }

                    endPointRecord.CostSoFar = endPointCostSoFar;
                    endPointRecord.Edge = edge;
                    
                    if (!openList.Contains(endPoint)) openList.Push(endPointRecord);
                }
                
                openList.Remove(currentPointRecord);
                closedList.Push(currentPointRecord);
            }
            
            if (!currentPointRecord.Point.Equals(goalPoint)) return null;

            Path<T> path = new Path<T>();
            while (!currentPointRecord.Point.Equals(startPoint))
            {
                path.Add(currentPointRecord.Edge);
                currentPointRecord = closedList.Find(currentPointRecord.Edge.From);
            }
            path.Reverse();

            openList.Free();
            closedList.Free();
            
            return path;
        }
    }
}
