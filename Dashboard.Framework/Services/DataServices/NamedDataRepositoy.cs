using System.Collections.Generic;
using System.Linq;


namespace NoeticTools.SystemsDashboard.Framework.Services.DataServices
{
    public class NamedDataRepositoy : IDataSink, IDataSource
    {
        private readonly IDictionary<string, object> _values = new Dictionary<string, object>();
        private readonly IList<IChangeListener> _listeners = new List<IChangeListener>();

        public NamedDataRepositoy(string name, int id)
        {
            ShortName = name;
            Name = $"{name}.{id}";
        }

        public string ShortName { get; }
        public string Name { get; }

        public void AddListener(IChangeListener listener)
        {
            _listeners.Add(listener);
        }

        public void Write<T>(string name, T value)
        {
            _values[name] = value;
            NotifyValueChanged();
        }

        private void NotifyValueChanged()
        {
            var listeners = _listeners.ToArray();
            foreach (var listener in listeners)
            {
                listener.OnChanged();
            }
        }

        public T Read<T>(string name)
        {
            if (!_values.ContainsKey(name))
            {
                _values.Add(name, default(T));
            }
            return (T)_values[name];
        }
    }
}