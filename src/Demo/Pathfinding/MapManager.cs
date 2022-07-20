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
        private TileMapExt _baseTm;
        private TileMapExt _tm;
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
            _baseTm = GetNode<TileMapExt>("BaseTm");
            _tm = GetNode<TileMapExt>("BaseTm/Tm");
            _lineDrawer = GetNode<Line2D>("BaseTm/LineDrawer");

            CreateMap(_diagonalMove);
        }

        public Coordinate GlobalToCoordinate(Vector2 globalPosition) =>
            new Coordinate(_baseTm.WorldToMap(_baseTm.ToLocal(globalPosition)));

        public Vector2 CoordinateToGlobal(Coordinate coordinate)
        {
            Vector2 pos = _baseTm.ToGlobal(_baseTm.MapToWorld(coordinate.ToVector));
            switch (_baseTm.Mode)
            {
                case TileMap.ModeEnum.Square: 
                    return new Vector2(pos + _baseTm.CellSize / 2f);
                case TileMap.ModeEnum.Isometric:
                    return new Vector2(pos.x, pos.y + _baseTm.CellSize.y / 2f);
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

        public List<Tile> AvailableTiles(Tile startTile, float maxCost)
        {
            return AvailableTs(_map, startTile, maxCost);
        }

        public void DrawPath(Path<Tile> path)
        {
            _lineDrawer.ClearPoints();
            _lineDrawer.AddPoint(CoordinateToGlobal(path[0].From.Coor));
            for (int i = 0; i < path.Count; i++)
                _lineDrawer.AddPoint(CoordinateToGlobal(path[i].To.Coor));
        }

        public void ClearPath()
        {
            _lineDrawer.ClearPoints();
        }

        public void DrawAvailableTiles(List<Tile> tiles)
        {
            _tm.Clear();
            foreach (Tile tile in tiles) _tm.SetCell(tile.Coor, 0);
        }

        public void ClearAvailableTiles()
        {
            _tm.Clear();
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
            foreach (int id in _baseTm.GetTileIds())
            {
                List<Coordinate> coordinates = _baseTm.GetCellsById(id);
                foreach (Coordinate coordinate in coordinates) mapData.Add(coordinate, id);
            }
            _map.Initialize(mapData, diagonalMove);
        }
        
        private void CreateMapFromScript(Coordinate size, bool diagonalMove = false)
        {
            _map.Initialize(size, diagonalMove);
            _baseTm.Clear();
            var tilesIterator = _map.TileIterator();
            while (tilesIterator.MoveNext())
            {
                Tile tile = tilesIterator.Current;
                if (tile == null) continue;
                _baseTm.SetCell(tile.Coor, tile.Cost);
            }
        }
    }
}
