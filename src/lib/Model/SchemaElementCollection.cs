namespace modeling;

public sealed class SchemaElementCollection : ReadOnlyKeyedCollection<string, ISchemaElement>
{
    public SchemaElementCollection(IEnumerable<ISchemaElement> elements) : base(elements) { }
}
