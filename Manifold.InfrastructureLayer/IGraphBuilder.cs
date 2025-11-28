using Manifold.DomainLayer.Entities;

namespace Manifold.InfrastructureLayer;
public interface IGraphBuilder
{
    List<GraphElement> BuildGraph();
}
