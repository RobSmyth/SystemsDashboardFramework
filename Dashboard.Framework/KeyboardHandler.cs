using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;


namespace NoeticTools.Dashboard.Framework
{
    public class KeyboardHandler : IKeyboardHandler
    {
        private readonly Dictionary<Key, Action<Key>> _globalKeyHandlers;
        private readonly IList<IKeyHandler> _keyHandlers = new List<IKeyHandler>();

        public KeyboardHandler(IDashboardTileNavigator tileNavigator, IDashboardNavigator dashboardNavigator, IDashboardController controller)
        {
            _globalKeyHandlers = new Dictionary<Key, Action<Key>>
            {
                // todo - Replace these with a dashboard navigation plugin
                {Key.Home, key => dashboardNavigator.ShowFirstDashboard()},
                {Key.End, key => dashboardNavigator.ShowLastDashboard()},
                {Key.PageDown, key => dashboardNavigator.NextDashboard()},
                {Key.PageUp, key => dashboardNavigator.PrevDashboard()},
                {Key.F3, key => controller.ShowNavigationPane()},
                {Key.Escape, key => dashboardNavigator.ShowCurrentDashboard()},

                // F2 - used by tiles for tile edit

                {Key.F4, key => controller.ToggleGroupPanelsEditMode()},
                {Key.F5, key => controller.Refresh()},

                // todo - Replace these with a tile navigator plugin
                {Key.Right, key => tileNavigator.MoveRight()},
                {Key.Left, key => tileNavigator.MoveLeft()},
                {Key.Up, key => tileNavigator.MoveUp()},
                {Key.Down, key => tileNavigator.MoveDown()},

                // DEL - will be used to delete a tile
            };
        }

        public void OnKeyDown(Key key)
        {
            if (Keyboard.Modifiers == ModifierKeys.None && _globalKeyHandlers.ContainsKey(key))
            {
                _globalKeyHandlers[key](key);
                return;
            }

            // TODO - CNTRL C, X, & V keys to cut & paste
            // TODO - SHIFT arrow keys to move tiles (maybe)

            // TODO - how to invoke dashboard configuration? ALT-F2? ... slide out panel?

            foreach (var handler in _keyHandlers.Where(x => x.CanHandle(key)))
            {
                handler.Handle(key);
            }
        }

        public void Register(IKeyHandler keyHandler)
        {
            _keyHandlers.Add(keyHandler);
        }
    }
}