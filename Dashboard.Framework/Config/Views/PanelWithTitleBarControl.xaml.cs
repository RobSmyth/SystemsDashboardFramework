using System.Windows;
using System.Windows.Controls;
using NoeticTools.Dashboard.Framework.Commands;


namespace NoeticTools.Dashboard.Framework.Config.Views
{
    public partial class PaneWithTitleBarControl : UserControl
    {
        public PaneWithTitleBarControl()
        {
            InitializeComponent();
        }

        public PaneWithTitleBarControl(string title, UIElement childView, RoutedCommands commands) : this()
        {
            CommandBindings.Add(commands.CloseCommandBinding);
            CommandBindings.Add(commands.SaveCommandBinding);
            CommandBindings.Add(commands.DeleteCommandBinding);
            PlaceholderGrid.Children.Add(childView);
            Title.Text = title;
        }
    }
}