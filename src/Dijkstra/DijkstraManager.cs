using System.Collections.Generic;
using Godot;

namespace Dijkstra
{
    public class DijkstraManager : Node
    {
        public override void _Ready()
        {
            base._Ready();
            
            Dots a = new Dots("A");
            Dots b = new Dots("B");
            Dots c = new Dots("C");
            Dots d = new Dots("D");
            Dots e = new Dots("E");
            Dots f = new Dots("F");
            Graph graph = new Graph(
                new Edge(a, b, 2), new Edge(b, c, 1), new Edge(c, d, 1), new Edge(d, e, 1),
                new Edge(e, f, 1), new Edge(a, c, 1), new Edge(c, f, 1)
            );

            List<Edge> path = FindPath(graph, a, f);
            foreach (Edge edge in path) edge.Print();
        }

        public List<Edge> FindPath(Graph graph, object startPoint, object goalPoint)
        {
            PointRecord startPointRecord = new PointRecord(startPoint);

            PathFindingList openList = new PathFindingList();
            openList.Add(startPointRecord);
            PathFindingList closedList = new PathFindingList();

            PointRecord currentPointRecord = null;
            while (openList.Count > 0)
            {
                currentPointRecord = openList.SmallestPointRecord();

                if (currentPointRecord.Point == goalPoint) break;

                IEnumerable<Edge> edges = graph.GetEdges(currentPointRecord);

                foreach (Edge edge in edges)
                {
                    object endPoint = edge.To;
                    int endPointCost = currentPointRecord.CostSoFar + edge.Cost;

                    PointRecord endPointRecord;
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
                        endPointRecord = new PointRecord(endPoint); 
                    }

                    endPointRecord.CostSoFar = endPointCost;
                    endPointRecord.Edge = edge;
                    
                    if (!openList.Contains(endPoint)) openList.Add(endPointRecord);
                }
                
                openList.Remove(currentPointRecord);
                closedList.Add(currentPointRecord);
            }
            
            if (currentPointRecord?.Point != goalPoint) return null;
            
            List<Edge> path = new List<Edge>();
            while (currentPointRecord?.Point != startPoint)
            {
                path.Add(currentPointRecord?.Edge);
                currentPointRecord = closedList.Find(currentPointRecord?.Edge.From);
            }
            path.Reverse();
            return path;
        }
    }
}
