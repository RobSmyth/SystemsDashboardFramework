using System.Windows.Controls;
using System.Windows.Input;
using NoeticTools.TeamStatusBoard.Framework.Commands;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Views
{
    public partial class DataSourcesConfigControl : UserControl
    {
        public DataSourcesConfigControl()
        {
            InitializeComponent();
        }

        public DataSourcesConfigControl(IApplicationCommands commands) : this()
        {
            commands.BindViewToAllCommands(this);
            commands.SaveCommand.Executed += SaveCommandBinding_Executed;
        }

        private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
        }
    }
}