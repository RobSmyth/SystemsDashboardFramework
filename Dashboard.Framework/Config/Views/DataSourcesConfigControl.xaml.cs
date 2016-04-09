using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Services;


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