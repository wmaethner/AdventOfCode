using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.HelperClasses
{
    public class Graph
    {
        private int V;
        private int E;
        private HashSet<int>[] adj;

        public Graph(int V)
        {
            if (V < 0)
            {
                throw new ArgumentException("Number of vertices must be non-negative");
            }
            this.V = V;
            E = 0;
            adj = new HashSet<int>[V];
            for (int i = 0; i < V; i++)
            {
                adj[i] = new HashSet<int>();
            }
        }

        public int Vertices()
        {
            return V;
        }

        public int Edges()
        {
            return E;
        }







        private void validateVertex(int v)
        {
            if (v < 0 || v > V)
            {
                throw new ArgumentOutOfRangeException("v", $"Vertex {v} is not between 0 and {V - 1}");
            }
        }

    }
}
