namespace NoeticTools.SystemsDashboard.Framework.Plugins.PropertyEditControls
{
    public class CheckboxPropertyViewPlugin : IPlugin
    {
        public int Rank => 0;

        public void Register(IServices services)
        {
            services.PropertyEditControlProviders.Register(new CheckboxPropertyViewProvider());
        }
    }
}