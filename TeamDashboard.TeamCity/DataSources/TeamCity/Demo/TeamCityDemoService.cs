using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Demo
{
    public sealed class TeamCityDemoService : ITimerListener
    {
        private const string DemoProjectName = "TSB.Demo.Project 1";
        private const string DemoAgentName1 = "TSB.Demo.Agent 1";
        private const string DemoAgentName2 = "TSB.Demo.Agent 2";
        private readonly ITeamCityService _teamCityService;

        public TeamCityDemoService(ITeamCityService teamCityService, IServices services)
        {
            _teamCityService = teamCityService;
            services.Timer.QueueCallback(TimeSpan.FromSeconds(0.5), this);
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            var project = _teamCityService.Projects.Add(DemoProjectName);

            _teamCityService.Agents.Add(DemoAgentName1);
            _teamCityService.Agents.Add(DemoAgentName2);
        }
    }
}
