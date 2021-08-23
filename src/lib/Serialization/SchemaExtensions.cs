using System.Text.Json;
using System.Text.Json.Serialization;

namespace modeling;

public static class SchemaExtensions
{
    public static string ToJson(this Schema schema)
    {
        return JsonSerializer.Serialize(schema, options);
    }

    static JsonSerializerOptions options = new JsonSerializerOptions
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter(), new SchemaElementConverter() }
    };
}
