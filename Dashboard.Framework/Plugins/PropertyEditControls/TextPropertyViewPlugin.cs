using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.PropertyEditControls
{
    public class TextPropertyViewPlugin : IPlugin
    {
        public int Rank => 50;

        public void Register(IServices services)
        {
            services.PropertyEditControlProviders.Register(new TextPropertyViewProvider());
            services.PropertyEditControlProviders.Register(new AutoCompleteTextPropertyViewProvider());
        }
    }
}