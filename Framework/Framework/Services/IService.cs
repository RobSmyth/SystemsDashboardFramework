namespace NoeticTools.TeamStatusBoard.Framework.Services
{
    public interface IService
    {
        string Name { get; }
        void Stop();
        void Start();
    }
}