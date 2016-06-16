using System;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Agents
{
    public class NullBuildAgent : IBuildAgent
    {
        public NullBuildAgent(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public BuildAgentStatus Status => BuildAgentStatus.Unknown;
        public bool IsRunning => false;
        public bool IsOnline { get { return false; } set { throw new InvalidOperationException(); } }
        public bool IsAuthorised { get { return false; } set { throw new InvalidOperationException(); } }

        public void IsNotKnown()
        {
        }

        public void IsKnown()
        {
        }
    }
}