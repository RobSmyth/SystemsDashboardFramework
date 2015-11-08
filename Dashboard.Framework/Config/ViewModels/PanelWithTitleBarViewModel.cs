using System.Windows;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Config.Commands;
using NoeticTools.Dashboard.Framework.Config.Views;


namespace NoeticTools.Dashboard.Framework.Config.ViewModels
{
    public class PanelWithTitleBarViewModel : NotifyingViewModelBase
    {
        private string _title;

        public PanelWithTitleBarViewModel(string title)
        {
            Title = title;
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
    }
}