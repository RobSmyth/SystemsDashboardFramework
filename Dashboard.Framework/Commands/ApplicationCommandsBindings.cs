using System.Windows;
using System.Windows.Input;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles;


namespace NoeticTools.TeamStatusBoard.Framework.Commands
{
    public class ApplicationCommandsBindings
    {
        public readonly CommandBinding SaveCommandBinding = new CommandBinding(ApplicationCommands.Save);
        public readonly CommandBinding CloseCommandBinding = new CommandBinding(ApplicationCommands.Close);
        public readonly CommandBinding DeleteCommandBinding = new CommandBinding(ApplicationCommands.Delete);
        public readonly CommandBinding OpenCommandBinding = new CommandBinding(ApplicationCommands.Open);

        public ApplicationCommandsBindings()
        {
            ApplicationCommands.Open.InputGestures.Add(new MouseGesture(MouseAction.LeftDoubleClick));
            ApplicationCommands.Open.InputGestures.Add(new KeyGesture(Key.Enter));
            ApplicationCommands.Open.InputGestures.Add(new KeyGesture(Key.F2));
        }

        public void BindView(TileConfiguration tile, FrameworkElement view, ITileLayoutController layoutController)
        {
            BindViewToDeleteCommand(tile, view, layoutController);
            BindViewToOpenCommand(view);
        }

        private void BindViewToDeleteCommand(TileConfiguration tile, UIElement view, ITileLayoutController layoutController)
        {
            view.CommandBindings.Add(DeleteCommandBinding);
            DeleteCommandBinding.Executed += (sender, args) =>
            {
                var senderElement = ((FrameworkElement)sender);
                if (ReferenceEquals(view, senderElement) && senderElement.IsKeyboardFocusWithin)
                {
                    layoutController.Remove(tile);
                }
            };
        }

        private void BindViewToOpenCommand(FrameworkElement view)
        {
            view.CommandBindings.Add(OpenCommandBinding);
            OpenCommandBinding.Executed += (sender, args) =>
            {
                var senderElement = ((FrameworkElement)sender);
                if (ReferenceEquals(view, senderElement) && senderElement.IsKeyboardFocusWithin)
                {
                    var viewModel = view.DataContext as ITileViewModel;
                    if (viewModel != null && viewModel.ConfigureCommand.CanExecute(null))
                    {
                        viewModel.ConfigureCommand.Execute(null);
                        args.Handled = true;
                    }
                }
            };
        }
    }
}