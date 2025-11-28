using Manifold.DomainLayer.Enums;

namespace Manifold.DomainLayer.Entities;
public class Pipe : GraphElement
{
    public GraphElement From { get; set; }

    public GraphElement To { get; set; }

    public ContentTypes Content { get; set; } = ContentTypes.Air;

    public Pipe(string id, GraphElement from, GraphElement to)
        : base(id)
    {
        this.From = from;
        this.To = to;
    }

    public bool Connects(GraphElement a, GraphElement b)
    {
        return (From == a && To == b) || (From == b && To == a);
    }

    public GraphElement GetNext(GraphElement n)
    {
        if (n == From) return To;
        if (n == To) return From;
        throw new InvalidOperationException("Node is not connected to this pipe.");
    }

}
