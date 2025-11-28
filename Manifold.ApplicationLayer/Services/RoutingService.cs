using Manifold.DomainLayer.Entities;

namespace Manifold.ApplicationLayer.Services;
public class RoutingService : IRoutingService
{
    public RoutingService()
    {
    }

    public List<GraphElement> GetPath(GraphElement source, GraphElement target)
    {
        var path = PathFinder.FindShortestPath(source, target);

        SwitchValves(path);

        return path;
    }

    public void SwitchValves(List<GraphElement> path)
    {
        for (int i = 0; i < path.Count - 1; i++)
        {
            GraphElement current = path[i];

            GraphElement next = path[i + 1];

            if (current is Valve valve)
            {
                var pipe = current.ConnectedPipes
                  .FirstOrDefault(p => p.Connects(current, next)) 
                  ?? throw new Exception($"No pipe found between {current.Id} and {next.Id}");

                valve.SwitchTo(pipe);
            }
        }
    }

}
