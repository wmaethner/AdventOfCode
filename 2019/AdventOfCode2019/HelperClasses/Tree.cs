using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.HelperClasses
{
    public class Tree
    {
        private Node root;

        Dictionary<string, Node> nodes = new Dictionary<string, Node>();


        public Tree(string rootKey)
        {
            root = AddNode(rootKey);
        }

        public void AddNode(string childNode, string parentNode)
        {
            // Don't readd node
            //if (nodes.ContainsKey(childNode))
            //{
            //    return;
            //}

            Node parent = AddNode(parentNode);
            Node child = AddNode(childNode);

            child.ParentNode = parent;
            parent.ChildNodes.Add(child);

            UpdateSizes(root);            
        }

        public int TotalSize()
        {
            int totalSize = 0;
            foreach (Node node in nodes.Values)
            {
                totalSize += node.Size;
            }
            return totalSize;
        }

        public int NodeSize(string key)
        {
            return nodes[key].Size;
        }

        public string CommonAncestor(string node1, string node2)
        {
            Stack<string> path1 = new Stack<string>();
            Node node = nodes[node1];
            while (node != null)
            {
                path1.Push(node.Key);
                node = node.ParentNode;
            }

            Stack<string> path2 = new Stack<string>();
            node = nodes[node2];
            while (node != null)
            {
                path2.Push(node.Key);
                node = node.ParentNode;
            }

            string key1 = path1.Pop();
            string key2 = path2.Pop();
            string ancestor = "";

            while (key1 == key2)
            {
                ancestor = key1;

                key1 = path1.Pop();
                key2 = path2.Pop();
            }

            return ancestor;
        }

        private Node AddNode(string key)
        {
            if (nodes.ContainsKey(key))
            {
                return nodes[key];
            }
            else
            {
                Node node = new Node()
                {
                    Key = key,
                    ParentNode = null,
                    ChildNodes = new List<Node>(),
                    Size = 0
                };
                nodes[key] = node;
                return node;
            }           
        }

        private void UpdateSizes(Node node)
        {
            if (node.ChildNodes.Count == 0)
            {
                return;
            }
            foreach (Node child in node.ChildNodes)
            {
                child.Size = node.Size + 1;
                UpdateSizes(child);
            }
        }
    }

    public class Node
    {
        public string Key { get; set; }
        public Node ParentNode { get; set; }
        public List<Node> ChildNodes { get; set; }
        public int Size { get; set; }
    }
}
