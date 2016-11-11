using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.PropertyEditControls.Combobox
{
    public class ComboboxTextPropertyViewPlugin : IPlugin
    {
        public int Rank => 50;

        public void Register(IServices services)
        {
            services.PropertyEditControlProviders.Register(new ComboboxTextPropertyViewProvider());
        }
    }
}