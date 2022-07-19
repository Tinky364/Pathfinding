using System.Collections.Generic;
using System.Text;
using Godot;

namespace Pathfinding
{
    public class Pathfinder : Node
    {
        public enum Type { AStar, Dijkstra }

        public List<Edge<T>> AStar<T>(IGraph<T> graph, T startPoint, T goalPoint) where T : IPoint
        {
            PointRecord<T> startPointRecord = new PointRecord<T>(
                startPoint, startPoint.EstimateTotalCost(goalPoint)
            );

            PathFindingList<T> openList = new PathFindingList<T>(startPointRecord);
            PathFindingList<T> closedList = new PathFindingList<T>();

            PointRecord<T> currentPointRecord = startPointRecord;
            while (openList.Count > 0)
            {
                currentPointRecord = openList.SmallestPointRecord();

                if (currentPointRecord.Point.Equals(goalPoint)) break;

                List<Edge<T>>.Enumerator edges = graph.GetEdges(currentPointRecord);
                float endPointEstimatedTotalCost = 0;
                while (edges.MoveNext())
                {
                    Edge<T> edge = edges.Current;
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

            List<Edge<T>> path = new List<Edge<T>>();
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

        public List<Edge<T>> Dijkstra<T>(IGraph<T> graph, T startPoint, T goalPoint) where T : IPoint
        {
            PointRecord<T> startPointRecord = new PointRecord<T>(startPoint);

            PathFindingList<T> openList = new PathFindingList<T>(startPointRecord);
            PathFindingList<T> closedList = new PathFindingList<T>();

            PointRecord<T> currentPointRecord = startPointRecord;
            while (openList.Count > 0)
            {
                currentPointRecord = openList.SmallestPointRecord();

                if (currentPointRecord.Point.Equals(goalPoint)) break;

                List<Edge<T>>.Enumerator edges = graph.GetEdges(currentPointRecord);

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
            
            List<Edge<T>> path = new List<Edge<T>>();
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

        public void PrintPath<T>(List<Edge<T>> path) where T : IPoint
        {
            StringBuilder sb = new StringBuilder(
                $"Path: Start = {path[0].From.Name}, Goal = {path[path.Count - 1].To.Name} \n" +
                $"{path[0].From.Name} -> {path[0].To.Name}"
            );
            for (int i = 1; i < path.Count; i++) sb.Append($" -> {path[i].To.Name}");
            sb.Append("\n------------------------");
            GD.Print(sb.ToString());
        }
    }
}
