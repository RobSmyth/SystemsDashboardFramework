﻿using Moq;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Plugins.DataSources.StaticProperties;
using NUnit.Framework;


namespace SystemsDashboard.Tests.Plugins.DataSources.StaticProperties
{
    [TestFixture]
    public class StaticPropertiesServiceTests : MockingTestFixtureBase
    {
        private StaticPropertiesServices _target;
        private Mock<IDashboardConfigurationServices> _configServices;

        protected override void TearDown()
        {
            _configServices = NewMock<IDashboardConfigurationServices>();
            _target = new StaticPropertiesServices(_configServices.Object);
        }

        protected override void SetUp()
        {
        }

        [Test]
        public void CanInstantiate()
        { }
    }
}
