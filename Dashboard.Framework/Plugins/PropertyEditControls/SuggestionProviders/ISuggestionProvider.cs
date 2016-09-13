using System.Collections.Generic;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.PropertyEditControls.SuggestionProviders
{
    public interface ISuggestionProvider<T>
    {
        IEnumerable<string> UpdateSuggestions();
    }
}