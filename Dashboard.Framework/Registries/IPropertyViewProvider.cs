using System.Windows;
using NoeticTools.TeamStatusBoard.Framework.Config;


namespace NoeticTools.TeamStatusBoard.Framework.Registries
{
    public interface IPropertyViewProvider
    {
        bool CanHandle(string elementType);
        FrameworkElement Create(IPropertyViewModel viewModel, int rowIndex, string elementName);
    }
}