using Manifold.DomainLayer.Enums;

namespace Manifold.DomainLayer.Entities;
public class Chamber : GraphElement
{
    public ContentTypes Content { get; set; } = ContentTypes.Air;

    public Chamber(string id)
        : base(id)
    {
    }
}
