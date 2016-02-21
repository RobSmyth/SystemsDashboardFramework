using Moq;
using NUnit.Framework;


namespace SystemsDashboard.Tests
{
    public abstract class MockingTestFixtureBase
    {
        private MockRepository _repository;

        [SetUp]
        public void SetUpBase()
        {
            _repository = new MockRepository(MockBehavior.Strict);
            SetUp();
        }

        [TearDown]
        public void TearDownBase()
        {
            TearDown();
            _repository.VerifyAll();
        }

        protected abstract void TearDown();

        protected abstract void SetUp();

        protected Mock<T> NewMock<T>() where T : class
        {
            return _repository.Create<T>();
        }
    }
}