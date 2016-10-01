using System;
using System.Collections.Generic;


namespace NoeticTools.TeamStatusBoard.Framework.Config.SuggestionProviders
{
    public sealed class TextListSuggestionProvider : ISuggestionProvider<object>
    {
        private readonly string[] _values;

        public TextListSuggestionProvider(params string[] values)
        {
            _values = values;
        }
        public IEnumerable<object> Get()
        {
            return _values;
        }
    }
}