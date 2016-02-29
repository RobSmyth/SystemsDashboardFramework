using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Registries;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.PropertyEditControls
{
    public class TimeSpanPropertyViewProvider : IPropertyViewProvider
    {
        public bool CanHandle(string elementType)
        {
            return elementType.Equals("TimeSpan", StringComparison.InvariantCulture);
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

            var binding = new Binding("Value");
            BindingOperations.SetBinding(textbox, TextBox.TextProperty, binding);

            return textbox;
        }
    }
}