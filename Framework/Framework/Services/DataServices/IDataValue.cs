using System.Collections.Generic;
using System.Windows.Media;
using NoeticTools.TeamStatusBoard.Common;


namespace NoeticTools.TeamStatusBoard.Framework.Services.DataServices
{
    public interface IDataValue
    {
        EventBroadcaster Broadcaster { get; }
        string Name { get; }
        PropertiesFlags Flags { get; set; }
        List<string> Tags { get; }
        object Instance { get; set; }
        double Double { get; set; }
        Color Colour { get; set; }
        string String { get; set; }
        bool Boolean { get; set; }
        bool NotSet { get; }
        SolidColorBrush SolidColourBrush { get; }
        int Integer { get; set; }
    }
}