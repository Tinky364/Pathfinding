using System.Collections.Generic;
using Godot;
using Pathfinding;

namespace Demo.Pathfinding
{
    public class MapManager : Pathfinder
    {
        [Export(PropertyHint.Enum, "Editor,Script")]
        private string _mapType;
        [Export]
        private bool _diagonalMove;
        [Export(PropertyHint.Range, "0,100,or_greater")]
        private int _xSize = 20;
        [Export(PropertyHint.Range, "0,100,or_greater")]
        private int _ySize = 15;

        private Map _map;
        private TileMapExt _tileMap;
        private Line2D _lineDrawer;
        private readonly Coordinate _size;

        public MapManager()
        {
            _size = new Coordinate(_xSize, _ySize);
        }

        public override void _Ready()
        {
            base._Ready();

            _map = GetNode<Map>("Map");
            _tileMap = GetNode<TileMapExt>("TileMap");
            _lineDrawer = GetNode<Line2D>("LineDrawer");

            CreateMap(_diagonalMove);
        }

        public Coordinate GlobalToCoordinate(Vector2 globalPosition) =>
            new Coordinate(_tileMap.WorldToMap(_tileMap.ToLocal(globalPosition)));

        public Vector2 CoordinateToGlobal(Coordinate coordinate)
        {
            Vector2 pos = _tileMap.ToGlobal(_tileMap.MapToWorld(coordinate.ToVector));
            switch (_tileMap.Mode)
            {
                case TileMap.ModeEnum.Square: 
                    return new Vector2(pos + _tileMap.CellSize / 2f);
                case TileMap.ModeEnum.Isometric:
                    return new Vector2(pos.x, pos.y + _tileMap.CellSize.y / 2f);
                case TileMap.ModeEnum.Custom:
                default: return pos;
            }
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

        private void CreateMap(bool diagonalMove = false)
        {
            switch (_mapType)
            {
                case "Editor":
                    CreateMapFromEditor(diagonalMove);
                    break;
                case "Script":
                    CreateMapFromScript(_size, diagonalMove);
                    break;
            }
        }
        
        private void CreateMapFromEditor(bool diagonalMove = false)
        {
            Dictionary<Coordinate, int> mapData = new Dictionary<Coordinate, int>();
            foreach (int id in _tileMap.GetTileIds())
            {
                List<Coordinate> coordinates = _tileMap.GetCellsById(id);
                foreach (Coordinate coordinate in coordinates) mapData.Add(coordinate, id);
            }
            _map.Initialize(mapData, diagonalMove);
        }
        
        private void CreateMapFromScript(Coordinate size, bool diagonalMove = false)
        {
            _map.Initialize(size, diagonalMove);
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
