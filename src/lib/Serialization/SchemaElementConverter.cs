using System.Text.Json;
using System.Text.Json.Serialization;
using modeling;

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
        var o2 = new JsonSerializerOptions { WriteIndented = options.WriteIndented, Converters = { new JsonStringEnumConverter() } };
        switch (value)
        {
            case StructuredType type: JsonSerializer.Serialize<StructuredType>(writer, type, o2); break;
            case EnumType @enum: JsonSerializer.Serialize<EnumType>(writer, @enum, o2); break;
            default: throw new NotImplementedException();
        }

        // writer.WriteStartObject();
        // foreach (var property in value.GetType().GetProperties())
        // {
        //     if (!property.CanRead)
        //         continue;
        //     var propertyValue = property.GetValue(value);
        //     writer.WritePropertyName(property.Name);
        //     JsonSerializer.Serialize(writer, propertyValue, options);
        // }
        // writer.WriteEndObject();
    }
}
