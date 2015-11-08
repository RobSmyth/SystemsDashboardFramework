using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Commands;
using NoeticTools.Dashboard.Framework.Config.Parameters;


namespace NoeticTools.Dashboard.Framework.Config.Views
{
    public partial class ParametersConfigControl : UserControl
    {
        private readonly TileConfigurationConverter _tileConfigurationConverter;

        public ParametersConfigControl()
        {
            InitializeComponent();
        }

        public ParametersConfigControl(RoutedCommands commands, IEnumerable<IConfigurationElement> parameters, TileConfigurationConverter tileConfigurationConverter) : this()
        {
            _tileConfigurationConverter = tileConfigurationConverter;
            foreach (var parameter in parameters)
            {
                Add(parameter);
            }

            CommandBindings.Add(commands.SaveCommandBinding);
        }

        private void Add(IConfigurationElement parameter)
        {
            var rowIndex = AddRow();

            var textBlock = new TextBlock
            {
                Text = Name.Replace('_', ' '),
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(5, 5, 5, 5),
                FontSize = 12.0
            };

            Grid.SetRow(textBlock, rowIndex);
            Grid.SetColumn(textBlock, 0);
            PlaceholderGrid.Children.Add(textBlock);

            var creatorLookup = new Dictionary<ElementType, Func<IConfigurationElement, UIElement>>
            {
                {ElementType.Boolean, CreateCheckBox },
                {ElementType.Text, CreateTextBox },
                {ElementType.DateTime, CreateTextBox },
                {ElementType.Hyperlink, CreateHyperlink },
            };

            Add(rowIndex, creatorLookup[parameter.ValueType](parameter));
        }

        private int AddRow()
        {
            var rowIndex = PlaceholderGrid.RowDefinitions.Count;
            PlaceholderGrid.RowDefinitions.Add(new RowDefinition());
            return rowIndex;
        }

        private UIElement CreateHyperlink(IConfigurationElement parameter)
        {
            var hyperlink = new Hyperlink { Command = (ICommand)parameter.Parameters[1] };
            hyperlink.Inlines.Add((string)parameter.Parameters[0]);

            var textBox = new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(5, 5, 5, 5),
                FontSize = 12.0
            };
            textBox.Inlines.Add(hyperlink);
            return textBox;
        }

        private UIElement CreateCheckBox(IConfigurationElement parameter)
        {
            return new CheckBox
            {
                IsChecked = _tileConfigurationConverter.GetBool(Name),
                Margin = new Thickness(5, 5, 5, 5),
                Name = $"Param_{Name}"
            };
        }

        private UIElement CreateTextBox(IConfigurationElement parameter)
        {
            return new TextBox
            {
                Text = _tileConfigurationConverter.GetParameterValueText(parameter),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(5, 5, 5, 5),
                FontSize = 12.0,
                Name = $"Param_{Name}"
            };
        }

        private void Add(int rowIndex, UIElement checkBox)
        {
            Grid.SetRow(checkBox, rowIndex);
            Grid.SetColumn(checkBox, 1);
            PlaceholderGrid.Children.Add(checkBox);
        }
    }
}