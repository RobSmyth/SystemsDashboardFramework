using System;
using System.Collections.Generic;


namespace NoeticTools.TeamStatusBoard.Framework.Config.SuggestionProviders
{
    public sealed class TextListSuggestionProvider : ISuggestionProvider<string>
    {
        private readonly string[] _values;

        public TextListSuggestionProvider(params string[] values)
        {
            _values = values;
        }
        public IEnumerable<string> Get()
        {
            return _values;
        }
    }
}