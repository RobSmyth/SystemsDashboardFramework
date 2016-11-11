using System.Collections.Generic;
using Atlassian.Jira;


namespace NoeticTools.TeamStatusBoard.DataSource.Jira.DataSource
{
    public interface IJiraChannel
    {
        JiraNamedEntity[] Filters { get; }
        JiraNamedEntity[] Projects { get; }
        void Connect();
        IEnumerable<Issue> GetIssuesFromFilter(string filterName);
        IEnumerable<Issue> GetIssues(string projectName);
        void Disconnect();
        IEnumerable<Issue> GetIssuesQuery(string query);
        IEnumerable<CustomField> GetCustomFields();
    }
}