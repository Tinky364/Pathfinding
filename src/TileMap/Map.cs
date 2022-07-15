using System.Collections.Generic;
using Dijkstra;
using Godot;

namespace TileMap
{
    public class Map : Reference, IGraph<Tile>
    {
        private readonly Dictionary<string, Tile> _map;
        private readonly Dictionary<string, List<Edge<Tile>>> _edges;
        public int XSize { get; }
        public int YSize { get; }

        public Map() { }

        public Map(int xSize, int ySize)
        {
            XSize = xSize;
            YSize = ySize;
            _map = new Dictionary<string, Tile>();
            InitializeMap(XSize, YSize);
            _edges = new Dictionary<string, List<Edge<Tile>>>();
            InitializeEdges(XSize, YSize);
        }

        public Tile GetTile(int x, int y)
        {
            if (x >= XSize || y >= YSize || x < 0 || y < 0)
            {
                GD.PushError($"Trying to reach tile({x},{y}) does not exist.");
            }
            return _map[Tile.ToName(x, y)];
        }

        public Tile GetTile(string name)
        {
            if (!_map.TryGetValue(name, out Tile tile)) 
                GD.PushError("Trying to reach tile does not exist.");
            return tile;
        }
        
        public IEnumerable<Edge<Tile>> GetEdges(PointRecord<Tile> pointRecord)
        {
            foreach (var pair in _edges)
            {
                if (pair.Key == pointRecord.Point.Name) return pair.Value;
            }
            GD.PushError("Trying to reach tile's edges does not exist.");
            return null;
        }
        
        private void InitializeMap(int xSize, int ySize)
        {
            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    Tile tile = new Tile(x, y);
                    _map.Add(tile.Name, tile);
                }
            }
        }

        private void InitializeEdges(int xSize, int ySize)
        {
            foreach (var pair in _map)
            {
                List<string> neighborsNames = Tile.NeighborsNames(pair.Value, xSize, ySize);

                List<Edge<Tile>> edges = new List<Edge<Tile>>();
                foreach (string name in neighborsNames)
                {
                    Tile tile = GetTile(name);
                    edges.Add(new Edge<Tile>(pair.Value, tile, tile.Cost));
                }
                _edges.Add(pair.Key, edges);
            }
        }
    }
}
