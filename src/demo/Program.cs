using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using modeling;


internal record User(int id, int? managerId)
{
}

internal record struct Rel(int from, int to, int distance)
{
}

public class Program
{
    public static void Main()
    {
        var schema = ModelBuilder.Create(typeof(Service));
        File.WriteAllText("model.rsdl.json", schema.ToJson());

        using (var writer = ModelWriter.Create(ModelFormat.RSDL, File.Create("model.rsdl"))) // 
        {
            writer.WriteSchema(schema);
        }

        using (var writer = ModelWriter.Create(ModelFormat.CSDL_XML, File.Create("model.csdl.xml")))
        {
            writer.WriteSchema(schema);
        }

        // PathsBuilder.Build(schema);


        // var a = new applicationApiConfiguration("0000-00", new[] {
        //     new apiFeatureConfiguration("nested-members-in-unified-group", 2, new[] { new featureParameter("always-log", true) }),
        //     new apiFeatureConfiguration("service-principal-as-owners", 2, new featureParameter[] { })
        // });

        // Console.WriteLine(JsonSerializer.Serialize(a, new JsonSerializerOptions { WriteIndented = true }));

        // Console.WriteLine(AsHeader(a));
    }

    private static string AsHeader(applicationApiConfiguration a) => "Accept-Feature: " + string.Join(", ", a.apiFeatureConfigurations.Select(Format));

    private static string Format(apiFeatureConfiguration f) => $"{f.name}; version={f.version}{/*Format(f.parameters)*/ ""}";

    // private static string Format(IEnumerable<featureParameter> ps, string sep = "; ") => (ps == null || !ps.Any()) ? "" : sep + string.Join(sep, ps.Select(p => $"{p.name}={p.value}"));
}



public record Service(
    IReadOnlyCollection<applicationApiConfiguration> applicationApiConfigurations,
    IReadOnlyCollection<application> applications
)
{ }

public record application(
    [property: Key] string id,
    string displayName,
    string appId,
    IReadOnlyCollection<apiFeatureConfiguration> apiFeatureConfigurations
)
{ }

public record applicationApiConfiguration(
    [property: Key] string appId,
    IReadOnlyCollection<apiFeatureConfiguration> apiFeatureConfigurations
)
{ }

[OpenType]
public record apiFeatureConfiguration(
    [property: Key] string name,
    int version
// IReadOnlyCollection<featureParameter> parameters
)
{ }

// public record struct featureParameter(
//     string name,
//     object value
// )
// { }


