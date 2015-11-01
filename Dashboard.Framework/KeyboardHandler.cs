using System;
using System.Collections.Generic;
using System.Windows.Input;


namespace NoeticTools.Dashboard.Framework
{
    public class KeyboardHandler
    {
        private readonly Dictionary<Key, Action<Key>> _keyDownHandlers;

        public KeyboardHandler(IDashboardTileNavigator tileNavigator, IDashboardNavigator dashboardNavigator,
            IDashboardController controller)
        {
            _keyDownHandlers = new Dictionary<Key, Action<Key>>
            {
                {Key.Home, key => dashboardNavigator.ShowFirstDashboard()},
                {Key.End, key => dashboardNavigator.ShowLastDashboard()},
                {Key.PageDown, key => dashboardNavigator.NextDashboard()},
                {Key.PageUp, key => dashboardNavigator.PrevDashboard()},
                {Key.F1, key => controller.ShowHelpPane()},
                // F2 - used by tiles for tile edit
                {Key.F3, key => controller.ShowNavigationPane()},
                {Key.F4, key => dashboardNavigator.ToggleShowPanesMode()},
                {Key.F5, key => controller.Refresh()},
                {Key.Escape, key => dashboardNavigator.ShowCurrentDashboard()},
                {Key.Right, key => tileNavigator.MoveRight()},
                {Key.Left, key => tileNavigator.MoveLeft()},
                {Key.Up, key => tileNavigator.MoveUp()},
                {Key.Down, key => tileNavigator.MoveDown()}
                // INS - will be used to insert a new tile
                // DEL - will be used to delete a tile
            };
        }

        public void OnKeyDown(Key key)
        {
            if (Keyboard.Modifiers == ModifierKeys.None && _keyDownHandlers.ContainsKey(key))
            {
                _keyDownHandlers[key](key);
            }

            // TODO - CNTRL C, X, & V keys to cut & paste
            // TODO - SHIFT arrow keys to move tiles (maybe)

            // TODO - how to invoke dashboard configuration? ALT-F2? ... slide out panel?
        }
    }
}