using System.Text.Json.Serialization;

namespace modeling;

public sealed record StructuredType(string Name, Properties Properties) : ISchemaElement
{
    public StructuredType(string name, params Property[] properties) :
        this(name, new Properties(properties))
    { }

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)] public string Key => Name;

    public bool IsEntity => Properties.Any(p => p.IsKey);

    public SchemaElementKind Kind => SchemaElementKind.StructuredType;

    public IEnumerable<string> Keys => Properties.Where(p => p.IsKey).Select(p => p.Name);

    public bool IsOpen { get; init; }
}
