using Manifold.DomainLayer.Entities;

namespace Manifold.ApplicationLayer.Services;
public interface IRoutingService
{
    List<GraphElement> GetPath(GraphElement source, GraphElement target);
    void SwitchValves(List<GraphElement> path);
}
