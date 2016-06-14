using System.Threading.Tasks;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Agents;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Channel
{
    public interface ITeamCityIoChannel
    {
        string[] ProjectNames { get; }
        bool IsConnected { get; }
        void Connect();
        void Disconnect();
        string[] GetConfigurationNames(string projectName);
        Task<IBuildAgent[]> GetAgents();
        Task<IBuildAgent> GetAgent(string name);
    }
}