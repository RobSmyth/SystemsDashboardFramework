using Moq;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NUnit.Framework;


namespace NoeticTools.TeamStatusBoard.Tests.Services.DataServices
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
            _target = new DataServer();
        }

        [Test]
        public void GetDataSource_CreatesSink_WhenNoSinkExists()
        {
            Assert.IsNotNull(_target.GetDataSource("1"));
        }

        [Test]
        public void GetDataSource_CreatesSourceAndRegistersWithSink_WhenSinkExsists()
        {
            var sink1 = NewMock<IDataSource>();
            _sinkFactory.Setup(x => x.Create("1")).Returns(sink1.Object);
            _target.GetDataSource("1");

            Assert.IsNotNull(_target.GetDataSource("1"));
        }

        [Test]
        public void GetDataSource_ReturnsDifferentSource_WhenDuplicateNameAndNoSinkExists()
        {
            Assert.IsNotNull(_target.GetDataSource("1"));
            Assert.IsNotNull(_target.GetDataSource("1"));
        }

        [Test]
        public void GetDataSource_ReturnsDifferentSourcesRegisteredWithSink_WhenDuplicateNameAndSinkExists()
        {
            var sink1 = NewMock<IDataSource>();
            _sinkFactory.Setup(x => x.Create("1")).Returns(sink1.Object);
            _target.GetDataSource("1");

            Assert.IsNotNull(_target.GetDataSource("1"));
            Assert.IsNotNull(_target.GetDataSource("1"));
        }
    }
}