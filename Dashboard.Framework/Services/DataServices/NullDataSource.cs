using System.Collections.Generic;


namespace NoeticTools.TeamStatusBoard.Framework.Services.DataServices
{
    public class NullDataSource : IDataSource, IDataChangeListener
    {
        public NullDataSource(string name)
        {
            Name = name;
        }

        public string Name { get; }

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
    }
}