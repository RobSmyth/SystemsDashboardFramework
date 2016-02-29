using System;


namespace NoeticTools.SystemsDashboard.Framework.Services.DataServices
{
    public class DataSource : IDataSource, IDataChangeListener
    {
        public string Name { get; }

        public DataSource(string name)
        {
            Name = name;
        }

        public void AddListener(IDataChangeListener listener)
        {
            throw new NotImplementedException();
        }

        public void OnChanged()
        {
        }
    }
}