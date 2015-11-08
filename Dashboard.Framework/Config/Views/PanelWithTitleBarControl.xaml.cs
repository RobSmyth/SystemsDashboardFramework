using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Commands;
using NoeticTools.Dashboard.Framework.Config.ViewModels;


namespace NoeticTools.Dashboard.Framework.Config.Views
{
    public partial class PaneWithTitleBarControl : UserControl
    {
        public PaneWithTitleBarControl()
        {
            InitializeComponent();
        }

        public PaneWithTitleBarControl(PanelWithTitleBarViewModel panelWithTitleBarViewModel, ParametersConfigControl childView) : this()
        {
            PlaceholderGrid.Children.Add(childView);
            DataContext = panelWithTitleBarViewModel;
        }

        private void CloseCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
            //e.Handled = true;
        }
    }
}