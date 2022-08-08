using System.Text;
using Godot;

namespace Pathfinding
{
    public class PathFindingList<T> : Object where T : IPoint
    {
        public PointRecord<T> SmallestPointRecord => _head;
        public bool IsEmpty => _head == null;
        public int Count { get; private set; }

        private PointRecord<T> _head;
        private Pathfinder.Type _curType;

        public PathFindingList() { }
        
        public PathFindingList(PointRecord<T> head)
        {
            Push(head);
        }
        
        public void Push(PointRecord<T> pointRecord)
        {
            Count++;
            if (_head == null)
            {
                _head = pointRecord;
                _curType = _head.CurType;
                return;
            }
            PointRecord<T> iterator = _head;
            switch (_curType)
            {
                case Pathfinder.Type.AStar:
                {
                    if (_head.EstimatedTotalCost > pointRecord.EstimatedTotalCost)
                    {
                        pointRecord.Next = _head;
                        _head = pointRecord;
                        break;
                    }
                    
                    while (iterator.Next != null &&
                           iterator.Next.EstimatedTotalCost < pointRecord.EstimatedTotalCost)
                    {
                        iterator = iterator.Next;
                    }

                    pointRecord.Next = iterator.Next;
                    iterator.Next = pointRecord;
                    break;
                }
                case Pathfinder.Type.Dijkstra:
                {
                    if (_head.CostSoFar > pointRecord.CostSoFar)
                    {
                        pointRecord.Next = _head;
                        _head = pointRecord;
                        break;
                    }
                    
                    while (iterator.Next != null && iterator.Next.CostSoFar < pointRecord.CostSoFar)
                        iterator = iterator.Next;

                    pointRecord.Next = iterator.Next;
                    iterator.Next = pointRecord;
                    break;
                }
            }
        }

        public void RemoveSmallestPointRecord()
        {
            if (IsEmpty) return;
            
            Count--;
            PointRecord<T> temp = _head;
            _head = _head.Next;
            temp.Next = null;
        }

        public bool Contains(PointRecord<T> pointRecord)
        {
            if (IsEmpty) return false;

            PointRecord<T> iterator = _head;
            while (iterator != null)
            {
                if (iterator == pointRecord) return true;
                iterator = iterator.Next;
            }
            return false;
        }

        public bool Contains(T point)
        {
            if (IsEmpty) return false;

            PointRecord<T> iterator = _head;
            while (iterator != null)
            {
                if (iterator.Point.Equals(point)) return true;
                iterator = iterator.Next;
            }
            return false;
        }
        
        public PointRecord<T> Find(T point)
        {
            if (IsEmpty) return null;

            PointRecord<T> iterator = _head;
            while (iterator != null)
            {
                if (iterator.Point.Equals(point)) return iterator;
                iterator = iterator.Next;
            }
            return null;
        }

        public bool Find(T point, out PointRecord<T> pointRecord)
        {
            pointRecord = null;
            if (IsEmpty) return false;

            PointRecord<T> iterator = _head;
            while (iterator != null)
            {
                if (iterator.Point.Equals(point))
                {
                    pointRecord = iterator;
                    return true;
                }
                iterator = iterator.Next;
            }
            return false;
        }

        public void Remove(PointRecord<T> pointRecord)
        {
            if (IsEmpty) return;

            if (_head == pointRecord)
            {
                Count--;
                _head = _head.Next;
                pointRecord.Next = null;
                return;
            }
            
            PointRecord<T> preIterator = _head;
            PointRecord<T> iterator = _head;
            while (iterator != null)
            {
                if (iterator == pointRecord)
                {
                    Count--;
                    preIterator.Next = iterator.Next;
                    iterator.Next = null;
                    return;
                }
                preIterator = iterator;
                iterator = iterator.Next;
            }
        }
        
        public void Remove(T point)
        {
            if (IsEmpty) return;

            if (_head.Point.Equals(point))
            {
                Count--;
                PointRecord<T> temp = _head;
                _head = _head.Next;
                temp.Next = null;
                return;
            }
            
            PointRecord<T> preIterator = _head;
            PointRecord<T> iterator = _head;
            while (iterator != null)
            {
                if (iterator.Point.Equals(point))
                {
                    Count--;
                    preIterator.Next = iterator.Next;
                    iterator.Next = null;
                    return;
                }
                preIterator = iterator;
                iterator = iterator.Next;
            }
        }
        
        public override string ToString()
        {
            if (IsEmpty) return "PathfinderList is empty.";
            
            PointRecord<T> iterator = _head;
            StringBuilder sb = new StringBuilder(
                $"({iterator.Point.Key} = {iterator.EstimatedTotalCost})"
            );
            iterator = iterator.Next;
            while (iterator != null)
            {
                sb.Append($" - ({iterator.Point.Key} = {iterator.EstimatedTotalCost})");
                iterator = iterator.Next;
            }
            return sb.ToString();
        }

        public void Clear()
        {
            FreeAllPointRecords();
            Count = 0;
            _head = null;
        }

        public new void Free()
        {
            FreeAllPointRecords();
            base.Free();
        }

        private void FreeAllPointRecords()
        {
            if (IsEmpty) return;
            
            PointRecord<T> iterator = _head;
            while (iterator != null)
            {
                PointRecord<T> temp = iterator;
                iterator = iterator.Next;
                temp.Free();
            }
        }
    }
}
