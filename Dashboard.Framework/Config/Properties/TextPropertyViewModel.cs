using System;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class TextPropertyViewModel : PropertyViewModel
    {
        public TextPropertyViewModel(string name, INamedValueReader tileConfigurationConverter, Func<string[]> parametersFunc = null) 
            : base(name, "Text", tileConfigurationConverter, parametersFunc)
        {
        }
    }
}