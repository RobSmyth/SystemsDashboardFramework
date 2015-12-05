using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;


namespace NoeticTools.SystemsDashboard.Framework.Input
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
                {Key.F5, key => controller.Refresh()}
            };
        }

        public void OnKeyDown(Key key)
        {
            if (Keyboard.Modifiers == ModifierKeys.None && _globalKeyHandlers.ContainsKey(key))
            {
                _globalKeyHandlers[key](key);
                return;
            }

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