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
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private readonly string _title;
        private bool _canExecute = true;

        public TileConfigureCommand(string title, TileConfigurationConverter tileConfigurationConverter, IElementViewModel[] parameters, IDashboardController dashboardController)
        {
            _title = title;
            _tileConfigurationConverter = tileConfigurationConverter;
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
            var controller = new ConfigationViewController(_title, _tileConfigurationConverter, _parameters);
            _dashboardController.ShowOnSidePane(controller, _title);
        }

        public event EventHandler CanExecuteChanged;

        private void OnChanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }
    }
}