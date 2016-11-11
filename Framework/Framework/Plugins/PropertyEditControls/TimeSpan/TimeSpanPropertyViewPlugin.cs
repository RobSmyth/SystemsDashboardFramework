using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.PropertyEditControls.TimeSpan
{
    public class TimeSpanPropertyViewPlugin : IPlugin
    {
        public int Rank => 50;

        public void Register(IServices services)
        {
            services.PropertyEditControlProviders.Register(new TimeSpanPropertyViewProvider());
        }
    }
}