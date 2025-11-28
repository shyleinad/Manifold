namespace Manifold.DomainLayer.Entities;
public class Valve : GraphElement
{
    public List<Pipe> ActivePipes { get; set; } = new();

    public Valve(string id)
        : base(id)
    {
    }

    public void SwitchTo(Pipe pipe)
    {
        ActivePipes.Clear();
        ActivePipes.Add(pipe);
    }
}
