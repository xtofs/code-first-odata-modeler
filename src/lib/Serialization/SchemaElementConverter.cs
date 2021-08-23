using System.Text.Json;
using System.Text.Json.Serialization;

namespace modeling;

public class SchemaElementConverter : JsonConverter<ISchemaElement>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeof(ISchemaElement).IsAssignableFrom(typeToConvert);
    }

    public override ISchemaElement Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, ISchemaElement value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }
        switch (value)
        {
            case StructuredType type: JsonSerializer.Serialize<StructuredType>(writer, type, internalOptions); break;
            case EnumType @enum: JsonSerializer.Serialize<EnumType>(writer, @enum, internalOptions); break;
            default: throw new NotImplementedException();
        }
    }

    private static JsonSerializerOptions internalOptions = new JsonSerializerOptions
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };
}
