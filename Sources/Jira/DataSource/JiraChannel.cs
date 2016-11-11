using System.Collections.Generic;
using Atlassian.Jira;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;


namespace NoeticTools.TeamStatusBoard.DataSource.Jira.DataSource
{
    public class JiraChannel : IJiraChannel
    {
        private readonly IJiraChannel _currentState;

        public JiraChannel(RunOptions runOption, string username, string password, string url, IClock clock)
        {
            _currentState = runOption.EmulateMode ? (IJiraChannel) new JiraChannelEmulateState() : new JiraChannelState(username, password, url, clock);
        }

        public JiraNamedEntity[] Filters => _currentState.Filters;
        public JiraNamedEntity[] Projects => _currentState.Projects;

        public void Connect()
        {
            _currentState.Connect();
        }

        public IEnumerable<Issue> GetIssuesFromFilter(string filterName)
        {
            return _currentState.GetIssuesFromFilter(filterName);
        }

        public IEnumerable<Issue> GetIssues(string projectName)
        {
            return _currentState.GetIssues(projectName);
        }

        public IEnumerable<Issue> GetIssuesQuery(string query)
        {
            return _currentState.GetIssuesQuery(query);
        }

        public IEnumerable<CustomField> GetCustomFields()
        {
            return _currentState.GetCustomFields();
        }

        public void Disconnect()
        {
        }
    }
}