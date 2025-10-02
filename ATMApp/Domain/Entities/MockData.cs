using System.Text.Json;
using System.Text.Json.Serialization;

namespace ATMApp.Domain.Entities
{
    internal class MockData
    {
        public static List<Account> LoadAccountsFromJson(string path)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = {new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)}
            };

            string json = File.ReadAllText(path);

            return JsonSerializer.Deserialize<List<Account>>(json, options) ?? new List<Account>();
        }
    }
}
