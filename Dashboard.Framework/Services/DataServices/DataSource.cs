using System;


namespace NoeticTools.SystemsDashboard.Framework.Services.DataServices
{
    public class DataSource : IDataSource, IChangeListener
    {
        public string Name { get; }

        public DataSource(string name)
        {
            Name = name;
        }

        public void AddListener(IChangeListener listener)
        {
            throw new NotImplementedException();
        }

        public void OnChanged()
        {
        }
    }
}