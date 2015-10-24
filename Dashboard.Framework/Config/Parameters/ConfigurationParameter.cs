using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Dashboard.Config.Parameters
{
    public class ConfigurationParameter : ConfigurationElementBase, IConfigurationParameter, IConfigurationView
    {
        private readonly TileConfiguration _tileConfiguration;

        public ConfigurationParameter(string name, object defaultValue, TileConfiguration tileConfiguration)
        {
            _tileConfiguration = tileConfiguration;
            Name = name;
            DefaultValue = defaultValue;
        }

        public string Name { get; set; }
        public object DefaultValue { get; set; }

        public void Show(Grid parametersGrid, TileConfiguration tileConfiguration)
        {
            int rowIndex = AddRow(parametersGrid);

            var textBlock = new TextBlock
            {
                Text = Name.Replace('_', ' '),
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(10, 5, 10, 5),
                FontSize = 12.0
            };
            Grid.SetRow(textBlock, rowIndex);
            Grid.SetColumn(textBlock, 0);
            parametersGrid.Children.Add(textBlock);

            if (DefaultValue is bool)
            {
                AddCheckBox(parametersGrid, tileConfiguration, rowIndex);
            }
            else
            {
                AddTextBox(parametersGrid, tileConfiguration, rowIndex);
                
            }
        }

        private void AddCheckBox(Grid parametersGrid, TileConfiguration tileConfiguration, int rowIndex)
        {
            var checkBox = new CheckBox()
            {
                IsChecked = tileConfiguration.GetBool(Name),
                Margin = new Thickness(10, 5, 10, 5),
                Name = string.Format("Param_{0}", Name),
            };
            Grid.SetRow(checkBox, rowIndex);
            Grid.SetColumn(checkBox, 1);
            parametersGrid.Children.Add(checkBox);
        }

        private void AddTextBox(Grid parametersGrid, TileConfiguration tileConfiguration, int rowIndex)
        {
            var textBox = new TextBox
            {
                Text = tileConfiguration.GetParameterValueText(this),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(10, 5, 10, 5),
                FontSize = 12.0,
                Name = string.Format("Param_{0}", Name),
            };
            Grid.SetRow(textBox, rowIndex);
            Grid.SetColumn(textBox, 1);
            parametersGrid.Children.Add(textBox);
        }

        public void Save(Grid parametersGrid)
        {
            var name = string.Format("Param_{0}", Name);
            if (DefaultValue is bool)
            {
                var checkbox =
                    (CheckBox)
                        parametersGrid.Children.Cast<FrameworkElement>()
                            .Single<FrameworkElement>(x => x.Name.Equals(name));
                _tileConfiguration.SetParameter(Name, checkbox.IsChecked);
            }
            else
            {
                var textBox =
                    (TextBox)
                        parametersGrid.Children.Cast<FrameworkElement>()
                            .Single<FrameworkElement>(x => x.Name.Equals(name));
                _tileConfiguration.SetParameterValue(this, textBox.Text);
            }
        }
    }
}