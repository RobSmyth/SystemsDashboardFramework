using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;


namespace NoeticTools.Dashboard.Framework.Config.Parameters
{
    public class ConfigurationElement : ConfigurationElementBase, IConfigurationParameter, IConfigurationElement
    {
        private readonly TileConfigurationConverter _tileConfigurationConverter;

        public ConfigurationElement(string name, ElementType valueType, TileConfigurationConverter tileConfigurationConverter)
        {
            _tileConfigurationConverter = tileConfigurationConverter;
            Name = name;
            ValueType = valueType;
            Parameters = new object[0];
        }

        public string Name { get; set; }
        public ElementType ValueType { get; set; }
        public object[] Parameters { get; }

        public void Save(Panel parametersPanel)
        {
            var name = $"Param_{Name}";
            if (ValueType == ElementType.Boolean)
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
    }
}