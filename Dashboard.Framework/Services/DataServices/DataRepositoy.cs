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
            var dataValue = GetDatum(name, value);
            dataValue.Flags = flags;
            dataValue.Tags.Clear();
            dataValue.Tags.AddRange(tags);
            dataValue.Instance = value;
        }

        public IEnumerable<DataValue> Find(Func<DataValue, bool> predicate)
        {
            return _values.Values.Where(x => predicate(x)).OrderBy(y => y.Name).ToArray();
        }

        public IDataValue GetDatum(string name, object defaultValue = null)
        {
            if (!_values.ContainsKey(name))
            {
                _values[name] = new DataValue(name, defaultValue, PropertiesFlags.None, NotifyValueChanged);
            }
            return _values[name];
        }

        public IEnumerable<IDataValue> GetDatums(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetAllNames()
        {
            return _values.Keys.OrderBy(x => x).ToArray();
        }

        public void Write<T>(string name, T value)
        {
            GetDatum(name).Instance = value;
        }

        public T Read<T>(string name)
        {
            try
            {
                return (T)Convert.ChangeType(GetDatum(name).Instance, typeof(T));
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