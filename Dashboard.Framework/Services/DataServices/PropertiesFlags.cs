using System;


namespace NoeticTools.TeamStatusBoard.Framework.Services.DataServices
{
    [Flags]
    public enum PropertiesFlags
    {
        None = 0,
        ReadWrite = 0,
        ReadOnly = 1,
    }
}