using System;
using System.IO;
using NoeticTools.Dashboard.Framework;
using NoeticTools.Dashboard.Framework.DataSources.Jira;
using NoeticTools.Dashboard.Framework.Time;
using NUnit.Framework;


namespace TeamDashboard.Tests.DataSources.Jira
{
    [TestFixture]
    public class JiraChannelTests
    {
        private string _userName;
        private string _password;
        private string _url;
        private IClock _clock;

        [SetUp]
        public void SetUp()
        {
            _clock = new Clock();

            var credentialsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TeamDashboard");
            if (!Directory.Exists(credentialsFilePath))
            {
                Directory.CreateDirectory(credentialsFilePath);
            }

            credentialsFilePath = Path.Combine(credentialsFilePath, "credentials.txt");

            if (!File.Exists(credentialsFilePath))
            {
                using (var file = File.CreateText(credentialsFilePath))
                {
                    file.WriteLine("username");
                    file.WriteLine("password");
                    file.WriteLine("JIRA.url");
                    file.Close();
                }
            }

            using (var file = File.OpenText(credentialsFilePath))
            {
                _userName = file.ReadLine();
                _password = file.ReadLine();
                _url = file.ReadLine();

                if (!string.IsNullOrWhiteSpace(_url))
                {
                    _userName = _userName.Trim();
                    _password = _password.Trim();
                    _url = _url.Trim();
                }

                file.Close();
            }
        }

        [Test]
        public void CanReadIssuesForProject()
        {
            var target = new JiraChannel(new RunOptions(), _userName, _password, _url, _clock);
            target.Connect();

            var issues = target.GetIssues("CERN");
            foreach (var issue in issues)
            {
                Console.WriteLine("{0} - {1}", issue.Key, issue.Summary);
            }

            target.Disconnect();
        }

        [Test]
        public void CanReadIssuesFromFilter()
        {
            var target = new JiraChannel(new RunOptions(), _userName, _password, _url, _clock);
            target.Connect();

            var issues = target.GetIssuesFromFilter("CEREBRO 2.0");
            foreach (var issue in issues)
            {
                Console.WriteLine("{0} - {1}", issue.Key, issue.Summary);
                Console.WriteLine("\t{0} - {1}", issue.Type, issue.Assignee);
            }
        }
    }
}