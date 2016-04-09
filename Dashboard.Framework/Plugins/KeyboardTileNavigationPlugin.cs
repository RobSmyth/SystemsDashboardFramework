using System;
using System.Collections.Generic;
using System.Windows.Input;
using NoeticTools.SystemsDashboard.Framework.Input;
using NoeticTools.SystemsDashboard.Framework.Plugins;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins
{
    public class KeyboardTileNavigationPlugin : IPlugin
    {
        public int Rank => 25;

        public void Register(IServices services)
        {
            var tileNavigator = services.DashboardController.TileNavigator;
            var keyHandlers = new Dictionary<Key, Action<Key>>
            {
                {Key.Right, key => tileNavigator.MoveRight()},
                {Key.Left, key => tileNavigator.MoveLeft()},
                {Key.Up, key => tileNavigator.MoveUp()},
                {Key.Down, key => tileNavigator.MoveDown()}
            };
            services.KeyboardHandler.Register(new LookupKeyboardHandler(keyHandlers));
        }
    }
}