using System.Collections.Generic;
using Atlassian.Jira;

namespace NoeticTools.Dashboard.Framework.DataSources.Jira
{
    public class JiraChannel : IJiraChannel
    {
        private Atlassian.Jira.Jira _jira;
        private readonly IJiraChannel _currentState;

        public JiraChannel(RunOptions runOption)
        {
            _currentState = runOption.EmulateMode ? new JiraChannelEmulateState() : new JiraChannelState();
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

        public void Disconnect()
        {
        }
    }
}
