using System;
using System.Drawing;
using System.Linq;
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
            var valuesPanel = new StackPanel() { HorizontalAlignment = HorizontalAlignment.Stretch, Orientation = Orientation.Vertical };
            var values = (viewModel.Value.String).Split(',');
            foreach (var value in values)
            {
                valuesPanel.Children.Add(CreatePropertyViewElement(viewModel, value, () => viewModel.Value.String = GetCompountValue(valuesPanel)));
            }

            var panel = new StackPanel() { HorizontalAlignment = HorizontalAlignment.Stretch, Orientation = Orientation.Vertical };
            panel.Children.Add(valuesPanel);
            var addButton = new Button() { Content = "Add", HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(1), Padding = new Thickness(4,1,4,1)};
            addButton.Click += (a,b) => valuesPanel.Children.Add(CreatePropertyViewElement(viewModel, "", () => viewModel.Value.String = GetCompountValue(valuesPanel)));
            panel.Children.Add(addButton);

            return new Border() {BorderThickness = new Thickness(1), BorderBrush = System.Windows.Media.Brushes.Gray, Child = panel, Padding = new Thickness(1)};
        }

        private string GetCompountValue(Panel valuesPanel)
        {
            var values = valuesPanel.Children.Cast<ComboBox>().Select(x => x.Text).Where(y => !string.IsNullOrWhiteSpace(y));
            return string.Join(",", values);
        }

        private static FrameworkElement CreatePropertyViewElement(IPropertyViewModel viewModel, string initialValue, Action onFocusLostAction)
        {
            var comboBox = new ComboBox
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                FontSize = 12.0,
                DataContext = viewModel,
                IsEditable = true,
                IsTextSearchEnabled = true,
                IsTextSearchCaseSensitive = false,
                StaysOpenOnEdit = true,
                Text = initialValue,
                Margin = new Thickness(1)
            };
            comboBox.GotFocus += (a, b) => comboBox.IsDropDownOpen = false;
            comboBox.LostFocus += (a, b) => onFocusLostAction();

            BindingOperations.SetBinding(comboBox, ComboBox.ItemsSourceProperty, new Binding("Parameters"));

            return comboBox;
        }
    }
}