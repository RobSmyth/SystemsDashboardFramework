using NoeticTools.Dashboard.Framework.Plugins.Tiles;
using NoeticTools.Dashboard.Framework.Tiles;


namespace NoeticTools.Dashboard.Framework.Plugins.PropertyEditControls
{
    public class CheckboxPropertyViewPlugin : IPlugin
    {
        public void Register(IServices services)
        {
            services.PropertyEditControlProviders.Register(new CheckboxPropertyViewProvider());
        }

        public int Rank => 0;
    }
}