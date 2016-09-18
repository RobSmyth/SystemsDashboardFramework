using System;
using System.Collections.Generic;


namespace NoeticTools.TeamStatusBoard.Framework.Services.DataServices
{
    public class DataValue
    {
        private object _value;
        private readonly Action _notifyValueChanged;

        public DataValue(object value, PropertiesFlags flags, Action notifyValueChanged, params string[] tags)
        {
            Flags = flags;
            _value = value;
            _notifyValueChanged = notifyValueChanged;
            Tags = new List<string>(tags);
        }

        public PropertiesFlags Flags { get; set; }
        public List<string> Tags { get; set; }

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