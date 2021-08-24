namespace modeling;

public interface ISchemaElement : IKeyed<string>
{
    string Name { get; }
    SchemaElementKind Kind { get; }
}
