using System.Collections.Generic;
using Atlassian.Jira;


namespace NoeticTools.Dashboard.Framework.DataSources.Jira
{
    public class JiraChannelState : JiraChannelEmulateState
    {
        private Atlassian.Jira.Jira _jira;

        public void Connect()
        {
            _jira = new Atlassian.Jira.Jira("http://jira/", "username", "password") {MaxIssuesPerRequest = 200}; //>>>
        }

        public IEnumerable<Issue> GetIssuesFromFilter(string filterName)
        {
            return _jira.GetIssuesFromFilter(filterName, 0);
        }

        public IEnumerable<Issue> GetIssues(string projectName)
        {
            //var project = _jira.GetProjects().Single(x => x.Name.Equals(projectName));

            var filters = _jira.GetFilters();

            return _jira.Issues;
            //return _jira.Issues.Where(x => x.Project.Equals(projectName, StringComparison.InvariantCulture)).ToArray();
        }

        public void Disconnect()
        {
        }
    }
}