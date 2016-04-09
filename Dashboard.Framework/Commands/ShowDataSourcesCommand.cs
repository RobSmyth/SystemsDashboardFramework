using System;
using System.Windows.Input;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Config.Controllers;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Commands
{
    public class ShowDataSourcesCommand : ICommand
    {
        private readonly IDashboardController _dashboardController;

        public ShowDataSourcesCommand(IDashboardController dashboardController)
        {
            _dashboardController = dashboardController;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var commands = new TsbCommands();
            commands.ShowDataSourcesBinding.PreviewCanExecute += (x, y) =>
            {
                y.CanExecute = false;
                y.Handled = true;
            };
            var controller = new DataSourcesViewController(commands);
            _dashboardController.ShowOnSidePane(controller.CreateView(), "Data Sources");
        }

        public event EventHandler CanExecuteChanged;
    }
}