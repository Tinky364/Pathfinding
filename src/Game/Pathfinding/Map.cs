using System.Collections.Generic;
using Pathfinding;
using Godot;

namespace Game.Pathfinding
{
    public class Map : Node, IGraph<Tile>
    {
        [Export(PropertyHint.Range, "0,100,or_greater")]
        public int XSize { get; private set; } = 10;
        [Export(PropertyHint.Range, "0,100,or_greater")]
        public int YSize { get; private set; } = 10;

        private readonly Dictionary<string, Tile> _map;
        private readonly Dictionary<string, List<Edge<Tile>>> _edges;

        public Map()
        {
            _map = new Dictionary<string, Tile>();
            _edges = new Dictionary<string, List<Edge<Tile>>>();
        }
        
        public override void _EnterTree()
        {
            base._EnterTree();
            InitializeMap(XSize, YSize);
            InitializeEdges(XSize, YSize);
        }

        public Tile GetTile(string name)
        {
            if (!_map.TryGetValue(name, out Tile tile)) 
                GD.PushError($"Tile {name} does not exist.");
            return tile;
        }
        
        public Tile GetTile(int x, int y) => GetTile(Tile.ToName(x, y));

        public List<Edge<Tile>> GetEdges(PointRecord<Tile> pointRecord)
        {
            if (!_edges.TryGetValue(pointRecord.Point.Name, out List<Edge<Tile>> list))
                GD.PushError($"Edges of the tile {pointRecord.Point.Name} does not exist.");
            return list;
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
            foreach (Tile tile in _map.Values)
            {
                IEnumerable<string> neighborsNames = Tile.NeighborsNames(tile, xSize, ySize);

                List<Edge<Tile>> edges = new List<Edge<Tile>>();
                foreach (string name in neighborsNames)
                {
                    Tile neighbor = GetTile(name);
                    edges.Add(
                        new Edge<Tile>(
                            tile, neighbor,
                            Tile.CostOfMove(tile, neighbor)
                        )
                    );
                }
                _edges.Add(tile.Name, edges);
            }
        }
    }
}
