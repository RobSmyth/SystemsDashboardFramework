using System;
using System.Collections.Generic;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Input;
using NoeticTools.Dashboard.Framework.Tiles;


namespace NoeticTools.Dashboard.Framework.Plugins
{
    public class KeyboardDashboardNavigationPlugin : IPlugin
    {
        private readonly IDictionary<Key, Action<Key>> _keyHandlers;

        public KeyboardDashboardNavigationPlugin(IDashboardNavigator dashboardNavigator, IDashboardController controller)
        {
            _keyHandlers = new Dictionary<Key, Action<Key>>
            {
                {Key.Home, key => dashboardNavigator.ShowFirstDashboard()},
                {Key.End, key => dashboardNavigator.ShowLastDashboard()},
                {Key.PageDown, key => dashboardNavigator.NextDashboard()},
                {Key.PageUp, key => dashboardNavigator.PrevDashboard()},
                {Key.F3, key => controller.ShowNavigationPane()},
                {Key.Escape, key => dashboardNavigator.ShowCurrentDashboard()},
            };
        }

        public void Register(IServices services)
        {
            services.KeyboardHandler.Register(new LookupKeyboardHandler(_keyHandlers));
        }
    }
}