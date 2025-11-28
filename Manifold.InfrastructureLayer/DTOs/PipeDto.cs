namespace Manifold.InfrastructureLayer.DTOs;
public class PipeDto : GraphElementDto
{
    public required string From { get; set; }
    public required string To { get; set; }
    public string Content { get; set; } = "Air";
}
