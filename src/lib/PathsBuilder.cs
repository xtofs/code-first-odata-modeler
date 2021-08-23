using modeling;

public class PathsBuilder
{
    public static void Build(Schema schema)
    {
        var paths = new PathsBuilder(schema.Elements);

        paths.ShowProperties("", schema.Service.Properties);
    }

    private SchemaElementCollection env;

    public PathsBuilder(SchemaElementCollection env)
    {
        this.env = env;
    }

    private void ShowProperties(string prefix, Properties properties, int d = 5)
    {
        if (d <= 0) return;

        foreach (var prop in properties)
        {
            if (env.TryGetValue(prop.Type, out var schemaElement))
            {
                ShowProperty(prefix, prop, schemaElement, d);
            }
        }
    }

    private void ShowProperty(string prefix, Property prop, ISchemaElement element, int d)
    {
        var url = $"{prefix}/{prop.Name}";
        Console.WriteLine("GET {0} \n\t# {1}", url, CreateTypeRef(prop, element));

        if (element is StructuredType type)
        {
            if (type.IsEntity && prop.IsMultiValue)
            {
                url += "/{" + string.Join(",", type.Keys) + "}";
                Console.WriteLine("GET {0} \n\t# {1}", url, $"a single {type.Name} Entity");
            }

            ShowProperties(url, type.Properties, d - 1);
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