using System;
using System.Windows.Controls;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Config.ViewModels;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Views
{
    public partial class AutoCompletePropertyTextBox : UserControl
    {
        public AutoCompletePropertyTextBox() : this(new NullPropertyViewModel())
        {
        }

        public AutoCompletePropertyTextBox(IPropertyViewModel viewModel)
        {
            InitializeComponent();
            comboBox.DataContext = viewModel;
            comboBox.Text = viewModel.Value.String;
            comboBox.GotFocus += (a, b) => comboBox.IsDropDownOpen = false;
            comboBox.LostFocus += (a, b) =>
            {
                var textProperty = comboBox.SelectedItem as ITextProperty;
                if (textProperty != null)
                {
                    viewModel.Value.String = textProperty.Text;
                }
            };
        }
    }
}
