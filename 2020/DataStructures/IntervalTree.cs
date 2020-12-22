using System;
namespace DataStructures
{
    //TODO: Can consolidate the nodes after adding a range by checking if child
    //      nodes are within parent nodes range (impossible to skip a level)

    public class IntervalTree
    {
        private IntervalTreeNode _root;

        public IntervalTree()
        {         
        }

        public void AddRange(int min, int max)
        {
            if (_root == null)
            {
                _root = new IntervalTreeNode(min, max);
                return;
            }

            _root.AddRange(min, max);
        }

        public bool WithinRange(int val)
        {
            return _root.PointInRangeRecurse(val);
        }
    }

    class IntervalTreeNode
    {
        private int _min;
        private int _max;

        public IntervalTreeNode LeftNode { get; set; }
        public IntervalTreeNode RightNode { get; set; }

        public IntervalTreeNode(int min, int max)
        {
            _min = min;
            _max = max;
        }

        private bool RangeWithinRange(int min, int max)
        {
            // Min is within range
            if (PointInRange(min))
            {
                return true;
            }

            // Max is within range
            if (PointInRange(max))
            {
                return true;
            }

            return min < _min && max > _max;
        }

        private bool PointInRange(int val)
        {
            return _min <= val && val <= _max;
        }

        public bool PointInRangeRecurse(int val)
        {
            if (val < _min)
            {
                return LeftNode == null ? false : LeftNode.PointInRangeRecurse(val);
            }

            if (val > _max)
            {
                return RightNode == null ? false : RightNode.PointInRangeRecurse(val);
            }

            return true;
        }

        

        public void AddRange(int min, int max)
        {
            if (RangeWithinRange(min, max))
            {
                _min = Math.Min(_min, min);
                _max = Math.Max(_max, max);
                return;
            }

            if (max < _min)
            {
                AddLeft(min, max);
            }
            else
            {
                AddRight(min, max);
            }
        }

        private void AddLeft(int min, int max)
        {
            if (LeftNode == null)
            {
                LeftNode = new IntervalTreeNode(min, max);
                return;
            }

            LeftNode.AddRange(min, max);
        }

        private void AddRight(int min, int max)
        {
            if (RightNode == null)
            {
                RightNode = new IntervalTreeNode(min, max);
                return;
            }

            RightNode.AddRange(min, max);
        }
    }
}
