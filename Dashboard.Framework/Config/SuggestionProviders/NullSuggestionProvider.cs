using System.Collections.Generic;
using System.Linq;


namespace NoeticTools.TeamStatusBoard.Framework.Config.SuggestionProviders
{
    public sealed class NullSuggestionProvider : ISuggestionProvider<string>
    {
        public IEnumerable<string> Get()
        {
            return new string[0];
        }
    }
}
