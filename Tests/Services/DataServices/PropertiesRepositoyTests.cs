﻿using NoeticTools.SystemsDashboard.Framework.Services.DataServices;
using NUnit.Framework;


namespace SystemsDashboard.Tests.Services.DataServices
{
    [TestFixture]
    public class PropertiesRepositoyTests : MockingTestFixtureBase
    {
        private DataRepositoy _target;

        protected override void SetUp()
        {
            _target = new DataRepositoy("A");
        }
            
        protected override void TearDown()
        {
        }

        [Test]
        public void Name()
        {
            Assert.AreEqual("A", _target.Name);
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