using System;
using System.Xml.Serialization;


namespace NoeticTools.SystemsDashboard.Framework.Config
{
    [XmlType("tile")]
    public sealed class TileConfiguration : ItemConfigurationBase
    {
        public TileConfiguration()
        {
            TypeId = PaneTileTypeId;
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
        ///     Child tiles.
        /// </summary>
        [XmlArray(ElementName = "tiles")]
        public TileConfiguration[] Tiles { get; set; }

        public bool IsInColumn(int columnNumber)
        {
            return ColumnNumber <= columnNumber && ColumnNumber + ColumnSpan - 1 >= columnNumber;
        }

        public bool IsInRow(int rowNumber)
        {
            return RowNumber <= rowNumber && RowNumber + RowSpan - 1 >= rowNumber;
        }

        public void SetLocation(TileConfiguration tile)
        {
            RowNumber = tile.RowNumber;
            ColumnNumber = tile.ColumnNumber;
            RowSpan = tile.RowSpan;
            ColumnSpan = tile.ColumnSpan;
        }

        public static string PaneTileTypeId = "Pane";
    }
}