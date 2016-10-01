using System.Collections.Generic;
using System.Linq;


namespace NoeticTools.TeamStatusBoard.Framework.Config.SuggestionProviders
{
    public sealed class NullSuggestionProvider : ISuggestionProvider<object>
    {
        public IEnumerable<object> Get()
        {
            return new object[0];
        }
    }
}
