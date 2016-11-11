using NoeticTools.TeamStatusBoard.Common.ViewModels;
using NoeticTools.TeamStatusBoard.Framework.Persistence.Xml;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class TileColumnSpanViewModel : TileSpanViewModelBase
    {
        public TileColumnSpanViewModel(TileConfiguration tileConfiguration) : base(tileConfiguration, "Column span")
        {
        }

        protected override int Span { get { return TileConfiguration.ColumnSpan; } set { TileConfiguration.ColumnSpan = value; } }
    }
}