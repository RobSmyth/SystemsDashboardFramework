using System.Collections.Generic;
using Atlassian.Jira;


namespace NoeticTools.Dashboard.Framework.DataSources.Jira
{
    public class JiraChannel : IJiraChannel
    {
        private readonly IJiraChannel _currentState;

        public JiraChannel(RunOptions runOption, string username, string password, string url, IClock clock)
        {
            _currentState = runOption.EmulateMode ? (IJiraChannel)new JiraChannelEmulateState() : new JiraChannelState(username, password, url, clock);
        }

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

        public JiraNamedEntity[] Filters => _currentState.Filters;
        public JiraNamedEntity[] Projects => _currentState.Projects;
    }
}