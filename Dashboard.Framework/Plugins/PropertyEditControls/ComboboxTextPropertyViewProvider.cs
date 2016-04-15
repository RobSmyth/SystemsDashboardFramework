using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Registries;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.PropertyEditControls
{
    public class ComboboxTextPropertyViewProvider : IPropertyViewProvider
    {
        public bool CanHandle(string elementType)
        {
            return elementType.Equals("TextFromCombobox", StringComparison.InvariantCulture);
        }

        public FrameworkElement Create(IPropertyViewModel viewModel, int rowIndex, string elementName)
        {
            var textbox = new ComboBox
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                FontSize = 12.0,
                Name = elementName,
                DataContext = viewModel
            };

            BindingOperations.SetBinding(textbox, ComboBox.SelectedItemProperty, new Binding("Value"));
            BindingOperations.SetBinding(textbox, ComboBox.ItemsSourceProperty, new Binding("Parameters"));

            return textbox;
        }
    }
}