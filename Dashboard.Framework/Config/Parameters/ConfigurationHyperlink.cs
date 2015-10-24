using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Dashboard.Config.Parameters
{
    public class ConfigurationHyperlink : ConfigurationElementBase, IConfigurationView
    {
        private readonly ICommand _command;
        private readonly string _text;

        public ConfigurationHyperlink(string text, ICommand command)
        {
            _text = text;
            _command = command;
        }

        public void Show(Grid parametersGrid, TileConfiguration tileConfiguration)
        {
            int rowIndex = AddRow(parametersGrid);

            var hyperlink = new Hyperlink {Command = _command};
            hyperlink.Inlines.Add(_text);

            var textBox = new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(10, 5, 10, 5),
                FontSize = 12.0,
            };
            textBox.Inlines.Add(hyperlink);

            Grid.SetRow(textBox, rowIndex);
            Grid.SetColumn(textBox, 1);
            parametersGrid.Children.Add(textBox);
        }

        public void Save(Grid parametersGrid)
        {
        }
    }
}