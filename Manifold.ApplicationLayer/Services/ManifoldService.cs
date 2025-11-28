using Manifold.DomainLayer.Entities;
using Manifold.DomainLayer.Enums;

namespace Manifold.ApplicationLayer.Services;
public class ManifoldService : IManifoldService
{
    private readonly IRoutingService routingService;
    private readonly IFlowService flowService;
    private readonly IList<GraphElement> graph;

    public ManifoldService(IRoutingService routingService, IFlowService flowService, IList<GraphElement> graph)
    {
        this.routingService = routingService;
        this.flowService = flowService;
        this.graph = graph;
    }

    public void FillChamber(string chamberId, ContentTypes type)
    {
        Chamber? chamber = graph.OfType<Chamber>().FirstOrDefault(c => c.Id == chamberId) 
            ?? throw new ArgumentException($"Chamber with ID {chamberId} not found.");
        
        GraphElement source = graph.OfType<SourceValve>().First(s => s.Content == type);

        List<GraphElement> path = routingService.GetPath(source, chamber);

        flowService.UpdateFlow(path, type);

        chamber.Content = type;
    }

    public void EmptyChamber(string chamberId, string wasteValveId, bool hazardous)
    {
        Chamber? chamber = graph.OfType<Chamber>().FirstOrDefault(c => c.Id == chamberId) 
            ?? throw new ArgumentException($"Chamber with ID {chamberId} not found.");

        WasteValve? target = graph.OfType<WasteValve>().FirstOrDefault(wv => wv.Id == wasteValveId)
            ?? throw new ArgumentException($"Waste valve with ID {wasteValveId} not found.");

        // Blow air in chamber
        GraphElement source = graph.OfType<SourceValve>().First(s => s.Content == ContentTypes.Air);

        List<GraphElement> path = (routingService.GetPath(source, chamber));

        flowService.UpdateFlow(path, ContentTypes.Air);

        // Drain chamber content
        target.IsHazardous = hazardous;

        chamber.Content = ContentTypes.Air;

        path = routingService.GetPath(chamber, target);

        flowService.UpdateFlow(path, chamber.Content);
    }
}
