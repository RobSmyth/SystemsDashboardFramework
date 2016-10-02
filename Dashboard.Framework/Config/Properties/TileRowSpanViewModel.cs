using NoeticTools.TeamStatusBoard.Common.ViewModels;
using NoeticTools.TeamStatusBoard.Framework.Config.XmlTypes;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.Framework.Config.Properties
{
    public class TileRowSpanViewModel : TileSpanViewModelBase
    {
        public TileRowSpanViewModel(TileConfiguration tileConfiguration) : base(tileConfiguration, "Row span")
        {
        }

        protected override int Span { get { return TileConfiguration.RowSpan; } set { TileConfiguration.RowSpan = value; } }
    }
}