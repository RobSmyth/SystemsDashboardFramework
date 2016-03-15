﻿using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Plugins.DataSources.StaticProperties;
using NUnit.Framework;


namespace SystemsDashboard.Tests.Plugins.DataSources.StaticProperties
{
    [TestFixture]
    public class StaticPropertiesServicePluginTests : MockingTestFixtureBase
    {
        private StaticPropertiesServicePlugIn _target;

        protected override void TearDown()
        {
        }

        protected override void SetUp()
        {
            _target = new StaticPropertiesServicePlugIn();
        }

        [Test]
        public void Register_DoesNothing()
        {
            var services = NewMock<IServices>();
            _target.Register(services.Object);
        }
    }
}