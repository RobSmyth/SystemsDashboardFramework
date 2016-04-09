using System.Windows;
using System.Windows.Input;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Config.Views;
using DataSourcesConfigControl = NoeticTools.TeamStatusBoard.Framework.Config.Views.DataSourcesConfigControl;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Controllers
{
    public sealed class DataSourcesViewController : NotifyingViewModelBase
    {
        private readonly TsbCommands _commands;
        private PaneWithTitleBarControl _panelView;

        public DataSourcesViewController(TsbCommands commands)
        {
            _commands = commands;
        }

        public FrameworkElement CreateView()
        {
            _commands.CloseCommandBinding.Executed += CloseCommandBinding_Executed;

            var view = new DataSourcesConfigControl(_commands) { DataContext = this };

            _panelView = new PaneWithTitleBarControl("Data sources", view, _commands)
            {
                Width = double.NaN,
                Height = double.NaN
            };

            return _panelView;
        }

        private void CloseCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _panelView.Visibility = Visibility.Collapsed;
        }
    }
}