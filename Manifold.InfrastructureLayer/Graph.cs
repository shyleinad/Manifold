using Manifold.InfrastructureLayer.DTOs;

namespace Manifold.InfrastructureLayer;

public class Graph
{
    public List<SourceValveDto> SourceValves { get; set; } = new();
    public List<ValveDto> Valves { get; set; } = new();
    public List<WasteValveDto> WasteValves { get; set; } = new();
    public List<ChamberDto> Chambers { get; set; } = new();
    public List<PipeDto> Pipes { get; set; } = new();
}
