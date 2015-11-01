using System;
using NoeticTools.Dashboard.Framework;
using NoeticTools.Dashboard.Framework.DataSources.Jira;
using NUnit.Framework;


namespace TeamDashboard.Tests.DataSources.Jira
{
    [TestFixture]
    public class JiraChannelTests
    {
        [Test]
        public void CanReadIssuesForProject()
        {
            var target = new JiraChannel(new RunOptions());
            target.Connect();

            var issues = target.GetIssues("ERN");
            foreach (var issue in issues)
            {
                Console.WriteLine("{0} - {1}", issue.Key, issue.Summary);
            }

            target.Disconnect();
        }

        [Test]
        public void CanReadIssuesFromFilter()
        {
            var target = new JiraChannel(new RunOptions());
            target.Connect();

            var issues = target.GetIssuesFromFilter("EREBRO 2.0.3");
            foreach (var issue in issues)
            {
                Console.WriteLine("{0} - {1}", issue.Key, issue.Summary);
                Console.WriteLine("\t{0} - {1}", issue.Type, issue.Assignee);
            }
        }
    }
}