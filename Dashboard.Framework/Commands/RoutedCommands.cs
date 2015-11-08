using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace NoeticTools.Dashboard.Framework.Commands
{
    public class RoutedCommands
    {
        public readonly CommandBinding SaveCommandBinding = new CommandBinding(ApplicationCommands.Save);
        public readonly CommandBinding CloseCommandBinding = new CommandBinding(ApplicationCommands.Close);
    }
}
