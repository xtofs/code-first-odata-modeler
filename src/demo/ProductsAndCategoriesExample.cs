
using System.ComponentModel.DataAnnotations;

namespace ProductsAndCategoriesExample;

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
