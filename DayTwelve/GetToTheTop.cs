namespace AdventOfCode2022.DayTwelve;

public class GetToTheTop
{
    private string[] _rawMap { get; set; }
    private List<Node> _visited { get; set; }

    public int GetToTheTopSteps()
    {
        _rawMap = File.ReadAllLines("DayTwelve/topography.txt");
        _visited = new List<Node>();

        for (int row = 0; row < _rawMap.Length; row++)
        {
            var line = _rawMap[row];

            for (int column = 0; column < line.Length; column++)
            {
                var marker = line[column];

                if (marker == 'S')
                {
                    _startNode = new Node { Row = row, Column = column, Map = _rawMap };
                }
            }
        }

        if (_startNode == null) throw new NotSupportedException("No start in this graph");

        _ = PopulateNeighbours(_startNode);

        var lowest = int.MaxValue;
        foreach (Node lowestNode in _visited.Where(n => n.Height == 'a'))
        {
            _previous.Clear();

            BreadthFirstSearch(lowestNode);

            var path = ShortestPath(lowestNode, _endNode);

            if (path < lowest) lowest = path;
        }

        //412 too high
        return lowest;
    }

    //https://www.koderdojo.com/blog/breadth-first-search-and-shortest-path-in-csharp-and-net-core
    //I dislike his code style but need to use it somehow so this is a butchered code
    private readonly Dictionary<Node, Node> _previous = new();
    private Node? _startNode;
    private Node? _endNode;

    private void BreadthFirstSearch(Node startNode)
    {
        var queue = new Queue<Node>();

        queue.Enqueue(startNode);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();

            foreach (Node neighbour in node.Neighbours)
            {
                if (_previous.ContainsKey(neighbour))
                    continue;

                _previous[neighbour] = node;
                queue.Enqueue(neighbour);
            }
        }
    }

    private int ShortestPath(Node startNode, Node endNode)
    {
        var path = new List<Node>();

        var current = endNode;
        while (!current.Equals(startNode))
        {
            path.Add(current);

            if (!_previous.ContainsKey(current)) return int.MaxValue;

            current = _previous[current];
        }

        return path.Count;
    }

    private Node PopulateNeighbours(Node node)
    {
        _visited.Add(node);

        if (node.IsEnd) _endNode = node;

        if (node.Row > 0)
        {
            var topNode = new Node { Row = node.Row - 1, Column = node.Column, Map = _rawMap };

            if (node.Reachable(topNode))
            {
                AddNeighbour(node, topNode);
            }
        }

        if (node.Row < _rawMap.Length - 1)
        {
            var bottomNode = new Node { Row = node.Row + 1, Column = node.Column, Map = _rawMap };

            if (node.Reachable(bottomNode))
            {
                AddNeighbour(node, bottomNode);
            }
        }

        if (node.Column > 0)
        {
            var leftNode = new Node { Row = node.Row, Column = node.Column - 1, Map = _rawMap };

            if (node.Reachable(leftNode))
            {
                AddNeighbour(node, leftNode);
            }
        }

        if (node.Column < _rawMap[0].Length - 1)
        {
            var rightNode = new Node { Row = node.Row, Column = node.Column + 1, Map = _rawMap };

            if (node.Reachable(rightNode))
            {
                AddNeighbour(node, rightNode);
            }
        }

        return node;
    }

    private void AddNeighbour(Node node, Node anotherNode)
    {
        node.Neighbours.Add(!_visited.Contains(anotherNode)
            ? PopulateNeighbours(anotherNode)
            : _visited[_visited.IndexOf(anotherNode)]);
    }

    class Node
    {
        public char Height
        {
            get
            {
                var height = Map[Row][Column];

                if (height == 'S') return 'a';
                if (height == 'E') return 'z';

                return height;
            }
        }

        public bool IsEnd => Map[Row][Column] == 'E';

        public List<Node> Neighbours { get; set; } = new();

        public int Row { get; set; }

        public int Column { get; set; }

        public string[] Map { get; set; }

        public bool Reachable(Node originNode)
        {
            var heightDifference = originNode.Height - Height;

            return heightDifference <= 1;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Node node)
            {
                return node.Row == Row && node.Column == Column;
            }

            return base.Equals(obj);
        }

        public override string ToString()
        {
            return $"{Height} (R:{Row}, C:{Column})";
        }
    }
}