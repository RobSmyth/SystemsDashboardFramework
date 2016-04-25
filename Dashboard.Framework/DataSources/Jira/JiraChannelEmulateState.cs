using System.Collections.Generic;
using Atlassian.Jira;


namespace NoeticTools.TeamStatusBoard.Framework.DataSources.Jira
{
    public class JiraChannelEmulateState : IJiraChannel
    {
        public JiraChannelEmulateState()
        {
            Filters = new JiraNamedEntity[0];
            Projects = new JiraNamedEntity[0];
        }

        public JiraNamedEntity[] Filters { get; }
        public JiraNamedEntity[] Projects { get; }

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

        public IEnumerable<Issue> GetIssuesQuery(string query)
        {
            return new Issue[0];
        }

        public IEnumerable<CustomField> GetCustomFields()
        {
            return new CustomField[0];
        }

        public void Disconnect()
        {
        }
    }
}