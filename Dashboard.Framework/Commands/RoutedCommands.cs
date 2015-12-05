using System.Windows.Input;


namespace NoeticTools.SystemsDashboard.Framework.Commands
{
    public class RoutedCommands
    {
        public readonly CommandBinding SaveCommandBinding = new CommandBinding(ApplicationCommands.Save);
        public readonly CommandBinding CloseCommandBinding = new CommandBinding(ApplicationCommands.Close);
        public readonly CommandBinding DeleteCommandBinding = new CommandBinding(ApplicationCommands.Delete);
    }
}