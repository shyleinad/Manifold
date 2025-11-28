using Manifold.DomainLayer.Entities;
using Manifold.DomainLayer.Enums;

namespace Manifold.ApplicationLayer.Services;
public interface IFlowService
{
    void UpdateFlow(List<GraphElement> path, ContentTypes content);
}
