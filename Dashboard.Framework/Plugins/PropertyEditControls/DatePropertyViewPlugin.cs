namespace NoeticTools.Dashboard.Framework.Plugins.PropertyEditControls
{
    public class DatePropertyViewPlugin : IPlugin
    {
        public void Register(IServices services)
        {
            services.PropertyEditControlProviders.Register(new DatePropertyViewProvider());
        }

        public int Rank => 0;
    }
}