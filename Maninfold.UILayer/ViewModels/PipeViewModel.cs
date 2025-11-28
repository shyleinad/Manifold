using Manifold.DomainLayer.Entities;
using Manifold.DomainLayer.Enums;
using System.Windows.Media;

namespace Maninfold.UILayer.ViewModels
{
    public class PipeViewModel : NotifyPropertyChanged
    {
        public string Id { get; }
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }

        public Pipe Model { get; }

        public Brush Brush => Model.Content switch
        {
            ContentTypes.Air => Brushes.Orange,
            ContentTypes.Alcohol => Brushes.Green,
            ContentTypes.Water => Brushes.SteelBlue,
            _ => Brushes.LightGray
        };

        public PipeViewModel(Pipe model, double x1, double y1, double x2, double y2)
        {
            Model = model;
            Id = model.Id;
            X1 = x1; Y1 = y1; X2 = x2; Y2 = y2;
        }

        public void Refresh() => Raise(nameof(Brush));
    }
}
