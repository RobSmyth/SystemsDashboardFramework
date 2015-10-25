using System;
using System.Collections.Generic;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Config.Parameters;
using NoeticTools.Dashboard.Framework.Config.ViewModels;

namespace NoeticTools.Dashboard.Framework.Config.Commands
{
    public class TileConfigureCommand : ICommand
    {
        private readonly IEnumerable<IConfigurationView> _parameters;
        private readonly TileConfiguration _tileConfiguration;
        private readonly string _title;
        private bool _canExecute = true;

        public TileConfigureCommand(string title, TileConfiguration tileConfiguration,
            IEnumerable<IConfigurationView> parameters)
        {
            _title = title;
            _tileConfiguration = tileConfiguration;
            _parameters = parameters;
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
            var viewModel = new TileConfigationViewModel(_title, _tileConfiguration, _parameters);
            viewModel.Show();
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