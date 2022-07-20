using Demo.Pathfinding;
using Godot;
using Pathfinding;

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
               OnMouseButtonLeftPressed(eventMouseButton);
            }
        }

        private void OnMouseButtonLeftPressed(InputEventMouseButton eventMouseButton)
        {
            if (_mapManager.Tile(eventMouseButton.GlobalPosition, out Tile tile))
            {
                if (_selectedTile == null)
                {
                    _selectedTile = tile;
                    GD.Print($"Start Tile: {tile}");
                }
                else if (_selectedTile == tile)
                {
                    _selectedTile = null;
                    GD.Print("Start Tile: Empty");
                }
                else
                {
                    if (_mapManager.AStar(out Path<Tile> path, _selectedTile, tile))
                    {
                        _mapManager.DrawPath(path);
                        GD.Print($"Goal Tile: {tile}");
                        GD.Print($"Path cost: {path.Cost}");
                    }
                    else
                    {
                        GD.Print($"Goal Tile: {tile}");
                        GD.Print("No path!");
                    }
                    _selectedTile = null;
                }
            }
        }
    }
}
