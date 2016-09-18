using System;
using System.Collections.Generic;
using System.Linq;


namespace NoeticTools.TeamStatusBoard.Framework.Services.DataServices
{
    public class DataRepositoy : IDataSource
    {
        private readonly IDictionary<string, ValueProperties> _valueProperties = new Dictionary<string, ValueProperties>();
        private readonly IDictionary<string, object> _values = new Dictionary<string, object>();
        private readonly IList<IDataChangeListener> _listeners = new List<IDataChangeListener>();

        public DataRepositoy(string typeName)
        {
            TypeName = typeName;
        }

        public string TypeName { get; }

        public void AddListener(IDataChangeListener listener)
        {
            _listeners.Add(listener);
        }

        public bool IsReadOnly(string name)
        {
            return _valueProperties.ContainsKey(name) && _valueProperties[name] == ValueProperties.ReadOnly;
        }

        public void SetProperties(string name, ValueProperties properties)
        {
            _valueProperties[name] = properties;
        }

        public IEnumerable<string> GetAllNames()
        {
            return _values.Keys.OrderBy(x => x).ToArray();
        }

        public void Write<T>(string name, T value)
        {
            _values[name] = value;
            NotifyValueChanged();
        }

        public T Read<T>(string name)
        {
            if (!_values.ContainsKey(name))
            {
                _values.Add(name, default(T));
            }
            try
            {
                return (T)Convert.ChangeType(_values[name], typeof(T));
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        private void NotifyValueChanged()
        {
            var listeners = _listeners.ToArray();
            foreach (var listener in listeners)
            {
                listener.OnChanged();
            }
        }
    }
}