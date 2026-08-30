using System;
using System.Collections.Generic;

namespace GraphAlgorithms

{
    public struct AnalysisResult
    {
        public HashSet<Tuple<int, int>> Bridges { get; }
        public HashSet<int> ArticulationPoints { get; }

        public AnalysisResult(HashSet<Tuple<int, int>> bridges, HashSet<int> articulationPoints)
        {
            Bridges = bridges;
            ArticulationPoints = articulationPoints;
        }
    }

    public static class UndirectedGraphAnalyzer
    {
        public static AnalysisResult Analyze(Dictionary<int, List<int>> graph)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            var vertices = new HashSet<int>(graph.Keys);
            foreach (var kvp in graph)
            {
                if (kvp.Value != null)
                {
                    foreach (var neighbor in kvp.Value)
                    {
                        vertices.Add(neighbor);
                    }
                }
            }

            var tin = new Dictionary<int, int>();
            var low = new Dictionary<int, int>();
            var visited = new HashSet<int>();
            var bridges = new HashSet<Tuple<int, int>>();
            var articulationPoints = new HashSet<int>();
            int timer = 0;

            foreach (var vertex in vertices)
            {
                if (!visited.Contains(vertex))
                {
                    Dfs(vertex, -1, graph, visited, tin, low, ref timer, bridges, articulationPoints);
                }
            }

            return new AnalysisResult(bridges, articulationPoints);
        }

        private static void Dfs(
            int u,
            int parent,
            Dictionary<int, List<int>> graph,
            HashSet<int> visited,
            Dictionary<int, int> tin,
            Dictionary<int, int> low,
            ref int timer,
            HashSet<Tuple<int, int>> bridges,
            HashSet<int> articulationPoints)
        {
            visited.Add(u);
            tin[u] = low[u] = ++timer;
            int children = 0;
            bool parentSkipped = false;

            if (graph.TryGetValue(u, out var neighbors) && neighbors != null)
            {
                foreach (var v in neighbors)
                {
                    if (v == u)
                    {
                        continue;
                    }

                    if (v == parent)
                    {
                        if (!parentSkipped)
                        {
                            parentSkipped = true;
                            continue;
                        }
                    }

                    if (visited.Contains(v))
                    {
                        low[u] = Math.Min(low[u], tin[v]);
                    }
                    else
                    {
                        children++;
                        Dfs(v, u, graph, visited, tin, low, ref timer, bridges, articulationPoints);
                        low[u] = Math.Min(low[u], low[v]);

                        if (low[v] > tin[u])
                        {
                            int min = Math.Min(u, v);
                            int max = Math.Max(u, v);
                            bridges.Add(Tuple.Create(min, max));
                        }

                        if (parent != -1 && low[v] >= tin[u])
                        {
                            articulationPoints.Add(u);
                        }
                    }
                }
            }

            if (parent == -1 && children > 1)
            {
                articulationPoints.Add(u);
            }
        }
    }
}