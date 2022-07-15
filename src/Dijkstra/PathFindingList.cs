using System.Collections.Generic;
using Advanced.Algorithms;
using Advanced.Algorithms.DataStructures;
using Godot;

namespace Dijkstra
{
    public class PathFindingList<T> : Reference where T : IPoint
    {
        private readonly FibonacciHeap<PointRecord<T>> _heap;

        public int Count => _heap.Count;

        public IEnumerable<PointRecord<T>> All => _heap;

        public PathFindingList()
        {
            _heap = new FibonacciHeap<PointRecord<T>>(SortDirection.Ascending);
        }
        
        public void Add(PointRecord<T> pointRecord)
        {
            _heap.Insert(pointRecord);
        }

        public void RemoveSmallestPointRecord()
        {
            _heap.Extract();
        }

        public PointRecord<T> SmallestPointRecord() => _heap.Peek();

        public bool Contains(PointRecord<T> pointRecord)
        {
            foreach (PointRecord<T> record in _heap)
            {
                if (ReferenceEquals(pointRecord, record)) return true;
            }
            return false;
        }

        public bool Contains(T point)
        {
            foreach (PointRecord<T> pointRecord in _heap)
            {
                if (pointRecord.Point.Equals(point)) return true;
            }
            return false;
        }

        public PointRecord<T> Find(T point)
        {
            foreach (PointRecord<T> pointRecord in _heap)
            {
                if (pointRecord.Point.Equals(point)) return pointRecord;
            }
            return null;
        }
    }
}
