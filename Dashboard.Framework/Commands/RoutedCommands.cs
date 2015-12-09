using System.Windows;
using System.Windows.Input;
using NoeticTools.SystemsDashboard.Framework.Config;


namespace NoeticTools.SystemsDashboard.Framework.Commands
{
    public class RoutedCommands
    {
        public readonly CommandBinding SaveCommandBinding = new CommandBinding(ApplicationCommands.Save);
        public readonly CommandBinding CloseCommandBinding = new CommandBinding(ApplicationCommands.Close);
        public readonly CommandBinding DeleteCommandBinding = new CommandBinding(ApplicationCommands.Delete);
        public readonly CommandBinding OpenCommandBinding = new CommandBinding(ApplicationCommands.Open);

        public RoutedCommands()
        {
            //ApplicationCommands.Open.InputGestures.Add(new MouseGesture(MouseAction.LeftDoubleClick));
            //ApplicationCommands.Open.InputGestures.Add(new KeyGesture(Key.Enter));
        }

        public void BindView(TileConfiguration tile, UIElement view, ITileLayoutController layoutController)
        {
            BindViewToDeleteCommand(tile, view, layoutController);
            BindViewToOpenCommand(tile, view, layoutController);
        }

        private void BindViewToDeleteCommand(TileConfiguration tile, UIElement view, ITileLayoutController layoutController)
        {
            view.CommandBindings.Add(DeleteCommandBinding);
            DeleteCommandBinding.Executed += (sender, args) =>
            {
                var frameworkElement = ((FrameworkElement)sender);
                if (ReferenceEquals(view, frameworkElement) && frameworkElement.IsKeyboardFocusWithin)
                {
                    layoutController.Remove(tile);
                }
            };
        }

        private void BindViewToOpenCommand(TileConfiguration tile, UIElement view, ITileLayoutController layoutController)
        {
            view.CommandBindings.Add(OpenCommandBinding);
            OpenCommandBinding.Executed += (sender, args) =>
            {
                var frameworkElement = ((FrameworkElement)sender);
                if (ReferenceEquals(view, frameworkElement) && frameworkElement.IsKeyboardFocusWithin)
                {
                    // todo
                    //layoutController.Remove(tile);
                }
            };
        }
    }
}