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
            Assert.IsNotNull(_target.GetDataSource("1", "A"));
        }

        [Test]
        public void GetDataSource_CreatesSourceAndRegistersWithSink_WhenSinkExsists()
        {
            var sink1 = NewMock<IDataSource>();
            _sinkFactory.Setup(x => x.Create("1", "A")).Returns(sink1.Object);
            sink1.SetupGet(x => x.Name).Returns("A.1");
            _target.GetDataSource("1", "A");

            Assert.IsNotNull(_target.GetDataSource("1", "A.1"));
        }

        [Test]
        public void GetDataSource_ReturnsDifferentSource_WhenDuplicateNameAndNoSinkExists()
        {
            Assert.IsNotNull(_target.GetDataSource("1", "A"));
            Assert.IsNotNull(_target.GetDataSource("1", "A"));
        }

        [Test]
        public void GetDataSource_ReturnsDifferentSourcesRegisteredWithSink_WhenDuplicateNameAndSinkExists()
        {
            var sink1 = NewMock<IDataSource>();
            _sinkFactory.Setup(x => x.Create("1", "A")).Returns(sink1.Object);
            sink1.SetupGet(x => x.Name).Returns("A.1");
            _target.GetDataSource("1", "A");

            Assert.IsNotNull(_target.GetDataSource("1", "A.1"));
            Assert.IsNotNull(_target.GetDataSource("1", "A.1"));
        }
    }
}