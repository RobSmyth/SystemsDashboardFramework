using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Registries;


namespace NoeticTools.Dashboard.Framework.Plugins.PropertyEditControls
{
    public class DatePropertyViewProvider : IPropertyViewProvider
    {
        public bool CanHandle(string elementType)
        {
            return elementType.Equals("DateTime", StringComparison.InvariantCulture);
        }

        public FrameworkElement Create(IPropertyViewModel propertyViewModel, int rowIndex, string elementName)
        {
            var textbox = new TextBox
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                FontSize = 12.0,
                Name = elementName,
                DataContext = propertyViewModel
            };

            var binding = new Binding("Value") {Converter = new UtcDateTimeValueConverter(), StringFormat = "d", ConverterCulture = CultureInfo.CurrentCulture};
            BindingOperations.SetBinding(textbox, TextBox.TextProperty, binding);

            return textbox;
        }
    }
}