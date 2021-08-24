using System.Text.Json.Serialization;

namespace modeling;


public sealed record Schema(Service Service, SchemaElementCollection Elements)
{
    public Schema(Service service, params StructuredType[] elements) : this(service, new SchemaElementCollection(elements)) { }
}

public sealed class SchemaElementCollection : ReadOnlyKeyedCollection<string, ISchemaElement>
{
    public SchemaElementCollection(IEnumerable<ISchemaElement> elements) : base(elements) { }
}

public sealed record Service(Properties Properties)
{
    public Service(params Property[] properties) :
        this(new Properties(properties))
    { }
}

public interface ISchemaElement : IKeyed<string>
{
    string Name { get; }
    SchemaElementKind Kind { get; }
}

public enum SchemaElementKind { StructuredType, EnumType }

public sealed record Property(string Name, string Type, bool isNavigation) : IKeyed<string>
{
    public bool IsKey { get; init; }

    public bool IsMultiValue { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)] public string Key => Name;
}

public sealed class Properties : ReadOnlyKeyedCollection<string, Property>
{
    public Properties(IEnumerable<Property> properties) : base(properties)
    {
    }

    public static implicit operator Properties(Property[] properties) => new Properties(properties);
}

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

public sealed record EnumType(string Name, Members Members) : ISchemaElement
{
    public EnumType(string name, params Member[] members) : this(name, new Members(members)) { }

    public bool IsEntity => false;

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)] public string Key => Name;

    public SchemaElementKind Kind => SchemaElementKind.EnumType;
}

public sealed record Member(string Name) : IKeyed<string>
{
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)] public string Key => Name;
}

public sealed class Members : ReadOnlyKeyedCollection<string, Member>
{
    public Members(IEnumerable<Member> members) : base(members) { }
}
