using System;
using System.Windows;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Config.Views;

namespace NoeticTools.Dashboard.Framework.Config.Commands
{
    public class CloseCommand : ICommand
    {
        private readonly TileConfigurationControl _view;

        public CloseCommand(TileConfigurationControl view)
        {
            _view = view;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _view.Visibility = Visibility.Collapsed;
        }

        public event EventHandler CanExecuteChanged;
    }
}