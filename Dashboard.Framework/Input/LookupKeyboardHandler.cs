using System;
using System.Collections.Generic;
using System.Windows.Input;


namespace NoeticTools.SystemsDashboard.Framework.Input
{
    public class LookupKeyboardHandler : IKeyHandler
    {
        private readonly IDictionary<Key, Action<Key>> _keyHandlers;

        public LookupKeyboardHandler(IDictionary<Key, Action<Key>> keyHandlers)
        {
            _keyHandlers = keyHandlers;
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