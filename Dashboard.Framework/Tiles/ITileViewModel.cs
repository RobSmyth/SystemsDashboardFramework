using System.Windows;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Config;


namespace NoeticTools.Dashboard.Framework.Tiles
{
    public interface ITileViewModel : IConfigurationChangeListener
    {
        ICommand ConfigureCommand { get; }
        FrameworkElement CreateView();
    }
}