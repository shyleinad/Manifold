namespace Manifold.DomainLayer.Entities;
public class GraphElement
{
    public string Id { get; set; }

    public List<Pipe> ConnectedPipes { get; set; } = new();

    public GraphElement(string id)
    {
        this.Id = id;
    }
}
