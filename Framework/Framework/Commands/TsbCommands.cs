using System.Windows;
using System.Windows.Input;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles;
using NoeticTools.TeamStatusBoard.Persistence.Xml;


namespace NoeticTools.TeamStatusBoard.Framework.Commands
{
    public class TsbCommands : IApplicationCommands
    {
        public static readonly RoutedUICommand ShowDataSources = new RoutedUICommand("Show data sources", "ShowDataSourcesCommand", typeof (TsbCommands));

        public readonly CommandBinding SaveCommandBinding = new CommandBinding(ApplicationCommands.Save);
        public readonly CommandBinding CloseCommandBinding = new CommandBinding(ApplicationCommands.Close);
        public readonly CommandBinding DeleteCommandBinding = new CommandBinding(ApplicationCommands.Delete);
        public readonly CommandBinding OpenCommandBinding = new CommandBinding(ApplicationCommands.Open);
        public readonly CommandBinding ShowDataSourcesBinding = new CommandBinding(ShowDataSources);
        private static ICommand _showDataSourcesCommand;

        static TsbCommands()
        {
            ShowDataSources.InputGestures.Add(new KeyGesture(Key.F10));
            ApplicationCommands.Open.InputGestures.Add(new MouseGesture(MouseAction.LeftDoubleClick));
            ApplicationCommands.Open.InputGestures.Add(new KeyGesture(Key.Enter));
            ApplicationCommands.Open.InputGestures.Add(new KeyGesture(Key.F2));
        }

        public TsbCommands()
        {
            ShowDataSourcesBinding.PreviewCanExecute += (x, y) =>
            {
                y.CanExecute = _showDataSourcesCommand.CanExecute(y.Parameter);
                y.Handled = false;
            };
            ShowDataSourcesBinding.Executed += (x, y) => { _showDataSourcesCommand.Execute(y.Parameter); };
        }

        public CommandBinding SaveCommand
        {
            get { return SaveCommandBinding; }
        }

        public CommandBinding CloseCommand
        {
            get { return CloseCommandBinding; }
        }

        public CommandBinding DeleteCommand
        {
            get { return DeleteCommandBinding; }
        }

        public CommandBinding OpenCommand
        {
            get { return OpenCommandBinding; }
        }

        public void BindView(TileConfiguration tile, FrameworkElement view, ITileLayoutController layoutController)
        {
            BindViewToDeleteCommand(tile, view, layoutController);
            BindViewToOpenCommand(view);
            view.CommandBindings.Add(ShowDataSourcesBinding);
            view.CommandBindings.Add(CloseCommandBinding);
            view.CommandBindings.Add(SaveCommandBinding);
        }

        public void BindViewToAllCommands(UIElement element)
        {
            element.CommandBindings.Add(ShowDataSourcesBinding);
            element.CommandBindings.Add(CloseCommandBinding);
            element.CommandBindings.Add(SaveCommandBinding);
            element.CommandBindings.Add(DeleteCommandBinding);
        }

        public static void SetDefaultShowDataSources(ICommand showDataSourcesCommand)
        {
            _showDataSourcesCommand = showDataSourcesCommand;
        }

        private void BindViewToDeleteCommand(TileConfiguration tile, UIElement view, ITileLayoutController layoutController)
        {
            view.CommandBindings.Add(DeleteCommandBinding);
            DeleteCommandBinding.Executed += (sender, args) =>
            {
                var senderElement = ((FrameworkElement) sender);
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
                var senderElement = ((FrameworkElement) sender);
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