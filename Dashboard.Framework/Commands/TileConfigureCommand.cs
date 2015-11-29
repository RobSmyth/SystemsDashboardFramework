using System;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Config.Controllers;


namespace NoeticTools.Dashboard.Framework.Commands
{
    public class TileConfigureCommand : ICommand
    {
        private readonly IElementViewModel[] _parameters;
        private readonly IDashboardController _dashboardController;
        private readonly string _title;
        private bool _canExecute = true;

        public TileConfigureCommand(string title, IElementViewModel[] parameters, IDashboardController dashboardController)
        {
            _title = title;
            _parameters = parameters;
            _dashboardController = dashboardController;
        }

        public bool CanExecute(object parameter)
        {
            if (parameter is bool)
            {
                _canExecute = (bool) parameter;
                OnChanExecuteChanged();
            }
            return _canExecute;
        }

        public void Execute(object parameter)
        {
            var controller = new ConfigationViewController(_title, _parameters);
            _dashboardController.ShowOnSidePane(controller, _title);
        }

        private void OnChanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            handler?.Invoke(this, new EventArgs());
        }

        public event EventHandler CanExecuteChanged;
    }
}