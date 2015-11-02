using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace NoeticTools.Dashboard.Framework.Config.Parameters
{
    public class ConfigurationParameter : ConfigurationElementBase, IConfigurationParameter, IConfigurationView
    {
        private readonly TileConfigurationConverter _tileConfigurationConverter;

        public ConfigurationParameter(string name, object defaultValue, TileConfigurationConverter tileConfigurationConverter)
        {
            _tileConfigurationConverter = tileConfigurationConverter;
            Name = name;
            DefaultValue = defaultValue;
        }

        public string Name { get; set; }
        public object DefaultValue { get; set; }

        public void Show(Grid parametersGrid, TileConfigurationConverter tileConfigurationConverter)
        {
            var rowIndex = AddRow(parametersGrid);

            var textBlock = new TextBlock
            {
                Text = Name.Replace('_', ' '),
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(5, 5, 5, 5),
                FontSize = 12.0
            };
            Grid.SetRow(textBlock, rowIndex);
            Grid.SetColumn(textBlock, 0);
            parametersGrid.Children.Add(textBlock);

            if (DefaultValue is bool)
            {
                AddCheckBox(parametersGrid, tileConfigurationConverter, rowIndex);
            }
            else
            {
                AddTextBox(parametersGrid, tileConfigurationConverter, rowIndex);
            }
        }

        public void Save(Panel parametersPanel)
        {
            var name = $"Param_{Name}";
            if (DefaultValue is bool)
            {
                var checkbox =
                    (CheckBox)
                        parametersPanel.Children.Cast<FrameworkElement>()
                            .Single(x => x.Name.Equals(name));
                _tileConfigurationConverter.SetParameter(Name, checkbox.IsChecked);
            }
            else
            {
                var textBox =
                    (TextBox)
                        parametersPanel.Children.Cast<FrameworkElement>()
                            .Single(x => x.Name.Equals(name));
                _tileConfigurationConverter.SetParameterValue(this, textBox.Text);
            }
        }

        private void AddCheckBox(Grid parametersGrid, TileConfigurationConverter tileConfigurationConverter, int rowIndex)
        {
            var checkBox = new CheckBox
            {
                IsChecked = tileConfigurationConverter.GetBool(Name),
                Margin = new Thickness(5, 5, 5, 5),
                Name = $"Param_{Name}"
            };
            Grid.SetRow(checkBox, rowIndex);
            Grid.SetColumn(checkBox, 1);
            parametersGrid.Children.Add(checkBox);
        }

        private void AddTextBox(Grid parametersGrid, TileConfigurationConverter tileConfigurationConverter, int rowIndex)
        {
            var textBox = new TextBox
            {
                Text = tileConfigurationConverter.GetParameterValueText(this),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(5, 5, 5, 5),
                FontSize = 12.0,
                Name = $"Param_{Name}"
            };
            Grid.SetRow(textBox, rowIndex);
            Grid.SetColumn(textBox, 1);
            parametersGrid.Children.Add(textBox);
        }
    }
}