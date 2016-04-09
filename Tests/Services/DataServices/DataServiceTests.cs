using Moq;
using NoeticTools.SystemsDashboard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NUnit.Framework;


namespace SystemsDashboard.Tests.Services.DataServices
{
    [TestFixture]
    public class DataServiceTests : MockingTestFixtureBase
    {
        private DataServer _target;
        private Mock<IDataRepositoryFactory> _sinkFactory;

        protected override void TearDown()
        {
        }

        protected override void SetUp()
        {
            _sinkFactory = NewMock<IDataRepositoryFactory>();
            _target = new DataServer(_sinkFactory.Object);
        }

        [Test]
        public void GetDataSource_CreatesSink_WhenNoSinkExists()
        {
            Assert.IsNotNull(_target.GetDataSource("A"));
        }

        [Test]
        public void GetDataSource_ReturnsDifferentSource_WhenDuplicateNameAndNoSinkExists()
        {
            Assert.IsNotNull(_target.GetDataSource("A"));
            Assert.IsNotNull(_target.GetDataSource("A"));
        }

        [Test]
        public void GetDataSource_CreatesSourceAndRegistersWithSink_WhenSinkExsists()
        {
            var sink1 = NewMock<IDataSource>();
            _sinkFactory.Setup(x => x.Create("A", 1)).Returns(sink1.Object);
            sink1.SetupGet(x => x.Name).Returns("A.1");
            _target.GetDataSource("A");

            Assert.IsNotNull(_target.GetDataSource("A.1"));
        }

        [Test]
        public void GetDataSource_ReturnsDifferentSourcesRegisteredWithSink_WhenDuplicateNameAndSinkExists()
        {
            var sink1 = NewMock<IDataSource>();
            _sinkFactory.Setup(x => x.Create("A", 1)).Returns(sink1.Object);
            sink1.SetupGet(x => x.Name).Returns("A.1");
            _target.GetDataSource("A");

            Assert.IsNotNull(_target.GetDataSource("A.1"));
            Assert.IsNotNull(_target.GetDataSource("A.1"));
        }
    }
}
