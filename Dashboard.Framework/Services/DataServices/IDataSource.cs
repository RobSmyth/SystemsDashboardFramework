using System.Collections.Generic;
using NoeticTools.SystemsDashboard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.Framework.Services.DataServices
{
    public interface IDataSource
    {
        string Name { get; }
        void Write<T>(string name, T value);
        T Read<T>(string name);
        IEnumerable<string> GetAllNames();
        void AddListener(IDataChangeListener listener);
    }
}