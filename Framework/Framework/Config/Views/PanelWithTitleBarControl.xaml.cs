using System.Windows;
using System.Windows.Controls;
using NoeticTools.TeamStatusBoard.Framework.Commands;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Views
{
    public partial class PaneWithTitleBarControl : UserControl
    {
        public PaneWithTitleBarControl()
        {
            InitializeComponent();
        }

        public PaneWithTitleBarControl(string title, UIElement childView, IApplicationCommands commands) : this()
        {
            commands.BindViewToAllCommands(this);
            PlaceholderGrid.Children.Add(childView);
            Title.Text = title;
        }
    }
}