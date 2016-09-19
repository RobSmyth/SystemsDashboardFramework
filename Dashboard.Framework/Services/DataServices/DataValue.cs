using System;
using System.Collections.Generic;
using System.Windows.Media;


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
            Broadcaster = new EventBroadcaster();
        }

        public EventBroadcaster Broadcaster { get; }

        public string Name { get; }
        public PropertiesFlags Flags { get; set; }
        public List<string> Tags { get; }

        public object Value
        {
            get { return _value; }
            set
            {
                if (_value != null && _value.Equals(value))
                {
                    return;
                }
                _value = value;
                _notifyValueChanged();
                Broadcaster.Fire();
            }
        }

        public string GetString()
        {
            return Convert.ToString(Value);
        }

        public Color GetColour()
        {
            try
            {
                return (Color)ColorConverter.ConvertFromString(GetString());
            }
            catch (Exception)
            {
                return (Color)ColorConverter.ConvertFromString("Gray");
            }
        }

        public double GetDouble()
        {
            return Convert.ToDouble(Value);
        }
    }
}