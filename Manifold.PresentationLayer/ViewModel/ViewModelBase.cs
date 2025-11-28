using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Manifold.PresentationLayer.ViewModel;
public class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void Raise([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
