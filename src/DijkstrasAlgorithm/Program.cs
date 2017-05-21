using System;

namespace DijkstrasAlgorithm
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var graph = new JsonGraphReader().Read(@"..\..\graph.json");
            var search = new Search(graph);

            foreach(var p in search.BestPath("b", "a"))
            {
                Console.Write(" => " + p);
            }

            Console.WriteLine();
            Console.WriteLine(search);

            Console.ReadKey();
        }
    }
}