using System;
using System.Windows.Input;

namespace Dashboard.Framework.Config.Commands
{
    public class NullCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return false;
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }

        public event EventHandler CanExecuteChanged;
    }
}