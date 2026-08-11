using System;
using System.Collections.Generic;

public class DancingLinks
{
    private class Node
    {
        public Node Left { get; set; }
        public Node Right { get; set; }
        public Node Up { get; set; }
        public Node Down { get; set; }
        public Node Header { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
    }

    private class HeaderNode : Node
    {
        public int Size { get; set; }
    }

    private Node root;
    private Dictionary<int, HeaderNode> headers;
    private Dictionary<int, List<Node>> rows;

    public DancingLinks(int rowsCount, int columnsCount)
    {
        root = new Node();
        root.Left = root;
        root.Right = root;
        headers = new Dictionary<int, HeaderNode>();
        this.rows = new Dictionary<int, List<Node>>();

        for (int i = 0; i < columnsCount; i++)
        {
            var header = new HeaderNode { Column = i };
            header.Left = root;
            header.Right = root;
            header.Up = header;
            header.Down = header;
            header.Size = 0;
            headers.Add(i, header);
            InsertNode(header, root);
        }
    }

    private void InsertNode(Node node, Node rightNode)
    {
        node.Right = rightNode;
        node.Left = rightNode.Left;
        rightNode.Left.Right = node;
        rightNode.Left = node;
    }

    private void RemoveNode(Node node)
    {
        node.Left.Right = node.Right;
        node.Right.Left = node.Left;
    }

    private void ReinsertNode(Node node)
    {
        InsertNode(node, node.Right);
    }

    public void AddRow(int row, int[] columns)
    {
        if (!rows.ContainsKey(row))
        {
            rows.Add(row, new List<Node>());
        }

        foreach (var column in columns)
        {
            var node = new Node { Row = row, Column = column };
            var header = headers[column];
            InsertNode(node, header);
            rows[row].Add(node);
            header.Size++;
        }
    }

    public void RemoveRow(int row)
    {
        if (rows.ContainsKey(row))
        {
            foreach (var node in rows[row])
            {
                var header = headers[node.Column];
                header.Size--;
                RemoveNode(node);
            }
            rows.Remove(row);
        }
    }

    public void RemoveColumn(int column)
    {
        var header = headers[column];
        RemoveNode(header);
        headers.Remove(column);
    }

    public List<int[]> SolveExactCover()
    {
        var solutions = new List<int[]>();
        SolveExactCoverRecursive(solutions, new List<int>(), root.Right);
        return solutions;
    }

    private bool SolveExactCoverRecursive(List<int[]> solutions, List<int> currentSolution, Node node)
    {
        if (node == root)
        {
            solutions.Add(currentSolution.ToArray());
            return true;
        }

        var header = (HeaderNode)node;
        if (header.Size == 0)
        {
            return false;
        }

        RemoveNode(header);
        foreach (var row in rows)
        {
            if (row.Value.Exists(n => n.Column == header.Column))
            {
                foreach (var nodeInRow in row.Value)
                {
                    var nodeHeader = nodeInRow.Header;
                    RemoveNode(nodeHeader);
                }
                if (SolveExactCoverRecursive(solutions, currentSolution.Concat(new int[] { row.Key }).ToList(), node.Right))
                {
                    return true;
                }
                foreach (var nodeInRow in row.Value)
                {
                    var nodeHeader = nodeInRow.Header;
                    ReinsertNode(nodeHeader);
                }
            }
        }
        ReinsertNode(header);
        return false;
    }
}