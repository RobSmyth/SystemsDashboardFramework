using System;
using System.Collections.Generic;


namespace NoeticTools.TeamStatusBoard.Framework.Services.DataServices
{
    public class DataValue
    {
        private object _value;
        private readonly Action _notifyValueChanged;

        public DataValue(string name, object value, PropertiesFlags flags, Action notifyValueChanged, params string[] tags)
        {
            Name = name;
            Flags = flags;
            _value = value;
            _notifyValueChanged = notifyValueChanged;
            Tags = new List<string>(tags);
        }

        public string Name { get; }
        public PropertiesFlags Flags { get; set; }
        public List<string> Tags { get; }

        public object Value
        {
            get { return _value; }
            set
            {
                _value = value;
                _notifyValueChanged();
            }
        }
    }
}