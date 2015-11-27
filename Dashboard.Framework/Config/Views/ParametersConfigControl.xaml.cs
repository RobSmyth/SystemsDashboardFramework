using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Commands;
using NoeticTools.Dashboard.Framework.Config.Parameters;


namespace NoeticTools.Dashboard.Framework.Config.Views
{
    public partial class ParametersConfigControl : UserControl
    {
        private readonly IEnumerable<IElementViewModel> _elementViewModels;
        private readonly Thickness _elementMargin = new Thickness(5, 3, 5, 3);

        public ParametersConfigControl()
        {
            InitializeComponent();
        }

        public ParametersConfigControl(RoutedCommands commands, IEnumerable<IElementViewModel> elementViewModels) : this()
        {
            CommandBindings.Add(commands.SaveCommandBinding);
            commands.SaveCommandBinding.Executed += SaveCommandBinding_Executed;

            _elementViewModels = elementViewModels.ToArray();
            foreach (var parameter in _elementViewModels)
            {
                Add(parameter);
            }
        }

        private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            foreach (var elementViewModel in _elementViewModels)
            {
                SetParameterValue(elementViewModel);
            }
        }

        private void SetParameterValue(IElementViewModel elementViewModel)
        {
            var name = GetUIlementName(elementViewModel);
            if (elementViewModel.ElementType == ElementType.Boolean)
            {
                var checkbox = (CheckBox)PlaceholderGrid.Children.Cast<FrameworkElement>().Single(x => x.Name.Equals(name));
                elementViewModel.Value = checkbox.IsChecked;
            }
            else
            {
                var textBox = (TextBox)PlaceholderGrid.Children.Cast<FrameworkElement>().Single(x => x.Name.Equals(name));
                elementViewModel.Value = textBox.Text;
            }
        }

        private static string GetUIlementName(IElementViewModel elementViewModel)
        {
            return $"Param_{elementViewModel.Name}";
        }

        private void Add(IElementViewModel elementViewModel)
        {
            var rowIndex = AddRow();

            if (!string.IsNullOrWhiteSpace(elementViewModel.Name))
            {
                var textBlock = new TextBlock
                {
                    Text = elementViewModel.Name.Replace('_', ' '),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = _elementMargin,
                    FontSize = 12.0
                };

                PlaceholderGrid.Children.Add(textBlock);
                Grid.SetRow(textBlock, rowIndex);
                Grid.SetColumn(textBlock, 0);
            }

            var creatorLookup = new Dictionary<ElementType, Func<IElementViewModel, int, UIElement>>
            {
                {ElementType.Boolean, CreateCheckBox },
                {ElementType.Text, CreateTextBox },
                {ElementType.DateTime, CreateTextBox },
                {ElementType.Hyperlink, CreateHyperlink },
                {ElementType.Divider, CreateDivider },
                {ElementType.SelectedText, CreateComboBox },
            };

            Add(rowIndex, creatorLookup[elementViewModel.ElementType](elementViewModel, rowIndex));
        }

        private int AddRow()
        {
            var rowIndex = PlaceholderGrid.RowDefinitions.Count;
            PlaceholderGrid.RowDefinitions.Add(new RowDefinition() {MinHeight = 5});
            return rowIndex;
        }

        private UIElement CreateDivider(IElementViewModel elementViewModel, int rowIndex)
        {
            PlaceholderGrid.RowDefinitions[rowIndex].MinHeight = 15;
            return null;
        }

        private UIElement CreateHyperlink(IElementViewModel elementViewModel, int rowIndex)
        {
            var hyperlink = new Hyperlink { Command = (ICommand)elementViewModel.Parameters[1] };
            hyperlink.Inlines.Add((string)elementViewModel.Parameters[0]);

            var textBox = new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = _elementMargin,
                FontSize = 12.0
            };
            textBox.Inlines.Add(hyperlink);
            return textBox;
        }

        private UIElement CreateCheckBox(IElementViewModel elementViewModel, int rowIndex)
        {
            var checkbox = new CheckBox
            {
                IsChecked = (bool) elementViewModel.Value, // todo - use binding
                Margin = _elementMargin,
                Name = GetUIlementName(elementViewModel),
                DataContext = elementViewModel
            };

            var binding = new Binding("Value");
            BindingOperations.SetBinding(checkbox, CheckBox.IsCheckedProperty, binding);

            return checkbox;
        }

        private UIElement CreateTextBox(IElementViewModel elementViewModel, int rowIndex)
        {
            var textbox = new TextBox
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = _elementMargin,
                FontSize = 12.0,
                Name = GetUIlementName(elementViewModel),
                DataContext = elementViewModel
            };

            var binding = new Binding("Value");
            BindingOperations.SetBinding(textbox, TextBox.TextProperty, binding);

            return textbox;
        }

        private UIElement CreateComboBox(IElementViewModel elementViewModel, int rowIndex)
        {
            var textbox = new ComboBox
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = _elementMargin,
                FontSize = 12.0,
                Name = GetUIlementName(elementViewModel),
                DataContext = elementViewModel
            };

            BindingOperations.SetBinding(textbox, ComboBox.SelectedItemProperty, new Binding("Value"));
            BindingOperations.SetBinding(textbox, ComboBox.ItemsSourceProperty, new Binding("Parameters"));

            return textbox;
        }

        private void Add(int rowIndex, UIElement uiElement)
        {
            if (uiElement == null)
            {
                return;
            }
            PlaceholderGrid.Children.Add(uiElement);
            Grid.SetRow(uiElement, rowIndex);
            Grid.SetColumn(uiElement, 1);
        }
    }
}