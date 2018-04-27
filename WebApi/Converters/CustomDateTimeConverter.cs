using Newtonsoft.Json.Converters;

namespace WebApi.Converters
{
    class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            DateTimeFormat = "dd/MM/yyyy";
        }
    }
}