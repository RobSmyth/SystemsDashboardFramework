using System;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;


namespace NoeticTools.TeamStatusBoard.DataSources.TestData
{
    public class TestDataService : IService
    {
        private const string PropertyTag = "TestData";

        public TestDataService(IServices services, IDataSource dataSource)
        {
            var datum = new SawToothDatum(dataSource.GetDatum("SawTooth5seconds", 0.0), TimeSpan.FromSeconds(5.0), services) {Flags = PropertiesFlags.ReadOnly};
            datum.Tags.Add(PropertyTag);

            datum = new SawToothDatum(dataSource.GetDatum("SawTooth20seconds", 0.0), TimeSpan.FromSeconds(20.0), services) { Flags = PropertiesFlags.ReadOnly };
            datum.Tags.Add(PropertyTag);
        }

        public string Name => "TestData";

        public void Stop()
        {
        }

        public void Start()
        {
        }
    }
}