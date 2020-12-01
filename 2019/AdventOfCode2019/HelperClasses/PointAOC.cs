using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AdventOfCode2019.HelperClasses
{
    public class PointAOC : IComparable<PointAOC>
    {
        private int X;
        private int Y;

        public PointAOC(int x, int y)
        {
            X = x;
            Y = y;
        }

        public double slopeTo(PointAOC other)
        {
            if (CompareTo(other) == 0)
            {
                return double.NegativeInfinity;
            }

            if (Y == other.Y)
            {
                return 0;
            }

            if (X == other.X)
            {
                return double.PositiveInfinity;
            }

            return ((double)other.Y - Y) / ((double)other.X - X);
        }

        public int CompareTo(PointAOC other)
        {
            if (Y == other.Y)
            {
                return X.CompareTo(other.X);
            }
            else
            {
                return Y.CompareTo(other.Y);
            }
        }

        //public IComparer<PointAOC> slopeOrder()
        //{
        //    return new SlopeOrder();
        //}

        //internal class SlopeOrder : IComparer<PointAOC>
        //{
        //    //public override int Compare(PointAOC x, PointAOC y)
        //    //{
        //    //    //double slope1 = 
        //    //}
        //}


    }
}
