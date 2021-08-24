using System.Text.Json.Serialization;

namespace modeling;

public sealed record EnumType(string Name, Members Members) : ISchemaElement
{
    public EnumType(string name, params Member[] members) : this(name, new Members(members)) { }

    public bool IsEntity => false;

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)] public string Key => Name;

    public SchemaElementKind Kind => SchemaElementKind.EnumType;
}
