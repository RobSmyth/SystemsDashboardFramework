namespace NoeticTools.TeamStatusBoard.Framework.Services.DataServices
{
    public class DataSourcePropertyParser
    {
        public DataSourcePropertyParser(string address)
        {
            var elements = address.Split('.');
            IsValid = elements.Length >= 2;
            if (!IsValid)
            {
                return;
            }
            TypeName = elements[0];
            PropertyName = address.Substring(TypeName.Length + 1);
        }

        public string PropertyName { get; set; }

        public string TypeName { get; }

        public bool IsValid { get; }
    }
}