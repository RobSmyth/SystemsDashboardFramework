using NoeticTools.SystemsDashboard.Framework.Plugins.PropertyEditControls;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.PropertyEditControls
{
    public class ComboboxTextPropertyViewPlugin : IPlugin
    {
        public int Rank => 0;

        public void Register(IServices services)
        {
            services.PropertyEditControlProviders.Register(new ComboboxTextPropertyViewProvider());
            ;
        }
    }
}