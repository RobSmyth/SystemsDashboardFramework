using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;


namespace NoeticTools.Dashboard.Framework.Input
{
    public class KeyboardHandler : IKeyboardHandler
    {
        private readonly Dictionary<Key, Action<Key>> _globalKeyHandlers;
        private readonly IList<IKeyHandler> _keyHandlers = new List<IKeyHandler>();

        public KeyboardHandler(IDashboardController controller)
        {
            _globalKeyHandlers = new Dictionary<Key, Action<Key>>
            {
                // F2 - used by tiles for tile edit

                {Key.F4, key => controller.ToggleGroupPanelsEditMode()},
                {Key.F5, key => controller.Refresh()},

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