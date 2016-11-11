using System;
using NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Projects;
using NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.TcSharpInterop;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;


namespace NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.TestDataSource
{
    public sealed class TeamCityTestDataService : ITimerListener
    {
        private const string DemoProjectName = "Project 1";
        private const string DemoAgentName1 = "Agent 1";
        private const string DemoAgentName2 = "Agent 2";
        private readonly ITeamCityService _teamCityService;

        public TeamCityTestDataService(ITeamCityService teamCityService, IServices services)
        {
            _teamCityService = teamCityService;
            services.Timer.QueueCallback(TimeSpan.FromSeconds(0.5), this);
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            var project = new Project(new NullInteropProject(DemoProjectName, "Test (demo) project"), new TestDataBuildConfigurationFactory());
            _teamCityService.Projects.Add(project);

            _teamCityService.Agents.Add(DemoAgentName1);
            _teamCityService.Agents.Add(DemoAgentName2);
        }
    }
}
