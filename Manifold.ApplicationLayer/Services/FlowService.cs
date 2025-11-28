using Manifold.DomainLayer.Entities;
using Manifold.DomainLayer.Enums;

namespace Manifold.ApplicationLayer.Services;
public class FlowService : IFlowService
{
    public void UpdateFlow(List<GraphElement> path, ContentTypes content)
    {
        for (int i = 0; i < path.Count - 1; i++)
        {
            GraphElement from = path[i];
            GraphElement to = path[i + 1];

            Pipe? pipe = path[i].ConnectedPipes.FirstOrDefault(p => p.Connects(from, to)) 
                ?? throw new NullReferenceException($"No pipe found between {from.Id} and {to.Id}");

            pipe.Content = content;
        }
    }
}
