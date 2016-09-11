using System.Collections.Generic;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public interface ISuggestionProvider<T>
    {
        IEnumerable<string> UpdateSuggestions(IPropertyViewModel propertyViewModel);
    }
}