using Moq;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Plugins.DataSources.StaticProperties;
using NUnit.Framework;


namespace SystemsDashboard.Tests.Plugins.DataSources.StaticProperties
{
    [TestFixture]
    public class StaticPropertiesServiceTests : MockingTestFixtureBase
    {
        private StaticPropertiesService _target;
        private Mock<IServices> _services;

        protected override void TearDown()
        {
            _services = NewMock<IServices>();
            _target = new StaticPropertiesService(_services.Object);
        }

        protected override void SetUp()
        {
        }

        [Test]
        public void CanInstantiate()
        { }
    }
}
