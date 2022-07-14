using System.Collections.Generic;
using Godot;

namespace Dijkstra
{
    public class PathFindingList : Reference
    {
        private readonly List<PointRecord> _list;

        public int Count => _list.Count;

        public PathFindingList()
        {
            _list = new List<PointRecord>();
        }
        
        public void Add(PointRecord pointRecord)
        {
            _list.Add(pointRecord);
            _list.Sort();
        }

        public void Remove(PointRecord pointRecord)
        {
            _list.Remove(pointRecord);
            _list.Sort();
        }

        public PointRecord SmallestPointRecord() => _list[0];

        public IEnumerable<PointRecord> AllRecords() => _list;

        public bool Contains(PointRecord pointRecord) => _list.Contains(pointRecord);

        public bool Contains(object point)
        {
            foreach (PointRecord pointRecord in _list)
            {
                if (pointRecord.Point == point) return true;
            }
            return false;
        }

        public PointRecord Find(object point)
        {
            foreach (PointRecord pointRecord in _list)
            {
                if (pointRecord.Point == point) return pointRecord;
            }
            return null;
        }
    }
}
