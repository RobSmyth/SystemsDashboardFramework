using System;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.DataSources.TestData
{
    public class TestDataService : IService
    {
        private const string PropertyTag = "TestData";

        public TestDataService(IServices services, IDataSource dataSource)
        {
            var datum = new TriangeWaveformDatum(dataSource.GetDatum("TriangleWaveform5seconds", 0.0), TimeSpan.FromSeconds(5.0), services) {Flags = PropertiesFlags.ReadOnly};
            datum.Tags.Add(PropertyTag);

            datum = new TriangeWaveformDatum(dataSource.GetDatum("TriangleWaveform20seconds", 0.0), TimeSpan.FromSeconds(20.0), services) { Flags = PropertiesFlags.ReadOnly };
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