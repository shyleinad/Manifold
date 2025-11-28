using Manifold.DomainLayer.Entities;

namespace Manifold.ApplicationLayer;

public class PathFinder
{
    // Breadth-first search algorithm
    public static List<GraphElement> FindShortestPath(GraphElement start, GraphElement target)
    {
        // Store parents to reconstruct the path
        Dictionary<GraphElement, GraphElement?> parents = new Dictionary<GraphElement, GraphElement?>();

        // Queue for nodes to be visited to ensure shortest path
        Queue<GraphElement> queue = new Queue<GraphElement>();

        queue.Enqueue(start);

        parents[start] = null;

        while (queue.Count > 0)
        {
            GraphElement current = queue.Dequeue();

            if (current == target)
            {
                break;
            }

            if (current.ConnectedPipes.Count == 0)
            {
                continue;
            }

            List<Pipe> pipes = current.ConnectedPipes;

            foreach (var pipe in pipes)
            {
                GraphElement next = pipe.GetNext(current);

                if (!parents.ContainsKey(next))
                {
                    parents[next] = current;
                    queue.Enqueue(next);
                }
            }
        }

        if (!parents.ContainsKey(target))
        {
            return new List<GraphElement>();
        }

        // Reconstruct the shortest path
        List<GraphElement> path = new List<GraphElement>();

        for (var n = target; n != null; n = parents[n])
            path.Add(n);

        path.Reverse();

        return path;
    }
}
