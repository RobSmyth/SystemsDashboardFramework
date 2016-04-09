using System.Windows;
using System.Windows.Controls;
using NoeticTools.SystemsDashboard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Commands;


namespace NoeticTools.SystemsDashboard.Framework.Config.Views
{
    public partial class PaneWithTitleBarControl : UserControl
    {
        public PaneWithTitleBarControl()
        {
            InitializeComponent();
        }

        public PaneWithTitleBarControl(string title, UIElement childView, ApplicationCommandsBindings commandsBindings) : this()
        {
            CommandBindings.Add(commandsBindings.CloseCommandBinding);
            CommandBindings.Add(commandsBindings.SaveCommandBinding);
            CommandBindings.Add(commandsBindings.DeleteCommandBinding);
            PlaceholderGrid.Children.Add(childView);
            Title.Text = title;
        }
    }
}