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
        void SetProperties(string name, ValueProperties properties);
    }
}