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
        var url = $"{prefix}/{prop.Name}";
        Console.WriteLine("GET {0} \n\t# {1}", url, CreateTypeRef(prop, propType));

        if (propType is StructuredType type)
        {
            if (type.IsEntity && prop.IsMultiValue)
            {
                url += "/{" + string.Join(",", type.Keys) + "}";
                Console.WriteLine("GET {0} \n\t# {1}", url, $"a {type.Name} entity");
            }

            ShowProperties(url, type.Properties, visited.Push(type));
        }

        static string CreateTypeRef(Property prop, ISchemaElement element)
        {
            var isEntityRef = element is StructuredType type ? type.IsEntity : false;
            // var @ref = prop.IsMultiValue ? isEntityRef ? $"{{ {element.Name} }}" : $"[{element.Name}]" : $"{element.Name}";
            var @ref = prop.IsMultiValue ? isEntityRef ? $"a set of {element.Name}" : $"a list of {element.Name}" : $"a single {element.Name}";
            return @ref;
        }
    }
}