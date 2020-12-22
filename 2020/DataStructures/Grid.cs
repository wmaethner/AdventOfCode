using System;
using System.Collections.Generic;
using System.Drawing;

namespace DataStructures
{
    public class Grid
    {
        Dictionary<Point, bool> _grid;

        public Grid() : this(0)
        {
        }

        public Grid(int size)
        {
            _grid = new Dictionary<Point, bool>();

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    _grid[new Point(col, row)] = false;
                }
            }
        }

        public void SetNode(int row, int col, bool value)
        {
            _grid[Point(row, col)] = value;
        }

        public bool GetNodeValue(int row, int col)
        {
            return _grid.ContainsKey(Point(row, col)) ? _grid[Point(row, col)] : false;
        }

        public int ActiveNodesInRange(int topRow, int bottomRow, int leftCol, int rightCol)
        {
            int count = 0;

            for (int row = topRow; row <= bottomRow; row++)
            {
                for (int col = leftCol; col <= rightCol; col++)
                {
                    count += GetNodeValue(row, col) ? 1 : 0;
                }
            }

            return count;
        }

        private Point Point(int row, int col)
        {
            return new Point(col, row);
        }
    }
}
