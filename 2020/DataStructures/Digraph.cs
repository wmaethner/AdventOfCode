using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures
{
    public class Digraph<T>
    {
        private Dictionary<T, List<T>> verticesAndChildren;        
        private List<T> visited;

        public Digraph()
        {
            verticesAndChildren = new Dictionary<T, List<T>>();
        }

        /// <summary>
        /// Adds a vertex or does nothing if the vertex already exists
        /// </summary>
        /// <param name="key"></param>
        public void AddVertex(T key)
        {
            if (!verticesAndChildren.ContainsKey(key))
            {
                verticesAndChildren[key] = new List<T>();
            }
        }

        public void AddEdge(T parent, T child)
        {
            if (!(VertexExists(parent) && VertexExists(child)))
            {
                return;
            }

            if (!EdgeExists(parent, child))
            {
                verticesAndChildren[parent].Add(child);
            }            
        }

        public IEnumerable<T> GetChildren(T key)
        {
            if (!VertexExists(key))
            {
                return null;
            }

            return verticesAndChildren[key];
        }

        public IEnumerable<T> GetDescendants(T key)
        {
            visited = new List<T>();
            DepthFirstSearch(key);
            return visited;
        }

        private void DepthFirstSearch(T key)
        {
            visited.Add(key);
            foreach (T child in GetChildren(key))
            {
                if (!visited.Contains(child))
                {
                    DepthFirstSearch(child);
                }
            }
        }

        public Digraph<T> Reverse()
        {
            Digraph<T> reverse = new Digraph<T>();

            foreach (T key in verticesAndChildren.Keys)
            {
                reverse.AddVertex(key);
                foreach (T childKey in GetChildren(key))
                {
                    reverse.AddVertex(childKey);
                    reverse.AddEdge(childKey, key);
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
            if (!(VertexExists(parent) && VertexExists(child)))
            {
                return false;
            }

            return verticesAndChildren[parent].Contains(child);
        } 
    }

    class Node
    {
        public List<Node> ChildNodes { get; set; }
    }
}
