using System.Collections.Generic;
using System.Windows.Media;


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
    }
}