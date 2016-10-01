using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Config.Views;
using NoeticTools.TeamStatusBoard.Framework.Registries;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.PropertyEditControls.Text
{
    public class AutoCompleteTextPropertyViewProvider : IPropertyViewProvider
    {
        public bool CanHandle(PropertyType propertyType)
        {
            return propertyType == PropertyType.AutoCompleteText;
        }

        public FrameworkElement Create(IPropertyViewModel viewModel, int rowIndex, string elementName)
        {
            var view = new AutoCompletePropertyTextBox(viewModel);
            BindingOperations.SetBinding(view.comboBox, ComboBox.ItemsSourceProperty, new Binding("Parameters"));
            return view;
        }
    }
}