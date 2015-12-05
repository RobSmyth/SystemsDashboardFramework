using NoeticTools.SystemsDashboard.Framework.Plugins.PropertyEditControls;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.PropertyEditControls
{
    public class DatePropertyViewPlugin : IPlugin
    {
        public int Rank => 0;

        public void Register(IServices services)
        {
            services.PropertyEditControlProviders.Register(new DatePropertyViewProvider());
        }
    }
}