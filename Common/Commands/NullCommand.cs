using System;
using System.Windows.Input;


namespace NoeticTools.TeamStatusBoard.Common.Commands
{
    public class NullCommand : ICommand
    {
        private readonly bool _canExecute;

        public NullCommand(bool canExecute = false)
        {
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public void Execute(object parameter)
        {
        }

        public event EventHandler CanExecuteChanged;
    }
}