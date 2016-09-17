using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Registries;
using Binding = System.Windows.Data.Binding;
using ComboBox = System.Windows.Controls.ComboBox;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using Orientation = System.Windows.Controls.Orientation;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.PropertyEditControls.Text
{
    public class CompoundAutoCompleteTextPropertyViewProvider : IPropertyViewProvider
    {
        public bool CanHandle(PropertyType propertyType)
        {
            return propertyType == PropertyType.CompoundAutoCompleteText;
        }

        public FrameworkElement Create(IPropertyViewModel viewModel, int rowIndex, string elementName)
        {
            var upperPanel = new StackPanel() { HorizontalAlignment = HorizontalAlignment.Stretch, Orientation = Orientation.Vertical };
            upperPanel.Children.Add(CreateComboBox(viewModel, "A"));
            upperPanel.Children.Add(CreateComboBox(viewModel, "B"));

            var panel = new StackPanel() { HorizontalAlignment = HorizontalAlignment.Stretch, Orientation = Orientation.Vertical };
            panel.Children.Add(upperPanel);
            panel.Children.Add(new Button() { Content = "Add", HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(1), Padding = new Thickness(4,1,4,1)});

            return new Border() {BorderThickness = new Thickness(1), BorderBrush = System.Windows.Media.Brushes.Gray, Child = panel, Padding = new Thickness(1)};
        }

        private static FrameworkElement CreateComboBox(IPropertyViewModel viewModel, string elementName)
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
                Text = (string) viewModel.Value,
                Margin = new Thickness(1)
            };
            comboBox.GotFocus += (a, b) => comboBox.IsDropDownOpen = false;
            comboBox.LostFocus += (a, b) => viewModel.Value = comboBox.Text;

            BindingOperations.SetBinding(comboBox, ComboBox.ItemsSourceProperty, new Binding("Parameters"));


            return comboBox;
        }
    }
}