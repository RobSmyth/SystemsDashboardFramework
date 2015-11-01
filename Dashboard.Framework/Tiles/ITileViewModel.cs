using System.Windows;
using System.Windows.Input;
using Dashboard.Config;

namespace NoeticTools.Dashboard.Framework.Tiles
{
    public interface ITileViewModel : IConfigurationChangeListener
    {
        FrameworkElement CreateView();
        ICommand ConfigureCommand { get; }
    }
}