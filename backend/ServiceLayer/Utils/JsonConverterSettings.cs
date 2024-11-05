using System.Text.Json.Serialization;
using System.Text.Json;

namespace ServiceLayer.Utils
{
    public static class JsonConverterSettings
    {
        public static JsonSerializerOptions Options => GetOptions();

        private static JsonSerializerOptions GetOptions()
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
            };

            return options;
        }
    }

}
