using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        int[,] maze = {
            { 0, 1, 0, 0, 0 },
            { 0, 1, 0, 1, 0 },
            { 0, 0, 0, 1, 0 },
            { 0, 1, 0, 1, 0 },
            { 0, 0, 0, 0, 0 }
        };
        (int, int) start = (0, 0);
        (int, int) end = (4, 4);

        Console.WriteLine("BFS:");
        var bfsPath = BFS(maze, start, end);
        PrintPath(bfsPath, maze);

        Console.WriteLine("\nDFS:");
        var dfsPath = DFS(maze, start, end);
        PrintPath(dfsPath, maze);

        Console.WriteLine("\nGreedy Best-First Search:");
        var gbfsPath = GreedyBestFirstSearch(maze, start, end);
        PrintPath(gbfsPath, maze);

        Console.WriteLine("\nA* Search:");
        var aStarPath = AStar(maze, start, end);
        PrintPath(aStarPath, maze);
    }

    static List<(int, int)> BFS(int[,] maze, (int, int) start, (int, int) end)
    {
        var rows = maze.GetLength(0);
        var cols = maze.GetLength(1);
        var directions = new (int, int)[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
        var queue = new Queue<((int, int), List<(int, int)>)>();
        var visited = new HashSet<(int, int)>();
        var path = new List<(int, int)>();

        queue.Enqueue((start, new List<(int, int)> { start }));
        visited.Add(start);

        while (queue.Count > 0)
        {
            var (current, currentPath) = queue.Dequeue();
            if (current == end)
                return currentPath;

            foreach (var (dr, dc) in directions)
            {
                var (r, c) = (current.Item1 + dr, current.Item2 + dc);
                if (r >= 0 && r < rows && c >= 0 && c < cols && maze[r, c] == 0 && !visited.Contains((r, c)))
                {
                    visited.Add((r, c));
                    var newPath = new List<(int, int)>(currentPath) { (r, c) };
                    queue.Enqueue(((r, c), newPath));
                }
            }
        }
        return path; // no path found
    }

    static List<(int, int)> DFS(int[,] maze, (int, int) start, (int, int) end)
    {
        var rows = maze.GetLength(0);
        var cols = maze.GetLength(1);
        var directions = new (int, int)[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
        var stack = new Stack<((int, int), List<(int, int)>)>();
        var visited = new HashSet<(int, int)>();
        var path = new List<(int, int)>();

        stack.Push((start, new List<(int, int)> { start }));
        visited.Add(start);

        while (stack.Count > 0)
        {
            var (current, currentPath) = stack.Pop();
            if (current == end)
                return currentPath;

            foreach (var (dr, dc) in directions)
            {
                var (r, c) = (current.Item1 + dr, current.Item2 + dc);
                if (r >= 0 && r < rows && c >= 0 && c < cols && maze[r, c] == 0 && !visited.Contains((r, c)))
                {
                    visited.Add((r, c));
                    var newPath = new List<(int, int)>(currentPath) { (r, c) };
                    stack.Push(((r, c), newPath));
                }
            }
        }
        return path; // no path found
    }

    static List<(int, int)> GreedyBestFirstSearch(int[,] maze, (int, int) start, (int, int) end)
    {
        var rows = maze.GetLength(0);
        var cols = maze.GetLength(1);
        var directions = new (int, int)[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
        var priorityQueue = new PriorityQueue<((int, int), List<(int, int)>), int>();
        var visited = new HashSet<(int, int)>();
        var path = new List<(int, int)>();

        int Heuristic((int, int) pos) => Math.Abs(pos.Item1 - end.Item1) + Math.Abs(pos.Item2 - end.Item2);

        priorityQueue.Enqueue((start, new List<(int, int)> { start }), Heuristic(start));
        visited.Add(start);

        while (priorityQueue.Count > 0)
        {
            var (current, currentPath) = priorityQueue.Dequeue();
            if (current == end)
                return currentPath;

            foreach (var (dr, dc) in directions)
            {
                var (r, c) = (current.Item1 + dr, current.Item2 + dc);
                if (r >= 0 && r < rows && c >= 0 && c < cols && maze[r, c] == 0 && !visited.Contains((r, c)))
                {
                    visited.Add((r, c));
                    var newPath = new List<(int, int)>(currentPath) { (r, c) };
                    priorityQueue.Enqueue(((r, c), newPath), Heuristic((r, c)));
                }
            }
        }
        return path; // no path found
    }

    static List<(int, int)> AStar(int[,] maze, (int, int) start, (int, int) end)
    {
        var rows = maze.GetLength(0);
        var cols = maze.GetLength(1);
        var directions = new (int, int)[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
        var priorityQueue = new PriorityQueue<((int, int), List<(int, int)>), int>();
        var visited = new HashSet<(int, int)>();
        var path = new List<(int, int)>();

        int Heuristic((int, int) pos) => Math.Abs(pos.Item1 - end.Item1) + Math.Abs(pos.Item2 - end.Item2);
        int Cost((int, int) pos) => new List<(int, int)>(path).Count;

        priorityQueue.Enqueue((start, new List<(int, int)> { start }), Heuristic(start));
        visited.Add(start);

        while (priorityQueue.Count > 0)
        {
            var (current, currentPath) = priorityQueue.Dequeue();
            if (current == end)
                return currentPath;

            foreach (var (dr, dc) in directions)
            {
                var (r, c) = (current.Item1 + dr, current.Item2 + dc);
                if (r >= 0 && r < rows && c >= 0 && c < cols && maze[r, c] == 0 && !visited.Contains((r, c)))
                {
                    visited.Add((r, c));
                    var newPath = new List<(int, int)>(currentPath) { (r, c) };
                    int totalCost = Cost((r, c)) + Heuristic((r, c));
                    priorityQueue.Enqueue(((r, c), newPath), totalCost);
                }
            }
        }
        return path; // no path found
    }

    static void PrintPath(List<(int, int)> path, int[,] maze)
    {
        if (path.Count == 0)
        {
            Console.WriteLine("No path found");
            return;
        }

        int rows = maze.GetLength(0);
        int cols = maze.GetLength(1);
        var mazeWithPath = (int[,])maze.Clone();

        foreach (var (r, c) in path)
            mazeWithPath[r, c] = 2; // 2 represents the path in the output

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
                Console.Write(mazeWithPath[i, j] + " ");
            Console.WriteLine();
        }
    }
}

class PriorityQueue<TItem, TPriority> where TPriority : IComparable
{
    private readonly SortedDictionary<TPriority, Queue<TItem>> _dict = new();

    public void Enqueue(TItem item, TPriority priority)
    {
        if (!_dict.ContainsKey(priority))
            _dict[priority] = new Queue<TItem>();

        _dict[priority].Enqueue(item);
    }

    public TItem Dequeue()
    {
        if (_dict.Count == 0)
            throw new InvalidOperationException("The priority queue is empty.");

        // Get the first (smallest) priority
        var firstPair = _dict.First();
        var firstQueue = firstPair.Value;
        var item = firstQueue.Dequeue();

        // Remove the priority if its queue is empty
        if (firstQueue.Count == 0)
            _dict.Remove(firstPair.Key);

        return item;
    }

    public int Count
    {
        get
        {
            int count = 0;
            foreach (var pair in _dict)
                count += pair.Value.Count;
            return count;
        }
    }
}