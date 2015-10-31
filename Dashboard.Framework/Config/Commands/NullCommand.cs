using System;
using System.Windows.Input;

namespace NoeticTools.Dashboard.Framework.Config.Commands
{
    public class NullCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return false;
        }

        public void Execute(object parameter)
        {
            throw new InvalidOperationException();
        }

        public event EventHandler CanExecuteChanged;
    }
}