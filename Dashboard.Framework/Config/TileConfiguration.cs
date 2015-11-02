using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;


namespace NoeticTools.Dashboard.Framework.Config
{
    [XmlType("tile")]
    public class TileConfiguration
    {
        public static string BlankTileTypeId = "Pane";

        public TileConfiguration()
        {
            TypeId = BlankTileTypeId;
            Values = new DashboardConfigValuePair[0];
            Tiles = new TileConfiguration[0];
        }

        /// <summary>
        ///     The tile's type ID. This is used to load the tile's controller.
        /// </summary>
        [XmlAttribute(AttributeName = "typeId")]
        public string TypeId { get; set; }

        /// <summary>
        ///     The tile's unique ID.
        /// </summary>
        [XmlAttribute(AttributeName = "id")]
        public Guid Id { get; set; }

        /// <summary>
        ///     Tile's row number starting at 1. Row 1 is the top row.
        /// </summary>
        [XmlAttribute(AttributeName = "row")]
        public int RowNumber { get; set; }

        /// <summary>
        ///     Tile's column number starting at 1. Column 1 is the left most row.
        /// </summary>
        [XmlAttribute(AttributeName = "column")]
        public int ColumnNumber { get; set; }

        [XmlAttribute(AttributeName = "rowSpan")]
        public int RowSpan { get; set; }

        [XmlAttribute(AttributeName = "columnSpan")]
        public int ColumnSpan { get; set; }

        /// <summary>
        ///     Configuration values.
        /// </summary>
        [XmlArray(ElementName = "values")]
        public DashboardConfigValuePair[] Values { get; set; }

        /// <summary>
        ///     Child tiles.
        /// </summary>
        [XmlArray(ElementName = "tiles")]
        public TileConfiguration[] Tiles { get; set; }

        public DashboardConfigValuePair GetParameter(string name, string defaultValue)
        {
            var pair =
                Values.SingleOrDefault(x => x.Key.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            if (pair == null)
            {
                pair = new DashboardConfigValuePair {Key = name, Value = defaultValue};
                var list = new List<DashboardConfigValuePair>(Values);
                list.Add(pair);
                Values = list.ToArray();
            }
            return pair;
        }

        public TileConfiguration GetTileConfiguration(Guid tileId)
        {
            if (Id == tileId)
            {
                return this;
            }
            foreach (
                var config in
                    Tiles.Select(tile => tile.GetTileConfiguration(tileId)).Where(config => config != null))
            {
                return config;
            }
            return null;
        }
    }
}