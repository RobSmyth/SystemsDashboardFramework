using System.Windows;
using NoeticTools.SystemsDashboard.Framework.Config;


namespace NoeticTools.SystemsDashboard.Framework
{
    public interface IViewController : IConfigurationChangeListener
    {
        FrameworkElement CreateView();
    }
}