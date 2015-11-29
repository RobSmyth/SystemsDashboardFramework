using System.Windows;
using NoeticTools.Dashboard.Framework.Config;


namespace NoeticTools.Dashboard.Framework.Tiles
{
    public interface IViewController : IConfigurationChangeListener
    {
        FrameworkElement CreateView();
    }
}