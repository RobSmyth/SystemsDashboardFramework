using System.Xml.Serialization;


namespace NoeticTools.TeamStatusBoard.Framework.Config
{
    [XmlType("dashboard")]
    public class DashboardConfiguration : IDashboardConfiguration
    {
        public DashboardConfiguration()
        {
            Name = string.Empty;
            DisplayName = string.Empty;
        }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "displayName")]
        public string DisplayName { get; set; }

        [XmlElement(ElementName = "root")]
        public TileConfiguration RootTile { get; set; }
    }
}