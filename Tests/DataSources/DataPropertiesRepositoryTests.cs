using Moq;
using NoeticTools.TeamStatusBoard.Framework.DataSources;
using NUnit.Framework;


namespace NoeticTools.TeamStatusBoard.Tests.DataSources
{
    [TestFixture]
    public class DataPropertiesRepositoryTests : MockingTestFixtureBase
    {
        private DataPropertiesRepository _target;
        private Mock<IDataPropertyViewModelFactory> _factory;

        protected override void SetUp()
        {
            _factory = NewMock<IDataPropertyViewModelFactory>();

            _target = new DataPropertiesRepository(_factory.Object);
        }

        protected override void TearDown()
        {
        }

        [TestCase("Provider1.PropertyA")]
        [TestCase("Provider1.PropertyE")]
        [TestCase("Provider1.Property_B")]
        [TestCase("Provider1.Propertyb")]
        [TestCase("Provider0.PropertyB")]
        [TestCase("PropertyC")]
        [TestCase("")]
        public void Has_ReturnsFalse_IfPropertyNotRegistered(string propertyName)
        {
            SetupSampleProperties();

            Assert.IsFalse(_target.Has<int>(propertyName));
        }

        [TestCase("Provider1.PropertyB")]
        [TestCase("Provider1.PropertyC")]
        [TestCase("Provider2.PropertyD")]
        public void Has_ReturnsTrue_IfPropertyRegistered(string propertyName)
        {
            SetupSampleProperties();

            Assert.IsTrue(_target.Has<int>(propertyName));
        }

        [TestCase("Provider1")]
        [TestCase("Provider1_PropertyD")]
        [TestCase("PropertyD")]
        [TestCase("")]
        public void Get_ReturnsNullPropertys_IfNameInvalid(string invalidPropertyName)
        {
            SetupSampleProperties();

            var property1 = _target.Get<int>(invalidPropertyName);

            Assert.AreEqual(typeof (NullDataPropertyViewModel<int>), property1.GetType());
        }

        private void SetupSampleProperties()
        {
            var property1 = NewMock<IDataPropertyViewModel<int>>();
            var property2 = NewMock<IDataPropertyViewModel<int>>();
            var property3 = NewMock<IDataPropertyViewModel<int>>();
            var property4 = NewMock<IDataPropertyViewModel<int>>();
            var stringProperty1 = NewMock<IDataPropertyViewModel<string>>();
            var stringProperty2 = NewMock<IDataPropertyViewModel<string>>();
            _factory.Setup(x => x.Create<int>("Provider1.PropertyB")).Returns(property1.Object);
            _factory.Setup(x => x.Create<int>("Provider1.PropertyC")).Returns(property2.Object);
            _factory.Setup(x => x.Create<int>("Provider1.PropertyD")).Returns(property3.Object);
            _factory.Setup(x => x.Create<int>("Provider2.PropertyD")).Returns(property4.Object);
            _factory.Setup(x => x.Create<string>("Provider1.StringA")).Returns(stringProperty1.Object);
            _factory.Setup(x => x.Create<string>("Provider2.StringB")).Returns(stringProperty2.Object);

            _target.Add("Provider1", "PropertyB", 7);
            _target.Add("Provider1", "PropertyC", 11);
            _target.Add("Provider1", "PropertyD", 13);
            _target.Add("Provider1", "StringA", "my Value");
            _target.Add("Provider2", "StringB", "my Value");
            _target.Add("Provider2", "PropertyD", 17);

            _factory.Verify(x => x.Create<int>(It.IsAny<string>()), Times.Exactly(4));
            _factory.Verify(x => x.Create<string>(It.IsAny<string>()), Times.Exactly(2));
        }

        [Test]
        public void Get_ReturnsDifferentPropertys_IfSameNameButDifferentProvider()
        {
            SetupSampleProperties();

            var property1 = _target.Get<int>("Provider1.PropertyD");
            var property2 = _target.Get<int>("Provider2.PropertyD");

            Assert.AreNotSame(property1, property2);
        }

        [Test]
        public void Get_ReturnsPropertyFromFactory_IfNoPropertiesRegistered()
        {
            var propertyViewModel = NewMock<IDataPropertyViewModel<int>>();
            _factory.Setup(x => x.Create<int>("Provider1.PropertyZ")).Returns(propertyViewModel.Object);

            var property = _target.Get<int>("Provider1.PropertyZ");

            Assert.AreSame(propertyViewModel.Object, property);
        }

        [Test]
        public void Get_ReturnsSamePropertyFromFactory_WhenCalledSecondTime()
        {
            var propertyViewModel = NewMock<IDataPropertyViewModel<int>>();
            _factory.Setup(x => x.Create<int>("Provider1.PropertyZ")).Returns(propertyViewModel.Object);

            var property1 = _target.Get<int>("Provider1.PropertyZ");
            var property2 = _target.Get<int>("Provider1.PropertyZ");

            Assert.AreSame(propertyViewModel.Object, property1);
            Assert.AreSame(property1, property2);
            _factory.Verify(x => x.Create<int>("Provider1.PropertyZ"), Times.Once);
        }

        [Test]
        public void GetWithProviderName_ReturnsSamePropertyFromFactory_WhenCalledSecondTime()
        {
            var propertyViewModel = NewMock<IDataPropertyViewModel<int>>();
            _factory.Setup(x => x.Create<int>("Provider1.PropertyZ")).Returns(propertyViewModel.Object);

            var property1 = _target.Get<int>("Provider1.PropertyZ");
            var property2 = _target.Get<int>("Provider1", "PropertyZ");

            Assert.AreSame(propertyViewModel.Object, property1);
            Assert.AreSame(property1, property2);
            _factory.Verify(x => x.Create<int>("Provider1.PropertyZ"), Times.Once);
        }

        [Test]
        public void Has_ReturnsFalse_IfNoPropertyRegistered()
        {
            Assert.IsFalse(_target.Has<int>("Provider1.PropertyB"));
        }
    }
}