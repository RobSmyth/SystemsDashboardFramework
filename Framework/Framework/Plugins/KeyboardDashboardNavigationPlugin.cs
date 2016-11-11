using System;
using System.Collections.Generic;
using System.Windows.Input;
using NoeticTools.TeamStatusBoard.Framework.Input;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins
{
    public class KeyboardDashboardNavigationPlugin : IPlugin
    {
        public int Rank => 25;

        public void Register(IServices services)
        {
            var controller = services.DashboardController;
            var navigator = controller.DashboardNavigator;

            var keyHandlers = new Dictionary<Key, Action<Key>>
            {
                {Key.Home, key => navigator.ShowFirstDashboard()},
                {Key.End, key => navigator.ShowLastDashboard()},
                {Key.PageDown, key => navigator.NextDashboard()},
                {Key.PageUp, key => navigator.PrevDashboard()},
                {Key.F3, key => controller.ShowNavigationPane()},
                {Key.Escape, key => navigator.ShowCurrentDashboard()}
            };
            services.KeyboardHandler.Register(new LookupKeyboardHandler(keyHandlers));
        }
    }
}