using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IEnumerable<IElementViewModel> _parameters;

        public ParametersConfigControl()
        {
            InitializeComponent();
        }

        public ParametersConfigControl(RoutedCommands commands, IEnumerable<IElementViewModel> parameters) : this()
        {
            CommandBindings.Add(commands.SaveCommandBinding);
            commands.SaveCommandBinding.Executed += SaveCommandBinding_Executed;

            _parameters = parameters.ToArray();
            foreach (var parameter in _parameters)
            {
                Add(parameter);
            }
        }

        private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            foreach (var parameter in _parameters)
            {
                SetParameterValue(parameter);
            }
        }

        private void SetParameterValue(IElementViewModel parameter)
        {
            var name = GetUIlementName(parameter);
            if (parameter.ElementType == ElementType.Boolean)
            {
                var checkbox = (CheckBox)PlaceholderGrid.Children.Cast<FrameworkElement>().Single(x => x.Name.Equals(name));
                parameter.Value = checkbox.IsChecked;
            }
            else
            {
                var textBox = (TextBox)PlaceholderGrid.Children.Cast<FrameworkElement>().Single(x => x.Name.Equals(name));
                parameter.Value = textBox.Text;
            }
        }

        private static string GetUIlementName(IElementViewModel parameter)
        {
            return $"Param_{parameter.Name}";
        }

        private void Add(IElementViewModel parameter)
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

            var creatorLookup = new Dictionary<ElementType, Func<IElementViewModel, UIElement>>
            {
                {ElementType.Boolean, CreateCheckBox },
                {ElementType.Text, CreateTextBox },
                {ElementType.DateTime, CreateTextBox },
                {ElementType.Hyperlink, CreateHyperlink },
            };

            Add(rowIndex, creatorLookup[parameter.ElementType](parameter));
        }

        private int AddRow()
        {
            var rowIndex = PlaceholderGrid.RowDefinitions.Count;
            PlaceholderGrid.RowDefinitions.Add(new RowDefinition());
            return rowIndex;
        }

        private UIElement CreateHyperlink(IElementViewModel parameter)
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

        private UIElement CreateCheckBox(IElementViewModel parameter)
        {
            return new CheckBox
            {
                IsChecked = (bool)parameter.Value, // todo - use binding
                Margin = new Thickness(5, 5, 5, 5),
                Name = GetUIlementName(parameter)
            };
        }

        private UIElement CreateTextBox(IElementViewModel parameter)
        {
            return new TextBox
            {
                Text = (string)parameter.Value, // todo - use binding
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(5, 5, 5, 5),
                FontSize = 12.0,
                Name = GetUIlementName(parameter)
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