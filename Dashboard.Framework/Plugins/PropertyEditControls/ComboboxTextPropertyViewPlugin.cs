using NoeticTools.Dashboard.Framework.Plugins.Tiles;
using NoeticTools.Dashboard.Framework.Tiles;


namespace NoeticTools.Dashboard.Framework.Plugins.PropertyEditControls
{
    public class ComboboxTextPropertyViewPlugin : IPlugin
    {
        public void Register(IServices services)
        {
            services.PropertyEditControlProviders.Register(new ComboboxTextPropertyViewProvider());;
        }

        public int Rank => 0;
    }
}