namespace NoeticTools.SystemsDashboard.Framework.Plugins.PropertyEditControls
{
    public class TimeSpanPropertyViewPlugin : IPlugin
    {
        public int Rank => 0;

        public void Register(IServices services)
        {
            services.PropertyEditControlProviders.Register(new TimeSpanPropertyViewProvider());
        }
    }
}