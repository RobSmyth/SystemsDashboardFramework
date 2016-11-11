using NoeticTools.TeamStatusBoard.Common.ViewModels;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.Persistence.Xml;


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