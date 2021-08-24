namespace modeling;

public sealed record Schema(Service Service, SchemaElementCollection Elements)
{
    public Schema(Service service, params StructuredType[] elements) : this(service, new SchemaElementCollection(elements)) { }
}
