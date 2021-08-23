using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using modeling;

public class Program
{
    public static void Main()
    {
        var schema = ModelBuilder.Create(typeof(Service));

        File.WriteAllText("model.rsdl.json", JsonSerializer.Serialize(schema, new JsonSerializerOptions { WriteIndented = true, Converters = { new JsonStringEnumConverter(), new SchemaElementConverter() } }));

        using (var writer = ModelWriter.Create(ModelFormat.RSDL, File.Create("model.rsdl"))) // Console.OpenStandardOutput()
        {
            writer.WriteSchema(schema);
        }
        Console.WriteLine();

        using (var writer = ModelWriter.Create(ModelFormat.CSDL_XML, File.Create("model.csdl.xml")))
        {
            writer.WriteSchema(schema);
        }

        PathsBuilder.Build(schema);
    }
}


public record Service(IReadOnlyCollection<AppCompatibilityModes> modes) { }

public record AppCompatibilityModes([property: Key] string appId, IReadOnlyCollection<CompatibilityMode> flags) { }

public record struct CompatibilityMode(string name, CompatibilityModeState state, IReadOnlyCollection<Parameter> parameters) { }

public enum CompatibilityModeState { Off, On }

public record struct Parameter(string name, object value) { }
