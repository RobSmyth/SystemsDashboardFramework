using System;
using System.Collections.Generic;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Tiles;


namespace NoeticTools.Dashboard.Framework.Plugins
{
    public class KeyboardTileNavigationPlugin : IPlugin, IKeyHandler
    {
        private readonly IDictionary<Key, Action<Key>> _keyHandlers;

        public KeyboardTileNavigationPlugin(IDashboardTileNavigator tileNavigator)
        {
            _keyHandlers = new Dictionary<Key, Action<Key>>
            {
                {Key.Right, key => tileNavigator.MoveRight()},
                {Key.Left, key => tileNavigator.MoveLeft()},
                {Key.Up, key => tileNavigator.MoveUp()},
                {Key.Down, key => tileNavigator.MoveDown()},
            };
        }

        public void Register(IServices services)
        {
            services.KeyboardHandler.Register(this);
        }

        bool IKeyHandler.CanHandle(Key key)
        {
            return Keyboard.Modifiers == ModifierKeys.None && _keyHandlers.ContainsKey(key);
        }

        void IKeyHandler.Handle(Key key)
        {
            _keyHandlers[key](key);
        }
    }
}