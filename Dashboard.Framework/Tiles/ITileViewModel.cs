using System.Windows;
using Dashboard.Config;

namespace NoeticTools.Dashboard.Framework.Tiles
{
    public interface ITileViewModel : IConfigurationChangeListener
    {
        FrameworkElement CreateView();
    }
}