using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures
{
    public class WeightedDigraph<T>
    {
        private Dictionary<T, List<Edge<T>>> verticesAndChildren;

        private List<Edge<T>> edges;
        private List<T> visited;

        public WeightedDigraph()
        {
            verticesAndChildren = new Dictionary<T, List<Edge<T>>>();
        }

        /// <summary>
        /// Adds a vertex or does nothing if the vertex already exists
        /// </summary>
        /// <param name="key"></param>
        public void AddVertex(T key)
        {
            if (!verticesAndChildren.ContainsKey(key))
            {
                verticesAndChildren[key] = new List<Edge<T>>();
            }
        }

        public void AddEdge(T parent, T child, int weight)
        {
            ValidateVertex(parent);
            ValidateVertex(child);

            if (!EdgeExists(parent, child))
            {
                verticesAndChildren[parent]
                    .Add(new Edge<T>()
                    {
                        Child = child,
                        Weight = weight
                    });
            }
        }

        private void ValidateVertex(T key)
        {
            if (!VertexExists(key))
            {
                AddVertex(key);
            }
        }

        public IEnumerable<Edge<T>> GetChildren(T key)
        {
            if (!VertexExists(key))
            {
                return null;
            }

            return verticesAndChildren[key];
        }

        public IEnumerable<Edge<T>> GetDescendants(T key)
        {
            edges = new List<Edge<T>>();
            visited = new List<T>();
            DepthFirstSearch(key);
            return edges;
        }

        private void DepthFirstSearch(T key)
        {
            visited.Add(key);
            foreach (Edge<T> childEdge in GetChildren(key))
            {
                edges.Add(childEdge);
                if (!visited.Contains(childEdge.Child))
                {
                    DepthFirstSearch(childEdge.Child);
                }
            }
        }

        public WeightedDigraph<T> Reverse()
        {
            WeightedDigraph<T> reverse = new WeightedDigraph<T>();

            foreach (T key in verticesAndChildren.Keys)
            {
                reverse.AddVertex(key);
                foreach (Edge<T> childEdge in GetChildren(key))
                {
                    reverse.AddVertex(childEdge.Child);
                    reverse.AddEdge(childEdge.Child, key, childEdge.Weight);
                }
            }

            return reverse;
        }



        private bool VertexExists(T key)
        {
            return verticesAndChildren.ContainsKey(key);
        }

        private bool EdgeExists(T parent, T child)
        {
            return false;
            //if (!(VertexExists(parent) && VertexExists(child)))
            //{
            //    return false;
            //}

            //return verticesAndChildren[parent].Contains(child);
        }
    }

    public class Edge<T>
    {
        public T Child { get; set; }
        public int Weight { get; set; }
    }
}
