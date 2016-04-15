using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Plugins.PropertyEditControls;
using NoeticTools.TeamStatusBoard.Framework.Registries;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.PropertyEditControls
{
    public class DatePropertyViewProvider : IPropertyViewProvider
    {
        public bool CanHandle(string elementType)
        {
            return elementType.Equals("DateTime", StringComparison.InvariantCulture);
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

            var binding = new Binding("Value") {Converter = new UtcDateTimeValueConverter(), StringFormat = "d", ConverterCulture = CultureInfo.CurrentCulture};
            BindingOperations.SetBinding(textbox, TextBox.TextProperty, binding);

            return textbox;
        }
    }
}