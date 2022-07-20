using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace Demo.Pathfinding
{
    public class TileMapExt : TileMap
    {
        public void SetCell(
            Coordinate coor, int tileId, bool flipX = false, bool flipY = false,
            bool transpose = false, Vector2? autotileCoord = null)
        {
            SetCell(coor.X, coor.Y, tileId, flipX, flipY, transpose, autotileCoord);
        }

        public List<Coordinate> GetCellsById(int id)
        {
            Array array = GetUsedCellsById(id);
            List<Coordinate> list = new List<Coordinate>();
            for (int i = 0; i < array.Count; i++)
            {
                if (array[i] is Vector2 vector) list.Add(new Coordinate(vector));
            }
            return list;
        }

        public List<Coordinate> GetCells()
        {
            Array array = GetUsedCells();
            List<Coordinate> list = new List<Coordinate>();
            for (int i = 0; i < array.Count; i++)
            {
                if (array[i] is Vector2 vector) list.Add(new Coordinate(vector));
            }
            return list;
        }

        public List<int> GetTileIds()
        {
            Array array = TileSet.GetTilesIds();
            List<int> ids = new List<int>();
            for (int i = 0; i < array.Count; i++)
            {
                if (array[i] is int id) ids.Add(id);
            }
            return ids;
        }
    }
}
