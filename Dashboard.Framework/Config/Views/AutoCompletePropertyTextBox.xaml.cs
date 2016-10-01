using System;
using System.Windows.Controls;


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
            comboBox.Text = (string)viewModel.Value;
            comboBox.GotFocus += (a, b) => comboBox.IsDropDownOpen = false;
            comboBox.LostFocus += (a, b) => viewModel.Value = comboBox.Text;
        }
    }
}
