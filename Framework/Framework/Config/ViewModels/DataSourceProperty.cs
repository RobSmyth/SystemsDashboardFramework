using System.Windows.Media;


namespace NoeticTools.TeamStatusBoard.Framework.Config.ViewModels
{
    public class DataSourceProperty : TextProperty
    {
        public DataSourceProperty(string text) : base(text, "p", Brushes.Gray, Brushes.DarkBlue)
        {
        }
    }
}