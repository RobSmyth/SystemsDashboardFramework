using System;
using System.IO;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.DataSources.Jira;
using NoeticTools.SystemsDashboard.Framework.Services.TimeServices;
using NUnit.Framework;


namespace SystemsDashboard.Tests.DataSources.Jira
{
    [TestFixture]
    public class JiraChannelTests
    {
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
                    file.WriteLine("JIRA.ProjectName");
                    file.WriteLine("JIRA.FiltetName");
                    file.Close();
                }
            }

            ReadCredentialsFile(credentialsFilePath);

            _target = new JiraChannel(new RunOptions(), _userName, _password, _url, _clock);
            _target.Connect();
        }

        [TearDown]
        public void TearDown()
        {
            _target.Disconnect();
        }

        private string _userName;
        private string _password;
        private string _url;
        private IClock _clock;
        private JiraChannel _target;
        private string _projectName;
        private string _filterName;

        private void ReadCredentialsFile(string credentialsFilePath)
        {
            using (var file = File.OpenText(credentialsFilePath))
            {
                _userName = file.ReadLine();
                _password = file.ReadLine();
                _url = file.ReadLine();
                _projectName = file.ReadLine();
                _filterName = file.ReadLine();

                if (!string.IsNullOrWhiteSpace(_filterName))
                {
                    _userName = _userName.Trim();
                    _password = _password.Trim();
                    _projectName = _projectName.Trim();
                    _filterName = _filterName.Trim();
                    _url = _url.Trim();
                }

                file.Close();
            }
        }

        [Test]
        [Explicit]
        public void CanReadCustomFields()
        {
            foreach (var customField in _target.GetCustomFields())
            {
                Console.WriteLine("{0}", customField.Name);
            }
        }

        [Test]
        [Explicit]
        public void CanReadFilters()
        {
            foreach (var filter in _target.Filters)
            {
                Console.WriteLine("{0}", filter.Name);
            }
        }

        [Test]
        [Explicit]
        public void CanReadIssuesForProject()
        {
            var issues = _target.GetIssues(_projectName);
            foreach (var issue in issues)
            {
                Console.WriteLine("{0} - {1}", issue.Key, issue.Summary);
            }
        }

        [Test]
        [Explicit]
        public void CanReadIssuesFromFilter()
        {
            var issues = _target.GetIssuesFromFilter(_filterName);
            foreach (var issue in issues)
            {
                Console.WriteLine("{0} - {1}", issue.Key, issue.Summary);
                Console.WriteLine("\t{0} - {1}", issue.Type, issue.Assignee);
            }
        }

        [Test]
        [Explicit]
        public void CanReadProjects()
        {
            foreach (var project in _target.Projects)
            {
                Console.WriteLine("{0}", project.Name);
            }
        }
    }
}