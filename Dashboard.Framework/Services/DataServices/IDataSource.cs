using System;
using System.Collections.Generic;


namespace NoeticTools.TeamStatusBoard.Framework.Services.DataServices
{
    public interface IDataSource
    {
        string TypeName { get; }
        void Write<T>(string name, T value);
        T Read<T>(string name);
        IEnumerable<string> GetAllNames();
        void AddListener(IDataChangeListener listener);
        bool IsReadOnly(string name);
        void Set(string name, object value, PropertiesFlags flags, params string[] tags);
        IEnumerable<DataValue> Find(Func<DataValue, bool> predicate);
        IDataValue GetDatum(string name, object value = null);
    }
}