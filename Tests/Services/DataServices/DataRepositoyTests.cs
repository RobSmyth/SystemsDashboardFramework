using NoeticTools.SystemsDashboard.Framework.Services.DataServices;
using NUnit.Framework;


namespace SystemsDashboard.Tests.Services.DataServices
{
    [TestFixture]
    public class DataRepositoyTests : MockingTestFixtureBase
    {
        private NamedDataRepositoy _target;

        protected override void SetUp()
        {
            _target = new NamedDataRepositoy("A", 1);
        }

        protected override void TearDown()
        {
        }

        [Test]
        public void NameFormat()
        {
            Assert.AreEqual("A", _target.ShortName);
            Assert.AreEqual("A.1", _target.Name);
        }

        [Test]
        public void SetValue_CanBeRead()
        {
            _target.Write("Value.1", 11);
            Assert.AreEqual(11, _target.Read<int>("Value.1"));
        }

        [Test]
        public void Read_ReturnsDefault_IfNotSet()
        {
            Assert.AreEqual(0, _target.Read<int>("Value.1"));
        }

        [Test]
        public void SetValue_NotifiesListener()
        {
            var listener = NewMock<IDataChangeListener>();
            _target.AddListener(listener.Object);
            listener.Setup(x => x.OnChanged());

            _target.Write("Value.1", 11);
        }
    }
}