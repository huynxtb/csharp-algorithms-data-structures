using System;
using System.Collections.Generic;

public class TarjansSccSolver
{
    private Dictionary<int, List<int>> _graph;
    private Dictionary<int, int> _discoveryTime; // Stores discovery times
    private Dictionary<int, int> _lowLink;      // Stores low-link values
    private Stack<int> _stack;                  // Stores vertices currently in the DFS path
    private HashSet<int> _onStack;              // Fast lookup for vertices on the stack
    private List<IList<int>> _sccs;             // Stores the found SCCs
    private int _index;                         // Global counter for discovery times

    /// <summary>
    /// Solves the Strongly Connected Components (SCC) problem for a directed graph
    /// using Tarjan's algorithm.
    /// </summary>
    /// <param name="adjacencyList">The directed graph represented as an adjacency list.
    /// Keys are vertex IDs, values are lists of target vertex IDs.</param>
    /// <returns>A list of SCCs, where each SCC is a list of vertex IDs.</returns>
    public IList<IList<int>> Solve(Dictionary<int, List<int>> adjacencyList)
    {
        _graph = adjacencyList ?? new Dictionary<int, List<int>>();
        _discoveryTime = new Dictionary<int, int>();
        _lowLink = new Dictionary<int, int>();
        _stack = new Stack<int>();
        _onStack = new HashSet<int>();
        _sccs = new List<IList<int>>();
        _index = 0;

        // Collect all unique vertices from keys and values to handle isolated vertices
        // and ensure all parts of a disconnected graph are visited.
        HashSet<int> allVertices = new HashSet<int>();
        foreach (var entry in _graph)
        {
            allVertices.Add(entry.Key);
            foreach (int neighbor in entry.Value)
            {
                allVertices.Add(neighbor);
            }
        }

        foreach (int vertex in allVertices)
        {
            if (!_discoveryTime.ContainsKey(vertex))
            {
                Dfs(vertex);
            }
        }

        return _sccs;
    }

    private void Dfs(int u)
    {
        _discoveryTime[u] = _index;
        _lowLink[u] = _index;
        _index++;

        _stack.Push(u);
        _onStack.Add(u);

        // Get neighbors, handle cases where a vertex might not have an entry in the adjacency list
        // but is present as a target (e.g., 1->2, 2->3, but 3 has no outgoing edges).
        List<int> neighbors;
        if (_graph.TryGetValue(u, out neighbors))
        {
            foreach (int v in neighbors)
            {
                if (!_discoveryTime.ContainsKey(v)) // Neighbor v not visited
                {
                    Dfs(v);
                    _lowLink[u] = Math.Min(_lowLink[u], _lowLink[v]);
                }
                else if (_onStack.Contains(v)) // Neighbor v is on stack (back-edge to an ancestor in DFS tree)
                {
                    _lowLink[u] = Math.Min(_lowLink[u], _discoveryTime[v]);
                }
            }
        }

        // If u is a root of an SCC
        if (_lowLink[u] == _discoveryTime[u])
        {
            List<int> currentScc = new List<int>();
            int w;
            do
            {
                w = _stack.Pop();
                _onStack.Remove(w);
                currentScc.Add(w);
            } while (w != u);
            _sccs.Add(currentScc);
        }
    }
}