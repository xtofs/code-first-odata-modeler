using System.Collections.Immutable;
using modeling;

public class PathsBuilder
{
    public static void Build(Schema schema)
    {
        var paths = new PathsBuilder(schema.Elements);

        paths.ShowProperties("", schema.Service.Properties, ImmutableStack<StructuredType>.Empty);
    }

    private SchemaElementCollection env;

    public PathsBuilder(SchemaElementCollection env)
    {
        this.env = env;
    }

    private void ShowProperties(string prefix, Properties properties, IImmutableStack<StructuredType> visited)
    {
        foreach (var prop in properties)
        {
            if (env.TryGetValue(prop.Type, out var propType))
            {
                ShowProperty(prefix, prop, propType, visited);
            }
        }
    }

    private void ShowProperty(string prefix, Property prop, ISchemaElement propType, IImmutableStack<StructuredType> visited)
    {
        if (visited.Contains(propType))
        {
            return;
        }
        Console.WriteLine("### {0}", CreateTypeRef(prop, propType));
        var url = $"{prefix}/{prop.Name}";
        Console.WriteLine("GET {0}", url);
        Console.WriteLine();

        if (propType is StructuredType type)
        {
            // add the {key} to the path
            if (type.IsEntity && prop.IsMultiValue)
            {
                url += "/{" + string.Join(",", type.Keys) + "}";
                Console.WriteLine("### get a {0} entity", type.Name);
                Console.WriteLine("GET {0}", url);
                Console.WriteLine();
                
            }

            ShowProperties(url, type.Properties, visited.Push(type));
        }
    }

    static string CreateTypeRef(Property prop, ISchemaElement element)
    {
        var isEntityRef = element is StructuredType type ? type.IsEntity : false;
        // var @ref = prop.IsMultiValue ? isEntityRef ? $"{{ {element.Name} }}" : $"[{element.Name}]" : $"{element.Name}";
        var @ref = prop.IsMultiValue ? isEntityRef ? $"get a set of {element.Name} entities" : $"get a list of {element.Name} items" : $"get a {element.Name}";
        return @ref;
    }
}