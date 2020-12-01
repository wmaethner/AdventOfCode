using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.HelperClasses
{
    public class PathNode
    {
        public int X { get; set; }
        public int Y { get; set; }

        public PathNode NextNode { get; set; }
        public int DistToNextNode { get; set; }

        public bool NodeOnPath(PathNode node)
        {
            PathNode currentNode = this;
            while (currentNode != node)
            {
                if (currentNode.NextNode == null)
                {
                    return false;  //node doesn't exist on path
                }
            }
            return true;    //we found the node on the path
        }

        public int DistanceTo(PathNode node)
        {
            PathNode currentNode = this;
            int distance = 0;
            while (currentNode != node)
            {
                if (currentNode.NextNode == null)
                {
                    return -1;  //node doesn't exist on path
                }

                distance += currentNode.DistToNextNode;
                currentNode = currentNode.NextNode;
            }
            return distance;
        }

        public override bool Equals(object obj)
        {
            try
            {
                PathNode node = (PathNode)obj;
                return ((this.X == node.X) && (this.Y == node.Y));
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
