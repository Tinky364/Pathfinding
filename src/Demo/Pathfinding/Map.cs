using System.Collections.Generic;
using Pathfinding;
using Godot;

namespace Demo.Pathfinding
{
    public class Map : Node, IGraph<Tile>
    {
        private Dictionary<string, Tile> _tiles;
        private Dictionary<string, List<Edge<Tile>>> _edges;

        public void Initialize(Coordinate size)
        {
            InitializeTiles(size);
            InitializeEdges();
        }
        
        public void Initialize(Dictionary<Coordinate, int> mapData)
        {
            InitializeTiles(mapData);
            InitializeEdges();
        }
        
        public Tile GetTile(string name)
        {
            if (!_tiles.TryGetValue(name, out Tile tile)) 
                GD.PushError($"Tile {name} does not exist.");
            return tile;
        }
        
        public Tile GetTile(Coordinate coor) => GetTile(Tile.ToName(coor));
        
        public bool TryGetTile(string name, out Tile tile) => _tiles.TryGetValue(name, out tile);

        public bool TryGetTile(Coordinate coor, out Tile tile) =>
            TryGetTile(Tile.ToName(coor), out tile);

        public Dictionary<string, Tile>.ValueCollection.Enumerator TileIterator() =>
            _tiles.Values.GetEnumerator();

        public List<Edge<Tile>>.Enumerator EdgeIterator(PointRecord<Tile> pointRecord)
        {
            if (!_edges.TryGetValue(pointRecord.Point.Name, out List<Edge<Tile>> list))
                GD.PushError($"Edges of the tile {pointRecord.Point.Name} does not exist.");
            return list?.GetEnumerator() ?? new List<Edge<Tile>>.Enumerator();
        }

        private void InitializeTiles(Coordinate size)
        {
            _tiles = new Dictionary<string, Tile>();
            RandomNumberGenerator rng = new RandomNumberGenerator();
            rng.Randomize();
            for (int x = 0; x < size.X; x++)
            {
                for (int y = 0; y < size.Y; y++)
                {
                    Tile tile = new Tile(new Coordinate(x, y), rng.RandiRange(1, 3));
                    _tiles.Add(tile.Name, tile);
                }
            }
        }
        
        private void InitializeTiles(Dictionary<Coordinate, int> mapData)
        {
            _tiles = new Dictionary<string, Tile>();
            foreach (var pair in mapData)
            {
                Tile tile = new Tile(pair.Key, pair.Value);
                _tiles.Add(tile.Name, tile);
            }
        }

        private void InitializeEdges()
        {
            _edges = new Dictionary<string, List<Edge<Tile>>>();
            foreach (Tile tile in _tiles.Values)
            {
                List<Edge<Tile>> edges = new List<Edge<Tile>>();
                IEnumerable<Coordinate> possibleNeighbors = Tile.PossibleNeighbors(tile);
                foreach (Coordinate coor in possibleNeighbors)
                {
                    if (TryGetTile(coor, out Tile neighbor))
                        edges.Add(new Edge<Tile>(tile, neighbor, Tile.CostOfMove(tile, neighbor)));
                }
                _edges.Add(tile.Name, edges);
            }
        }
    }
}
