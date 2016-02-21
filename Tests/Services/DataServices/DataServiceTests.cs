using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NoeticTools.SystemsDashboard.Framework.Services;
using NoeticTools.SystemsDashboard.Framework.Services.DataServices;
using NUnit.Framework;


namespace SystemsDashboard.Tests.Services
{
    [TestFixture]
    public class DataServiceTests : MockingTestFixtureBase
    {
        private DataService _target;
        private Mock<IDataRepositoryFactory> _sinkFactory;

        protected override void TearDown()
        {
        }

        protected override void SetUp()
        {
            _sinkFactory = NewMock<IDataRepositoryFactory>();
            _target = new DataService(_sinkFactory.Object);
        }

        [Test]
        public void RegisterSource_ReturnsSink()
        {
            var sink = NewMock<IDataSink>();
            _sinkFactory.Setup(x => x.Create("A", 1)).Returns(sink.Object);
            sink.SetupGet(x => x.Name).Returns("A.1");

            Assert.AreSame(sink.Object, _target.CreateDataSink("A"));
        }

        [Test]
        public void RegisterSource_ReturnsSink_WhenDuplicateName()
        {
            var sink1 = NewMock<IDataSink>();
            _sinkFactory.Setup(x => x.Create("SourceA", 1)).Returns(sink1.Object);
            sink1.SetupGet(x => x.ShortName).Returns("SourceA");
            sink1.SetupGet(x => x.Name).Returns("SourceA.1");
            var sink2 = NewMock<IDataSink>();
            _sinkFactory.Setup(x => x.Create("SourceA", 2)).Returns(sink2.Object);
            sink2.SetupGet(x => x.Name).Returns("SourceA.2");

            Assert.AreSame(sink1.Object, _target.CreateDataSink("SourceA"));
            Assert.AreSame(sink2.Object, _target.CreateDataSink("SourceA"));
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
            var sink1 = NewMock<IDataSink>();
            _sinkFactory.Setup(x => x.Create("A", 1)).Returns(sink1.Object);
            sink1.SetupGet(x => x.Name).Returns("A.1");
            sink1.Setup(x => x.AddListener(It.IsAny<IChangeListener>()));
            _target.CreateDataSink("A");

            Assert.IsNotNull(_target.GetDataSource("A.1"));

            sink1.Verify(x => x.AddListener(It.IsAny<IChangeListener>()), Times.Once());
        }

        [Test]
        public void GetDataSource_ReturnsDifferentSourcesRegisteredWithSink_WhenDuplicateNameAndSinkExists()
        {
            var sink1 = NewMock<IDataSink>();
            _sinkFactory.Setup(x => x.Create("A", 1)).Returns(sink1.Object);
            sink1.SetupGet(x => x.Name).Returns("A.1");
            sink1.Setup(x => x.AddListener(It.IsAny<IChangeListener>()));
            _target.CreateDataSink("A");

            Assert.IsNotNull(_target.GetDataSource("A.1"));
            Assert.IsNotNull(_target.GetDataSource("A.1"));

            sink1.Verify(x => x.AddListener(It.IsAny<IChangeListener>()), Times.Exactly(2));
        }
    }
}
