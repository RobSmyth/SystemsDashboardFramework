using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Registries;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.PropertyEditControls.Text
{
    public class TextPropertyViewProvider : IPropertyViewProvider
    {
        public bool CanHandle(PropertyType propertyType)
        {
            return propertyType == PropertyType.Text;
        }

        public FrameworkElement Create(IPropertyViewModel viewModel, int rowIndex, string elementName)
        {
            var textbox = new TextBox
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                FontSize = 12.0,
                Name = elementName,
                DataContext = viewModel
            };

            var binding = new Binding("Value.String");
            BindingOperations.SetBinding(textbox, TextBox.TextProperty, binding);

            return textbox;
        }
    }
}