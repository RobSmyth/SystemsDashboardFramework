using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atlassian.Jira;


namespace Dashboard.Jira
{
    class JiraChannel
    {
        private Atlassian.Jira.Jira _jira;

        public void Connect()
        {
            _jira = new Atlassian.Jira.Jira("http://<your_jira_server>", "<user>", "<password>");

            // test
            var issues = from i in _jira.Issues
                         where i.Project == "CEREBRO"
                         select i;
        }

        public void Disconnect()
        {
        }
    }
}
