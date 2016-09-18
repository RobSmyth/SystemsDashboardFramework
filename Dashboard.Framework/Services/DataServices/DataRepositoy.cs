using System;
using System.Collections.Generic;
using System.Linq;


namespace NoeticTools.TeamStatusBoard.Framework.Services.DataServices
{
    public class DataRepositoy : IDataSource
    {
        private readonly IDictionary<string, DataValue> _values = new Dictionary<string, DataValue>();
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
            return _values.ContainsKey(name) && ((_values[name].Flags & PropertiesFlags.ReadOnly) != PropertiesFlags.None);
        }

        public void Set(string name, object value, PropertiesFlags flags, params string[] tags)
        {
            if (!_values.ContainsKey(name))
            {
                _values[name] = new DataValue(name, value, flags, NotifyValueChanged, tags);
            }
            else
            {
                var dataValue = _values[name];
                dataValue.Flags = flags;
                dataValue.Tags.Clear();
                dataValue.Tags.AddRange(tags);
                dataValue.Value = value;
            }
        }

        public IEnumerable<DataValue> Find(Func<DataValue, bool> predicate)
        {
            return _values.Values.Where(x => predicate(x)).OrderBy(y => y.Name).ToArray();
        }

        public IEnumerable<string> GetAllNames()
        {
            return _values.Keys.OrderBy(x => x).ToArray();
        }

        public void Write<T>(string name, T value)
        {
            if (!_values.ContainsKey(name))
            {
                Set(name, value, PropertiesFlags.None);
            }
            else
            {
                _values[name].Value = value;
            }
        }

        public T Read<T>(string name)
        {
            if (!_values.ContainsKey(name))
            {
                Set(name, default(T), PropertiesFlags.None);
            }
            try
            {
                return (T)Convert.ChangeType(_values[name].Value, typeof(T));
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