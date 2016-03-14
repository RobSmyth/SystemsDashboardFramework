using Moq;
using NoeticTools.SystemsDashboard.Framework.Config.Properties;
using NoeticTools.SystemsDashboard.Framework.DataSources;
using NUnit.Framework;


namespace SystemsDashboard.Tests.DataSources
{
    [TestFixture]
    public class DataPropertyViewModelTests : MockingTestFixtureBase
    {
        private DataPropertyViewModel<int> _target;

        protected override void TearDown()
        {
        }

        protected override void SetUp()
        {
            _target = new DataPropertyViewModel<int>();
        }

        [Test]
        public void CanInstantiate()
        {
            Assert.IsNotNull(_target);
        }
    }
}