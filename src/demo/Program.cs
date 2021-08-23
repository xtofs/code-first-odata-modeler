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
        // File.WriteAllText("model.rsdl.json", schema.ToJson());

        using (var writer = ModelWriter.Create(ModelFormat.RSDL, File.Create("model.rsdl"))) // 
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

// http://docs.oasis-open.org/odata/odata-csdl-xml/v4.01/odata-csdl-xml-v4.01.html#sec_ProductsandCategoriesExample


public record Product(
    [property: Key] int ID,
    string Description,
    DateOnly ReleaseDate,
    DateOnly DiscontinuedDate,
    int Rating,
    decimal Price,
    string Currency,
    Category Category,
    Supplier Supplier
)
{ }

public record Category(
    [property: Key] int ID,
    string Name,
    IReadOnlyCollection<Product> Products)
{ }

public record Supplier(
    [property: Key] int ID,
    string Name,
    Address Address,
    int Concurrency,
    IReadOnlyCollection<Product> Products)
{ }


public record struct Address(
        string Street,
        string City,
        string State,
        string ZipCode,
        string CountryName,
        Country Country)
{ }


public record Country(
        [property: Key] string Code,
        string Name
)
{ }




public record Service(

    IReadOnlyCollection<Product> Products,
    IReadOnlyCollection<Category> Categories,
    IReadOnlyCollection<Supplier> Supliers,
    IReadOnlyCollection<Country> Countries
)
{ }
