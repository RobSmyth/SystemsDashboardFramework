using System.Collections.Generic;
using NoeticTools.SystemsDashboard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.Framework.Services.DataServices
{
    public class NullDataSource : IDataSource, IDataChangeListener
    {
        public NullDataSource(string name)
        {
            Name = name;
        }

        public void OnChanged()
        {
        }

        public string Name { get; }

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
    }
}