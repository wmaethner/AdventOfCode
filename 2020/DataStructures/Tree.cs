using System;
using System.Collections.Generic;

namespace DataStructures
{
    public class Tree<TKey, TValue>
    {
        Dictionary<TKey, Node<TValue>> _nodes;

        public Tree()
        {
            _nodes = new Dictionary<TKey, Node<TValue>>();
        }

        public void AddNode(TKey key, TValue value)
        {
            if (!NodeExists(key))
            {
                _nodes[key] = NewNode(value);
            }
        }

        public void AddChild(TKey parent, TKey child)
        {
            if (!NodeExists(parent) || !NodeExists(child))
            {
                throw new ArgumentException("Nodes don't exist.");
            }

            _nodes[parent].AddChild(_nodes[child]);
        }

        //public IEnumerable<TValue> GetChildren(TKey parent)
        //{

        //}

        //public IEnumerable<TValue> GetAncestors(TKey parent)
        //{
        //    Dictionary<TKey, TValue> ancestors = new Dictionary<TKey, TValue>();
        //    foreach (Node<TValue> node in _nodes[parent].GetChildren())
        //    {

        //    }
        //    return 
        //}

        private void GetAncestorsRecurse(TKey parent, ref Dictionary<TKey, TValue> nodesVisited)
        {
            foreach (Node<TValue> node in _nodes[parent].GetChildren())
            {

            }
        }

        public bool NodeExists(TKey key)
        {
            return _nodes.ContainsKey(key);
        }



        private Node<TValue> NewNode(TValue value)
        {
            return new Node<TValue>(value);
        }
    }

    public class Node<T>
    {
        List<Node<T>> Children;
        T Value;

        public Node(T value)
        {
            Children = new List<Node<T>>();
            Value = value;
        }

        public void AddChild(Node<T> node)
        {
            Children.Add(node);
        }

        public IEnumerable<Node<T>> GetChildren()
        {
            return Children.ToArray();
        }
    }
}
