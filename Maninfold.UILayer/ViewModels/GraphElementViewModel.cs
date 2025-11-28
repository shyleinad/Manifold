using Manifold.DomainLayer.Entities;
using System.ComponentModel;

namespace Maninfold.UILayer.ViewModels
{
    public class GraphElementViewModel : INotifyPropertyChanged
    {
        public string Id { get; }
        public double X { get; set; }
        public double Y { get; set; }
        public string ShortInfo { get; set; } = string.Empty;
        public GraphElement Element { get; }

        public GraphElementViewModel(GraphElement e)
        {
            Element = e;
            Id = e.Id;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void Notify(string p) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
    }
}
