using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DijkstrasAlgorithm
{
    public class Search
    {
        private readonly IDictionary<string, IDictionary<string, Path>> _searchData;
        private readonly Node<string, double> _graphRoot;

        public Search(Node<string, double> graphRoot)
        {
            _searchData = new Dictionary<string, IDictionary<string, Path>>();
            _graphRoot = graphRoot;
        }

        public IEnumerable<string> BestPath(string start, string end)
        {
            var paths = new List<string>();
            var next = end;

            if (!_searchData.ContainsKey(start))
            {
                Calculate(FindNode(_graphRoot, start));
            }

            var costData = _searchData[start];

            paths.Add(end);

            while (true)
            {
                var path = costData[next];

                if (path.PreviousVertex == null)
                {
                    break;
                }

                paths.Add(path.PreviousVertex);

                next = path.PreviousVertex;
            }

            paths.Reverse();

            return paths;
        }

        private Node<string, double> FindNode(Node<string, double> currentNode, string label, HashSet<string> visited = null)
        {
            visited = visited ?? new HashSet<string>();

            visited.Add(currentNode.Label);

            foreach (var child in currentNode.Children)
            {
                if (child.Key.Label == label) return child.Key;

                if (!visited.Contains(child.Key.Label))
                {
                    var cres = FindNode(child.Key, label, visited);

                    if (cres != null) return cres;
                }
            }

            return null;
        }

        private void Calculate(Node<string, double> start)
        {
            double currentCost = 0;

            var costData = _searchData[start.Label] = new Dictionary<string, Path>();

            while (true)
            {
                if (costData.ContainsKey(start.Label))
                {
                    costData[start.Label].Visited = true;
                }
                else
                {
                    costData[start.Label] = new Path() { Vertex = start.Label, Visited = true };
                }

                foreach (var child in start.Children)
                {
                    Path path;

                    if (!costData.TryGetValue(child.Key.Label, out path))
                    {
                        costData[child.Key.Label] = path = new Path()
                        {
                            PreviousVertex = start.Label,
                            Vertex = child.Key.Label,
                            Cost = child.Value + currentCost
                        };
                    }

                    if (child.Value + currentCost < path.Cost)
                    {
                        path.Cost = child.Value + currentCost;
                        path.PreviousVertex = start.Label;
                    }
                }

                var best = costData.Where(c => !c.Value.Visited).Join(start.Children, o => o.Key, i => i.Key.Label, (o, i) => o).OrderBy(x => x.Value.Cost).FirstOrDefault();

                if (best.Value == null)
                {
                    break;
                }

                start = start.Children.First(c => c.Key.Label == best.Key).Key;
                currentCost = best.Value.Cost;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var costs in _searchData)
            {
                sb.AppendLine($"{costs.Key}");
                sb.AppendLine("===================================");
                sb.AppendLine("Vertex\t|\tCost\t|\tPrevious Vertex");

                foreach (var item in costs.Value)
                {
                    sb.AppendLine($"{item.Value.Vertex}\t|\t{item.Value.Cost}\t|\t{item.Value.PreviousVertex}");
                }
            }

            return sb.ToString();
        }

        private class Path
        {
            public string PreviousVertex { get; set; }
            public string Vertex { get; set; }
            public bool Visited { get; set; }
            public double Cost { get; set; }
        }
    }
}