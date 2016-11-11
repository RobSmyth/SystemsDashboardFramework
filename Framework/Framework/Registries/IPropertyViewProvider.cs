using System.Windows;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;


namespace NoeticTools.TeamStatusBoard.Framework.Registries
{
    public interface IPropertyViewProvider
    {
        bool CanHandle(PropertyType propertyType);
        FrameworkElement Create(IPropertyViewModel viewModel, int rowIndex, string elementName);
    }
}