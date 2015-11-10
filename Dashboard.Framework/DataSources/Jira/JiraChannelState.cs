using System;
using System.Collections.Generic;
using System.Linq;
using Atlassian.Jira;


namespace NoeticTools.Dashboard.Framework.DataSources.Jira
{
    public class JiraChannelState : IJiraChannel
    {
        private const int MaxIssuesPerRequest = 100;
        private readonly string _username;
        private readonly string _password;
        private readonly string _url;
        private readonly IClock _clock;
        private Atlassian.Jira.Jira _jira;
        private readonly TimeRefreshedItems<JiraNamedEntity> _filters;
        private readonly TimeRefreshedItems<JiraNamedEntity> _projects;
        private readonly Dictionary<string, TimeRefreshedItems<Issue>> _filterItems;

        public JiraChannelState(string username, string password, string url, IClock clock)
        {
            _username = username;
            _password = password;
            _url = url;
            _clock = clock;
            _filters = new TimeRefreshedItems<JiraNamedEntity>(() => _jira.GetFilters(), TimeSpan.FromMinutes(5), clock);
            _projects = new TimeRefreshedItems<JiraNamedEntity>(() => _jira.GetProjects(), TimeSpan.FromMinutes(5), clock);
            _filterItems = new Dictionary<string, TimeRefreshedItems<Issue>>();
        }

        public void Connect()
        {
            _jira = new Atlassian.Jira.Jira(_url, _username, _password) {MaxIssuesPerRequest = MaxIssuesPerRequest };
        }

        public IEnumerable<Issue> GetIssuesFromFilter(string filterName)
        {
            if (!_filterItems.ContainsKey(filterName))
            {
                var reader = new JiraFilterIssuesReader(_jira, filterName);
                _filterItems.Add(filterName, new TimeRefreshedItems<Issue>(() => reader.GetIssuesFromFilter(), TimeSpan.FromSeconds(3), _clock));
            }
            return _filterItems[filterName].Items;
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

        public JiraNamedEntity[] Filters => _filters.Items;

        public JiraNamedEntity[] Projects => _projects.Items;
    }
}