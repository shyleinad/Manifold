using Manifold.DomainLayer.Entities;

namespace Maninfold.UILayer.ViewModels
{
    public class ValveViewModel : NotifyPropertyChanged
    {
        public string Id { get; }
        public double X { get; }
        public double Y { get; }
        public Valve Model { get; }

        public ValveViewModel(Valve model, double x, double y)
        {
            Model = model; Id = model.Id; X = x; Y = y;
        }
    }
}
