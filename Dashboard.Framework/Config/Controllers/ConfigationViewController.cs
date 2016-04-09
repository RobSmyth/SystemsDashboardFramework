using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Config.Views;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Services;
using ParametersConfigControl = NoeticTools.TeamStatusBoard.Framework.Config.Views.ParametersConfigControl;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Controllers
{
    public sealed class ConfigationViewController : NotifyingViewModelBase
    {
        private readonly IEnumerable<IPropertyViewModel> _parameters;
        private readonly IServices _services;
        private readonly string _title;
        private readonly ApplicationCommandsBindings _appCommandsBindings;
        private PaneWithTitleBarControl _panelView;

        public ConfigationViewController(string title, ApplicationCommandsBindings appCommandsBindings, IEnumerable<IPropertyViewModel> parameters, IServices services)
        {
            _title = title;
            _appCommandsBindings = appCommandsBindings;
            _parameters = parameters;
            _services = services;
        }

        public FrameworkElement CreateView()
        {
            _appCommandsBindings.CloseCommandBinding.Executed += CloseCommandBinding_Executed;

            var view = new ParametersConfigControl(_appCommandsBindings, _parameters, _services) {DataContext = this};

            _panelView = new PaneWithTitleBarControl(_title, view, _appCommandsBindings)
            {
                Width = double.NaN,
                Height = double.NaN
            };

            return _panelView;
        }

        private void CloseCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _panelView.Visibility = Visibility.Collapsed;
        }
    }
}