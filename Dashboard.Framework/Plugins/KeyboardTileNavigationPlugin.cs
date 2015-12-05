using System;
using System.Collections.Generic;
using System.Windows.Input;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Input;


namespace NoeticTools.SystemsDashboard.Framework.Plugins
{
    public class KeyboardTileNavigationPlugin : IPlugin
    {
        private readonly IDictionary<Key, Action<Key>> _keyHandlers;

        public KeyboardTileNavigationPlugin(IDashboardTileNavigator tileNavigator)
        {
            _keyHandlers = new Dictionary<Key, Action<Key>>
            {
                {Key.Right, key => tileNavigator.MoveRight()},
                {Key.Left, key => tileNavigator.MoveLeft()},
                {Key.Up, key => tileNavigator.MoveUp()},
                {Key.Down, key => tileNavigator.MoveDown()}
            };
        }

        public int Rank => 0;

        public void Register(IServices services)
        {
            services.KeyboardHandler.Register(new LookupKeyboardHandler(_keyHandlers));
        }
    }
}