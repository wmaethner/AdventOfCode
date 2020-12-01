using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.HelperClasses
{
    public class Grid<T>
    {
        private Dictionary<(int, int), T> _values = new Dictionary<(int, int), T>();
        private T _defaultValue;

        private int _minX;
        private int _minY;
        private int _maxX;
        private int _maxY;

        public Grid() : this(default(T))
        {

        }

        public Grid(T defaultValue)
        {
            _defaultValue = defaultValue;
        }

        public T this[int x, int y]
        {
            get
            {
                return GetValue(x, y);
            }

            set
            {
                _values[(x, y)] = value;
                Update();
            }
        }

        public bool PointSet(int x, int y)
        {
            return _values.ContainsKey((x, y));
        }

        public void DisplayGrid()
        {
            DisplayGridInt(null, false);
        }

        public void DisplayGrid(Dictionary<char, char> replacements)
        {
            DisplayGridInt(replacements, false);
        }

        public void DisplayGridFlipped()
        {
            DisplayGridInt(null, true);
        }

        public void DisplayGridFlipped(Dictionary<char, char> replacements)
        {
            DisplayGridInt(replacements, true);
        }



        //Could theoretically get into an infinite loop if you replace a value with another value you want replaced.
        //Actually strike that, it wouldn't be an infinite loop, but order does matter
        //e.g. Replace 'A' with 'B' then 'B' with 'A'
        private void DisplayGridInt(Dictionary<char,char> replacements, bool flipped)
        {
            int minx = int.MaxValue;
            int miny = int.MaxValue;
            int maxx = int.MinValue;
            int maxy = int.MinValue;
            foreach ((int, int) keys in _values.Keys)
            {
                minx = Math.Min(minx, keys.Item1);
                miny = Math.Min(miny, keys.Item2);
                maxx = Math.Max(maxx, keys.Item1);
                maxy = Math.Max(maxy, keys.Item2);
            }

            string[] display = new string[(maxy - miny) + 1];
            //int startRow = maxy, endRow = miny, changeRow = 1;
            //int startCol = maxy, endCol = miny, changeCol = 1;

            //Build display
            if (flipped)
            {
                int rowCount = 0;
                for (int row = miny; row <= maxy; row++)
                {
                    StringBuilder sb = new StringBuilder();

                    for (int col = minx; col <= maxx; col++)
                    {
                        sb.Append(this[col, row]);
                    }

                    display[rowCount++] = sb.ToString();
                }
            }
            else
            {
                int rowCount = 0;
                for (int row = maxy; row >= miny; row--)
                {
                    StringBuilder sb = new StringBuilder();

                    for (int col = minx; col <= maxx; col++)
                    {
                        sb.Append(this[col, row]);
                    }

                    display[rowCount++] = sb.ToString();
                }
            }
        
            //Make replacements
            if (replacements != null)
            {
                for (int line = 0; line < display.Length; line++)
                {
                    foreach (KeyValuePair<char, char> replace in replacements)
                    {
                        display[line] = display[line].Replace(replace.Key, replace.Value);
                    }
                }
            }
          
            //Finally display the grid
            foreach (string line in display)
            {
                Console.WriteLine(line);
            }
        }

        private T GetValue(int x, int y)
        {
            if (PointSet(x, y))
            {
                return _values[(x, y)];
            }
            else
            {
                return _defaultValue;
            }
        }

        private void Update()
        {
            _minX = int.MaxValue;
            _minY = int.MaxValue;
            _maxX = int.MinValue;
            _maxY = int.MinValue;
            foreach ((int, int) keys in _values.Keys)
            {
                _minX = Math.Min(_minX, keys.Item1);
                _minY = Math.Min(_minY, keys.Item2);
                _maxX = Math.Max(_maxX, keys.Item1);
                _maxY = Math.Max(_maxY, keys.Item2);
            }
        }

        public int MinX()
        {
            return _minX;
        }
        public int MinY()
        {
            return _minY;
        }
        public int MaxX()
        {
            return _maxX;
        }
        public int MaxY()
        {
            return _maxY;
        }
    }
}
