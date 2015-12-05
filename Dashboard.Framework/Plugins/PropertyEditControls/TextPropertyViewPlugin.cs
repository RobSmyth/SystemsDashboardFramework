using System;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Plugins.Tiles;
using NoeticTools.Dashboard.Framework.Plugins.Tiles.Date;
using NoeticTools.Dashboard.Framework.Tiles;
using NoeticTools.Dashboard.Framework.Time;


namespace NoeticTools.Dashboard.Framework.Plugins.PropertyEditControls
{
    public class TextPropertyViewPlugin : IPlugin
    {
        public void Register(IServices services)
        {
            services.PropertyEditControlProviders.Register(new TextPropertyViewProvider());
        }

        public int Rank => 0;
    }
}