using System;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class BoolPropertyViewModel : PropertyViewModel
    {
        public BoolPropertyViewModel(string name, INamedValueReader tileConfigurationConverter) 
            : base(name, "TextFromCombobox", tileConfigurationConverter, () => new[] {true.ToString(), false.ToString()})
        {
        }
    }
}