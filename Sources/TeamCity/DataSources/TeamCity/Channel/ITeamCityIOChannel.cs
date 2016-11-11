using System.Threading.Tasks;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Agents;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Channel
{
    public interface ITeamCityIoChannel
    {
        string[] ProjectNames { get; }
        bool IsConnected { get; }
        void Connect();
        void Disconnect();
        Task<IBuildAgent> GetAgent(string name);
    }
}