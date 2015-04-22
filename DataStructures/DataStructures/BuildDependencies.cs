using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DataStructures
{
    public class BuildDependencies
    {

        public static IEnumerable<string> BuildOrder(Dictionary<string, List<string>> moduleDependencies, string module)
        {
            var depthDictionary = new Dictionary<int, List<string>>();

            PopulateDepthDictionary(moduleDependencies, depthDictionary, 0, module);
            var sortedDepths = depthDictionary.Keys.ToList();
            sortedDepths.Sort();
            sortedDepths.Reverse();

            var addedModules = new HashSet<string>();
            var orderedModules = new List<string>();

            foreach (var depth in sortedDepths)
            {
                foreach (var item in depthDictionary[depth])
                {
                    if (!addedModules.Contains(item))
                    {
                        addedModules.Add(item);
                        orderedModules.Add(item);
                    }
                }
            }

            return orderedModules;
        }

        private static void PopulateDepthDictionary(Dictionary<string, List<string>> moduleDependencies,
            Dictionary<int, List<string>> depthDictionary, int depth, string module)
        {
            depth++;
            if (depthDictionary.ContainsKey(depth))
            {
                depthDictionary[depth].Add(module);
            }
            else
            {
                depthDictionary.Add(depth, new List<string>{module});
            }


            if (moduleDependencies.ContainsKey(module))
            {
                foreach (var dependency in moduleDependencies[module])
                {
                    PopulateDepthDictionary(moduleDependencies, depthDictionary, depth, dependency);
                }
            }
           
        }


        public static IEnumerable<string> BuildOrderWithQueues(Dictionary<string, List<string>> moduleDependencies, string module)
        {
           
            var addedModules = new HashSet<string>();
            var queuedModules = new Queue<string>();
            var orderedModules = new List<string>();

            queuedModules.Enqueue(module);

            while (!queuedModules.IsEmpty())
            {
                var node = queuedModules.Dequeue();
                orderedModules.Add(node);

                if (moduleDependencies.ContainsKey(node))
                {
                    foreach (var child in moduleDependencies[node])
                    {
                        queuedModules.Enqueue(child);
                    }
                }

            }

            orderedModules.Reverse();
            var modulesWithoutDups = new List<string>();

            foreach (var item in orderedModules)
            {
                if (!addedModules.Contains(item))
                {
                    addedModules.Add(item);
                    modulesWithoutDups.Add(item);
                }
            }

            return modulesWithoutDups;
        }

        [TestFixture]
        public class BuildTests
        {

            [Test]
            public void ModuleTest()
            {
                var input = new Dictionary<string, List<string>>
                {
                    {"A", new List<string> {"B", "C"}},
                    {"B", new List<string> {"D", "E", "F"}},
                    {"D", new List<string> {"F"}}
                };

                var sorted = BuildDependencies.BuildOrderWithQueues(input, "A");
            }
        }
    }
}
