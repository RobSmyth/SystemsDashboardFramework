using System;
using System.Windows.Input;


namespace NoeticTools.TeamStatusBoard.Framework.Commands
{
    public class CommandConduit : ICommand
    {
        private ICommand _inner;

        public CommandConduit(bool canExecute)
        {
            _inner = new NullCommand(canExecute);
        }

        public void SetTarget(ICommand inner)
        {
            _inner = inner;
        }

        public bool CanExecute(object parameter)
        {
            return _inner.CanExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _inner.Execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { _inner.CanExecuteChanged += value; }
            remove { _inner.CanExecuteChanged -= value; }
        }
    }
}