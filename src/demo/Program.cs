
using modeling;


public class Program
{
    public static void Main()
    {
        // var service = typeof(sharedUserProfiles.Service);
        var service = typeof(ProductsAndCategoriesExample.Service);

        var schema = ModelBuilder.Create(service);
        // File.WriteAllText("model.rsdl.json", schema.ToJson());

        using (var writer = ModelWriter.Create(ModelFormat.RSDL, File.Create("model.rsdl")))
        {
            writer.WriteSchema(schema);
        }

        using (var writer = ModelWriter.Create(ModelFormat.CSDL_XML, File.Create("model.csdl.xml")))
        {
            writer.WriteSchema(schema);
        }

        PathsBuilder.Build(schema);
    }
}
