using NoeticTools.SystemsDashboard.Framework.DataSources;
using NoeticTools.TeamStatusBoard.Framework.DataSources;
using NUnit.Framework;


namespace SystemsDashboard.Tests.DataSources
{
    [TestFixture]
    public class DataPropertyViewModelTests : MockingTestFixtureBase
    {
        private string _propertyChangedLog;

        protected override void TearDown()
        {
        }

        protected override void SetUp()
        {
            _propertyChangedLog = string.Empty;
        }

        [Test]
        public void Value_IsDefault_Initially()
        {
            Assert.AreEqual(default(int), new DataPropertyViewModel<int>().Value);
            Assert.AreEqual(default(string), new DataPropertyViewModel<string>().Value);
        }

        [Test]
        public void NotifiesPropertyChanged_WhenValueChanged()
        {
            var target = new DataPropertyViewModel<int>();
            target.PropertyChanged += Target_PropertyChanged;

            target.Value = 7;

            Assert.AreEqual("Value,", _propertyChangedLog);
            Assert.AreEqual(7, target.Value);
        }

        private void Target_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            _propertyChangedLog += e.PropertyName + ",";
        }
    }
}