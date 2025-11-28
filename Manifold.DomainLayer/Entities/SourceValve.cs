using Manifold.DomainLayer.Enums;

namespace Manifold.DomainLayer.Entities;
public class SourceValve : GraphElement
{
    public ContentTypes Content { get; set; }

    public SourceValve(string id, ContentTypes content)
        : base(id)
    {
        this.Content = content;
    }
}
