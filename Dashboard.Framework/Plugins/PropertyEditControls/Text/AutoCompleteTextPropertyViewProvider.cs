using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Registries;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.PropertyEditControls.Text
{
    public class AutoCompleteTextPropertyViewProvider : IPropertyViewProvider
    {
        public bool CanHandle(PropertyType propertyType)
        {
            return propertyType == PropertyType.AutoCompleteText;
        }

        public FrameworkElement Create(IPropertyViewModel viewModel, int rowIndex, string elementName)
        {
            var comboBox = new ComboBox
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                FontSize = 12.0,
                Name = elementName.Replace('.', '_'),
                DataContext = viewModel,
                IsEditable = true,
                IsTextSearchEnabled = true,
                IsTextSearchCaseSensitive = false,
                StaysOpenOnEdit = true,
                
                Text = (string)viewModel.Value,
            };
            comboBox.GotFocus += (a, b) => comboBox.IsDropDownOpen = true;
            comboBox.LostFocus += (a, b) => viewModel.Value = comboBox.Text;

            BindingOperations.SetBinding(comboBox, ComboBox.ItemsSourceProperty, new Binding("Parameters"));

            return comboBox;
        }
    }
}