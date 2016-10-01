using System.Collections.Generic;


namespace NoeticTools.TeamStatusBoard.Framework.Config.SuggestionProviders
{
    public interface ISuggestionProvider<T>
    {
        IEnumerable<T> Get();
    }
}