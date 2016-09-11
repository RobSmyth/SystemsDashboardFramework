using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Registries;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.PropertyEditControls
{
    public class AutoCompleteTextPropertyViewProvider : IPropertyViewProvider
    {
        public bool CanHandle(string elementType)
        {
            return elementType.Equals("AutoCompleteText", StringComparison.InvariantCulture);
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