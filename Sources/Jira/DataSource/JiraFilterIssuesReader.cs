using System.Collections.Generic;
using System.Linq;
using Atlassian.Jira;


namespace NoeticTools.TeamStatusBoard.DataSource.Jira.DataSource
{
    public class JiraFilterIssuesReader
    {
        private const int MaxNumberOfIssues = 500;
        private const int IssuesPerReadLimit = 100;
        private readonly Atlassian.Jira.Jira _jira;
        private readonly string _filtername;

        public JiraFilterIssuesReader(Atlassian.Jira.Jira jira, string filtername)
        {
            _jira = jira;
            _filtername = filtername;
        }

        public IEnumerable<Issue> GetIssuesFromFilter()
        {
            var issues = new List<Issue>();
            var firstIssueIndex = 0;
            var read = true;
            while (read && firstIssueIndex < MaxNumberOfIssues)
            {
                var readIssues = _jira.GetIssuesFromFilter(_filtername, firstIssueIndex, IssuesPerReadLimit);
                issues.AddRange(readIssues);
                firstIssueIndex += readIssues.Count();
                read = readIssues.Any();
            }
            return issues;
        }
    }
}