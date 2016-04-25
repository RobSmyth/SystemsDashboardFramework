using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.PropertyEditControls
{
    public class CheckboxPropertyViewPlugin : IPlugin
    {
        public int Rank => 50;

        public void Register(IServices services)
        {
            services.PropertyEditControlProviders.Register(new CheckboxPropertyViewProvider());
        }
    }
}