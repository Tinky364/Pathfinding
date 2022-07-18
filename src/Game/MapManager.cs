using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Pathfinding;
using Godot;
using Pathfinding;

namespace Game
{
    public class MapManager : Pathfinder
    {
        private Map _map;
        private TileMap _tileMap;
        private Timer _timer;
        
        public override void _Ready()
        {
            base._Ready();

            _map = GetNode<Map>("Map");
            _tileMap = GetNode<TileMap>("TileMap");
            _timer = new Timer();
            AddChild(_timer);
            _timer.WaitTime = 2f;

            Path();
        }

        private async Task Path()
        {
            List<Edge<Tile>> path1 = AStar(_map, _map.GetTile(0, 0), _map.GetTile(8, 4));
            PrintPath(path1);

            _timer.Start();
            await ToSignal(_timer, "timeout");

            Path();
        }
        
    }
}
