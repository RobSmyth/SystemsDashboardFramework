using Moq;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Plugins.DataSources.StaticProperties;
using NUnit.Framework;


namespace SystemsDashboard.Tests.Plugins.DataSources.StaticProperties
{
    [TestFixture]
    public class VizBoardPropertiesServiceTests : MockingTestFixtureBase
    {
        private VizBoardPropertiesService _target;
        private Mock<IDashboardConfigurationServices> _configServices;
        private Mock<IServices> _applicationServices;

        protected override void SetUp()
        {
            _configServices = NewMock<IDashboardConfigurationServices>();
            _applicationServices = NewMock<IServices>();
            _target = new VizBoardPropertiesService(_configServices.Object, _applicationServices.Object);
        }

        protected override void TearDown()
        {
        }

        [Test]
        public void CanInstantiate()
        { }
    }
}
