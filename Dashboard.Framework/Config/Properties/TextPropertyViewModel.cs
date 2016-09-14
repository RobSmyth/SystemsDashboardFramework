using System;
using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class TextPropertyViewModel : PropertyViewModel
    {
        public TextPropertyViewModel(string name, INamedValueRepository tileConfigurationConverter, Func<string[]> parametersFunc = null) 
            : base(name, "Text", tileConfigurationConverter, parametersFunc)
        {
        }
    }
}