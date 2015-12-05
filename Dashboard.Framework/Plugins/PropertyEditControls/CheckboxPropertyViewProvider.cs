using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Registries;


namespace NoeticTools.Dashboard.Framework.Plugins.PropertyEditControls
{
    public class CheckboxPropertyViewProvider : IPropertyViewProvider
    {
        public bool CanHandle(string elementType)
        {
            return elementType == "Checkbox";
        }

        public FrameworkElement Create(IPropertyViewModel propertyViewModel, int rowIndex, string elementName)
        {
            var checkbox = new CheckBox
            {
                IsChecked = bool.Parse((string)propertyViewModel.Value),
                Name = elementName,
                DataContext = propertyViewModel
            };

            var binding = new Binding("Value");
            BindingOperations.SetBinding(checkbox, CheckBox.IsCheckedProperty, binding);

            return checkbox;
        }
    }
}