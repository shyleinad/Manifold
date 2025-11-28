using Manifold.DomainLayer.Entities;
using Manifold.DomainLayer.Enums;
using System.Windows.Media;

namespace Maninfold.UILayer.ViewModels
{
    public class ChamberViewModel : NotifyPropertyChanged
    {
        public string Id { get; }
        public double X { get; }
        public double Y { get; }

        public Chamber Model { get; }

        public Brush Brush => Model.Content switch
        {
            ContentTypes.Air => Brushes.LightGray,
            ContentTypes.Alcohol => Brushes.LightGreen,
            ContentTypes.Water => Brushes.LightBlue,
            _ => Brushes.LightGray
        };

        public ChamberViewModel(Chamber model, double x, double y)
        {
            Model = model;
            Id = model.Id;
            X = x; Y = y;
        }

        public void Refresh() => Raise(nameof(Brush));

    }
}
