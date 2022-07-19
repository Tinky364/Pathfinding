using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Pathfinding;
using Godot;
using Pathfinding;

namespace Demo
{
    public class MapManager : Pathfinder
    {
        private Map _map;
        private TileMap _tileMap;
        private Timer _timer;
        private RandomNumberGenerator _rng;
        
        public override void _Ready()
        {
            base._Ready();

            _map = GetNode<Map>("Map");
            _tileMap = GetNode<TileMap>("TileMap");
            _timer = new Timer();
            AddChild(_timer);
            _timer.WaitTime = 10f;
            _rng = new RandomNumberGenerator();
            _rng.Randomize();

            Path();
        }

        private async Task Path()
        {
            _timer.Start();
            await ToSignal(_timer, "timeout");

            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            List<Edge<Tile>> path1 = AStar(
                _map, _map.GetTile(_rng.RandiRange(0, 19), _rng.RandiRange(0, 19)),
                _map.GetTile(_rng.RandiRange(0, 19), _rng.RandiRange(0, 19))
            );
            
            stopWatch.Stop();
            GD.Print($"Total Execution Time: {stopWatch.ElapsedMilliseconds / 1000f}");
            PrintPath(path1);

            Path();
        }
        
    }
}
