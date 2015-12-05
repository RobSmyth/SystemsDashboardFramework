using System.Windows;
using NoeticTools.SystemsDashboard.Framework.Config;


namespace NoeticTools.SystemsDashboard.Framework.Registries
{
    public interface IPropertyViewProvider
    {
        bool CanHandle(string elementType);
        FrameworkElement Create(IPropertyViewModel propertyViewModel, int rowIndex, string elementName);
    }
}