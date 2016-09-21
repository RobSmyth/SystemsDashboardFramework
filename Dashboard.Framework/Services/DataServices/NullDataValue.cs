using System;
using System.Collections.Generic;
using System.Windows.Media;


namespace NoeticTools.TeamStatusBoard.Framework.Services.DataServices
{
    public sealed class NullDataValue : IDataValue
    {
        private readonly IDataValue _inner;

        public NullDataValue()
        {
            _inner = new DataValue("", DataValue.DefaultString, PropertiesFlags.None, () => { });
        }

        public NullDataValue(double value) : this()
        {
            Double = value;
        }

        public EventBroadcaster Broadcaster => _inner.Broadcaster;

        public string Name => _inner.Name;

        public PropertiesFlags Flags
        {
            get { return _inner.Flags; }
            set { _inner.Flags = value; }
        }

        public List<string> Tags => _inner.Tags;

        public object Instance
        {
            get { return _inner.Instance; }
            set { _inner.Instance = value; }
        }

        public double Double
        {
            get { return _inner.Double; }
            set { _inner.Double = value; }
        }

        public Color Colour
        {
            get { return _inner.Colour; }
            set { _inner.Colour = value; }
        }

        public string String
        {
            get { return _inner.String; }
            set { _inner.String = value; }
        }

        public bool Boolean
        {
            get { return _inner.Boolean; }
            set { _inner.Boolean = value; }
        }
    }
}