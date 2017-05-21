using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace DijkstrasAlgorithm
{
    public class JsonGraphReader
    {
        public Node<string, double> Read(string fileName)
        {
            var file = new FileInfo(fileName);

            using (var fs = file.OpenText())
            {
                var dict = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, int>>>(fs.ReadToEnd());

                var builder = new Dictionary<string, Node<string, double>>();

                Node<string, double> root = null;

                foreach (var item in dict)
                {
                    Node<string, double> node = null;

                    if (!builder.TryGetValue(item.Key, out node))
                    {
                        builder[item.Key] = node = new Node<string, double>() { Label = item.Key };
                    }

                    if (root == null) root = node;

                    foreach (var child in item.Value)
                    {
                        Node<string, double> cnode;

                        if (!builder.TryGetValue(child.Key, out cnode))
                        {
                            builder[child.Key] = cnode = new Node<string, double>() { Label = child.Key };
                        }

                        node.Children[cnode] = child.Value;
                    }
                }

                return root;
            }
        }
    }
}