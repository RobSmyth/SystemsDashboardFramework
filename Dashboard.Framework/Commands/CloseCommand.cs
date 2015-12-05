using System;
using System.Windows;
using System.Windows.Input;


namespace NoeticTools.SystemsDashboard.Framework.Commands
{
    public class CloseCommand : ICommand
    {
        private readonly UIElement _view;

        public CloseCommand(UIElement view)
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