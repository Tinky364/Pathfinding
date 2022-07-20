using Demo.Pathfinding;
using Godot;

namespace Demo
{
    public class Player : Node
    {
        private MapManager _mapManager;
        private Tile _selectedTile;
        
        public override void _Ready()
        {
            base._Ready();

            _mapManager = GetNode<MapManager>("../MapManager");
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            base._UnhandledInput(@event);

            if (@event is InputEventMouseButton eventMouseButton &&
                eventMouseButton.Pressed &&
                (ButtonList)eventMouseButton.ButtonIndex == ButtonList.Left)
            {
                if (_mapManager.Tile(eventMouseButton.GlobalPosition, out Tile tile))
                {
                    if (_selectedTile == null)
                    {
                        _selectedTile = tile;
                        GD.Print($"Selected Tile: {tile}");
                    }
                    else if (_selectedTile == tile)
                    {
                        _selectedTile = null;
                        GD.Print("Selected Tile: null");
                    }
                    else
                    {
                        if (_mapManager.AStar(out var path, _selectedTile, tile))
                        {
                            _mapManager.DrawPath(path);
                            GD.Print(path.Cost);
                        }
                        else
                        {
                            GD.Print("No path");
                        }
                        _selectedTile = null;
                    }
                }
            }
        }
    }
}
