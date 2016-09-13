using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using NoeticTools.TeamStatusBoard.Framework.Config;
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
            var comboBox = new ComboBox
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                FontSize = 12.0,
                Name = elementName.Replace('.','_'),
                DataContext = viewModel
            };

            BindingOperations.SetBinding(comboBox, ComboBox.SelectedItemProperty, new Binding("Value"));
            BindingOperations.SetBinding(comboBox, ComboBox.ItemsSourceProperty, new Binding("Parameters"));

            comboBox.SelectionChanged += (a, b) => viewModel.Value = comboBox.SelectedItem;

            return comboBox;
        }
    }
}