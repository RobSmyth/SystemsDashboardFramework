﻿using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public interface IPropertyViewModel
    {
        string Name { get; }

        /// <summary>
        ///     Name of view provider that will be used to create the view element (e.g. TextBox).
        /// </summary>
        PropertyType EditorType { get; }

        object[] Parameters { get; }
        IDataValue Value { get; set; }
    }
}