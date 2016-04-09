using System;
using System.Windows.Input;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Commands
{
    public class ShowDataSourcesCommand : ICommand
    {
        private readonly IDashboardController _dashboardController;
        private readonly ITileLayoutController _tileLayoutController;
        private readonly IServices _services;
        private readonly TileConfiguration _tile;
        private readonly string _title;
        private bool _canExecute = true;

        public ShowDataSourcesCommand(TileConfiguration tile, string title, IDashboardController dashboardController, ITileLayoutController tileLayoutController, IServices services)
        {
            _tile = tile;
            _title = title;
            _dashboardController = dashboardController;
            _tileLayoutController = tileLayoutController;
            _services = services;
        }

        public bool CanExecute(object parameter)
        {
            if (parameter is bool)
            {
                _canExecute = (bool)parameter;
                OnChanExecuteChanged();
            }
            return _canExecute;
        }

        public void Execute(object parameter)
        {
            //var routedCommands = new ApplicationCommandsBindings();
            //routedCommands.DeleteCommandBinding.Executed += DeleteCommandBinding_Executed;
            //var controller = new DataSourcesViewController(_title, routedCommands, _services);
            //_dashboardController.ShowOnSidePane(controller.CreateView(), _title);
        }

        private void DeleteCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _tileLayoutController.Remove(_tile);
        }

        private void OnChanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            handler?.Invoke(this, new EventArgs());
        }

        public event EventHandler CanExecuteChanged;
    }
}