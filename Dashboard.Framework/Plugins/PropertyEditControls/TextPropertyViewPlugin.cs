namespace NoeticTools.SystemsDashboard.Framework.Plugins.PropertyEditControls
{
    public class TextPropertyViewPlugin : IPlugin
    {
        public int Rank => 0;

        public void Register(IServices services)
        {
            services.PropertyEditControlProviders.Register(new TextPropertyViewProvider());
        }
    }
}