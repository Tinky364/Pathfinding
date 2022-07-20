using System.Collections.Generic;
using Godot;
using Pathfinding;

namespace Demo.Pathfinding
{
    public class MapManager : Pathfinder
    {
        [Export(PropertyHint.Enum, "Editor,Script")]
        private string _mapType;
        [Export(PropertyHint.Range, "0,100,or_greater")]
        private int _xSize = 20;
        [Export(PropertyHint.Range, "0,100,or_greater")]
        private int _ySize = 15;
        
        public Coordinate Size { get; private set; }

        private Map _map;
        private TileMapExt _tileMap;
        private Line2D _lineDrawer;

        public MapManager()
        {
            Size = new Coordinate(_xSize, _ySize);
        }

        public override void _Ready()
        {
            base._Ready();

            _map = GetNode<Map>("Map");
            _tileMap = GetNode<TileMapExt>("TileMap");
            _lineDrawer = GetNode<Line2D>("LineDrawer");

            switch (_mapType)
            {
               case "Editor":
                   CreateMapFromEditor();
                   break;
               case "Script":
                   CreateMapFromScript(Size);
                   break;
            }
        }

        public Coordinate GlobalToCoordinate(Vector2 globalPosition)
        {
            return new Coordinate(_tileMap.WorldToMap(_tileMap.ToLocal(globalPosition)));
        }

        public Vector2 CoordinateToGlobal(Coordinate coordinate)
        {
            return new Vector2(
                _tileMap.ToGlobal(_tileMap.MapToWorld(coordinate.ToVector)) + _tileMap.CellSize / 2f
            );
        }

        public bool Tile(Vector2 globalMousePosition, out Tile tile) =>
            _map.TryGetTile(GlobalToCoordinate(globalMousePosition), out tile);

        public Path<Tile> AStar(Tile startTile, Tile goalTile) =>
            AStar(_map, startTile, goalTile);

        public bool AStar(out Path<Tile> path, Tile startTile, Tile goalTile) =>
            AStar(out path, _map, startTile, goalTile);

        public void DrawPath(Path<Tile> path)
        {
            _lineDrawer.ClearPoints();
            _lineDrawer.AddPoint(CoordinateToGlobal(path[0].From.Coor));
            for (int i = 0; i < path.Count; i++)
                _lineDrawer.AddPoint(CoordinateToGlobal(path[i].To.Coor));
        }
        
        private void CreateMapFromEditor()
        {
            Dictionary<Coordinate, int> mapData = new Dictionary<Coordinate, int>();
            foreach (int id in _tileMap.GetTileIds())
            {
                List<Coordinate> coordinates = _tileMap.GetCellsById(id);
                foreach (Coordinate coordinate in coordinates)
                {
                    mapData.Add(coordinate, id);
                }
            }
            _map.Initialize(mapData);
        }
        
        private void CreateMapFromScript(Coordinate size)
        {
            _map.Initialize(size);
            _tileMap.Clear();
            var tilesIterator = _map.TileIterator();
            while (tilesIterator.MoveNext())
            {
                Tile tile = tilesIterator.Current;
                if (tile == null) continue;
                _tileMap.SetCell(tile.Coor, tile.Cost);
            }
        }
    }
}
