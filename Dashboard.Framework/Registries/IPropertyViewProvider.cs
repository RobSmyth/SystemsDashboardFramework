using System.Windows;
using NoeticTools.Dashboard.Framework.Config;


namespace NoeticTools.Dashboard.Framework.Registries
{
    public interface IPropertyViewProvider
    {
        bool CanHandle(string elementType);
        FrameworkElement Create(IPropertyViewModel propertyViewModel, int rowIndex, string elementName);
    }
}