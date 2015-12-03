using System.Windows;
using NoeticTools.Dashboard.Framework.Config;


namespace NoeticTools.Dashboard.Framework
{
    public interface IViewController : IConfigurationChangeListener
    {
        FrameworkElement CreateView();
    }
}