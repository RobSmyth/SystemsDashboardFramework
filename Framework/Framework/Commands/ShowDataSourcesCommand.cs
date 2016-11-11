using System;
using System.Windows.Input;
using NoeticTools.TeamStatusBoard.Framework.Config.Controllers;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Commands
{
    public class ShowDataSourcesCommand : ICommand
    {
        private readonly IServices _services;

        public ShowDataSourcesCommand(IServices services)
        {
            _services = services;
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
            var controller = new DataSourcesViewProvider(commands, _services);
            _services.DashboardController.ShowOnSidePane(controller.CreateView(), "Data Sources");
        }

        public event EventHandler CanExecuteChanged;
    }
}