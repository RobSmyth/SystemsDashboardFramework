using System;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Config.ViewModels;


namespace NoeticTools.Dashboard.Framework.Commands
{
    public class TileConfigureCommand : ICommand
    {
        private readonly IConfigurationView[] _parameters;
        private readonly IDashboardController _dashboardController;
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private readonly string _title;
        private bool _canExecute = true;

        public TileConfigureCommand(string title, TileConfigurationConverter tileConfigurationConverter, IConfigurationView[] parameters, IDashboardController dashboardController)
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
            var viewModel = new ConfigationViewController(_title, _tileConfigurationConverter, _parameters);
            _dashboardController.ShowOnSidePane(viewModel, _title);
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