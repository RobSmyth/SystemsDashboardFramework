using System.Collections.Generic;
using Atlassian.Jira;

namespace NoeticTools.Dashboard.Framework.DataSources.Jira
{
    public class JiraChannelEmulateState : IJiraChannel
    {
        public void Connect()
        {
        }

        public IEnumerable<Issue> GetIssuesFromFilter(string filterName)
        {
            return new Issue[0];
        }

        public IEnumerable<Issue> GetIssues(string projectName)
        {
            return new Issue[0];
        }

        public void Disconnect()
        {
        }
    }
}