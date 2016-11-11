using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Registries;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.PropertyEditControls.Checkbox
{
    public class CheckboxPropertyViewProvider : IPropertyViewProvider
    {
        public bool CanHandle(PropertyType propertyType)
        {
            return propertyType == PropertyType.Checkbox;
        }

        public FrameworkElement Create(IPropertyViewModel viewModel, int rowIndex, string elementName)
        {
            var value = viewModel.Value.String;
            var isChecked = !string.IsNullOrWhiteSpace(value) && bool.Parse(value);
            var checkbox = new CheckBox
            {
                IsChecked = isChecked,
                Name = elementName,
                DataContext = viewModel
            };

            var binding = new Binding("Value");
            BindingOperations.SetBinding(checkbox, CheckBox.IsCheckedProperty, binding);

            return checkbox;
        }
    }
}