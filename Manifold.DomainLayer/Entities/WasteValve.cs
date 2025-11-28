namespace Manifold.DomainLayer.Entities;
public class WasteValve : GraphElement
{
    public bool IsHazardous { get; set; } = true;

    public WasteValve(string id)
        : base(id)
    {
    }
}
