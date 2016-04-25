using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Views
{
    public partial class ParametersConfigControl : UserControl
    {
        private readonly IServices _services;
        private readonly IEnumerable<IPropertyViewModel> _elementViewModels;
        private readonly Thickness _elementMargin = new Thickness(5, 3, 5, 3);

        public ParametersConfigControl()
        {
            InitializeComponent();
        }

        public ParametersConfigControl(TsbCommands commandsBindings, IEnumerable<IPropertyViewModel> elementViewModels, IServices services) : this()
        {
            _services = services;
            CommandBindings.Add(commandsBindings.SaveCommandBinding);
            commandsBindings.SaveCommandBinding.Executed += SaveCommandBinding_Executed;

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

        private void SetParameterValue(IPropertyViewModel propertyViewModel)
        {
            if (propertyViewModel.ViewerName == "Hyperlink" || propertyViewModel.ViewerName == "Divider")
            {
                return;
            }

            var name = GetUIlementName(propertyViewModel);
            if (propertyViewModel.ViewerName == "Checkbox")
            {
                //var checkbox = (CheckBox) PlaceholderGrid.Children.Cast<FrameworkElement>().Single(x => x.Name.Equals(name));
                //propertyViewModel.Value = checkbox.IsChecked;
            }
            if (propertyViewModel.ViewerName == "TextFromCombobox")
            {
                var combobox = (ComboBox) PlaceholderGrid.Children.Cast<FrameworkElement>().Single(x => x.Name.Equals(name));
                propertyViewModel.Value = combobox.SelectedValue;
            }
        }

        private static string GetUIlementName(IPropertyViewModel propertyViewModel)
        {
            return $"Param_{propertyViewModel.Name.Replace(' ', '_')}";
        }

        private void Add(IPropertyViewModel propertyViewModel)
        {
            var rowIndex = AddRow();

            if (!string.IsNullOrWhiteSpace(propertyViewModel.Name))
            {
                var textBlock = new TextBlock
                {
                    Text = propertyViewModel.Name.Replace('_', ' '),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = _elementMargin,
                    FontSize = 12.0
                };

                PlaceholderGrid.Children.Add(textBlock);
                Grid.SetRow(textBlock, rowIndex);
                Grid.SetColumn(textBlock, 0);
            }

            var creatorLookup = new Dictionary<string, Func<IPropertyViewModel, int, UIElement>>
            {
                {"Hyperlink", CreateHyperlink},
                {"Divider", CreateDivider},
                {"Password", CreatePasswordBox}
            };

            if (creatorLookup.ContainsKey(propertyViewModel.ViewerName))
            {
                Add(rowIndex, creatorLookup[propertyViewModel.ViewerName](propertyViewModel, rowIndex));
            }
            else
            {
                var element = _services.PropertyEditControlProviders.Get(propertyViewModel.ViewerName).Create(propertyViewModel, rowIndex, GetUIlementName(propertyViewModel));
                element.Margin = _elementMargin;
                Add(rowIndex, element);
            }
        }

        private int AddRow()
        {
            var rowIndex = PlaceholderGrid.RowDefinitions.Count;
            PlaceholderGrid.RowDefinitions.Add(new RowDefinition {MinHeight = 5});
            return rowIndex;
        }

        private UIElement CreateDivider(IPropertyViewModel propertyViewModel, int rowIndex)
        {
            PlaceholderGrid.RowDefinitions[rowIndex].MinHeight = 15;
            return null;
        }

        private UIElement CreateHyperlink(IPropertyViewModel propertyViewModel, int rowIndex)
        {
            var hyperlink = new Hyperlink {Command = (ICommand) propertyViewModel.Parameters[1]};
            hyperlink.Inlines.Add((string) propertyViewModel.Parameters[0]);

            var textBox = new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = _elementMargin,
                FontSize = 12.0
            };
            textBox.Inlines.Add(hyperlink);
            return textBox;
        }

        private UIElement CreatePasswordBox(IPropertyViewModel propertyViewModel, int rowIndex)
        {
            var passwordBox = new PasswordBox
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = _elementMargin,
                FontSize = 12.0,
                Name = GetUIlementName(propertyViewModel),
                DataContext = propertyViewModel,
                Password = (string) propertyViewModel.Value
            };

            return passwordBox;
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