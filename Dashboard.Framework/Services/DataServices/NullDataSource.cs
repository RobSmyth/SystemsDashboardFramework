using System.Collections.Generic;


namespace NoeticTools.TeamStatusBoard.Framework.Services.DataServices
{
    public class NullDataSource : IDataSource, IDataChangeListener
    {
        public NullDataSource(string typeName)
        {
            TypeName = typeName;
        }

        public string TypeName { get; }

        public void OnChanged()
        {
        }

        public void Write<T>(string name, T value)
        {
        }

        public T Read<T>(string name)
        {
            return default(T);
        }

        public IEnumerable<string> GetAllNames()
        {
            return new string[0];
        }

        public void AddListener(IDataChangeListener listener)
        {
        }

        public bool IsReadOnly(string name)
        {
            return true;
        }

        public void Set(string name, object value, PropertiesFlags flags, params string[] tags)
        {
        }
    }
}