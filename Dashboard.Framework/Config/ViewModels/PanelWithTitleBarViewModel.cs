using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Commands;
using NoeticTools.Dashboard.Framework.Config.Commands;
using NoeticTools.Dashboard.Framework.Config.Views;
using NoeticTools.Dashboard.Framework.Tiles.Dashboards;


namespace NoeticTools.Dashboard.Framework.Config.ViewModels
{
    public class PanelWithTitleBarViewModel : NotifyingViewModelBase, ICloseableViewModel
    {
        private string _title;

        public PanelWithTitleBarViewModel(PaneWithTitleBarControl view, string title)
        {
            Title = title;
            CloseCommand = new CloseCommand(view);
            SaveCommand = new NullCommand();
            ConfigureCommand = new NullCommand();
            view.DataContext = this;
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (value == _title) return;
                _title = value;
                OnPropertyChanged();
            }
        }

        public ICommand ConfigureCommand { get; }

        public ICommand SaveCommand { get; }

        public ICommand CloseCommand { get; }
    }
}