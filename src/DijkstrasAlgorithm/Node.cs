using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DijkstrasAlgorithm
{
    public class Node<T, C> where T : IEquatable<T> where C : IComparable<C>
    {
        public Node()
        {
            Children = new Dictionary<Node<T, C>, C>();
        }

        public T Label { get; set; }

        public IDictionary<Node<T, C>, C> Children { get; private set; }

        public override string ToString()
        {
            return string.Format($"{Label} ({Children.Count})");
        }
    }
}