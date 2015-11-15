﻿using System.Collections.Generic;
using Atlassian.Jira;


namespace NoeticTools.Dashboard.Framework.DataSources.Jira
{
    public interface IJiraChannel
    {
        void Connect();
        IEnumerable<Issue> GetIssuesFromFilter(string filterName);
        IEnumerable<Issue> GetIssues(string projectName);
        void Disconnect();
        JiraNamedEntity[] Filters { get; }
        JiraNamedEntity[] Projects { get; }
        IEnumerable<Issue> GetIssuesQuery(string query);
        IEnumerable<CustomField> GetCustomFields();
    }
}